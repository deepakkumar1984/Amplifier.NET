/*    
*    Parallel.cs
*
﻿*    Copyright (C) 2012 Jan-Arne Sobania, Frank Feinbube, Ralf Diestelkämper
*
*    This library is free software: you can redistribute it and/or modify
*    it under the terms of the GNU Lesser General Public License as published by
*    the Free Software Foundation, either version 3 of the License, or
*    (at your option) any later version.
*
*    This library is distributed in the hope that it will be useful,
*    but WITHOUT ANY WARRANTY; without even the implied warranty of
*    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*    GNU Lesser General Public License for more details.
*
*    You should have received a copy of the GNU Lesser General Public License
*    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*
*    jan-arne [dot] sobania [at] gmx [dot] net
*    Frank [at] Feinbube [dot] de
*    ralf [dot] diestelkaemper [at] hotmail [dot] com
*
*/


﻿#define USE_HOST_POINTER

using System;
using System.Threading;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System.IO;

using OpenClKernel = System.Int32;

namespace Hybrid.MsilToOpenCL
{
    public class Parallel
    {
        private static HlGraphCache hlGraphCache = new HlGraphCache();
		private static HlGraphCache subGraphCache = new HlGraphCache();
        private static int HlGraphSequenceNumber = 0;

        public static void ForGpu(int fromInclusive, int toExclusive, Action<int> action, OpenCLNet.Device device)
        {
            HlGraphEntry hlGraphEntry = GetHlGraph(action.Method, 1, device);

            System.Diagnostics.Debug.Assert(hlGraphEntry.fromInclusiveLocation != null && hlGraphEntry.fromInclusiveLocation.Count == 1);
            System.Diagnostics.Debug.Assert(hlGraphEntry.toExclusiveLocation != null && hlGraphEntry.toExclusiveLocation.Count == 1);

            using (InvokeContext ctx = new InvokeContext(hlGraphEntry.HlGraph))
            {
                if (hlGraphEntry.fromInclusiveLocation.Count > 0)
                    ctx.PutArgument(hlGraphEntry.fromInclusiveLocation[0], fromInclusive);

                if (hlGraphEntry.toExclusiveLocation.Count > 0)
                    ctx.PutArgument(hlGraphEntry.toExclusiveLocation[0], toExclusive);

                DoInvoke(new int[] { toExclusive - fromInclusive }, action.Target, hlGraphEntry, ctx, device);
            }
        }

        public static void ForGpu(int fromInclusiveX, int toExclusiveX, int fromInclusiveY, int toExclusiveY, Action<int, int> action, OpenCLNet.Device device)
        {
            HlGraphEntry hlGraphEntry = GetHlGraph(action.Method, 2, device);

            System.Diagnostics.Debug.Assert(hlGraphEntry.fromInclusiveLocation != null && hlGraphEntry.fromInclusiveLocation.Count == 2);
            System.Diagnostics.Debug.Assert(hlGraphEntry.toExclusiveLocation != null && hlGraphEntry.toExclusiveLocation.Count == 2);

            using (InvokeContext ctx = new InvokeContext(hlGraphEntry.HlGraph))
            {
                if (hlGraphEntry.fromInclusiveLocation.Count > 0)
                    ctx.PutArgument(hlGraphEntry.fromInclusiveLocation[0], fromInclusiveX);

                if (hlGraphEntry.toExclusiveLocation.Count > 0)
                    ctx.PutArgument(hlGraphEntry.toExclusiveLocation[0], toExclusiveX);

                if (hlGraphEntry.fromInclusiveLocation.Count > 1)
                    ctx.PutArgument(hlGraphEntry.fromInclusiveLocation[1], fromInclusiveY);

                if (hlGraphEntry.toExclusiveLocation.Count > 1)
                    ctx.PutArgument(hlGraphEntry.toExclusiveLocation[1], toExclusiveY);

                DoInvoke(new int[] { toExclusiveX - fromInclusiveX, toExclusiveY - fromInclusiveY }, action.Target, hlGraphEntry, ctx, device);
            }
        }

