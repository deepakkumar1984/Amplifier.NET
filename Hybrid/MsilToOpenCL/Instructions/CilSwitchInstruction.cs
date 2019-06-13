/*    
*    CilSwitchInstruction.cs
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
    // TODO support switch on Strings
    class CilSwitchInstruction: CilInstruction
    {
        private List<int> m_BranchTargetOffsets;

        public static CilInstruction Create(OpCode Opcode, byte[] IL, int Offset, int NextOffset, MethodBase ParentMethodBase)
        {
            int count = ReadInt32(IL, Offset + Opcode.Size);
            List<int> BranchTargets = new List<int>(count);

            for (int i=0; i<count; i++)
                BranchTargets.Add(NextOffset + ReadInt32(IL, Offset + Opcode.Size + (i+1)*4));

            return new CilSwitchInstruction(Opcode, Offset, BranchTargets);
        }

        private CilSwitchInstruction(OpCode Opcode, int Offset, List<int> BranchTargetOffsets)
            : base(Opcode, Offset)
        {
            if (!(Opcode == OpCodes.Switch))
            {
                throw new ArgumentException("Opcode");
            }

            m_BranchTargetOffsets = BranchTargetOffsets;
        }

        public override IEnumerable<int> BranchTargetOffsets
        {
            get
            {
                foreach (int i in m_BranchTargetOffsets)
                    yield return i;
            }
        }

        public override string ToString()
        {
            string str = "";
            foreach (int i in m_BranchTargetOffsets)
                str += "IL_" + i.ToString("X4") + " ";

            return base.ToString() + " " + str;
        }

        public override void WriteCode(System.IO.TextWriter writer, int indent, int CurStack)
        {
            WriteInstHeader(writer, indent);
            writer.WriteLine("{0}{1}", IndentString(indent), this.ToString());
        }

        public override List<HighLevel.Instruction> GetHighLevel(HighLevel.HlGraph Context)
        {
            List<HighLevel.Instruction> List = new List<HighLevel.Instruction>();

            for(int i=0; i<m_BranchTargetOffsets.Count; i++) // Add all conditional branches 
            {
                HighLevel.Node Argument = new HighLevel.EqualsNode(Context.ReadStackLocationNode(Context.StackPointer), new HighLevel.IntegerConstantNode(i));
                List.Add(new HighLevel.ConditionalBranchInstruction(Argument, Context.GetBlock(m_BranchTargetOffsets[i])));
            }

            return List;
        }
    }
}
