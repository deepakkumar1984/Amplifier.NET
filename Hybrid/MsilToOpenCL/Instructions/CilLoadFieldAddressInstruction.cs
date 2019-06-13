/*    
*    CilLoadFieldAddressInstruction.cs
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
using System.Reflection;
using System.Reflection.Emit;

namespace Hybrid.MsilToOpenCL.Instructions
{
    public class CilLoadFieldAddressInstruction : CilInstruction
    {
        private FieldInfo m_FieldInfo;

        public static CilInstruction Create(OpCode Opcode, byte[] IL, int Offset, int NextOffset, MethodBase ParentMethodBase)
        {
            int Token = ReadInt32(IL, Offset + Opcode.Size);
            FieldInfo FieldInfo = ParentMethodBase.Module.ResolveField(Token);
            return new CilLoadFieldAddressInstruction(Opcode, Offset, FieldInfo);
        }

        private CilLoadFieldAddressInstruction(OpCode Opcode, int Offset, FieldInfo FieldInfo)
            : base(Opcode, Offset)
        {
            if (Opcode != OpCodes.Ldflda)
            {
                throw new ArgumentException("Opcode");
            }
            m_FieldInfo = FieldInfo;
        }

        public FieldInfo FieldInfo
        {
            get
            {
                return m_FieldInfo;
            }
        }

        public override string ToString()
        {
            return base.ToString() + " " + m_FieldInfo.DeclaringType.FullName + "::" + m_FieldInfo.Name;
        }

        public override void WriteCode(System.IO.TextWriter writer, int indent, int CurStack)
        {
            WriteInstHeader(writer, indent);
            writer.WriteLine("{0}{1} = &({2}).__field(\"{3}\");", IndentString(indent), StackName(CurStack), StackName(CurStack), m_FieldInfo.DeclaringType.FullName + "::" + m_FieldInfo.Name);
        }

        public override List<HighLevel.Instruction> GetHighLevel(HighLevel.HlGraph Context)
        {
            List<HighLevel.Instruction> List = new List<HighLevel.Instruction>();

            HighLevel.Node Argument;
            if (FieldInfo.IsStatic)
            {
                Argument = new HighLevel.LocationNode(Context.StaticFieldLocation(FieldInfo));
            }
            else
            {
                Argument = new HighLevel.InstanceFieldNode(Context.ReadStackLocationNode(Context.StackPointer), FieldInfo);
            }

            bool isStruct = FieldInfo.FieldType.IsValueType && !FieldInfo.FieldType.IsPrimitive && !FieldInfo.FieldType.IsEnum;
            if (!isStruct)
            {
                Argument = new HighLevel.AddressOfNode(Argument);
            }

            List.Add(new HighLevel.AssignmentInstruction(Context.DefineStackLocationNode(Context.StackPointer), Argument));
            return List;
        }
    }
}
