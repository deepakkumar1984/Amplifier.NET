/*    
*    CilBinaryNumericInstruction.cs
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
    public class CilBinaryNumericInstruction : CilInstruction
    {
        public static CilInstruction Create(OpCode Opcode, byte[] IL, int Offset, int NextOffset, MethodBase ParentMethodBase)
        {
            return new CilBinaryNumericInstruction(Opcode, Offset);
        }

        private CilBinaryNumericInstruction(OpCode Opcode, int Offset)
            : base(Opcode, Offset)
        {
            if (!(Opcode == OpCodes.Add || Opcode == OpCodes.Sub || Opcode == OpCodes.Mul || Opcode == OpCodes.Div || Opcode == OpCodes.Rem))
            {
                throw new ArgumentException("Opcode");
            }
        }

        public override void WriteCode(System.IO.TextWriter writer, int indent, int CurStack)
        {
            WriteInstHeader(writer, indent);
            writer.WriteLine("{0}{1} = {2} {3} {4}", IndentString(indent), StackName(CurStack - 1), StackName(CurStack - 1), GetOperatorSymbol(Opcode), StackName(CurStack));
        }

        public override List<HighLevel.Instruction> GetHighLevel(HighLevel.HlGraph Context)
        {
            List<HighLevel.Instruction> List = new List<HighLevel.Instruction>();

            HighLevel.Node Argument;
            HighLevel.Node Left = Context.ReadStackLocationNode(Context.StackPointer - 1), Right = Context.ReadStackLocationNode(Context.StackPointer);
            if (Opcode == OpCodes.Add)
            {
                Argument = new HighLevel.AddNode(Left, Right);
            }
            else if (Opcode == OpCodes.Sub)
            {
                Argument = new HighLevel.SubNode(Left, Right);
            }
            else if (Opcode == OpCodes.Mul)
            {
                Argument = new HighLevel.MulNode(Left, Right);
            }
            else if (Opcode == OpCodes.Div)
            {
                Argument = new HighLevel.DivNode(Left, Right);
            }
            else if (Opcode == OpCodes.Rem)
            {
                Argument = new HighLevel.ModNode(Left, Right);
            }
            else
            {
                throw new InvalidOperationException();
            }

            List.Add(new HighLevel.AssignmentInstruction(Context.DefineStackLocationNode(Context.StackPointer - 1), Argument));
            return List;
        }

        public static string GetOperatorSymbol(OpCode Opcode)
        {
            if (Opcode == OpCodes.Add)
            {
                return "+";
            }
            else if (Opcode == OpCodes.Sub)
            {
                return "-";
            }
            else if (Opcode == OpCodes.Mul)
            {
                return "*";
            }
            else if (Opcode == OpCodes.Div)
            {
                return "/";
            }
            else if (Opcode == OpCodes.Rem)
            {
                return "%";
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
