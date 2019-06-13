/*    
*    BinaryOperatorNode.cs
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
    public abstract class BinaryOperatorNode : Node
    {
        public BinaryOperatorNode(NodeType NodeType, Node Left, Node Right)
            : base(NodeType, null, false)
        {
            SubNodes.Add(Left);
            SubNodes.Add(Right);

            this.DataType = GetResultType(Left, Right);
        }

        public override string ToString()
        {
            if (SubNodes.Count == 0)
            {
                return "(??? " + Symbol + " ???)";
            }
            else if (SubNodes.Count == 1)
            {
                return "(" + SubNodes[0].ToString() + " " + Symbol + " ???)";
            }
            else if (SubNodes.Count == 2)
            {
                return "(" + SubNodes[0].ToString() + " " + Symbol + " " + SubNodes[1].ToString() + ")";
            }
            else
            {
                return "(??? <too many childs for binary operator node> ???)";
            }
        }

        public virtual Type GetResultType(Node Left, Node Right)
        {
            if (Left == null || Left.DataType == null || Right == null || Right.DataType == null)
            {
                return null;
            }

            Type LeftType = Left.DataType, RightType = Right.DataType;
            if (LeftType == typeof(byte))
            {
                if (RightType == typeof(int) || RightType == typeof(uint) || RightType == typeof(long) || RightType == typeof(ulong) ||
                    RightType == typeof(IntPtr) || RightType == typeof(UIntPtr))
                {
                    return RightType;
                }
                else if (RightType == typeof(byte)  || RightType==typeof(ushort))
                {
                    return typeof(uint);
                }

            }
            else if (LeftType == typeof(int))
            {
                if (RightType == typeof(int) || RightType == typeof(IntPtr) || RightType == typeof(uint))
                {
                    return RightType;
                }
                else if (RightType == typeof(byte) || RightType == typeof(sbyte) || RightType == typeof(short) || RightType == typeof(ushort))
                {
                    return LeftType;
                }
            }
            else if (LeftType == typeof(uint))
            {
                if (RightType == typeof(byte) || RightType == typeof(short) || RightType == typeof(ushort) || RightType == typeof(int) || RightType == typeof(uint))
                {
                    return LeftType;
                }
                else if (RightType == typeof(IntPtr))
                {
                    return RightType;
                }
            }
            else if (LeftType == typeof(long))
            {
                if (RightType == LeftType)
                {
                    return RightType;
                }
            }
            else if (LeftType == typeof(ulong))
            {
                if (RightType == LeftType)
                {
                    return RightType;
                }
            }
            else if (LeftType == typeof(IntPtr))
            {
                if (RightType == typeof(int) || RightType == typeof(IntPtr))
                {
                    return RightType;
                }
            }
            else if (LeftType == typeof(float) || LeftType == typeof(double))
            {
                if (RightType == typeof(float) || RightType == typeof(double))
                {
                    return (LeftType == typeof(double) || RightType == typeof(double)) ? typeof(double) : typeof(float);
                }
            }

            System.Diagnostics.Debug.Assert(false, string.Format("Sorry, data type combination Left={0}, Right={1} not supported yet.", LeftType, RightType));
            // TODO
            return null;
        }

        public abstract string Symbol { get; }
    }
}
