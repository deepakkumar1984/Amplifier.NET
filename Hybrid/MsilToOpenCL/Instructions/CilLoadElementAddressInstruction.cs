/*    
*    CilLoadElementAddressInstruction.cs
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
    public class CilLoadElementAddressInstruction : CilInstruction
    {
        private Type m_ArrayType;

        public static CilInstruction CreateWithType(OpCode Opcode, byte[] IL, int Offset, int NextOffset, MethodBase ParentMethodBase)
        {
            int Token = ReadInt32(IL, Offset + Opcode.Size);
            Type ElementType = ParentMethodBase.Module.ResolveType(Token);
            Type ArrayType = Array.CreateInstance(ElementType, 1).GetType();
            return Create(Opcode, IL, Offset, NextOffset, ParentMethodBase, ArrayType);
        }

        private static CilInstruction Create(OpCode Opcode, byte[] IL, int Offset, int NextOffset, MethodBase ParentMethodBase, Type ArrayType)
        {
            return new CilLoadElementAddressInstruction(Opcode, Offset, ArrayType);
        }

        private CilLoadElementAddressInstruction(OpCode Opcode, int Offset, Type ArrayType)
            : base(Opcode, Offset)
        {
            System.Diagnostics.Debug.Assert(ArrayType.IsArray);
            m_ArrayType = ArrayType;
        }

        public Type ArrayType
        {
            get
            {
                return m_ArrayType;
            }
        }

        public override string ToString()
        {
            return base.ToString() + " " + m_ArrayType.GetElementType();
        }

        public override void WriteCode(System.IO.TextWriter writer, int indent, int CurStack)
        {
            WriteInstHeader(writer, indent);
            writer.WriteLine("{0}{1} = &(({2})[{3}]);", IndentString(indent), StackName(CurStack - 1), StackName(CurStack - 1), StackName(CurStack));
        }

        public override List<HighLevel.Instruction> GetHighLevel(HighLevel.HlGraph Context)
        {
            List<HighLevel.Instruction> List = new List<HighLevel.Instruction>();

            HighLevel.ArrayAccessNode Argument = new HighLevel.ArrayAccessNode(ArrayType);
            Argument.SubNodes.Add(Context.ReadStackLocationNode(Context.StackPointer - 1));
            Argument.SubNodes.Add(Context.ReadStackLocationNode(Context.StackPointer));

            List.Add(new HighLevel.AssignmentInstruction(Context.DefineStackLocationNode(Context.StackPointer - 1), new HighLevel.AddressOfNode(Argument)));
            return List;
        }
    }
}
