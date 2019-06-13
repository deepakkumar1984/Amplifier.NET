/*    
*    Instruction.cs
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

namespace Hybrid.MsilToOpenCL.HighLevel
{
	public enum InstructionType {
		Assignment,
		Return,
		Branch,
		ConditionalBranch
	}

	public abstract class Instruction {
		private InstructionType m_InstructionType;
		private Node m_Result;
		private Node m_Argument;

		public Instruction(InstructionType InstructionType)
			: this(InstructionType, null, null) {
		}

		public Instruction(InstructionType InstructionType, Node Result, Node Argument) {
			m_InstructionType = InstructionType;
			m_Result = Result;
			m_Argument = Argument;
		}

		public InstructionType InstructionType { get { return m_InstructionType; } }
		public Node Result { get { return m_Result; } set { m_Result = value; } }
		public Node Argument { get { return m_Argument; } set { m_Argument = value; } }
	}

	public class AssignmentInstruction : Instruction {
		public AssignmentInstruction(Node Result, Node Argument)
			: base(InstructionType.Assignment, Result, Argument) {
			if (Result != null && Result.DataType == null) {
				System.Diagnostics.Debug.Assert(Argument != null && Argument.DataType != null);
				Result.DataType = Argument.DataType;
			}
		}

		public override string ToString() {
			return ((Result == null) ? string.Empty : Result.ToString() + " = ") + (Argument == null ? "???;" : (Argument.ToString() + ";"));
		}
	}

	public class ReturnInstruction : Instruction {
		public ReturnInstruction(Node Argument)
			: base(InstructionType.Return, null, Argument) {
		}

		public override string ToString() {
			return (Argument == null) ? "return;" : "return (" + Argument.ToString() + ");";
		}
	}

	public class BranchInstruction : Instruction {
		private BasicBlock m_TargetBlock;

		public BranchInstruction(BasicBlock TargetBlock)
			: base(InstructionType.Branch) {
			m_TargetBlock = TargetBlock;
		}

		public BasicBlock TargetBlock { get { return m_TargetBlock; } set { m_TargetBlock = value; } }

		public override string ToString() {
			return (TargetBlock == null) ? "goto ???;" : ("goto " + TargetBlock.LabelName + ";");
		}
	}

	public class ConditionalBranchInstruction : Instruction {
		private BasicBlock m_TargetBlock;

		public ConditionalBranchInstruction(Node Argument, BasicBlock TargetBlock)
			: base(InstructionType.ConditionalBranch, null, Argument) {
			m_TargetBlock = TargetBlock;
		}

		public BasicBlock TargetBlock { get { return m_TargetBlock; } set { m_TargetBlock = value; } }

		public override string ToString() {
			string IfConstruct = (Argument == null) ? "if (???) " : "if (" + Argument.ToString() + ") ";
			return ((TargetBlock == null) ? (IfConstruct + "goto ???") : (IfConstruct + "goto " + TargetBlock.LabelName)) + ";";
		}
	}
}
