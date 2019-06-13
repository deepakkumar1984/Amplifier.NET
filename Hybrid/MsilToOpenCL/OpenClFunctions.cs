/*    
*    OpenClFunctions.cs
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
using System.Threading;

namespace Hybrid.MsilToOpenCL
{
    public static class OpenClFunctions
    {
        public static void __target_only()
        {
            string Name;
            try
            {
                System.Diagnostics.StackTrace StackTrace = new System.Diagnostics.StackTrace(Thread.CurrentThread, false);
                System.Diagnostics.StackFrame StackFrame = StackTrace.GetFrame(1);
                Name = StackFrame.GetMethod().Name;
            }
            catch (System.Exception ex)
            {
                throw new InvalidOperationException("Sorry, the target function does not have an implementation on the host CPU and can only be invoked from OpenCL.", ex);
            }
            throw new InvalidOperationException(string.Format("Sorry, the function '{0}' does not have an implementation on the host CPU and can only be invoked from OpenCL.", Name));
        }

        [OpenClAlias]
        public static uint get_work_dim()
        {
            __target_only();
            return 0;
        }

        [OpenClAlias]
        public static uint get_global_size(uint dimindx)
        {
            __target_only();
            return 0;
        }

        [OpenClAlias]
        public static uint get_global_id(uint dimindx)
        {
            __target_only();
            return 0;
        }

        [OpenClAlias]
        public static uint get_local_size(uint dimindx)
        {
            __target_only();
            return 0;
        }

        [OpenClAlias]
        public static uint get_local_id(uint dimindx)
        {
            __target_only();
            return 0;
        }

        [OpenClAlias]
        public static uint get_num_groups(uint dimindx)
        {
            __target_only();
            return 0;
        }

        [OpenClAlias]
        public static uint get_group_id(uint dimindx)
        {
            __target_only();
            return 0;
        }

        [OpenClAlias("MWC64X")]
        public static uint rnd()
        {
            // Chosen by unfair dice roll. Guaranteed NOT to be random...
            return 0;
        }
    }
}
