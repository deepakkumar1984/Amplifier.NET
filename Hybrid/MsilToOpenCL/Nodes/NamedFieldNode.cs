/*    
*    NamedFieldNode.cs
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
using System.Linq;
using System.Text;

namespace Hybrid.MsilToOpenCL.HighLevel
{
    public class NamedFieldNode : Node
    {
        private string m_FieldName;

        public NamedFieldNode(Node InstanceNode, string FieldName, Type FieldType)
            : base(NodeType.NamedField, FieldType, false)
        {
            SubNodes.Add(InstanceNode);
            m_FieldName = FieldName;
        }

        public string FieldName { get { return m_FieldName; } }

        public override string ToString()
        {
            if (SubNodes.Count == 0)
            {
                return "(???).__field_ref[\"" + (FieldName == null ? "???" : FieldName) + "\"]";
            }
            else if (SubNodes.Count == 1)
            {
                return SubNodes[0].ToString() + "." + ((FieldName == null) ? "???" : FieldName);
            }
            else
            {
                return "(??? <too many childs for named field access node> ???)";
            }
        }
    }
}
