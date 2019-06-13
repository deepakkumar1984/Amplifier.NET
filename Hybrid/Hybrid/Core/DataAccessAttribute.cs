/*    
*    DataAccessAttribute.cs
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

namespace Hybrid.Core
{
    // [DataAccess(Access.ReadOnly, Frequency.Once, Pattern.Linear, Stride=3)]
    // [DataAccess(Access.ReadOnly, Frequency.Frequent, Pattern.Arbitrary)]
    // [DataAccess(Access.ReadOnly, Frequency.Once, Pattern.Complex, AffectedPlacesCallback=myCallback)]

    public class DataAccessAttribute : Attribute 
    { 
        public enum Pattern { Linear, Complex, Arbitrary }
        public enum Access { ReadOnly, WriteOnly, ReadWrite }
        public enum Frequency { Never, Once, Seldom, Frequent}

        public delegate List<int> AffectedPlacesDelegate(int id);
        
        public Access access = Access.ReadWrite;
        public Pattern pattern = Pattern.Arbitrary;
        public Frequency frequency = Frequency.Seldom;

        public int Stride = 1;

        public AffectedPlacesDelegate AffectedPlacesCallback;

        public DataAccessAttribute(Access access, Frequency frequency, Pattern pattern)
        {
            this.access = access;
            this.pattern = pattern;
            this.frequency = frequency;
        }
    }
}