        private static void SetArguments(InvokeContext ctx, object Target, HighLevel.AccessPathEntry PathEntry)
        {
            if (PathEntry.ArgumentLocation != null)
                ctx.PutArgument(PathEntry.ArgumentLocation, Target);

            if (PathEntry.SubEntries != null)
                foreach (KeyValuePair<FieldInfo, HighLevel.AccessPathEntry> Entry in PathEntry.SubEntries)
                    SetArguments(ctx, Entry.Key.GetValue(Target), Entry.Value);
        }

        private static void DoInvoke(int[] WorkSize, object Target, HlGraphEntry CacheEntry, InvokeContext ctx, OpenCLNet.Device device)
        {
            HighLevel.HlGraph HLgraph = CacheEntry.HlGraph;

            foreach (KeyValuePair<FieldInfo, HighLevel.ArgumentLocation> Entry in HLgraph.StaticFieldMap)
                ctx.PutArgument(Entry.Value, Entry.Key.GetValue(null));

            SetArguments(ctx, Target, HLgraph.RootPathEntry);
            
            /*
            foreach (KeyValuePair<FieldInfo, HighLevel.ArgumentLocation> Entry in HLgraph.ThisFieldMap)
            {
                ctx.PutArgument(Entry.Value, Entry.Key.GetValue(Target));
            }
            foreach (KeyValuePair<FieldInfo, Dictionary<FieldInfo, HighLevel.ArgumentLocation>> Entry in HLgraph.OuterThisFieldMap) {
                object RealThis = Entry.Key.GetValue(Target);
                foreach (KeyValuePair<FieldInfo, HighLevel.ArgumentLocation> SubEntry in Entry.Value) {
                    ctx.PutArgument(SubEntry.Value, SubEntry.Key.GetValue(RealThis));
                }
            }*/

            foreach (KeyValuePair<HighLevel.ArgumentLocation, HighLevel.ArrayInfo> Entry in HLgraph.MultiDimensionalArrayInfo)
            {
                System.Diagnostics.Debug.Assert(Entry.Key.Index >= 0 && Entry.Key.Index < ctx.Arguments.Count);
                InvokeArgument BaseArrayArg = ctx.Arguments[Entry.Key.Index];
                System.Diagnostics.Debug.Assert(BaseArrayArg != null && BaseArrayArg.Value != null && BaseArrayArg.Value.GetType() == Entry.Key.DataType);
                System.Diagnostics.Debug.Assert(Entry.Key.DataType.IsArray && Entry.Key.DataType.GetArrayRank() == Entry.Value.DimensionCount);
                System.Diagnostics.Debug.Assert(BaseArrayArg.Value is Array);

                Array BaseArray = (System.Array)BaseArrayArg.Value;
                long BaseFactor = 1;
                for (int Dimension = 1; Dimension < Entry.Value.DimensionCount; Dimension++)
                {
                    int ThisDimensionLength = BaseArray.GetLength(Entry.Value.DimensionCount - 1 - (Dimension - 1));
                    BaseFactor *= ThisDimensionLength;
                    ctx.PutArgument(Entry.Value.ScaleArgument[Dimension], (int)BaseFactor);
                }
            }
            ctx.Complete();

            OpenCLInterop.CallOpenCLNet(WorkSize, CacheEntry, ctx, HLgraph, device);
        }

        public static int DumpCode = 0;	// 0-2: nothing, 3 = final, 4 = initial, 5 = after optimize, 6 = after OpenCL transform

        public static void PurgeCaches()
        {
            hlGraphCache.purge();
			subGraphCache.purge();
        }

        private static HlGraphEntry GetHlGraph(MethodInfo Method, int GidParamCount, OpenCLNet.Device device)
        {
			HlGraphEntry CacheEntry;

            if (device == null)
                device = OpenCLInterop.GetFirstGpu();

			if (device == null)
				device = OpenCLInterop.GetFirstCpu();

            if (device == null)
                throw new Exception("No OpenCL Compute Device found in the system!");

            if (hlGraphCache.TryGetValue(device.DeviceID, Method, out CacheEntry))
                return CacheEntry;

            CacheEntry = ConstructKernelHlGraphEntry(Method, GidParamCount);
			CacheEntry.Device = device;

			hlGraphCache.SetValue(device.DeviceID, Method, CacheEntry);
			return CacheEntry;
		}

