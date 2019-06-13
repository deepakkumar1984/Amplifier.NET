/*    
*    CilCallInstruction.cs
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
    public class CilCallInstruction : CilInstruction
    {
        private MethodInfo m_MethodInfo;

        public static CilInstruction Create(OpCode Opcode, byte[] IL, int Offset, int NextOffset, MethodBase ParentMethodBase)
        {
            int Token = ReadInt32(IL, Offset + Opcode.Size);
            MethodBase TargetMethodBase = ParentMethodBase.Module.ResolveMethod(Token);

            return new CilCallInstruction(Opcode, Offset, TargetMethodBase);
        }

        private CilCallInstruction(OpCode Opcode, int Offset, MethodBase MethodBase)
            : base(Opcode, Offset)
        {
            if (Opcode != OpCodes.Call)
            {
                throw new ArgumentException("Unsupported Opcode " + Opcode.ToString());
            }
            else if (MethodBase == null || MethodBase.IsConstructor)
            {
                throw new ArgumentNullException("MethodBase");
            }

            System.Diagnostics.Debug.Assert(MethodBase is MethodInfo);
            m_MethodInfo = (MethodInfo)MethodBase;
        }

        public MethodInfo MethodInfo
        {
            get
            {
                return m_MethodInfo;
            }
        }

        public override string ToString()
        {
            return base.ToString() + " " + m_MethodInfo.ToString();
        }

        public override int StackConsumeCount
        {
            get
            {
                System.Diagnostics.Debug.Assert(Opcode.StackBehaviourPop == StackBehaviour.Varpop);

                int ConsumeCount = 0;
                if ((MethodInfo.CallingConvention & CallingConventions.HasThis) != 0)
                {
                    ConsumeCount++;
                }

                ConsumeCount += MethodInfo.GetParameters().Length;
                return ConsumeCount;
            }
        }

        public override int StackProduceCount
        {
            get
            {
                System.Diagnostics.Debug.Assert(Opcode.StackBehaviourPush == StackBehaviour.Varpush);

                int ProduceCount = 0;
                if (MethodInfo.ReturnType != typeof(void))
                {
                    ProduceCount++;
                }

                return ProduceCount;
            }
        }

        public override void WriteCode(System.IO.TextWriter writer, int indent, int CurStack)
        {
            WriteInstHeader(writer, indent);
            writer.Write("{0}", IndentString(indent));
            if (StackProduceCount == 1)
            {
                writer.Write("{0} = ", StackName(CurStack - StackConsumeCount + 1));
            }
            else
            {
                System.Diagnostics.Debug.Assert(StackProduceCount == 0);
            }

            writer.Write("cilfn[\"{0}\"](", m_MethodInfo.DeclaringType.ToString() + "::" + m_MethodInfo.ToString());
            for (int i = StackConsumeCount - 1; i >= 0; i--)
            {
                writer.Write(StackName(CurStack - i));
            }
            writer.WriteLine(");");
        }

        public override List<HighLevel.Instruction> GetHighLevel(HighLevel.HlGraph Context)
        {
            List<HighLevel.Instruction> List = new List<HighLevel.Instruction>();

            HighLevel.Node Result = null;
            if (StackProduceCount != 0)
            {
                Result = Context.DefineStackLocationNode(Context.StackPointer - StackConsumeCount + 1, MethodInfo.ReturnType);
            }

            HighLevel.Node CallNode = new HighLevel.CallNode(MethodInfo);
            for (int i = StackConsumeCount - 1; i >= 0; i--)
            {
                CallNode.SubNodes.Add(Context.ReadStackLocationNode(Context.StackPointer - i));
            }

            Context.TranslateCallNode(ref Result, ref CallNode);

            List.Add(new HighLevel.AssignmentInstruction(Result, CallNode));
            return List;
        }
    }
}
