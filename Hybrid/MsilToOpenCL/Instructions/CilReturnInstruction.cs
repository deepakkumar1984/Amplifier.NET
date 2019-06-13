/*    
*    CilReturnInstruction.cs
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
    public class CilReturnInstruction : CilInstruction
    {
        private Type m_ReturnType;

        public static CilInstruction Create(OpCode Opcode, byte[] IL, int Offset, int NextOffset, MethodBase ParentMethodBase)
        {
            Type ReturnType = typeof(void);
            if (!ParentMethodBase.IsConstructor)
            {
                System.Diagnostics.Debug.Assert(ParentMethodBase is MethodInfo);
                ReturnType = ((MethodInfo)ParentMethodBase).ReturnType;
            }

            return new CilReturnInstruction(Opcode, Offset, ReturnType);
        }

        private CilReturnInstruction(OpCode Opcode, int Offset, Type ReturnType)
            : base(Opcode, Offset)
        {
            if (Opcode != OpCodes.Ret)
            {
                throw new ArgumentException();
            }
            m_ReturnType = ReturnType;
        }

        public override bool CanFallThrough
        {
            get
            {
                return false;
            }
        }

        public override int StackConsumeCount
        {
            get
            {
                System.Diagnostics.Debug.Assert(Opcode == OpCodes.Ret);
                System.Diagnostics.Debug.Assert(Opcode.StackBehaviourPop == StackBehaviour.Varpop);

                return (m_ReturnType == typeof(void)) ? 0 : 1;
            }
        }

        public override void WriteCode(System.IO.TextWriter writer, int indent, int CurStack)
        {
            WriteInstHeader(writer, indent);

            string ReturnArgument;
            if (StackConsumeCount == 0)
            {
                ReturnArgument = string.Empty;
            }
            else
            {
                ReturnArgument = " (" + StackName(CurStack) + ")";
            }
            writer.WriteLine("{0}return{1};", IndentString(indent), ReturnArgument);
        }

        public override List<HighLevel.Instruction> GetHighLevel(HighLevel.HlGraph Context)
        {
            List<HighLevel.Instruction> List = new List<HighLevel.Instruction>();
            HighLevel.Node Argument;

            if (StackConsumeCount == 0)
            {
                Argument = null;
            }
            else
            {
                Argument = Context.ReadStackLocationNode(Context.StackPointer);
            }

            List.Add(new HighLevel.ReturnInstruction(Argument));
            return List;
        }
    }
}
