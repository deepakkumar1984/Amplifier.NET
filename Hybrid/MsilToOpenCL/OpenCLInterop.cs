/*    
*    OpenCLInterop.cs
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


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Hybrid.MsilToOpenCL
{
    internal class OpenCLInterop
    {
        internal static void CallOpenCLNet(int[] WorkSize, HlGraphEntry CacheEntry, InvokeContext ctx, HighLevel.HlGraph HLgraph, OpenCLNet.Device device)
        {
            // We can invoke the kernel using the arguments from ctx now :)
            if (device == null)
            {
                device = GetFirstGpu();
                if (device == null)
                {
                    device = GetFirstCpu();
                }
                if (device == null)
                {
                    throw new ArgumentNullException("device");
                }
            }

            OpenCLNet.Platform Platform = device.Platform;

            OpenCLNet.Context context;
            OpenCLNet.Program program;

            lock (CacheEntry)
            {
                context = CacheEntry.Context;
                if (context == null)
                {
                    IntPtr[] properties = new IntPtr[]
                    {
                        new IntPtr((long)OpenCLNet.ContextProperties.PLATFORM), Platform.PlatformID,
                        IntPtr.Zero,
                    };
                    context = CacheEntry.Context = Platform.CreateContext(properties, new OpenCLNet.Device[] { device }, null, IntPtr.Zero);
                }

                program = CacheEntry.Program;
                if (program == null)
                {
                    StringBuilder source = new StringBuilder();
                    source.Append(GetOpenCLSourceHeader(Platform, device, CacheEntry));
                    source.Append(CacheEntry.Source);
                    source.Append(GetOpenCLSourceFooter(Platform, device));

                    program = context.CreateProgramWithSource(source.ToString());

                    try
                    {
                        program.Build();
                    }
                    catch (Exception ex)
                    {
                        string err = program.GetBuildLog(device);
                        throw new Exception(err, ex);
                    }

                    CacheEntry.Program = program;
                }
            }

            using (CallContext CallContext = new CallContext(context, device, OpenCLNet.CommandQueueProperties.PROFILING_ENABLE, program.CreateKernel(HLgraph.MethodName)))
            {
                OpenCLNet.CommandQueue commandQueue = CallContext.CommandQueue;

                for (int i = 0; i < ctx.Arguments.Count; i++)
                {
                    ctx.Arguments[i].WriteToKernel(CallContext, i);
                }

                //OpenCLNet.Event StartEvent, EndEvent;

                //commandQueue.EnqueueMarker(out StartEvent);

                IntPtr[] GlobalWorkSize = new IntPtr[WorkSize.Length];
                for (int i = 0; i < WorkSize.Length; i++)
                {
                    GlobalWorkSize[i] = new IntPtr(WorkSize[i]);
                }
                commandQueue.EnqueueNDRangeKernel(CallContext.Kernel, (uint)GlobalWorkSize.Length, null, GlobalWorkSize, null);

                for (int i = 0; i < ctx.Arguments.Count; i++)
                {
                    ctx.Arguments[i].ReadFromKernel(CallContext, i);
                }

                commandQueue.Finish();
                //commandQueue.EnqueueMarker(out EndEvent);
                //commandQueue.Finish();

                //ulong StartTime, EndTime;
                //StartEvent.GetEventProfilingInfo(OpenCLNet.ProfilingInfo.QUEUED, out StartTime);
                //EndEvent.GetEventProfilingInfo(OpenCLNet.ProfilingInfo.END, out EndTime);
            }
        }

        public static OpenCLNet.Device GetFirstGpu()
        {
            if (OpenCLNet.OpenCL.NumberOfPlatforms == 0)
                return null;

            foreach (OpenCLNet.Platform platform in OpenCLNet.OpenCL.GetPlatforms())
                foreach (OpenCLNet.Device device in platform.QueryDevices(OpenCLNet.DeviceType.GPU))
                    return device;

            return null;
        }

		internal static OpenCLNet.Device GetFirstCpu() {
			if (OpenCLNet.OpenCL.NumberOfPlatforms == 0)
				return null;

			foreach (OpenCLNet.Platform platform in OpenCLNet.OpenCL.GetPlatforms())
				foreach (OpenCLNet.Device device in platform.QueryDevices(OpenCLNet.DeviceType.CPU))
					return device;

			return null;
		}

        private static string GetOpenCLSourceHeader(OpenCLNet.Platform platform, OpenCLNet.Device device, HlGraphEntry KernelGraphEntry)
        {
            StringBuilder result = new System.Text.StringBuilder();

            result.AppendLine("// BEGIN GENERATED OpenCL");

            setExtensionIfAvailable(result, device, "cl_amd_fp64");
            setExtensionIfAvailable(result, device, "cl_khr_fp64");

            setExtensionIfAvailable(result, device, "cl_khr_global_int32_base_atomics");
            setExtensionIfAvailable(result, device, "cl_khr_global_int32_extended_atomics");
            setExtensionIfAvailable(result, device, "cl_khr_local_int32_base_atomics");
            setExtensionIfAvailable(result, device, "cl_khr_local_int32_extended_atomics");

            if (KernelGraphEntry.HlGraph.RandomStateLocation != null)
            {
                result.AppendLine();
                result.AppendLine("// Source: http://www.doc.ic.ac.uk/~dt10/research/rngs-gpu-mwc64x.html");
                result.AppendLine("uint MWC64X(uint2 *state)");
                result.AppendLine("{");
                result.AppendLine("    enum{ A=4294883355U };");
                result.AppendLine("    uint x=(*state).x, c=(*state).y;  // Unpack the state");
                result.AppendLine("    uint res=x^c;                     // Calculate the result");
                result.AppendLine("    uint hi=mul_hi(x,A);              // Step the RNG");
                result.AppendLine("    x=x*A+c;");
                result.AppendLine("    c=hi+(x<c);");
                result.AppendLine("    *state=(uint2)(x,c);              // Pack the state back up");
                result.AppendLine("    return res;                       // Return the next result");
                result.AppendLine("}");
            }

            return result.ToString();
        }

        private static string GetOpenCLSourceFooter(OpenCLNet.Platform platform, OpenCLNet.Device device)
        {
            StringBuilder result = new System.Text.StringBuilder();

            result.AppendLine();
            result.AppendLine("// END GENERATED OpenCL");

            return result.ToString();
        }

        private static void setExtensionIfAvailable(StringBuilder result, OpenCLNet.Device device, string extension)
        {
            if (device.HasExtension(extension))
                result.AppendLine("#pragma OPENCL EXTENSION " + extension + " : enable");
        }

        internal static void WriteOpenCL(HighLevel.HlGraph HLgraph, Type StructType, TextWriter writer)
        {
            if (StructType == null)
            {
                throw new ArgumentNullException("StructType");
            }
            else if (!StructType.IsValueType)
            {
                throw new ArgumentException(string.Format("Unable to generate OpenCL code for non-ValueType '{0}'", StructType.FullName));
            }

            writer.WriteLine();
            writer.WriteLine("// OpenCL structure definition for type '{0}'", StructType.FullName);
            writer.WriteLine("typedef {0} {{", GetOpenClType(HLgraph, StructType));
            FieldInfo[] Fields = StructType.GetFields();
            foreach (FieldInfo Field in Fields)
            {
                writer.WriteLine("\t{0} {1};", GetOpenClType(HLgraph, Field.FieldType), Field.Name);
            }
            writer.WriteLine("}} t_{0};", HLgraph.ValueTypeMap[StructType]);
        }

        internal static void WriteOpenCL(HighLevel.HlGraph HLgraph, TextWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("// OpenCL source for {2} method '{0}' of type '{1}'", HLgraph.MethodBase.ToString(), HLgraph.MethodBase.DeclaringType.ToString(), HLgraph.IsKernel ? "kernel" : "related");
            writer.WriteLine("{2}{0} {1}(", GetOpenClType(HLgraph, ((MethodInfo)HLgraph.MethodBase).ReturnType), HLgraph.MethodName, HLgraph.IsKernel ? "__kernel " : string.Empty);
            for (int i = 0; i < HLgraph.Arguments.Count; i++)
            {
                HighLevel.ArgumentLocation Argument = HLgraph.Arguments[i];

                string AttributeString = string.Empty;
                if ((Argument.Flags & HighLevel.LocationFlags.IndirectRead) != 0)
                {
                    AttributeString += "/*[in";
                }
                if ((Argument.Flags & HighLevel.LocationFlags.IndirectWrite) != 0)
                {
                    if (AttributeString == string.Empty)
                    {
                        AttributeString += "/*[out";
                    }
                    else
                    {
                        AttributeString += ",out";
                    }
                }

                if (AttributeString != string.Empty) { AttributeString += "]*/ "; }

                if ((Argument.DataType.IsArray || Argument.DataType.IsPointer || Argument.DataType.IsByRef)
                    && ((Argument.Flags & Hybrid.MsilToOpenCL.HighLevel.LocationFlags.PointerLocal) == 0))
                {
                    AttributeString += "__global ";
                }

                writer.WriteLine("\t{0}{1} {2}{3}", AttributeString, GetOpenClType(HLgraph, Argument.DataType), Argument.Name, i + 1 < HLgraph.Arguments.Count ? "," : string.Empty);
            }
            writer.WriteLine(")");
            writer.WriteLine("/*");
            writer.WriteLine("  Generated by CIL2OpenCL");
            writer.WriteLine("*/");
            writer.WriteLine("{");

            foreach (HighLevel.LocalVariableLocation LocalVariable in HLgraph.LocalVariables)
            {
                string AttributeString = string.Empty;
                if ((LocalVariable.Flags & HighLevel.LocationFlags.Read) != 0)
                {
                    if (AttributeString == string.Empty) { AttributeString += "/*["; } else { AttributeString += ","; }
                    AttributeString += "read";
                }
                if ((LocalVariable.Flags & HighLevel.LocationFlags.Write) != 0)
                {
                    if (AttributeString == string.Empty) { AttributeString += "/*["; } else { AttributeString += ","; }
                    AttributeString += "write";
                }
                if ((LocalVariable.Flags & HighLevel.LocationFlags.IndirectRead) != 0)
                {
                    if (AttributeString == string.Empty) { AttributeString += "/*["; } else { AttributeString += ","; }
                    AttributeString += "deref_read";
                }
                if ((LocalVariable.Flags & HighLevel.LocationFlags.IndirectWrite) != 0)
                {
                    if (AttributeString == string.Empty) { AttributeString += "/*["; } else { AttributeString += ","; }
                    AttributeString += "deref_write";
                }
                if (AttributeString == string.Empty) { AttributeString = "/*UNUSED*/ // "; } else { AttributeString += "]*/ "; }
                if ((LocalVariable.Flags & Hybrid.MsilToOpenCL.HighLevel.LocationFlags.PointerGlobal) != 0)
                {
                    AttributeString += "__global ";
                }

                writer.WriteLine("\t{0}{1} {2};", AttributeString, GetOpenClType(HLgraph, LocalVariable.DataType), LocalVariable.Name);
            }

            HighLevel.BasicBlock FallThroughTargetBlock = HLgraph.CanonicalStartBlock;
            for (int i = 0; i < HLgraph.BasicBlocks.Count; i++)
            {
                HighLevel.BasicBlock BB = HLgraph.BasicBlocks[i];

                if (BB == HLgraph.CanonicalEntryBlock || BB == HLgraph.CanonicalExitBlock)
                {
                    continue;
                }

                if (FallThroughTargetBlock != null && FallThroughTargetBlock != BB)
                {
                    writer.WriteLine("\tgoto {0};", FallThroughTargetBlock.LabelName);
                }

                FallThroughTargetBlock = null;

                writer.WriteLine();
                if (BB.LabelNameUsed)
                {
                    writer.WriteLine("{0}:", BB.LabelName);
                }
                else
                {
                    writer.WriteLine("//{0}: (unreferenced block label)", BB.LabelName);
                }

                foreach (HighLevel.Instruction Instruction in BB.Instructions)
                {
                    writer.WriteLine("\t{0}", Instruction.ToString());
                }

                if (BB.Successors.Count == 0)
                {
                    writer.WriteLine("\t// End of block is unreachable");
                }
                else if (BB.Successors[0] == HLgraph.CanonicalExitBlock)
                {
                    writer.WriteLine("\t// End of block is unreachable/canonical routine exit");
                }
                else
                {
                    FallThroughTargetBlock = BB.Successors[0];
                }
            }

            writer.WriteLine("}");
        }

        public static string GetOpenClType(HighLevel.HlGraph HlGraph, Type DataType)
        {
            return InnerGetOpenClType(HlGraph, DataType);
        }

        public static readonly Type RandomStateDataType = ConstructRandomStateDataType();

        private static Type ConstructRandomStateDataType()
        {
            return typeof(rnd_state);
        }

        private class rnd_state
        {
        }

        private static string InnerGetOpenClType(HighLevel.HlGraph HlGraph, Type DataType)
        {
            if (DataType == typeof(void))
            {
                return "void";
            }
            else if (DataType == typeof(sbyte))
            {
                return "char";
            }
            else if (DataType == typeof(byte))
            {
                return "uchar";
            }
            else if (DataType == typeof(short))
            {
                return "short";
            }
            else if (DataType == typeof(ushort))
            {
                return "ushort";
            }
            else if (DataType == typeof(int) || DataType == typeof(IntPtr) || DataType == typeof(bool))
            {
                return "int";
            }
            else if (DataType == typeof(uint) || DataType == typeof(UIntPtr))
            {
                return "uint";
            }
            else if (DataType == typeof(long))
            {
                return "long";
            }
            else if (DataType == typeof(ulong))
            {
                return "ulong";
            }
            else if (DataType == typeof(float))
            {
                return "float";
            }
            else if (DataType == typeof(double))
            {
                return "double";
            }
            else if (DataType.IsByRef)
            {
                return InnerGetOpenClType(HlGraph, DataType.GetElementType()) + "*";
            }
            else if (DataType.IsArray)
            {
                return InnerGetOpenClType(HlGraph, DataType.GetElementType()) + "*";
            }
            else if (object.ReferenceEquals(DataType, RandomStateDataType))
            {
                return "uint2";
            }
            else if (DataType.IsValueType && DataType.BaseType == typeof(System.ValueType) && HlGraph != null && HlGraph.ValueTypeMap != null)
            {
                string Name;
                if (!HlGraph.ValueTypeMap.TryGetValue(DataType, out Name))
                {
                    HlGraph.ValueTypeMap[DataType] = Name = "genstruct_" + HlGraph.ValueTypeMap.Count;
                }
                return "struct " + Name;
            }
            else
            {
                throw new ArgumentException(string.Format("Sorry, data type '{0}' cannot be mapped to OpenCL.", DataType));
            }
        }
    }
}