        internal static HlGraphEntry ConstructRelatedHlGraphEntry(MethodInfo Method, HighLevel.HlGraph ParentGraph, HlGraphCache RelatedGraphCache)
        {
            HlGraphEntry CacheEntry = ConstructHlGraphEntry(Method, 0, false, "Cil2OpenCL_Sub_Seq{0}");
            RelatedGraphCache.SetValue(IntPtr.Zero, Method, CacheEntry);
            ParentGraph.RelatedGraphs[Method] = CacheEntry;

            GenerateOpenCLSource(CacheEntry);
            return CacheEntry;
        }

        private static HlGraphEntry ConstructKernelHlGraphEntry(MethodInfo Method, int GidParamCount)
        {
            HlGraphEntry CacheEntry = ConstructHlGraphEntry(Method, GidParamCount, true, "Cil2OpenCL_Kernel_Seq{0}");

            GenerateOpenCLSource(CacheEntry);
            return CacheEntry;
        }

        private static Dictionary<Type, string> m_ValueTypeMap = new Dictionary<Type, string>();

		private static HlGraphEntry ConstructHlGraphEntry(MethodInfo Method, int GidParamCount, bool IsKernel, string NameTemplate) {
            TextWriter writer = System.Console.Out;
            string MethodName = string.Format(NameTemplate, HlGraphSequenceNumber++);
            HighLevel.HlGraph HLgraph = new HighLevel.HlGraph(Method, MethodName);
            HLgraph.IsKernel = IsKernel;
            HLgraph.ValueTypeMap = m_ValueTypeMap;

            if (!IsKernel && Method.DeclaringType.IsValueType && ((Method.CallingConvention & CallingConventions.HasThis) != 0))
            {
                System.Diagnostics.Debug.Assert(HLgraph.Arguments.Count > 0);
                System.Diagnostics.Debug.Assert(HLgraph.Arguments[0].DataType.IsByRef && HLgraph.Arguments[0].DataType.GetElementType() == Method.DeclaringType);
                HLgraph.KeepThis = true;
            }

            if (DumpCode > 3)
                WriteCode(HLgraph, writer);

            // Optimize it (just some copy propagation and dead assignment elimination to get rid of
            // CIL stack accesses)
            HLgraph.Optimize();

            if (DumpCode > 4)
                WriteCode(HLgraph, writer);

            // Convert all expression trees into something OpenCL can understand
            HLgraph.ConvertForOpenCl(subGraphCache);
            System.Diagnostics.Debug.Assert(HLgraph.KeepThis || !HLgraph.HasThisParameter);

            // Change the real first arguments (the "int"s of the Action<> method) to local variables
            // which get their value from OpenCL's built-in get_global_id() routine.
            // NOTE: ConvertArgumentToLocal removes the specified argument, so both calls need to specify
            //       an ArgumentId of zero!!!
            List<HighLevel.LocalVariableLocation> IdLocation = new List<HighLevel.LocalVariableLocation>();
            for (int i = 0; i < GidParamCount; i++)
            {
                IdLocation.Add(HLgraph.ConvertArgumentToLocal(0));
            }

            // Add fromInclusive and toExclusive as additional parameters
            List<HighLevel.ArgumentLocation> StartIdLocation = new List<HighLevel.ArgumentLocation>();
            List<HighLevel.ArgumentLocation> EndIdLocation = new List<HighLevel.ArgumentLocation>();
            for (int i = 0; i < GidParamCount; i++)
            {
                StartIdLocation.Add(HLgraph.InsertArgument(i * 2 + 0, "fromInclusive" + i, typeof(int), false));
                EndIdLocation.Add(HLgraph.InsertArgument(i * 2 + 1, "toExclusive" + i, typeof(int), false));
            }

            // "i0 = get_global_id(0) + fromInclusive0;"
            for (int i = 0; i < GidParamCount; i++)
            {
                HLgraph.CanonicalStartBlock.Instructions.Insert(i, new HighLevel.AssignmentInstruction(
                    new HighLevel.LocationNode(IdLocation[i]),
                    new HighLevel.AddNode(
                        new HighLevel.CallNode(typeof(OpenClFunctions).GetMethod("get_global_id", new Type[] { typeof(uint) }), new HighLevel.IntegerConstantNode(i)),
                        new HighLevel.LocationNode(StartIdLocation[i])
                        )
                    )
                );
            }

            // "if (i0 >= toExclusive0) return;"
            if (GidParamCount > 0)
            {
                HighLevel.BasicBlock ReturnBlock = null;
                foreach (HighLevel.BasicBlock BB in HLgraph.BasicBlocks)
                {
                    if (BB.Instructions.Count == 1 && BB.Instructions[0].InstructionType == HighLevel.InstructionType.Return)
                    {
                        ReturnBlock = BB;
                        break;
                    }
                }
                if (ReturnBlock == null)
                {
                    ReturnBlock = new HighLevel.BasicBlock("CANONICAL_RETURN_BLOCK");
                    ReturnBlock.Instructions.Add(new HighLevel.ReturnInstruction(null));
                    HLgraph.BasicBlocks.Add(ReturnBlock);
                }
                ReturnBlock.LabelNameUsed = true;
                for (int i = 0; i < GidParamCount; i++)
                {
                    HLgraph.CanonicalStartBlock.Instructions.Insert(GidParamCount + i, new HighLevel.ConditionalBranchInstruction(
                        new HighLevel.GreaterEqualsNode(
                            new HighLevel.LocationNode(IdLocation[i]),
                            new HighLevel.LocationNode(EndIdLocation[i])
                        ),
                        ReturnBlock
                        )
                    );
                }
            }

            // Create the argument to pass the random seed, if necessary
            if (!object.ReferenceEquals(HLgraph.RandomStateLocation, null) && HLgraph.RandomStateLocation.LocationType == HighLevel.LocationType.LocalVariable)
            {
                // This can only happen for kernels. All nested routines get a pointer to
                // the kernel's rnd_state instead

                System.Diagnostics.Debug.Assert(HLgraph.IsKernel);
                System.Diagnostics.Debug.Assert(object.ReferenceEquals(HLgraph.RandomSeedArgument, null));

                HLgraph.RandomSeedArgument = HLgraph.CreateArgument("rnd_seed", typeof(uint), false);

                if (GidParamCount > 0)
                {
                    HighLevel.Node LocalSeed = null;
                    for (int i = 0; i < GidParamCount; i++)
                    {
                        if (LocalSeed == null)
                        {
                            LocalSeed = new HighLevel.LocationNode(IdLocation[i]);
                        }
                        else
                        {
                            LocalSeed = new HighLevel.AddNode(
                                            new HighLevel.LocationNode(IdLocation[i]),
                                            new HighLevel.MulNode(
                                                LocalSeed,
                                                new HighLevel.IntegerConstantNode(0x10000)  /* TODO: what is a good factor here ??? */
                                            )
                                        );
                        }
                    }
                    HLgraph.CanonicalStartBlock.Instructions.Add(new HighLevel.AssignmentInstruction(
                        new HighLevel.NamedFieldNode(new HighLevel.LocationNode(HLgraph.RandomStateLocation), "x", typeof(uint)),
                        new HighLevel.AddNode(
                            LocalSeed,
                            new HighLevel.IntegerConstantNode(1)
                            )
                        )
                    );
                }
                else
                {
                    HLgraph.CanonicalStartBlock.Instructions.Add(new HighLevel.AssignmentInstruction(
                        new HighLevel.NamedFieldNode(new HighLevel.LocationNode(HLgraph.RandomStateLocation), "x", typeof(uint)),
                        new HighLevel.IntegerConstantNode(1)
                        )
                    );
                }

                HLgraph.CanonicalStartBlock.Instructions.Add(new HighLevel.AssignmentInstruction(
                    new HighLevel.NamedFieldNode(new HighLevel.LocationNode(HLgraph.RandomStateLocation), "y", typeof(uint)),
                    new HighLevel.LocationNode(HLgraph.RandomSeedArgument)
                    ));

                // Perform TWO warmup rounds, so our not-so-random start states
                // get a chance to really inflict changes to all 32 bits of
                // generated random numbers.
                HLgraph.CanonicalStartBlock.Instructions.Add(new HighLevel.AssignmentInstruction(
                    null,
                    new HighLevel.CallNode(
                        typeof(OpenClFunctions).GetMethod("rnd"),
                        new HighLevel.AddressOfNode(
                            new HighLevel.LocationNode(HLgraph.RandomStateLocation)
                            )
                        )
                    )
                );
                HLgraph.CanonicalStartBlock.Instructions.Add(new HighLevel.AssignmentInstruction(
                    null,
                    new HighLevel.CallNode(
                        typeof(OpenClFunctions).GetMethod("rnd"),
                        new HighLevel.AddressOfNode(
                            new HighLevel.LocationNode(HLgraph.RandomStateLocation)
                            )
                        )
                    )
                );
            }

            if (DumpCode > 5)
            {
                WriteCode(HLgraph, writer);
            }

            // Update location usage information
            HLgraph.AnalyzeLocationUsage();

            // Finally, add the graph to the cache
            HlGraphEntry CacheEntry = new HlGraphEntry(HLgraph, StartIdLocation, EndIdLocation);

            return CacheEntry;
        }

