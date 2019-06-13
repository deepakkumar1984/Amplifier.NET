/*    
*    CilInstruction.cs
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
using System.Reflection.Emit;
using System.Reflection;
using Hybrid.MsilToOpenCL.Instructions;

namespace Hybrid.MsilToOpenCL
{
    public abstract class CilInstruction
    {
        private int m_Offset;
        private OpCode m_Opcode;

        public CilInstruction(OpCode Opcode, int Offset)
        {
            m_Opcode = Opcode;
            m_Offset = Offset;
        }

        public OpCode Opcode
        {
            get
            {
                return m_Opcode;
            }
        }

        public int Offset
        {
            get
            {
                return m_Offset;
            }
        }

        public override string ToString()
        {
            return m_Offset.ToString("X8") + "    " + m_Opcode.ToString();
        }

        public virtual bool CanFallThrough
        {
            get
            {
                return true;
            }
        }

        public virtual IEnumerable<int> BranchTargetOffsets
        {
            get
            {
                yield break;
            }
        }

        public virtual int StackConsumeCount
        {
            get
            {
                switch (Opcode.StackBehaviourPop)
                {
                    case StackBehaviour.Pop0:
                        return 0;

                    case StackBehaviour.Pop1:
                    case StackBehaviour.Popi:
                    case StackBehaviour.Popref:
                        return 1;

                    case StackBehaviour.Pop1_pop1:
                    case StackBehaviour.Popi_pop1:
                    case StackBehaviour.Popi_popi:
                    case StackBehaviour.Popi_popi8:
                    case StackBehaviour.Popi_popr4:
                    case StackBehaviour.Popi_popr8:
                    case StackBehaviour.Popref_pop1:
                    case StackBehaviour.Popref_popi:
                        return 2;

                    case StackBehaviour.Popi_popi_popi:
                    case StackBehaviour.Popref_popi_pop1:
                    case StackBehaviour.Popref_popi_popi:
                    case StackBehaviour.Popref_popi_popi8:
                    case StackBehaviour.Popref_popi_popr4:
                    case StackBehaviour.Popref_popi_popr8:
                    case StackBehaviour.Popref_popi_popref:
                        return 3;

                    default:
                        throw new InvalidOperationException(string.Format("A StackBehaviourPop of \"{0}\" is unexpected.", Opcode.StackBehaviourPop));
                }
            }
        }

        public virtual int StackProduceCount
        {
            get
            {
                switch (Opcode.StackBehaviourPush)
                {
                    case StackBehaviour.Push0:
                        return 0;

                    case StackBehaviour.Push1:
                    case StackBehaviour.Pushi:
                    case StackBehaviour.Pushi8:
                    case StackBehaviour.Pushr4:
                    case StackBehaviour.Pushr8:
                    case StackBehaviour.Pushref:
                        return 1;

                    case StackBehaviour.Push1_push1:
                        return 2;

                    default:
                        throw new InvalidOperationException(string.Format("A StackBehaviourPush of \"{0}\" is unexpected.", Opcode.StackBehaviourPush));
                }
            }
        }

        protected static string IndentString(int indent)
        {
            switch (indent)
            {
                case 0:
                    return string.Empty;
                case 1:
                    return "\t";
                case 2:
                    return "\t\t";
                case 3:
                    return "\t\t\t";
                case 4:
                    return "\t\t\t\t";
                default:
                    return new string('\t', indent);
            }
        }

        protected string StackName(int StackPointer)
        {
            System.Diagnostics.Debug.Assert(StackPointer > 0);
            return string.Format("stack_{0}", StackPointer - 1);
        }

        public static string LabelName(int Offset)
        {
            System.Diagnostics.Debug.Assert(Offset >= 0);
            return string.Format("IL_{0:X8}", Offset);
        }

        protected void WriteInstHeader(System.IO.TextWriter writer, int indent)
        {
            writer.WriteLine();
            writer.WriteLine("{0}// (stcon={3,2},stprd={4,2})\t{2}", IndentString(indent), Offset, ToString(), StackConsumeCount, StackProduceCount);
        }

        public abstract void WriteCode(System.IO.TextWriter writer, int indent, int CurStack);

        public abstract List<HighLevel.Instruction> GetHighLevel(HighLevel.HlGraph Context);

        protected static sbyte ReadInt8(byte[] IL, int Offset)
        {
            return (sbyte)IL[Offset];
        }

        protected static byte ReadUInt8(byte[] IL, int Offset)
        {
            return IL[Offset];
        }

        protected static ushort ReadUInt16(byte[] IL, int Offset)
        {
            return (ushort)((ushort)IL[Offset + 0] | ((ushort)IL[Offset + 1] << 8));
        }

        public static int ReadInt32(byte[] IL, int Offset)
        {
            return (int)((uint)IL[Offset + 0] | ((uint)IL[Offset + 1] << 8) | ((uint)IL[Offset + 2] << 16) | ((uint)IL[Offset + 3] << 24));
        }

        protected static float ReadFloat(byte[] IL, int Offset)
        {
            // TODO: endianness
            return BitConverter.ToSingle(IL, Offset);
        }

        protected static double ReadDouble(byte[] IL, int Offset)
        {
            // TODO: endianness
            return BitConverter.ToDouble(IL, Offset);
        }

        public delegate CilInstruction ConstructCilInstruction(OpCode Opcode, byte[] IL, int Offset, int NextOffset, MethodBase ParentMethodBase);

        public static readonly Dictionary<OpCode, ConstructCilInstruction> Factory = GetCilFactoryMap();

        private static Dictionary<OpCode, ConstructCilInstruction> GetCilFactoryMap()
        {
            Dictionary<OpCode, ConstructCilInstruction> Map = new Dictionary<OpCode, ConstructCilInstruction>();
            Map.Add(OpCodes.Add, CilBinaryNumericInstruction.Create);

            Map.Add(OpCodes.Beq, CilComparisonAndBranchInstruction.Create);
            Map.Add(OpCodes.Beq_S, CilComparisonAndBranchInstruction.Create);
            Map.Add(OpCodes.Bge, CilComparisonAndBranchInstruction.Create);
            Map.Add(OpCodes.Bge_S, CilComparisonAndBranchInstruction.Create);
            Map.Add(OpCodes.Bge_Un, CilComparisonAndBranchInstruction.Create);
            Map.Add(OpCodes.Bge_Un_S, CilComparisonAndBranchInstruction.Create);
            Map.Add(OpCodes.Bgt, CilComparisonAndBranchInstruction.Create);
            Map.Add(OpCodes.Bgt_S, CilComparisonAndBranchInstruction.Create);
            Map.Add(OpCodes.Bgt_Un, CilComparisonAndBranchInstruction.Create);
            Map.Add(OpCodes.Bgt_Un_S, CilComparisonAndBranchInstruction.Create);
            Map.Add(OpCodes.Ble, CilComparisonAndBranchInstruction.Create);
            Map.Add(OpCodes.Ble_S, CilComparisonAndBranchInstruction.Create);
            Map.Add(OpCodes.Ble_Un, CilComparisonAndBranchInstruction.Create);
            Map.Add(OpCodes.Ble_Un_S, CilComparisonAndBranchInstruction.Create);
            Map.Add(OpCodes.Blt, CilComparisonAndBranchInstruction.Create);
            Map.Add(OpCodes.Blt_S, CilComparisonAndBranchInstruction.Create);
            Map.Add(OpCodes.Blt_Un, CilComparisonAndBranchInstruction.Create);
            Map.Add(OpCodes.Blt_Un_S, CilComparisonAndBranchInstruction.Create);
            Map.Add(OpCodes.Bne_Un, CilComparisonAndBranchInstruction.Create);
            Map.Add(OpCodes.Bne_Un_S, CilComparisonAndBranchInstruction.Create);

            Map.Add(OpCodes.Br, CilBranchInstruction.Create);
            Map.Add(OpCodes.Br_S, CilBranchInstruction.Create);

            Map.Add(OpCodes.Brfalse, CilConditionalBranchInstruction.Create);
            Map.Add(OpCodes.Brfalse_S, CilConditionalBranchInstruction.Create);
            Map.Add(OpCodes.Brtrue, CilConditionalBranchInstruction.Create);
            Map.Add(OpCodes.Brtrue_S, CilConditionalBranchInstruction.Create);

            Map.Add(OpCodes.Call, CilCallInstruction.Create);

            Map.Add(OpCodes.Ceq, CilBinaryComparisonInstruction.Create);
            Map.Add(OpCodes.Cgt, CilBinaryComparisonInstruction.Create);
            Map.Add(OpCodes.Cgt_Un, CilBinaryComparisonInstruction.Create);
            Map.Add(OpCodes.Clt, CilBinaryComparisonInstruction.Create);
            Map.Add(OpCodes.Clt_Un, CilBinaryComparisonInstruction.Create);

            Map.Add(OpCodes.Conv_R4, CilConvertInstruction.Create_R4);
            Map.Add(OpCodes.Conv_R8, CilConvertInstruction.Create_R8);
            Map.Add(OpCodes.Conv_U, CilConvertInstruction.Create_U);
            Map.Add(OpCodes.Conv_U1, CilConvertInstruction.Create_U1);
            Map.Add(OpCodes.Conv_U2, CilConvertInstruction.Create_U2);
            Map.Add(OpCodes.Conv_U4, CilConvertInstruction.Create_U4);

            Map.Add(OpCodes.Div, CilBinaryNumericInstruction.Create);
            Map.Add(OpCodes.Dup, CilDupInstruction.Create);

            Map.Add(OpCodes.Ldarg, CilLoadArgumentInstruction.Create);
            Map.Add(OpCodes.Ldarg_0, CilLoadArgumentInstruction.Create);
            Map.Add(OpCodes.Ldarg_1, CilLoadArgumentInstruction.Create);
            Map.Add(OpCodes.Ldarg_2, CilLoadArgumentInstruction.Create);
            Map.Add(OpCodes.Ldarg_3, CilLoadArgumentInstruction.Create);
            Map.Add(OpCodes.Ldarg_S, CilLoadArgumentInstruction.Create);

            Map.Add(OpCodes.Ldc_I4, CilLoadI4ConstantInstruction.Create);
            Map.Add(OpCodes.Ldc_I4_0, CilLoadI4ConstantInstruction.Create);
            Map.Add(OpCodes.Ldc_I4_1, CilLoadI4ConstantInstruction.Create);
            Map.Add(OpCodes.Ldc_I4_2, CilLoadI4ConstantInstruction.Create);
            Map.Add(OpCodes.Ldc_I4_3, CilLoadI4ConstantInstruction.Create);
            Map.Add(OpCodes.Ldc_I4_4, CilLoadI4ConstantInstruction.Create);
            Map.Add(OpCodes.Ldc_I4_5, CilLoadI4ConstantInstruction.Create);
            Map.Add(OpCodes.Ldc_I4_6, CilLoadI4ConstantInstruction.Create);
            Map.Add(OpCodes.Ldc_I4_7, CilLoadI4ConstantInstruction.Create);
            Map.Add(OpCodes.Ldc_I4_8, CilLoadI4ConstantInstruction.Create);
            Map.Add(OpCodes.Ldc_I4_M1, CilLoadI4ConstantInstruction.Create);
            Map.Add(OpCodes.Ldc_I4_S, CilLoadI4ConstantInstruction.Create);

            Map.Add(OpCodes.Ldc_R4, CilLoadR4ConstantInstruction.Create);
            Map.Add(OpCodes.Ldc_R8, CilLoadR8ConstantInstruction.Create);

            Map.Add(OpCodes.Ldelem, CilLoadElementInstruction.CreateWithType);
            Map.Add(OpCodes.Ldelema, CilLoadElementAddressInstruction.CreateWithType);

            Map.Add(OpCodes.Ldelem_I, CilLoadElementInstruction.Create_I);
            Map.Add(OpCodes.Ldelem_I1, CilLoadElementInstruction.Create_I1);
            Map.Add(OpCodes.Ldelem_I2, CilLoadElementInstruction.Create_I2);
            Map.Add(OpCodes.Ldelem_I4, CilLoadElementInstruction.Create_I4);
            Map.Add(OpCodes.Ldelem_I8, CilLoadElementInstruction.Create_I8);
            Map.Add(OpCodes.Ldelem_R4, CilLoadElementInstruction.Create_R4);
            Map.Add(OpCodes.Ldelem_R8, CilLoadElementInstruction.Create_R8);
            Map.Add(OpCodes.Ldelem_Ref, CilLoadElementInstruction.Create_Ref);
            Map.Add(OpCodes.Ldelem_U1, CilLoadElementInstruction.Create_U1);
            Map.Add(OpCodes.Ldelem_U2, CilLoadElementInstruction.Create_U2);
            Map.Add(OpCodes.Ldelem_U4, CilLoadElementInstruction.Create_U4);

            Map.Add(OpCodes.Ldfld, CilLoadFieldInstruction.Create);
            Map.Add(OpCodes.Ldflda, CilLoadFieldAddressInstruction.Create);

            Map.Add(OpCodes.Ldloc, CilLoadLocalInstruction.Create);
            Map.Add(OpCodes.Ldloc_0, CilLoadLocalInstruction.Create);
            Map.Add(OpCodes.Ldloc_1, CilLoadLocalInstruction.Create);
            Map.Add(OpCodes.Ldloc_2, CilLoadLocalInstruction.Create);
            Map.Add(OpCodes.Ldloc_3, CilLoadLocalInstruction.Create);
            Map.Add(OpCodes.Ldloc_S, CilLoadLocalInstruction.Create);

            Map.Add(OpCodes.Ldloca, CilLoadLocalAddressInstruction.Create);
            Map.Add(OpCodes.Ldloca_S, CilLoadLocalAddressInstruction.Create);

            Map.Add(OpCodes.Ldobj, CilLoadObjectInstruction.Create);

            Map.Add(OpCodes.Ldsfld, CilLoadFieldInstruction.Create);
            Map.Add(OpCodes.Ldsflda, CilLoadFieldAddressInstruction.Create);

            Map.Add(OpCodes.Mul, CilBinaryNumericInstruction.Create);

            Map.Add(OpCodes.Neg, CilUnaryNumericInstruction.Create);

            Map.Add(OpCodes.Nop, CilNopInstruction.Create);

            Map.Add(OpCodes.Pop, CilPopInstruction.Create);

            Map.Add(OpCodes.Rem, CilBinaryNumericInstruction.Create);

            Map.Add(OpCodes.Ret, CilReturnInstruction.Create);

            Map.Add(OpCodes.Stelem_I, CilStoreElementInstruction.Create_I);
            Map.Add(OpCodes.Stelem_I1, CilStoreElementInstruction.Create_I1);
            Map.Add(OpCodes.Stelem_I2, CilStoreElementInstruction.Create_I2);
            Map.Add(OpCodes.Stelem_I4, CilStoreElementInstruction.Create_I4);
            Map.Add(OpCodes.Stelem_I8, CilStoreElementInstruction.Create_I8);
            Map.Add(OpCodes.Stelem_R4, CilStoreElementInstruction.Create_R4);
            Map.Add(OpCodes.Stelem_R8, CilStoreElementInstruction.Create_R8);
            Map.Add(OpCodes.Stelem_Ref, CilStoreElementInstruction.Create_Ref);

            Map.Add(OpCodes.Stind_I1, CilStoreObjectInstruction.Create_I1);
            Map.Add(OpCodes.Stind_I2, CilStoreObjectInstruction.Create_I2);
            Map.Add(OpCodes.Stind_I4, CilStoreObjectInstruction.Create_I4);
            Map.Add(OpCodes.Stind_I8, CilStoreObjectInstruction.Create_I8);
            Map.Add(OpCodes.Stind_R4, CilStoreObjectInstruction.Create_R4);
            Map.Add(OpCodes.Stind_R8, CilStoreObjectInstruction.Create_R8);

            Map.Add(OpCodes.Stloc, CilStoreLocalInstruction.Create);
            Map.Add(OpCodes.Stloc_0, CilStoreLocalInstruction.Create);
            Map.Add(OpCodes.Stloc_1, CilStoreLocalInstruction.Create);
            Map.Add(OpCodes.Stloc_2, CilStoreLocalInstruction.Create);
            Map.Add(OpCodes.Stloc_3, CilStoreLocalInstruction.Create);
            Map.Add(OpCodes.Stloc_S, CilStoreLocalInstruction.Create);

            Map.Add(OpCodes.Stfld, CilStoreFieldInstruction.Create);

            Map.Add(OpCodes.Stobj, CilStoreObjectInstruction.Create);

            Map.Add(OpCodes.Sub, CilBinaryNumericInstruction.Create);

            Map.Add(OpCodes.Switch, CilSwitchInstruction.Create);

            return Map;
        }
    }
}
