/*    
*    CallNode.cs
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
    public class CallNode : Node
    {
        private System.Reflection.MethodInfo m_MethodInfo;
        private bool m_IsStaticCall;

        public CallNode(System.Reflection.MethodInfo MethodInfo)
            : base(NodeType.Call, MethodInfo.ReturnType, false)
        {
            m_MethodInfo = MethodInfo;
            m_IsStaticCall = !object.ReferenceEquals(MethodInfo, null) && ((MethodInfo.CallingConvention & System.Reflection.CallingConventions.HasThis) == 0);
        }

        public CallNode(System.Reflection.MethodInfo MethodInfo, params Node[] Arguments)
            : this(MethodInfo)
        {
            SubNodes.AddRange(Arguments);
        }

        public System.Reflection.MethodInfo MethodInfo { get { return m_MethodInfo; } }
        public bool IsStaticCall { get { return m_IsStaticCall; } set { m_IsStaticCall = value; } }

        public override string ToString()
        {
            StringBuilder String = new StringBuilder();
            int i = 0;

            if (!IsStaticCall)
            {
                if (SubNodes.Count == 0)
                {
                    String.Append("(???).");
                }
                else
                {
                    String.Append(SubNodes[0].ToString());
                    String.Append(".");
                }
                i++;
            }

            string Name = object.ReferenceEquals(HlGraph, null) ? OpenClAliasAttribute.Get(MethodInfo) : HlGraph.GetOpenClFunctionName(MethodInfo);
            if (Name == null)
            {
                Name = MethodInfo.Name;
            }
            String.Append(Name);
            String.Append("(");

            bool IsFirst = true;
            for (; i < SubNodes.Count; i++)
            {
                if (IsFirst)
                {
                    IsFirst = false;
                }
                else
                {
                    String.Append(", ");
                }
                String.Append(SubNodes[i].ToString());
            }

            String.Append(")");

            return String.ToString();
        }
    }
}
