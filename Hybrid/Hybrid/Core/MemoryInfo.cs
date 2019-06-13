/*    
*    MemoryInfo.cs
*
﻿*    Copyright (C) 2012  Frank Feinbube, Jan-Arne Sobania, Ralf Diestelkämper
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
*    Frank [at] Feinbube [dot] de
*    jan-arne [dot] sobania [at] gmx [dot] net
*    ralf [dot] diestelkaemper [at] hotmail [dot] com
*
*/


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hybrid
{
    public class MemoryInfo
    {
        public enum Access { ReadOnly, WriteOnly, ReadWrite }
        public enum Type { Global, Shared, Private, Cache }
        public enum PreferredAccessPattern { Linear, Areal } // to reflect constant memory and texture memory

        public Type MemoryType = Type.Global;
        public Access MemoryAccess = Access.ReadWrite;
        public PreferredAccessPattern MemoryPreferredAccessPattern = PreferredAccessPattern.Linear;

        public ulong Size;
        public ulong LineSize = 0;

		public MemoryInfo() { }

		public MemoryInfo(Type MemoryType, ulong Size) {
			this.MemoryType = MemoryType;
			this.Size = Size;
		}

		public MemoryInfo(Type MemoryType, Access MemoryAccess, ulong Size) {
			this.MemoryType = MemoryType;
			this.MemoryAccess = MemoryAccess;
			this.Size = Size;
		}
    }
}
