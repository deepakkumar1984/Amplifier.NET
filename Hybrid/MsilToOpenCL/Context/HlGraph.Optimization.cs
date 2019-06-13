/*    
*    HlGraph.Optimization.cs
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


using System;
using System.Collections.Generic;
using System.Text;

namespace Hybrid.MsilToOpenCL.HighLevel
{
    public partial class HlGraph
    {
        private class LocDef
        {
            public Node Node;
            public BasicBlock BasicBlock;
            public Instruction Instruction;
            public List<Location> UsedLocations;
            public bool SelfRef;

            public LocDef(BasicBlock BasicBlock, Instruction Instruction, Location DefinedLocation, Node Node)
            {
                this.BasicBlock = BasicBlock;
                this.Instruction = Instruction;
                this.Node = Node;
                this.UsedLocations = LocationUsage.ForTree(Node, false).UsedLocations;
                this.SelfRef = this.UsedLocations.Contains(DefinedLocation);
            }
        }

        private class CpState
        {
            public Dictionary<Location, LocDef> Defs = new Dictionary<Location, LocDef>();
        }

        private class LocationUsage
        {
            public List<Location> UsedLocations = new List<Location>();
            public List<Location> DefinedLocations = new List<Location>();
            public List<Location> IndirectDefinedLocations = new List<Location>();
            public List<Location> IndirectUsedLocations = new List<Location>();
            public Dictionary<System.Reflection.MethodInfo, HlGraphEntry> RelatedGraphs;

            public static LocationUsage ForTree(Node Node, bool TopIsDef)
            {
                LocationUsage LocationUsage = new LocationUsage();
                LocationUsage.TraverseTree(Node, TopIsDef, false);
                return LocationUsage;
            }

            public static LocationUsage ForInstruction(Instruction Instruction)
            {
                return ForInstruction(Instruction, null);
            }

            public static LocationUsage ForInstruction(Instruction Instruction, Dictionary<System.Reflection.MethodInfo, HlGraphEntry> RelatedGraphs)
            {
                LocationUsage LocationUsage = new LocationUsage();
                LocationUsage.RelatedGraphs = RelatedGraphs;
                LocationUsage.TraverseTree(Instruction.Argument, false, false);
                LocationUsage.TraverseTree(Instruction.Result, true, false);
                return LocationUsage;
            }

            private void TraverseTree(Node Node, bool TopIsDef, bool ArrayDef)
            {
                if (Node == null)
                {
                    return;
                }

                if (TopIsDef)
                {
                    if (Node.NodeType == NodeType.Location)
                    {
                        Location Location = ((LocationNode)Node).Location;
                        if (!DefinedLocations.Contains(Location))
                        {
                            DefinedLocations.Add(Location);
                        }
                    }
                    else if (Node.NodeType == NodeType.ArrayAccess)
                    {
                        TraverseTree(Node, false, true);
                    }
                    else if (Node.NodeType == NodeType.AddressOf)
                    {
                        TraverseTree(((AddressOfNode)Node).SubNodes[0], true, false);
                        TraverseTree(((AddressOfNode)Node).SubNodes[0], false, false);
                    }
                    else if (Node.NodeType == NodeType.Deref)
                    {
                        TraverseTree(((DerefNode)Node).SubNodes[0], false, false);
                    }
                    else if (Node.NodeType == NodeType.InstanceField)
                    {
                        // Do nothing here. ConvertForOpenCl will convert this to an argument location,
                        // so regular analysis treats it as such at a later pass
                    }
                    else if (Node.NodeType == NodeType.NamedField)
                    {
                        TraverseTree(((NamedFieldNode)Node).SubNodes[0], true, false);
                    }
                    else
                    {
                        System.Diagnostics.Debugger.Break();
                    }
                }
                else
                {
                    if (Node.NodeType == NodeType.Location)
                    {
                        Location Location = ((LocationNode)Node).Location;
                        if (!UsedLocations.Contains(Location))
                        {
                            UsedLocations.Add(Location);
                        }
                    }
                    else
                    {
                        HlGraphEntry RelatedGraphEntry;

                        if (Node.NodeType == NodeType.ArrayAccess && Node.SubNodes.Count > 0 && Node.SubNodes[0].NodeType == NodeType.Location)
                        {
                            Location ArrayLocation = ((LocationNode)Node.SubNodes[0]).Location;

                            if (ArrayDef && !IndirectDefinedLocations.Contains(ArrayLocation))
                            {
                                IndirectDefinedLocations.Add(ArrayLocation);
                            }
                            else if (!ArrayDef && !IndirectUsedLocations.Contains(ArrayLocation))
                            {
                                IndirectUsedLocations.Add(ArrayLocation);
                            }
                        }
                        else if (Node.NodeType == NodeType.Call && Node.SubNodes.Count > 0 && !object.ReferenceEquals(RelatedGraphs, null)
                            && RelatedGraphs.TryGetValue(((CallNode)Node).MethodInfo, out RelatedGraphEntry))
                        {
                            // Call to other generated method. Propagate indirect usage flags

                            HlGraph HlGraph = RelatedGraphEntry.HlGraph;

                            for (int i = 0; i < Math.Min(Node.SubNodes.Count, HlGraph.Arguments.Count); i++)
                            {
                                ArgumentLocation Argument = HlGraph.Arguments[i];

                                if ((Argument.Flags & (LocationFlags.IndirectRead | LocationFlags.IndirectWrite)) != 0)
                                {
                                    Node SubNode = Node.SubNodes[i];
                                    if (SubNode.NodeType == NodeType.Location)
                                    {
                                        Location ArrayLocation = ((LocationNode)SubNode).Location;
                                        if ((Argument.Flags & LocationFlags.IndirectRead) != 0 && !IndirectUsedLocations.Contains(ArrayLocation))
                                        {
                                            IndirectUsedLocations.Add(ArrayLocation);
                                        }
                                        if ((Argument.Flags & LocationFlags.IndirectWrite) != 0 && !IndirectDefinedLocations.Contains(ArrayLocation))
                                        {
                                            IndirectDefinedLocations.Add(ArrayLocation);
                                        }
                                    }
                                }
                            }
                        }

                        foreach (Node SubNode in Node.SubNodes)
                        {
                            TraverseTree(SubNode, (SubNode.NodeType == NodeType.AddressOf) ? true : false, false);
                        }
                    }
                }
            }
        }

        private class CopyPropagation
        {
            /// <summary>
            /// Perform intra-block copy propagation.
            /// </summary>
            /// <param name="Graph">HlGraph.</param>
            /// <returns>TRUE if any changes have occured, FALSE otherwise.</returns>
            public static bool Do(HlGraph Graph)
            {
                bool Changed = false;
                CpState State = new CpState();
                Dictionary<Location, LocDef> InsertedDict = new Dictionary<Location, LocDef>();
                List<Location> RemoveKeys = new List<Location>();
                List<KeyValuePair<Location, LocDef>> NewDefs = new List<KeyValuePair<Location, LocDef>>();

                //
                // Get the list of undefined locations at the end of each basic block
                //

                DataFlow<LocationList>.Result DfResult = DeadAssignmentElimination.DoDF(Graph);

                //
                // Iterate through basic blocks
                //

                foreach (BasicBlock BB in Graph.BasicBlocks)
                {

                    //
                    // Iterate through instructions
                    //

                    State.Defs.Clear();
                    for (int InstructionIndex = 0; InstructionIndex < BB.Instructions.Count; InstructionIndex++)
                    {
                        Instruction Instruction = BB.Instructions[InstructionIndex];

                        //
                        // Query lists of used and defined locations for the current instruction. If it uses
                        // a location that is present in the CpState, the definition can only be pasted if:
                        //
                        // 1) The definition is not of the form "x := f(..., x, ...)"; i.e., the location
                        //    does not depend on an equally-named one at the defining site,
                        //
                        // OR
                        //
                        // 2) The definition is of the form "x := f(..., x, ...)", and the definition is dead after
                        //    the current instruction.
                        //
                        // NOTE: Pasting the definition in case (2) duplicates evaluation of "f" at the current
                        // instruction, as dead assignment elimination will not be able to remove the original
                        // defining site due to location x being used here. Therefore, in this case, we need to
                        // remove the instruction itself if the definition is pasted, but we can only do so if the
                        // value is not used afterwards.
                        //

                        LocationUsage LocationUsage = LocationUsage.ForInstruction(Instruction);

                        foreach (Location Location in LocationUsage.UsedLocations)
                        {
                            LocDef LocDef;
                            if (State.Defs.TryGetValue(Location, out LocDef) && LocDef.SelfRef)
                            {
                                LocationList DeadList = DeadAssignmentElimination.BeforeInstruction(BB, InstructionIndex + 1, DfResult.ExitState[BB]);
                                if (!DeadList.Contains(Location))
                                {
                                    State.Defs.Remove(Location);
                                }
                            }
                        }

                        //
                        // Rewrite trees
                        //

                        Instruction.Argument = ForTree(BB, Instruction, State, Instruction.Argument, false, ref Changed, InsertedDict);
                        Instruction.Result = ForTree(BB, Instruction, State, Instruction.Result, true, ref Changed, InsertedDict);

                        //
                        // Change CpState to reflect any newly defined locations
                        //

                        foreach (Location Location in LocationUsage.DefinedLocations)
                        {
                            State.Defs.Remove(Location);
                            if (Location.LocationType == LocationType.CilStack || Location.LocationType == LocationType.LocalVariable)
                            {
                                System.Diagnostics.Debug.Assert(Instruction.InstructionType == InstructionType.Assignment);

                                if (Instruction.Result != null && Instruction.Result.NodeType == NodeType.Location)
                                {
                                    LocDef NewLocDef = new LocDef(BB, Instruction, Location, Instruction.Argument);
                                    NewDefs.Add(new KeyValuePair<Location, LocDef>(Location, NewLocDef));
                                }
                            }

                            //
                            // Remove other definition entries that have been invalidated by redefining one of their inputs
                            //

                            foreach (KeyValuePair<Location, LocDef> Entry in State.Defs)
                            {
                                if (Entry.Value.UsedLocations.Contains(Location))
                                {
                                    RemoveKeys.Add(Entry.Key);
                                }
                            }
                        }

                        //
                        // Remove and add entries to CpState as necessary
                        //

                        if (RemoveKeys != null && RemoveKeys.Count != 0)
                        {
                            foreach (Location RemoveLocation in RemoveKeys)
                            {
                                State.Defs.Remove(RemoveLocation);
                            }
                            RemoveKeys.Clear();
                        }

                        if (NewDefs != null && NewDefs.Count != 0)
                        {
                            foreach (KeyValuePair<Location, LocDef> Entry in NewDefs)
                            {
                                State.Defs.Add(Entry.Key, Entry.Value);
                            }
                            NewDefs.Clear();
                        }

                        //
                        // Remove instructions at defining sites that have become obsolete. This is only
                        // true for CilStack locations, as they are the only ones guaranteed to be consumed
                        // at most once. All other obsolete instructions are taken care of by dead code
                        // elimination.
                        //
                        // BUGBUG: this fails for the DUP instruction...
                        //

                        if (InsertedDict.Count != 0 && LocationUsage.UsedLocations.Count != 0)
                        {
                            foreach (Location Location in LocationUsage.UsedLocations)
                            {
                                LocDef RemoveLocDef;
                                if (Location.LocationType == LocationType.CilStack && InsertedDict.TryGetValue(Location, out RemoveLocDef))
                                {
                                    bool OK = RemoveLocDef.BasicBlock.Instructions.Remove(RemoveLocDef.Instruction);

                                    //System.Diagnostics.Debug.Assert(OK);
                                    System.Diagnostics.Debug.Assert(object.ReferenceEquals(RemoveLocDef.BasicBlock, BB));
                                    if (object.ReferenceEquals(RemoveLocDef.BasicBlock, BB) && OK)
                                    {
                                        System.Diagnostics.Debug.Assert(object.ReferenceEquals(BB.Instructions[InstructionIndex - 1], Instruction));
                                        InstructionIndex--;
                                    }
                                }
                            }
                        }
                        InsertedDict.Clear();
                    }
                }

                return Changed;
            }

            private static Node ForTree(BasicBlock BB, Instruction Instruction, CpState State, Node Node, bool TopIsDef, ref bool Changed, Dictionary<Location, LocDef> InsertedDict)
            {
                if (Node == null)
                {
                    return null;
                }

                if (!TopIsDef && Node.NodeType == NodeType.Location)
                {
                    LocationNode LocationNode = (LocationNode)Node;
                    LocDef LocDef;

                    if (LocationNode.Location.LocationType == LocationType.CilStack && State.Defs.TryGetValue(LocationNode.Location, out LocDef))
                    {
                        InsertedDict[LocationNode.Location] = LocDef;

                        Changed = true;
                        Node = LocDef.Node;
                    }
                }
                else
                {
                    for (int i = 0; i < Node.SubNodes.Count; i++)
                    {
                        Node SubNode = Node.SubNodes[i];
                        SubNode = ForTree(BB, Instruction, State, SubNode, (SubNode.NodeType == NodeType.AddressOf) ? true : false, ref Changed, InsertedDict);
                        Node.SubNodes[i] = SubNode;
                    }
                }

                return Node;
            }
        }

        /// <summary>
        /// Dataflow analysis implementation.
        /// </summary>
        /// <typeparam name="CTX">Context type.</typeparam>
        private class DataFlow<CTX>
        {
            public class Result
            {
                private Dictionary<BasicBlock, CTX> m_EntryState = new Dictionary<BasicBlock, CTX>();
                private Dictionary<BasicBlock, CTX> m_ExitState = new Dictionary<BasicBlock, CTX>();

                public Dictionary<BasicBlock, CTX> EntryState { get { return m_EntryState; } }
                public Dictionary<BasicBlock, CTX> ExitState { get { return m_ExitState; } }
            }

            public delegate void InitFn(BasicBlock Node, out CTX Entry, out CTX Exit);
            public delegate CTX NodeFn(BasicBlock Node, CTX Value);
            public delegate CTX EdgeFn(BasicBlock Source, BasicBlock Target, CTX Value);
            public delegate void MergeFn(ref CTX Left, CTX Right);

            public static Result Reverse(List<BasicBlock> BasicBlockList, InitFn InitFn, NodeFn NodeFn, EdgeFn EdgeFn, MergeFn MergeFn)
            {
                Result Result = new Result();
                foreach (BasicBlock BasicBlock in BasicBlockList)
                {
                    CTX Entry, Exit;
                    InitFn(BasicBlock, out Entry, out Exit);
                    Result.EntryState.Add(BasicBlock, Entry);
                    Result.ExitState.Add(BasicBlock, Exit);
                }

                bool Changed;

                do
                {
                    Changed = false;
                    foreach (BasicBlock BasicBlock in BasicBlockList)
                    {
                        CTX Ctx = default(CTX);
                        foreach (BasicBlock SuccessorBlock in BasicBlock.Successors)
                        {
                            MergeFn(ref Ctx, EdgeFn(BasicBlock, SuccessorBlock, Result.EntryState[SuccessorBlock]));
                        }

                        if (!object.Equals(Ctx, Result.ExitState[BasicBlock]))
                        {
                            Result.ExitState[BasicBlock] = Ctx;

                            CTX NewEntryCtx = NodeFn(BasicBlock, Ctx);

                            if (!object.Equals(NewEntryCtx, Result.EntryState[BasicBlock]))
                            {
                                Result.EntryState[BasicBlock] = NewEntryCtx;
                                Changed = true;
                            }
                        }
                    }
                } while (Changed);

                return Result;
            }
        }

        private class LocationList : SortedList<Location, Location>, IList<Location>
        {

            private class LocationComparer : IComparer<Location>
            {
                public static readonly LocationComparer Instance = new LocationComparer();

                #region IComparer<Location> Members

                public int Compare(Location x, Location y)
                {
                    int result = x.LocationType.CompareTo(y.LocationType);
                    if (result != 0)
                    {
                        return result;
                    }

                    return x.CompareToLocation(y);
                }

                #endregion
            }

            public LocationList()
                : base(LocationComparer.Instance)
            {
            }

            public LocationList(LocationList ex)
                : base(ex, LocationComparer.Instance)
            {
            }

            #region IList<Location> Members

            public int IndexOf(Location item)
            {
                return base.IndexOfKey(item);
            }

            public void Insert(int index, Location item)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public Location this[int index]
            {
                get
                {
                    return base.Keys[index];
                }
                set
                {
                    throw new Exception("The method or operation is not implemented.");
                }
            }

            #endregion

            #region ICollection<Location> Members

            public void Add(Location item)
            {
                if (!this.ContainsKey(item))
                {
                    Add(item, item);
                }
            }

            public bool Contains(Location item)
            {
                return ContainsKey(item);
            }

            public void CopyTo(Location[] array, int arrayIndex)
            {
                base.Keys.CopyTo(array, arrayIndex);
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            #endregion

            #region IEnumerable<Location> Members

            public new IEnumerator<Location> GetEnumerator()
            {
                return base.Keys.GetEnumerator();
            }

            #endregion

            public override int GetHashCode()
            {
                int Result = 0;
                foreach (Location Entry in this)
                {
                    Result += Entry.GetHashCode();
                }
                return Result;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is LocationList)) { return false; } else if (object.ReferenceEquals(obj, this)) { return true; }
                LocationList Other = (LocationList)obj;

                if (this.Count != Other.Count)
                {
                    return false;
                }
                foreach (Location Entry in this)
                {
                    if (!Other.Contains(Entry))
                    {
                        return false;
                    }
                }
                return true;
            }

            public static void MergeInlineAnd(ref LocationList Left, LocationList Right)
            {
                if (object.ReferenceEquals(Left, null))
                {
                    if (!object.ReferenceEquals(Right, null))
                    {
                        Left = new LocationList(Right);
                    }
                    return;
                }
                else if (object.ReferenceEquals(Right, null))
                {
                    return;
                }

                List<Location> RemoveKeys = null;
                foreach (Location Entry in Left)
                {
                    if (!Right.Contains(Entry))
                    {
                        if (RemoveKeys == null)
                        {
                            RemoveKeys = new List<Location>();
                        }
                        RemoveKeys.Add(Entry);
                    }
                }
                if (RemoveKeys != null)
                {
                    foreach (Location Entry in RemoveKeys)
                    {
                        Left.Remove(Entry);
                    }
                }
            }
        }

        /// <summary>
        /// Dead Assignment Elimination.
        /// </summary>
        private class DeadAssignmentElimination
        {
            /// <summary>
            /// Perform data-flow analysis to get dead definitions for each basic block.
            /// </summary>
            /// <param name="Graph">HlGraph.</param>
            /// <returns>Data flow results.</returns>
            /// <remarks>Dead definitions is an all-paths reverse data flow problem described by the
            /// following equations:
            /// 
            /// <code>
            /// InitFn(n)       := ( (n == CanonicalExit) ? ALL_STACK | ALL_LOCAL | ALL_ARG : TOP, TOP )
            /// NodeFn(n, l)    := l + n.DAD - n.UEU
            /// EdgeFn(s, t, l) := l
            /// MergeFn(l, r)   := (l &amp; r)
            /// </code>
            /// </remarks>
            public static DataFlow<LocationList>.Result DoDF(HlGraph Graph)
            {
                DataFlow<LocationList>.Result DfResult = DataFlow<LocationList>.Reverse(
                    Graph.BasicBlocks,
                    delegate(BasicBlock BasicBlock, out LocationList Entry, out LocationList Exit)
                    {
                        if (Graph.CanonicalExitBlock == BasicBlock)
                        {
                            Entry = new LocationList();
                            for (int i = 0; i < Graph.m_MaxStack; i++)
                            {
                                Entry.Add(new StackLocation(i));
                            }
                            foreach (LocalVariableLocation LocVar in Graph.LocalVariables)
                            {
                                Entry.Add(LocVar);
                            }
                            foreach (ArgumentLocation Arg in Graph.Arguments)
                            {
                                Entry.Add(Arg);
                            }
                            Exit = null;
                        }
                        else
                        {
                            Entry = Exit = null;
                        }
                    },
                    delegate(BasicBlock BasicBlock, LocationList DeadDefinitionList)
                    {
                        return BeforeInstruction(BasicBlock, 0, DeadDefinitionList);
                    },
                    delegate(BasicBlock Source, BasicBlock Target, LocationList DeadDefinitionList)
                    {
                        return DeadDefinitionList;
                    },
                    LocationList.MergeInlineAnd
                );
                return DfResult;
            }

            /// <summary>
            /// Get list of dead definitions at the point right before a specified instruction.
            /// </summary>
            /// <param name="BasicBlock">Basic block.</param>
            /// <param name="Index">Index of target instruction.</param>
            /// <param name="DeadDefinitionList">Optional. List of dead definitions at the end of the basic block.</param>
            /// <returns>List of dead definitions before instruction with specified <paramref name="Index"/>.</returns>
            public static LocationList BeforeInstruction(BasicBlock BasicBlock, int Index, LocationList DeadDefinitionList)
            {
                if (Index < 0)
                {
                    throw new ArgumentOutOfRangeException("Index");
                }

                if (DeadDefinitionList == null)
                {
                    DeadDefinitionList = new LocationList();
                }
                else
                {
                    DeadDefinitionList = new LocationList(DeadDefinitionList);
                }

                for (int i = BasicBlock.Instructions.Count - 1; i >= Index; i--)
                {
                    Instruction Instruction = BasicBlock.Instructions[i];

                    LocationUsage LocationUsage = LocationUsage.ForInstruction(Instruction);
                    foreach (Location Location in LocationUsage.DefinedLocations)
                    {
                        if (Location.LocationType == LocationType.CilStack || Location.LocationType == LocationType.LocalVariable || Location.LocationType == LocationType.Argument)
                        {
                            DeadDefinitionList.Add(Location);
                        }
                    }
                    foreach (Location Location in LocationUsage.UsedLocations)
                    {
                        if (Location.LocationType == LocationType.CilStack || Location.LocationType == LocationType.LocalVariable || Location.LocationType == LocationType.Argument)
                        {
                            DeadDefinitionList.Remove(Location);
                        }
                    }
                }
                return DeadDefinitionList;
            }

            public static bool Do(HlGraph Graph)
            {
                DataFlow<LocationList>.Result DfResult = DoDF(Graph);

                List<Location> DeadDefinitionList = new List<Location>();

                bool Changed = false;
                foreach (BasicBlock BasicBlock in Graph.BasicBlocks)
                {
                    DeadDefinitionList.Clear();

                    LocationList List = DfResult.ExitState[BasicBlock];
                    if (List == null)
                    {
                        System.Diagnostics.Debug.Assert(BasicBlock == Graph.CanonicalExitBlock);
                    }
                    else
                    {
                        DeadDefinitionList.AddRange(List);
                    }

                    Changed |= ForBlock(BasicBlock, ref DeadDefinitionList);
                }
                return Changed;
            }

            private static bool ForBlock(BasicBlock BasicBlock, ref List<Location> DeadDefinitionList)
            {
                if (DeadDefinitionList == null)
                {
                    DeadDefinitionList = new List<Location>();
                }

                bool InstructionsDeleted = false;

                for (int i = BasicBlock.Instructions.Count - 1; i >= 0; i--)
                {
                    Instruction Instruction = BasicBlock.Instructions[i];

                    bool IsDeadAssignment = false;

                    if (Instruction.InstructionType == InstructionType.Assignment &&
                        Instruction.Result != null && Instruction.Result.NodeType == NodeType.Location)
                    {
                        Location Location = ((LocationNode)Instruction.Result).Location;

                        // Work around default equality check for stack locations. We only
                        // care about the stack index, not the data type here...
                        if (Location.LocationType == LocationType.CilStack)
                        {
                            foreach (Location Other in DeadDefinitionList)
                            {
                                if (Other.LocationType == LocationType.CilStack && ((StackLocation)Other).Index == ((StackLocation)Location).Index)
                                {
                                    IsDeadAssignment = true;
                                    break;
                                }
                            }
                        }
                        else if (DeadDefinitionList.Contains(Location))
                        {
                            IsDeadAssignment = true;
                        }
                    }

                    // TODO: check for side effects...
                    if (IsDeadAssignment && HasSideEffects(Instruction.Argument))
                    {
                        Instruction.Result = null;
                    }
                    else if (IsDeadAssignment)
                    {
                        BasicBlock.Instructions.RemoveAt(i);
                        InstructionsDeleted = true;
                        continue;
                    }

                    LocationUsage LocationUsage = LocationUsage.ForInstruction(Instruction);

                    foreach (Location Location in LocationUsage.DefinedLocations)
                    {
                        if (!DeadDefinitionList.Contains(Location))
                        {
                            DeadDefinitionList.Add(Location);
                        }
                    }
                    foreach (Location Location in LocationUsage.UsedLocations)
                    {
                        DeadDefinitionList.Remove(Location);
                    }
                }

                return InstructionsDeleted;
            }
        }

        private static bool HasSideEffects(Node Node)
        {
            if (Node == null)
                return false;
            else if (Node.NodeType == NodeType.Call)
                return true;

            foreach (Node SubNode in Node.SubNodes)
            {
                if (HasSideEffects(SubNode))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Perform basic optimizations on high-level IL representation.
        /// </summary>
        public void Optimize()
        {
            bool Changed;
            do
            {
                Changed = CopyPropagation.Do(this);
                Changed |= DeadAssignmentElimination.Do(this);
            } while (Changed);
        }
    }
}
