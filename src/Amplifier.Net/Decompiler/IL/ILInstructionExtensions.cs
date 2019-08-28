using System;
using System.Collections.Generic;
using System.Text;
using Amplifier.Decompiler.Util;

namespace Amplifier.Decompiler.IL
{
	internal static class ILInstructionExtensions
	{
		public static T WithILRange<T>(this T target, ILInstruction sourceInstruction) where T : ILInstruction
		{
			target.AddILRange(sourceInstruction);
			return target;
		}

		public static T WithILRange<T>(this T target, Interval range) where T : ILInstruction
		{
			target.AddILRange(range);
			return target;
		}
	}
}
