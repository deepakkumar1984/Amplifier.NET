/*    
*    InstanceFieldNode.cs
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
    public class InstanceFieldNode : Node
    {
        private System.Reflection.FieldInfo m_FieldInfo;

        public InstanceFieldNode(Node InstanceNode, System.Reflection.FieldInfo FieldInfo)
            : base(NodeType.InstanceField, FieldInfo.FieldType, false)
        {
            SubNodes.Add(InstanceNode);
            m_FieldInfo = FieldInfo;
        }

        public System.Reflection.FieldInfo FieldInfo { get { return m_FieldInfo; } }

        public override string ToString()
        {
            if (SubNodes.Count == 0)
            {
                return "(???).__field_ref[\"" + (FieldInfo == null ? "???" : FieldInfo.ToString()) + "\"]";
            }
            else if (SubNodes.Count == 1)
            {
                string RefOp = ".";
                if (FieldInfo != null && FieldInfo.DeclaringType.IsValueType)
                    RefOp = "->";

                return SubNodes[0].ToString() + RefOp + ((FieldInfo == null) ? "???" : FieldInfo.Name);
            }
            else
            {
                return "(??? <too many childs for instance field access node> ???)";
            }
        }
    }
}
