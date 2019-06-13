/*    
*    OpenClAliasAttribute.cs
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
using System.Reflection;

namespace Hybrid.MsilToOpenCL
{
    /// <summary>
    /// This attribute marks methods that represent built-in OpenCL functions.
    /// If the Alias property is not specified, the .NET name of the method is used as-is in
    /// OpenCL. Otherwise, if the Alias is present, its value is used instead of the internal
    /// .NET name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class OpenClAliasAttribute : Attribute
    {
        private string m_Alias;
        public OpenClAliasAttribute()
            : this(string.Empty)
        {
        }

        public OpenClAliasAttribute(string Alias)
        {
            m_Alias = Alias;
        }

        public string Alias { get { return m_Alias; } }

        public static string Get(MethodInfo MethodInfo)
        {
            if (MethodInfo == null)
            {
                throw new ArgumentNullException("MethodInfo");
            }

            object[] Alias = MethodInfo.GetCustomAttributes(typeof(OpenClAliasAttribute), false);
            if (Alias.Length == 0)
            {
                return null;
            }
            else if (string.IsNullOrEmpty(((OpenClAliasAttribute)Alias[0]).Alias))
            {
                return MethodInfo.Name;
            }
            else
            {
                return ((OpenClAliasAttribute)Alias[0]).Alias;
            }
        }
    }
}
