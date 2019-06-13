/*    
*    Platform.cs
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

namespace Hybrid
{
    public class Platform
    {
        public List<ComputeDevice> ComputeDevices = new List<ComputeDevice>();

        public bool ContainsAGpu { get { return deviceOfTypeInDeviceList(ComputeDevice.DeviceTypes.Gpu); } }

        public Platform()
        {
            findOpenCLDevices();

            if (!deviceOfTypeInDeviceList(ComputeDevice.DeviceTypes.Cpu))
                findProcessors();
        }

        private void findOpenCLDevices()
        {
			foreach(Hybrid.Gpu.GpuComputeDevice d in Gpu.GpuComputeDevice.GpuComputeDevices())
				ComputeDevices.Add(d);
        }

        private bool deviceOfTypeInDeviceList(ComputeDevice.DeviceTypes type)
        {
            foreach (ComputeDevice computeDevice in ComputeDevices)
                if (computeDevice.DeviceType == type)
                    return true;

            return false;
        }

        private void findProcessors()
        {
			foreach (Hybrid.Cpu.CpuComputeDevice d in Cpu.CpuComputeDevice.CpuComputeDevices()) {
				ComputeDevices.Add(d);
			}
        }

        public double PredictPerformance(AlgorithmCharacteristics algorithmCharacteristics)
        {
            double result = 0.0;

            foreach (ComputeDevice computeDevice in ComputeDevices)
                result += computeDevice.PredictPerformance(algorithmCharacteristics);

            return result;
        }
    }
}