        private static void GenerateOpenCLSource(HlGraphEntry CacheEntry) {
            // Non-kernel methods include just their own code, but kernel methods include everything required
            if (!CacheEntry.HlGraph.IsKernel || CacheEntry.HlGraph.RelatedGraphs.Count == 0)
            {
                CacheEntry.Source = getOpenCLSource(CacheEntry.HlGraph);
                return;
            }

            // No recursion allowed, so we can get away with a topological sort of all involved functions
            // with no prototypes beforehand.
            // The following code has been adapted from
            // http://www.logarithmic.net/pfh-files/blog/01208083168/sort.py
            // "Tarjan's algorithm and topological sorting implementation in Python" by Paul Harrison
   
            // Step 1: get list of all involved methods
            List<HighLevel.HlGraph> Nodes = new List<HighLevel.HlGraph>();
            Nodes.Add(CacheEntry.HlGraph);
            for (int i = 0; i < Nodes.Count; i++)
            {
                HighLevel.HlGraph Entry = Nodes[i];
                foreach (HlGraphEntry SubEntry in Entry.RelatedGraphs.Values)
                {
                    if (!Nodes.Contains(SubEntry.HlGraph))
                    {
                        Nodes.Add(SubEntry.HlGraph);
                    }
                }
            }

            // Step 2: topological sort
            Dictionary<HighLevel.HlGraph, int> count = new Dictionary<HighLevel.HlGraph, int>();
            foreach (HighLevel.HlGraph Current in Nodes)
                count[Current] = 0;
            foreach (HighLevel.HlGraph Current in Nodes)
                foreach (HlGraphEntry SuccessorEntry in Current.RelatedGraphs.Values)
                    count[SuccessorEntry.HlGraph]++;

            List<HighLevel.HlGraph> Ready = new List<HighLevel.HlGraph>();
            List<HighLevel.HlGraph> Result = new List<HighLevel.HlGraph>(Nodes.Count);
            Ready.Add(CacheEntry.HlGraph);
            while (Ready.Count > 0)
            {
                HighLevel.HlGraph Current = Ready[Ready.Count - 1];
                Ready.RemoveAt(Ready.Count - 1);
                Result.Add(Current);
                System.Diagnostics.Debug.Assert(count[Current] == 0);

                foreach (HlGraphEntry Successor in Current.RelatedGraphs.Values)
                {
                    count[Successor.HlGraph]--;
                    if (count[Successor.HlGraph] == 0)
                        Ready.Add(Successor.HlGraph);
                }
            }

            // Step 3: check for recursions. If there is any strongly-connected component, count[s]
            // will never reach zero, so the Ready list runs empty without all functions being
            // inserted into the Result list.
            if (Result.Count != Nodes.Count)
                throw new InvalidOperationException("Unable to compute topological sort of functions. Recursions are not supported.");

            // Generate code for all HlGraphs in the Result list, in reverse order
            Result.Reverse();

            CacheEntry.Source = getOpenCLSource(Result);
        }

