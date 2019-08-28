using System;
using System.Collections.Generic;
using System.Text;

namespace Amplifier.Decompiler.TypeSystem
{
	public enum Nullability : byte
	{
		Oblivious = 0,
		NotNullable = 1,
		Nullable = 2
	}
}
