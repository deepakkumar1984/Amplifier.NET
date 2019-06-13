/*    
*    LocalVariableLocation.cs
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

namespace Hybrid.MsilToOpenCL.HighLevel
{
    public class LocalVariableLocation : Location
    {
        private int m_Index;
        private List<ArgumentLocation> m_RelatedArguments = new List<ArgumentLocation>();

        public LocalVariableLocation(int Index, string Name, Type DataType)
            : base(LocationType.LocalVariable, Name, Name, DataType)
        {
            m_Index = Index;
        }

        protected LocalVariableLocation(LocalVariableLocation ex)
            : base(ex)
        {
            m_Index = ex.m_Index;
        }

        public int Index { get { return m_Index; } }

        public override string ToString()
        {
            return "[local] " + (DataType == null ? "??? " : DataType.ToString() + " ") + Name;
        }

        public override int GetHashCode()
        {
            return m_Index.GetHashCode();
        }

        protected override bool InnerEquals(Location obj)
        {
            return ((LocalVariableLocation)obj).m_Index == m_Index;
        }

        internal override int CompareToLocation(Location Other)
        {
            if (object.ReferenceEquals(Other, null) || (Other.LocationType != this.LocationType))
            {
                throw new ArgumentException("Other");
            }

            return m_Index.CompareTo(((LocalVariableLocation)Other).m_Index);
        }

        public List<ArgumentLocation> RelatedArguments
        {
            get
            {
                return m_RelatedArguments;
            }
        }

        #region ICloneable members

        public override object Clone()
        {
            return new LocalVariableLocation(this);
        }

        #endregion
    }
}