        private static void writeOpenClStructs(Hybrid.MsilToOpenCL.HighLevel.HlGraph HLgraph, StringWriter Srcwriter)
        {
            Dictionary<Type, int> TypeMarks = new Dictionary<Type, int>();
            foreach (HighLevel.ArgumentLocation Argument in HLgraph.Arguments)
            {
                if (HLgraph.ValueTypeMap.ContainsKey(Argument.DataType))
                {
                    TypeMarks[Argument.DataType] = 1;
                }
                else if ((Argument.DataType.IsByRef || Argument.DataType.IsPointer) && HLgraph.ValueTypeMap.ContainsKey(Argument.DataType.GetElementType()))
                {
                    TypeMarks[Argument.DataType.GetElementType()] = 1;
                }
            }
            foreach (KeyValuePair<Type, int> Entry in TypeMarks)
            {
                OpenCLInterop.WriteOpenCL(HLgraph, Entry.Key, Srcwriter);
            }
        }

        private static string getOpenCLSource(HighLevel.HlGraph HLgraph)
        {
            using (StringWriter Srcwriter = new StringWriter())
            {
                OpenCLInterop.WriteOpenCL(HLgraph, Srcwriter);
                string OpenClSource = Srcwriter.ToString();

                if (DumpCode > 2)
                    System.Console.WriteLine(OpenClSource);

                return OpenClSource;
            }
        }

