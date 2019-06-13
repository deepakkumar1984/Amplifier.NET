/*    
*    CilBinaryComparisonInstruction.cs
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
    public class CilBinaryComparisonInstruction : CilInstruction
    {
        public static CilInstruction Create(OpCode Opcode, byte[] IL, int Offset, int NextOffset, MethodBase ParentMethodBase)
        {
            return new CilBinaryComparisonInstruction(Opcode, Offset);
        }

        private CilBinaryComparisonInstruction(OpCode Opcode, int Offset)
            : base(Opcode, Offset)
        {
            if (!(Opcode == OpCodes.Ceq || Opcode == OpCodes.Cgt || Opcode == OpCodes.Cgt_Un || Opcode == OpCodes.Clt || Opcode == OpCodes.Clt_Un))
            {
                throw new ArgumentException("Opcode");
            }
        }

        public override void WriteCode(System.IO.TextWriter writer, int indent, int CurStack)
        {
            WriteInstHeader(writer, indent);
            writer.WriteLine("{0}{1} = ({2} {3} {4}) ? 1 : 0", IndentString(indent), StackName(CurStack - 1), StackName(CurStack - 1), GetOperatorSymbol(Opcode), StackName(CurStack));
        }

        public static string GetOperatorSymbol(OpCode Opcode)
        {
            if (Opcode == OpCodes.Ceq)
            {
                return "==";
            }
            else if (Opcode == OpCodes.Cgt)
            {
                return "s>";
            }
            else if (Opcode == OpCodes.Cgt_Un)
            {
                return "u>";
            }
            else if (Opcode == OpCodes.Clt)
            {
                return "s<";
            }
            else if (Opcode == OpCodes.Clt_Un)
            {
                return "u<";
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public override List<HighLevel.Instruction> GetHighLevel(HighLevel.HlGraph Context)
        {
            List<HighLevel.Instruction> List = new List<HighLevel.Instruction>();

            HighLevel.Node Argument;
            if (Opcode == OpCodes.Ceq)
            {
                Argument = new HighLevel.EqualsNode(Context.ReadStackLocationNode(Context.StackPointer - 1), Context.ReadStackLocationNode(Context.StackPointer));
            }
            else if (Opcode == OpCodes.Cgt || Opcode == OpCodes.Cgt_Un)
            {
                Argument = new HighLevel.GreaterNode(Context.ReadStackLocationNode(Context.StackPointer - 1), Context.ReadStackLocationNode(Context.StackPointer));
            }
            else if (Opcode == OpCodes.Clt || Opcode == OpCodes.Clt_Un)
            {
                Argument = new HighLevel.LessNode(Context.ReadStackLocationNode(Context.StackPointer - 1), Context.ReadStackLocationNode(Context.StackPointer));
            }
            else
            {
                throw new InvalidOperationException();
            }

            List.Add(new HighLevel.AssignmentInstruction(Context.DefineStackLocationNode(Context.StackPointer - 1, typeof(int)), Argument));
            return List;
        }
    }
}
