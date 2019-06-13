/*    
*    ArrayAccessNode.cs
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
    public class ArrayAccessNode : Node
    {
        private System.Type m_ArrayType;

        public ArrayAccessNode(Type ArrayType)
            : base(NodeType.ArrayAccess, ArrayType.GetElementType(), false)
        {
            if (!ArrayType.IsArray)
            {
                throw new ArgumentException("ArrayAccessNode requires an array type.");
            }

            m_ArrayType = ArrayType;
        }

        public Type ArrayType { get { return m_ArrayType; } }

        public override string ToString()
        {
            if (SubNodes.Count == 0)
            {
                return "(???)[ ??? ]";
            }

            StringBuilder String = new StringBuilder();
            String.Append(SubNodes[0].ToString());
            String.Append("[");

            if (SubNodes.Count == 1)
            {
                String.Append(" ??? ]");
            }
            else
            {
                for (int i = 1; i < SubNodes.Count; i++)
                {
                    if (i > 1)
                    {
                        String.Append(", ");
                    }
                    String.Append(SubNodes[i].ToString());
                }
                String.Append("]");
            }

            return String.ToString();
        }

        internal void FlattenArrayType()
        {
            if (ArrayType.GetArrayRank() > 1)
            {
                m_ArrayType = System.Array.CreateInstance(ArrayType.GetElementType(), 1).GetType();
            }
        }
    }
}
