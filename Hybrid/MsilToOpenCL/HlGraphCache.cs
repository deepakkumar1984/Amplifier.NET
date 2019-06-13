/*    
*    HlGraphCache.cs
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
    public class HlGraphCache
    {
        private Dictionary<IntPtr, Dictionary<MethodInfo, HlGraphEntry>> hlGraphCache = new Dictionary<IntPtr, Dictionary<MethodInfo, HlGraphEntry>>();

        internal void purge()
        {
            lock (hlGraphCache)
            {
                hlGraphCache.Clear();
            }
        }

        internal bool TryGetValue(IntPtr deviceId, MethodInfo methodInfo, out HlGraphEntry hlGraphEntry)
        {
            lock (hlGraphCache)
            {
                if (hlGraphCache.ContainsKey(deviceId) && hlGraphCache[deviceId].TryGetValue(methodInfo, out hlGraphEntry))
                    return true;
                else
                {
                    hlGraphEntry = null;
                    return false;
                }
            }
        }

        internal void SetValue(IntPtr deviceId, MethodInfo methodInfo, HlGraphEntry hlGraphEntry)
        {
            lock (hlGraphCache)
            {
                if (!hlGraphCache.ContainsKey(deviceId))
                    hlGraphCache[deviceId] = new Dictionary<MethodInfo, HlGraphEntry>();

                hlGraphCache[deviceId][methodInfo] = hlGraphEntry;
            }
        }
    }
}
