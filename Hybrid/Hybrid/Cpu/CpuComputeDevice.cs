/*    
*    CpuComputeDevice.cs
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

namespace Hybrid.Cpu
{
    public class CpuComputeDevice : ComputeDevice
    {
        public static List<CpuComputeDevice> CpuComputeDevices()
        {
            List<CpuComputeDevice> result = new List<CpuComputeDevice>();

            ManagementClass processorInfos = new ManagementClass("Win32_Processor");
            foreach (ManagementObject processorInfo in processorInfos.GetInstances())
                result.Add(new CpuComputeDevice(processorInfo));

            return result;
        }

        public CpuComputeDevice(ManagementObject processorInfo)
        {
            DeviceType = DeviceTypes.Cpu;

            Name = processorInfo["Name"].ToString();
            Manufacturer = processorInfo["Manufacturer"].ToString();
            DeviceId = processorInfo["DeviceID"].ToString();

            uint numberOfCores = uint.Parse(processorInfo["NumberOfCores"].ToString());

            ComputeUnits = new List<ComputeUnit>();
            for (int i = 0; i < numberOfCores; i++)
                ComputeUnits.Add(new CpuComputeUnit(processorInfo));

            GlobalMemory = new MemoryInfo(MemoryInfo.Type.Global, getGlobalMemorySizeForProcessor(processorInfo));

            Caches = new List<MemoryInfo>();
			Caches.Add( // where does L2Cache really reside: on-core or on-die?
                new MemoryInfo(MemoryInfo.Type.Cache, uint.Parse(processorInfo["L2CacheSize"].ToString()))
                );
        }

        private ulong getGlobalMemorySizeForProcessor(ManagementObject processorInfo)
        {
            ulong overallCapacity = 0; // TODO: NUMA-aware

            ManagementClass physicalMemoryInfos = new ManagementClass("Win32_PhysicalMemory");
            foreach (ManagementObject physicalMemoryInfo in physicalMemoryInfos.GetInstances())
                overallCapacity += ulong.Parse(physicalMemoryInfo["Capacity"].ToString());

            ManagementClass processorInfos = new ManagementClass("Win32_Processor");
            int processorCount = processorInfos.GetInstances().Count;

            return overallCapacity / (ulong)processorCount;
        }

        override public void ParallelFor(int fromInclusive, int toExclusive, Action<int> action)
        {
            CpuParallelFor(fromInclusive, toExclusive, action);
        }

        public static void CpuParallelFor(int fromInclusive, int toExclusive, Action<int> action)
        {
            if (fromInclusive >= toExclusive)
                return;

            System.Threading.Tasks.Parallel.For(fromInclusive, toExclusive, action);
        }

        override public void ParallelFor(int fromInclusiveX, int toExclusiveX, int fromInclusiveY, int toExclusiveY, Action<int, int> action)
        {
            CpuParallelFor(fromInclusiveX, toExclusiveX, fromInclusiveY, toExclusiveY, action);
        }

        public static void CpuParallelFor(int fromInclusiveX, int toExclusiveX, int fromInclusiveY, int toExclusiveY, Action<int, int> action)
        {
            if (fromInclusiveX >= toExclusiveX)
                return;

            if (fromInclusiveY >= toExclusiveY)
                return;

            System.Threading.Tasks.Parallel.For(fromInclusiveX, toExclusiveX, delegate(int x)
            {
                System.Threading.Tasks.Parallel.For(fromInclusiveY, toExclusiveY, delegate(int y)
                {
                    action(x, y);
                });
            });
        }
    }
}
