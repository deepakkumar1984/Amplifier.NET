/*    
*    CilStoreObjectInstruction.cs
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
    public class CilStoreObjectInstruction : CilInstruction
    {
        private Type m_Type;

        public static CilInstruction Create(OpCode Opcode, byte[] IL, int Offset, int NextOffset, MethodBase ParentMethodBase)
        {
            int Token = ReadInt32(IL, Offset + Opcode.Size);
            Type Type = ParentMethodBase.Module.ResolveType(Token);
            return new CilStoreObjectInstruction(Opcode, Offset, Type);
        }

        public static CilInstruction Create_I1(OpCode Opcode, byte[] IL, int Offset, int NextOffset, MethodBase ParentMethodBase)
        {
            return new CilStoreObjectInstruction(Opcode, Offset, typeof(byte));
        }

        public static CilInstruction Create_I2(OpCode Opcode, byte[] IL, int Offset, int NextOffset, MethodBase ParentMethodBase)
        {
            return new CilStoreObjectInstruction(Opcode, Offset, typeof(short));
        }

        public static CilInstruction Create_I4(OpCode Opcode, byte[] IL, int Offset, int NextOffset, MethodBase ParentMethodBase)
        {
            return new CilStoreObjectInstruction(Opcode, Offset, typeof(int));
        }

        public static CilInstruction Create_I8(OpCode Opcode, byte[] IL, int Offset, int NextOffset, MethodBase ParentMethodBase)
        {
            return new CilStoreObjectInstruction(Opcode, Offset, typeof(long));
        }

        public static CilInstruction Create_R4(OpCode Opcode, byte[] IL, int Offset, int NextOffset, MethodBase ParentMethodBase)
        {
            return new CilStoreObjectInstruction(Opcode, Offset, typeof(float));
        }

        public static CilInstruction Create_R8(OpCode Opcode, byte[] IL, int Offset, int NextOffset, MethodBase ParentMethodBase)
        {
            return new CilStoreObjectInstruction(Opcode, Offset, typeof(double));
        }

        private CilStoreObjectInstruction(OpCode Opcode, int Offset, Type Type)
            : base(Opcode, Offset)
        {
            if (Opcode != OpCodes.Stind_I1 && Opcode != OpCodes.Stind_I2 && Opcode != OpCodes.Stind_I4 && Opcode != OpCodes.Stind_I8 &&
                Opcode != OpCodes.Stind_R4 && Opcode != OpCodes.Stind_R8 && Opcode != OpCodes.Stobj)
            {
                throw new ArgumentException("Opcode");
            }
            m_Type = Type;
        }

        public Type Type
        {
            get
            {
                return m_Type;
            }
        }

        public override string ToString()
        {
            return base.ToString() + " " + m_Type.ToString();
        }

        public override void WriteCode(System.IO.TextWriter writer, int indent, int CurStack)
        {
            WriteInstHeader(writer, indent);
            writer.WriteLine("{0}*({1}) = {2};", IndentString(indent), StackName(CurStack - 1), StackName(CurStack));
        }

        public override List<HighLevel.Instruction> GetHighLevel(HighLevel.HlGraph Context)
        {
            List<HighLevel.Instruction> List = new List<HighLevel.Instruction>();
            List.Add(new HighLevel.AssignmentInstruction(new HighLevel.DerefNode(Context.ReadStackLocationNode(Context.StackPointer - 1)), Context.ReadStackLocationNode(Context.StackPointer)));
            return List;
        }
    }
}
