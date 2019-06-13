/*    
*    CilInstructionReader.cs
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


using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace Hybrid.MsilToOpenCL
{
    class CilInstructionReader
    {
        private static readonly OpCode?[] OneByteOpcodes = new OpCode?[0x100];
        private static readonly OpCode?[] TwoByteOpcodes = new OpCode?[0x100];

        static CilInstructionReader()
        {
            foreach (FieldInfo fi in typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                OpCode OpCode = (OpCode)fi.GetValue(null);
                ushort Value = (ushort)OpCode.Value;
                if (Value < 0x100)
                {
                    OneByteOpcodes[Value] = OpCode;
                    System.Diagnostics.Debug.Assert(OpCode.Size == 1);
                }
                else if ((Value & 0xFF00) == 0xFE00)
                {
                    TwoByteOpcodes[Value & 0xFF] = OpCode;
                    System.Diagnostics.Debug.Assert(OpCode.Size == 2);
                }
            }
        }

        public static OpCode? GetOpCode(byte[] IL, int Offset)
        {
            byte op1 = IL[Offset];

            if (op1 == 0xFE)
            {
                return TwoByteOpcodes[IL[Offset + 1]];
            }
            else
            {
                return OneByteOpcodes[op1];
            }
        }

        public static CilInstruction Read(byte[] IL, int Offset, out int NextOffset, MethodBase ParentMethodBase)
        {
            OpCode? xOpcode = GetOpCode(IL, Offset);
            if (!xOpcode.HasValue)
            {
                throw new ArgumentException(string.Format("Sorry, IL opcode {0:X2} at offset {1} is unknown.", IL[Offset], Offset));
            }

            OpCode Opcode = xOpcode.Value;
            NextOffset = Offset + Opcode.Size;

            switch (Opcode.OperandType)
            {
                case OperandType.InlineNone:
                    break;

                case OperandType.ShortInlineBrTarget:
                case OperandType.ShortInlineI:
                case OperandType.ShortInlineVar:
                    NextOffset += 1;
                    break;

                case OperandType.InlineVar:
                    NextOffset += 2;
                    break;

                case OperandType.InlineBrTarget:
                case OperandType.InlineField:
                case OperandType.InlineI:
                case OperandType.InlineMethod:
                case OperandType.InlineSig:
                case OperandType.InlineString:
                case OperandType.InlineType:
                case OperandType.ShortInlineR:
                    NextOffset += 4;
                    break;

                case OperandType.InlineI8:
                case OperandType.InlineR:
                    NextOffset += 8;
                    break;

                case OperandType.InlineSwitch:
                    NextOffset += 4 + CilInstruction.ReadInt32(IL, NextOffset) * 4;
                    break;

                default:
                    throw new InvalidOperationException(string.Format("Sorry, IL inst {0} @ offset {1:X} uses unknown operand type {2}.", Opcode, Offset, Opcode.OperandType));
            }

            CilInstruction.ConstructCilInstruction Create;
            if (!CilInstruction.Factory.TryGetValue(Opcode, out Create))
            {
                throw new InvalidOperationException(string.Format("Sorry, IL inst {0} @ offset {1:X} cannot be represented by CilInstruction.", Opcode, Offset));
            }

            return Create(Opcode, IL, Offset, NextOffset, ParentMethodBase);
        }
    }
}
