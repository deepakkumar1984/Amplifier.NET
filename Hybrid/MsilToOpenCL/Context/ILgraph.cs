/*    
*    ILgraph.cs
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

namespace Hybrid.MsilToOpenCL.HighLevel
{
    public class ILgraph
    {
        public List<CilInstruction> ILinstList = new List<CilInstruction>();
        public List<ILBB> BBlist = new List<ILBB>();
        public List<int> targetOffsets = new List<int>();
        public ILBB BBentry;
        public ILBB BBexit;
        public ILBB BBstart;

        public CilInstruction InstructionForOffset(int offset)
        {
            foreach (CilInstruction inst in ILinstList)
            {
                if (inst.Offset == offset)
                {
                    return inst;
                }
            }
            return null;
        }

        public ILBB BBForEntryOffset(int offset)
        {
            foreach (ILBB bb in BBlist)
            {
                if (bb.offset == offset)
                {
                    return bb;
                }
            }
            return null;
        }

        public static ILgraph FromILbyteArray(System.Reflection.MethodBase MethodBase)
        {
            byte[] IL = MethodBase.GetMethodBody().GetILAsByteArray();

            ILgraph newGraph = new ILgraph();
            newGraph.targetOffsets.Add(0);

            //
            // Read IL instruction stream
            //

            for (int nextOffset, offset = 0; offset < IL.Length; offset = nextOffset)
            {
                CilInstruction newInst = CilInstructionReader.Read(IL, offset, out nextOffset, MethodBase);

                newGraph.targetOffsets.AddRange(newInst.BranchTargetOffsets);

                newGraph.ILinstList.Add(newInst);
            }

            // Verify that all jump targets are on instruction boundaries
            foreach (int offset in newGraph.targetOffsets)
            {
                if (newGraph.InstructionForOffset(offset) == null)
                {
                    throw new InvalidOperationException(string.Format("A jump instruction targets IL offset {0:X}, but it is not an instruction boundary.", offset));
                }
            }

            //
            // Convert instruction stream into basic blocks
            //

            {
                ILBB BB = null, FallThroughSource;
                newGraph.BBlist.Add(newGraph.BBentry = FallThroughSource = new ILBB());
                newGraph.BBlist.Add(newGraph.BBexit = new ILBB());
                newGraph.BBlist.Add(newGraph.BBstart = FallThroughSource.FallThroughTarget = new ILBB());

                FallThroughSource = newGraph.BBstart;
                FallThroughSource.stackCountOnEntry = 0;

                foreach (CilInstruction inst in newGraph.ILinstList)
                {
                    if (BB == null || newGraph.targetOffsets.Contains(inst.Offset))
                    {
                        ILBB nextBB = new ILBB();
                        newGraph.BBlist.Add(nextBB);
                        nextBB.offset = inst.Offset;

                        if (newGraph.BBstart == null)
                        {
                            newGraph.BBstart = nextBB;
                            nextBB.stackCountOnEntry = 0;
                        }

                        if (!newGraph.targetOffsets.Contains(nextBB.offset))
                        {
                            newGraph.targetOffsets.Add(nextBB.offset);
                        }

                        if (FallThroughSource != null)
                        {
                            FallThroughSource.FallThroughTarget = nextBB;
                        }

                        BB = nextBB;
                    }

                    BB.list.Add(inst);

                    switch (inst.Opcode.FlowControl)
                    {
                        default:
                        case System.Reflection.Emit.FlowControl.Meta:
                        case System.Reflection.Emit.FlowControl.Next:
                            FallThroughSource = BB;
                            break;

                        case System.Reflection.Emit.FlowControl.Cond_Branch:
                            BB.FinalTransfer = inst;
                            FallThroughSource = BB;
                            BB = null;
                            break;

                        case System.Reflection.Emit.FlowControl.Branch:
                            BB.FinalTransfer = inst;
                            FallThroughSource = null;
                            BB = null;
                            break;

                        case System.Reflection.Emit.FlowControl.Throw:
                            BB.FinalTransfer = inst;
                            FallThroughSource = null;
                            BB = null;
                            break;

                        case System.Reflection.Emit.FlowControl.Return:
                            BB.FinalTransfer = inst;
                            BB.FallThroughTarget = newGraph.BBexit;
                            FallThroughSource = null;
                            BB = null;
                            break;
                    }
                }
            }

            System.Diagnostics.Debug.Assert(newGraph.BBlist[0].offset == 0);

            //
            // Check stack pointer
            //

            bool Changed;
            do
            {
                Changed = false;
                foreach (ILBB BB in newGraph.BBlist)
                {
                    if (!BB.stackCountOnEntry.HasValue || BB.stackCountOnExit.HasValue)
                    {
                        continue;
                    }

                    int CurStack = BB.stackCountOnEntry.Value;
                    for (int i = 0; i < BB.list.Count; i++)
                    {
                        CilInstruction inst = BB.list[i];

                        int consume = inst.StackConsumeCount;
                        int produce = inst.StackProduceCount;

                        if (consume > CurStack)
                        {
                            throw new InvalidOperationException(string.Format("BB[{0:X}]INST[{1}] @ IL offset {2:X}: stack overconsumption. avail = {3}, consume = {4}", BB.offset, i, inst.Offset, CurStack, consume));
                        }
                        CurStack = CurStack - consume + produce;
                    }
                    BB.stackCountOnExit = CurStack;

                    if (BB.FallThroughTarget != null)
                    {
                        if (!BB.FallThroughTarget.stackCountOnEntry.HasValue)
                        {
                            BB.FallThroughTarget.stackCountOnEntry = CurStack;
                            Changed = true;
                        }
                        else if (BB.FallThroughTarget.stackCountOnEntry.Value != CurStack)
                        {
                            throw new InvalidOperationException(string.Format("BB[{0:X}]: invalid stack count merge. new = {1}, old = {2}", BB.FallThroughTarget.offset, CurStack, BB.FallThroughTarget.stackCountOnEntry.Value));
                        }
                    }

                    if (BB.FinalTransfer != null)
                    {
                        foreach (int targetOffset in BB.FinalTransfer.BranchTargetOffsets)
                        {
                            ILBB targetBB = newGraph.BBForEntryOffset(targetOffset);
                            System.Diagnostics.Debug.Assert(targetBB != null);

                            if (!targetBB.stackCountOnEntry.HasValue)
                            {
                                targetBB.stackCountOnEntry = CurStack;
                                Changed = true;
                            }
                            else if (targetBB.stackCountOnEntry.Value != CurStack)
                            {
                                throw new InvalidOperationException(string.Format("BB[{0:X}]: invalid stack count merge. new = {1}, old = {2}", targetBB.offset, CurStack, targetBB.stackCountOnEntry.Value));
                            }
                        }
                    }
                }
            } while (Changed);

            return newGraph;
        }
    }
}
