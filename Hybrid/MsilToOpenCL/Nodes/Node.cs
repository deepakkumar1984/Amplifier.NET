/*    
*    Node.cs
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
    public abstract class Node
    {
        private NodeType m_NodeType;
        private Type m_DataType;
        private List<Node> m_SubNodes;
        private HlGraph m_HlGraph;

        protected Node(NodeType NodeType, Type DataType, bool IsLeaf)
        {
            m_NodeType = NodeType;
            m_DataType = DataType;
            m_SubNodes = new List<Node>();
        }

        public HlGraph HlGraph { get { return m_HlGraph; } set { m_HlGraph = value; } }
        public NodeType NodeType { get { return m_NodeType; } }
        public virtual Type DataType { get { return m_DataType; } set { m_DataType = value; } }
        public List<Node> SubNodes { get { return m_SubNodes; } }
    }
}
