/*    
*    UnaryOperatorNode.cs
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
    public abstract class UnaryOperatorNode : Node
    {
        public UnaryOperatorNode(NodeType NodeType, Node Argument)
            : base(NodeType, null, false)
        {
            SubNodes.Add(Argument);

            this.DataType = GetResultType(Argument);
        }

        public override string ToString()
        {
            if (SubNodes.Count == 0)
            {
                return Symbol + "(???)";
            }
            else if (SubNodes.Count == 1)
            {
                return "(" + Symbol + SubNodes[0].ToString() + ")";
            }
            else
            {
                return "(??? <too many childs for unary operator node> ???)";
            }
        }

        public virtual Type GetResultType(Node Argument)
        {
            if (Argument == null || Argument.DataType == null)
            {
                return null;
            }

            Type Type = Argument.DataType;

            if (Type == typeof(int) || Type == typeof(long) || Type == typeof(IntPtr) || Type == typeof(float) || Type == typeof(double))
            {
                return Type;
            }

            // TODO
            return null;
        }

        public abstract string Symbol { get; }
    }
}
