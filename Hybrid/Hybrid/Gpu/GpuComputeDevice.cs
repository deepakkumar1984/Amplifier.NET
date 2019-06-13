/*    
*    GpuComputeDevice.cs
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

namespace Hybrid.Gpu
{
    public class GpuComputeDevice : ComputeDevice
    {
        OpenCLNet.Device device;

        public static List<GpuComputeDevice> GpuComputeDevices()
        {
            List<GpuComputeDevice> result = new List<GpuComputeDevice>();

            if (OpenCLNet.OpenCL.NumberOfPlatforms == 0)
                return new List<GpuComputeDevice>();

            OpenCLNet.Platform[] platforms = OpenCLNet.OpenCL.GetPlatforms();
            foreach (OpenCLNet.Platform platform in platforms)
            {
                OpenCLNet.Device[] devices = {};
                try
                {
                    devices = platform.QueryDevices(OpenCLNet.DeviceType.GPU);
                }
                catch (OpenCLNet.OpenCLException exception)
                {
                    //Some Intel OCL Versions throw an INVALID_VALUE exception, when they are asked for GPUs
                    if (exception.ErrorCode == OpenCLNet.ErrorCode.INVALID_VALUE)
                    {
                        continue;
                    }
                    else
                    {
                        throw exception;
                    }
                }

                foreach (OpenCLNet.Device device in devices)
                    result.Add(new GpuComputeDevice(device));
            }

            return result;
        }

        public GpuComputeDevice(OpenCLNet.Device device)
        {
            this.device = device;

            DeviceType = getDeviceType(device);

            Name = device.Name;
            Manufacturer = device.Vendor;

            DeviceId = device.DeviceID.ToString(); // TODO: get real device id from handle

            ComputeUnits = new List<ComputeUnit>();
            for (int i = 0; i < device.MaxComputeUnits; i++)
                ComputeUnits.Add(new GpuComputeUnit(device));

            GlobalMemory = new MemoryInfo(MemoryInfo.Type.Global,device.GlobalMemSize);

            // TODO L2Cache
        }

        private ComputeDevice.DeviceTypes getDeviceType(OpenCLNet.Device device)
        {
            ComputeDevice.DeviceTypes deviceType = ComputeDevice.DeviceTypes.Unknown;

            if (device.DeviceType == OpenCLNet.DeviceType.CPU)
                deviceType = ComputeDevice.DeviceTypes.Cpu;

            if (device.DeviceType == OpenCLNet.DeviceType.GPU)
                deviceType = ComputeDevice.DeviceTypes.Gpu;

            if (device.DeviceType == OpenCLNet.DeviceType.ACCELERATOR)
                deviceType = ComputeDevice.DeviceTypes.Accelerator;

            return deviceType;
        }

        override public void ParallelFor(int fromInclusive, int toExclusive, Action<int> action)
        {
            if (fromInclusive >= toExclusive)
                return;

            Hybrid.MsilToOpenCL.Parallel.ForGpu(fromInclusive, toExclusive, action, device);
        }

        public static void GpuParallelFor(int fromInclusive, int toExclusive, Action<int> action)
        {
            if (fromInclusive >= toExclusive)
                return;

            Hybrid.MsilToOpenCL.Parallel.ForGpu(fromInclusive, toExclusive, action, null);
        }

        override public void ParallelFor(int fromInclusiveX, int toExclusiveX, int fromInclusiveY, int toExclusiveY, Action<int, int> action)
        {
            if (fromInclusiveX >= toExclusiveX)
                return;

            if (fromInclusiveY >= toExclusiveY)
                return;

            Hybrid.MsilToOpenCL.Parallel.ForGpu(fromInclusiveX, toExclusiveX, fromInclusiveY, toExclusiveY, action, device);
        }

        public static void GpuParallelFor(int fromInclusiveX, int toExclusiveX, int fromInclusiveY, int toExclusiveY, Action<int, int> action)
        {
            if (fromInclusiveX >= toExclusiveX)
                return;

            if (fromInclusiveY >= toExclusiveY)
                return;

            Hybrid.MsilToOpenCL.Parallel.ForGpu(fromInclusiveX, toExclusiveX, fromInclusiveY, toExclusiveY, action, null);
        }

        public static void GpuParallelInvoke(params Action[] actions)
        {
            // TODO: Uncomment me
            // throw new NotImplementedException();

            System.Threading.Tasks.Parallel.Invoke(actions);
        }

        public static void GpuReInitialize()
        {
            Hybrid.MsilToOpenCL.Parallel.PurgeCaches();
        }
    }
}
