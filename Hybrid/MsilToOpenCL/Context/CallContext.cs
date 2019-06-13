/*    
*    CallContext.cs
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

namespace Hybrid.MsilToOpenCL
{
    internal class CallContext : IDisposable
    {
        public OpenCLNet.Context Context;
        public OpenCLNet.CommandQueue CommandQueue;
        public OpenCLNet.Kernel Kernel;

        public CallContext(OpenCLNet.Context Context, OpenCLNet.Device Device, OpenCLNet.CommandQueueProperties CqProperties, OpenCLNet.Kernel Kernel)
        {
            this.Context = Context;
            this.CommandQueue = Context.CreateCommandQueue(Device, CqProperties);
            this.Kernel = Kernel;
        }

        ~CallContext()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (Kernel != null)
            {
                Kernel.Dispose();
                Kernel = null;
            }
            if (CommandQueue != null)
            {
                CommandQueue.Dispose();
                CommandQueue = null;
            }
            System.GC.SuppressFinalize(this);
        }
    }
}
