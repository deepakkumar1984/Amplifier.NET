/*    
*    StackState.cs
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
    public class StackState
    {
        private List<StackLocation> m_StackLocations;
        private bool m_Complete;

        public List<StackLocation> StackLocations { get { return m_StackLocations; } }
        public bool Complete { get { return m_Complete; } set { m_Complete = value; } }

        public StackState(int EntryCount, bool? Complete)
        {
            m_StackLocations = new List<StackLocation>(EntryCount);
            for (int i = 0; i < EntryCount; i++)
            {
                m_StackLocations.Add(CreateStackLocation(i, null));
            }

            m_Complete = (Complete.HasValue ? Complete.Value : (m_StackLocations.Count == 0));
        }

        public StackState(StackState ex)
        {
            m_StackLocations = new List<StackLocation>(ex.StackLocations.Count);
            foreach (StackLocation Location in ex.StackLocations)
            {
                m_StackLocations.Add((StackLocation)Location.Clone());
            }
        }

        public static StackLocation CreateStackLocation(int Index, Type DataType)
        {
            return new StackLocation(Index, DataType);
        }

        public override string ToString()
        {
            return string.Format("StackStage: complete={0}, count={1}", m_Complete, m_StackLocations.Count);
        }
    }
}
