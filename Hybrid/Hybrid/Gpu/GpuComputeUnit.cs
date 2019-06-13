/*    
*    GpuComputeUnit.cs
*
﻿*    Copyright (C) 2012  Frank Feinbube, Jan-Arne Sobania, Ralf Diestelkämper
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
*    Frank [at] Feinbube [dot] de
*    jan-arne [dot] sobania [at] gmx [dot] net
*    ralf [dot] diestelkaemper [at] hotmail [dot] com
*
*/


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace Hybrid.Gpu
{
    public class GpuComputeUnit : ComputeUnit
    {
        public GpuComputeUnit(OpenCLNet.Device device)
        {
            AtomicsSupported = device.Extensions.Contains("atomics");
            DoublePrecisionFloatingPointSupported = device.Extensions.Contains("fp64");

            ProcessingElements = new List<ProcessingElement>();
            for (int i = 0; i < getProcessingElementCount(device); i++)
                ProcessingElements.Add(new GpuProcessingElement(device));

            SharedMemory = getSharedMemory(device);
            Caches = getCaches(device);

        }

        private List<MemoryInfo> getCaches(OpenCLNet.Device device)
        {
            List<MemoryInfo> result = new List<MemoryInfo>();

            if (device.GlobalMemCacheSize > 0)
                result.Add(
                    new MemoryInfo(MemoryInfo.Type.Cache, mapCacheAccess(device.GlobalMemCacheType), device.GlobalMemCacheSize)
                );

            result.Add(
                new MemoryInfo(MemoryInfo.Type.Cache, MemoryInfo.Access.ReadOnly, device.MaxConstantBufferSize)
            );

            return result;
        }

        private MemoryInfo getSharedMemory(OpenCLNet.Device device)
        {
            switch (device.LocalMemType)
            {
                case OpenCLNet.DeviceLocalMemType.GLOBAL:
                    return null;
                case OpenCLNet.DeviceLocalMemType.LOCAL:
                    return new MemoryInfo(MemoryInfo.Type.Shared,device.LocalMemSize);
                default:
                    throw new Exception("LocalMemType " + device.LocalMemType.ToString() + " is unknown.");
            }
        }

        private uint getProcessingElementCount(OpenCLNet.Device device)
        {
            return 8; // TODO: Get Real Values.
        }

        private MemoryInfo.Access mapCacheAccess(OpenCLNet.DeviceMemCacheType deviceMemCacheType)
        {
            if (deviceMemCacheType == OpenCLNet.DeviceMemCacheType.READ_ONLY_CACHE)
                return MemoryInfo.Access.ReadOnly;

            if (deviceMemCacheType == OpenCLNet.DeviceMemCacheType.READ_WRITE_CACHE)
                return MemoryInfo.Access.ReadWrite;

            throw new Exception("OpenCLNet.DeviceMemCacheType " + deviceMemCacheType + " unknown.");
        }
    }
}
