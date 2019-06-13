/*    
*    CilComparisonAndBranchInstruction.cs
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
using System.Reflection.Emit;
using System.Reflection;

namespace Hybrid.MsilToOpenCL.Instructions
{
    public class CilComparisonAndBranchInstruction : CilInstruction
    {
        private int m_BranchTargetOffset;

        public static CilInstruction Create(OpCode Opcode, byte[] IL, int Offset, int NextOffset, MethodBase ParentMethodBase)
        {
            int BranchTarget;
            if (Opcode.OperandType == OperandType.ShortInlineBrTarget)
            {
                BranchTarget = NextOffset + ReadInt8(IL, Offset + Opcode.Size);
            }
            else if (Opcode.OperandType == OperandType.InlineBrTarget)
            {
                BranchTarget = NextOffset + ReadInt32(IL, Offset + Opcode.Size);
            }
            else
            {
                throw new ArgumentException("Opcode \"" + Opcode.ToString() + "\" invalid for CilBinaryComparisonAndBranchInstruction.");
            }

            return new CilComparisonAndBranchInstruction(Opcode, Offset, BranchTarget);
        }

        private CilComparisonAndBranchInstruction(OpCode Opcode, int Offset, int BranchTargetOffset)
            : base(Opcode, Offset)
        {
            if (!(Opcode == OpCodes.Beq || Opcode == OpCodes.Beq_S ||
                Opcode == OpCodes.Bge || Opcode == OpCodes.Bge_S || Opcode == OpCodes.Bge_Un || Opcode == OpCodes.Bge_Un_S ||
                Opcode == OpCodes.Bgt || Opcode == OpCodes.Bgt_S || Opcode == OpCodes.Bgt_Un || Opcode == OpCodes.Bgt_Un_S ||
                Opcode == OpCodes.Ble || Opcode == OpCodes.Ble_S || Opcode == OpCodes.Ble_Un || Opcode == OpCodes.Ble_Un_S ||
                Opcode == OpCodes.Blt || Opcode == OpCodes.Blt_S || Opcode == OpCodes.Blt_Un || Opcode == OpCodes.Blt_Un_S ||
                Opcode == OpCodes.Bne_Un || Opcode == OpCodes.Bne_Un_S
                ))
            {
                throw new ArgumentException("Opcode");
            }

            m_BranchTargetOffset = BranchTargetOffset;
        }

        public override IEnumerable<int> BranchTargetOffsets
        {
            get
            {
                yield return m_BranchTargetOffset;
            }
        }

        public override string ToString()
        {
            return base.ToString() + " IL_" + m_BranchTargetOffset.ToString("X4");
        }

        public override void WriteCode(System.IO.TextWriter writer, int indent, int CurStack)
        {
            WriteInstHeader(writer, indent);
            writer.WriteLine("{0}if ({1} {2} {3}) goto {4}", IndentString(indent), StackName(CurStack - 1), GetOperatorSymbol(Opcode), StackName(CurStack), LabelName(m_BranchTargetOffset));
        }

        public override List<HighLevel.Instruction> GetHighLevel(HighLevel.HlGraph Context)
        {
            List<HighLevel.Instruction> List = new List<HighLevel.Instruction>();

            HighLevel.Node Argument;
            if (Opcode == OpCodes.Beq || Opcode == OpCodes.Beq_S)
            {
                Argument = new HighLevel.EqualsNode(Context.ReadStackLocationNode(Context.StackPointer - 1), Context.ReadStackLocationNode(Context.StackPointer));
            }
            else if (Opcode == OpCodes.Bne_Un || Opcode == OpCodes.Bne_Un_S)
            {
                Argument = new HighLevel.NotEqualsNode(Context.ReadStackLocationNode(Context.StackPointer - 1), Context.ReadStackLocationNode(Context.StackPointer));
            }
            else if (Opcode == OpCodes.Bge || Opcode == OpCodes.Bge_S || Opcode == OpCodes.Bge_Un || Opcode == OpCodes.Bge_Un_S)
            {
                Argument = new HighLevel.GreaterEqualsNode(Context.ReadStackLocationNode(Context.StackPointer - 1), Context.ReadStackLocationNode(Context.StackPointer));
            }
            else if (Opcode == OpCodes.Bgt || Opcode == OpCodes.Bgt_S || Opcode == OpCodes.Bgt_Un || Opcode == OpCodes.Bgt_Un_S)
            {
                Argument = new HighLevel.GreaterNode(Context.ReadStackLocationNode(Context.StackPointer - 1), Context.ReadStackLocationNode(Context.StackPointer));
            }
            else if (Opcode == OpCodes.Ble || Opcode == OpCodes.Ble_S || Opcode == OpCodes.Ble_Un || Opcode == OpCodes.Ble_Un_S)
            {
                Argument = new HighLevel.LessEqualsNode(Context.ReadStackLocationNode(Context.StackPointer - 1), Context.ReadStackLocationNode(Context.StackPointer));
            }
            else if (Opcode == OpCodes.Blt || Opcode == OpCodes.Blt_S || Opcode == OpCodes.Blt_Un || Opcode == OpCodes.Blt_Un_S)
            {
                Argument = new HighLevel.LessNode(Context.ReadStackLocationNode(Context.StackPointer - 1), Context.ReadStackLocationNode(Context.StackPointer));
            }
            else
            {
                throw new InvalidOperationException();
            }

            List.Add(new HighLevel.ConditionalBranchInstruction(Argument, Context.GetBlock(m_BranchTargetOffset)));
            return List;
        }

        public static string GetOperatorSymbol(OpCode Opcode)
        {
            if (Opcode == OpCodes.Beq || Opcode == OpCodes.Beq_S)
            {
                return "==";
            }
            else if (Opcode == OpCodes.Bne_Un || Opcode == OpCodes.Bne_Un_S)
            {
                return "u!=";
            }
            else if (Opcode == OpCodes.Bge || Opcode == OpCodes.Bge_S)
            {
                return "s>=";
            }
            else if (Opcode == OpCodes.Bge_Un || Opcode == OpCodes.Bge_Un_S)
            {
                return "u>=";
            }
            else if (Opcode == OpCodes.Bgt || Opcode == OpCodes.Bgt_S)
            {
                return "s>";
            }
            else if (Opcode == OpCodes.Bgt_Un || Opcode == OpCodes.Bgt_Un_S)
            {
                return "u>";
            }
            else if (Opcode == OpCodes.Ble || Opcode == OpCodes.Ble_S)
            {
                return "s<=";
            }
            else if (Opcode == OpCodes.Ble_Un || Opcode == OpCodes.Ble_Un_S)
            {
                return "u<=";
            }
            else if (Opcode == OpCodes.Blt || Opcode == OpCodes.Blt_S)
            {
                return "s<";
            }
            else if (Opcode == OpCodes.Blt_Un || Opcode == OpCodes.Blt_Un_S)
            {
                return "u<";
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
