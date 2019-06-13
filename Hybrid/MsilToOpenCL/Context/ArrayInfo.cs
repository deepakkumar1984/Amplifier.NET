/*    
*    ArrayInfo.cs
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
    public class ArrayInfo
    {
        private ArgumentLocation m_ArrayArgument;
        private List<Node> m_ScaleNode;
        private List<ArgumentLocation> m_ScaleArgument;
        private int m_DimensionCount;

        public ArrayInfo(ArgumentLocation ArrayArgument)
        {
            m_ArrayArgument = ArrayArgument;
            m_DimensionCount = ArrayArgument.DataType.GetArrayRank();

            m_ScaleNode = new List<Node>(DimensionCount);
            m_ScaleArgument = new List<ArgumentLocation>(DimensionCount);

            for (int i = 0; i < DimensionCount; i++)
            {
                m_ScaleNode.Add(null);
                m_ScaleArgument.Add(null);
            }
        }

        public int DimensionCount { get { return m_DimensionCount; } }

        public List<Node> ScaleNode { get { return m_ScaleNode; } }
        public List<ArgumentLocation> ScaleArgument { get { return m_ScaleArgument; } }
    }
}