        private static string getOpenCLSource(List<HighLevel.HlGraph> List)
        {
            using (StringWriter Srcwriter = new StringWriter())
            {
                foreach(HighLevel.HlGraph HLgraph in List)
                    writeOpenClStructs(HLgraph, Srcwriter);

                foreach (HighLevel.HlGraph HLgraph in List)
                    OpenCLInterop.WriteOpenCL(HLgraph, Srcwriter);

                string OpenClSource = Srcwriter.ToString();

                if (DumpCode > 2)
                    System.Console.WriteLine(OpenClSource);

                return OpenClSource;
            }
        }

        private static void WriteCode(HighLevel.HlGraph HLgraph, TextWriter writer)
        {
            writer.WriteLine("// begin {0}", HLgraph.MethodBase);

            if (HLgraph.MethodBase.IsConstructor)
            {
                writer.Write("constructor {0}::{1} (", ((System.Reflection.ConstructorInfo)HLgraph.MethodBase).DeclaringType, HLgraph.MethodBase.Name);
            }
            else
            {
                writer.Write("{0} {1}(", ((MethodInfo)HLgraph.MethodBase).ReturnType, HLgraph.MethodBase.Name);
            }

            for (int i = 0; i < HLgraph.Arguments.Count; i++)
            {
                if (i > 0)
                {
                    writer.Write(", ");
                }

                HighLevel.ArgumentLocation Argument = HLgraph.Arguments[i];
                string AttributeString = string.Empty;
                if ((Argument.Flags & HighLevel.LocationFlags.IndirectRead) != 0)
                {
                    AttributeString += "__deref_read ";
                }
                if ((Argument.Flags & HighLevel.LocationFlags.IndirectWrite) != 0)
                {
                    AttributeString += "__deref_write ";
                }

                writer.Write("{0}{1} {2}", AttributeString, Argument.DataType, Argument.Name);
            }

            writer.WriteLine(") {");

            foreach (HighLevel.LocalVariableLocation LocalVariable in HLgraph.LocalVariables)
            {
                writer.WriteLine("\t{0} {1};", LocalVariable.DataType, LocalVariable.Name);
            }

            for (int i = 0; i < HLgraph.BasicBlocks.Count; i++)
            {
                HighLevel.BasicBlock BB = HLgraph.BasicBlocks[i];

                if (BB == HLgraph.CanonicalEntryBlock || BB == HLgraph.CanonicalExitBlock)
                {
                    continue;
                }

                writer.WriteLine();
                writer.WriteLine("{0}:", BB.LabelName);
                foreach (HighLevel.Instruction Instruction in BB.Instructions)
                {
                    writer.WriteLine("\t{0}", Instruction.ToString());
                }

                if (BB.Successors.Count == 0)
                {
                    writer.WriteLine("\t// unreachable code");
                }
                else if (i + 1 == HLgraph.BasicBlocks.Count || HLgraph.BasicBlocks[i + 1] != BB.Successors[0])
                {
                    if (BB.Successors[0] == HLgraph.CanonicalExitBlock)
                    {
                        writer.WriteLine("\t// to canonical routine exit");
                    }
                    else
                    {
                        writer.WriteLine("\tgoto {0};", BB.Successors[0].LabelName);
                    }
                }
            }

            writer.WriteLine("}");
            writer.WriteLine("// end");
            writer.WriteLine();
        }
    }
}
