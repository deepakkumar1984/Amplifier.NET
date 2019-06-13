/*    
*    SystemCharacteristics.cs
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
using Hybrid.Examples;
using System.Xml.Serialization;
using System.IO;

namespace Hybrid.Benchmark
{
    public static class SystemCharacteristics
    {
        public class DoublePair
        {
            public double Key;
            public double Value;
        }

        public class Example
        {
            public string Name;
            public List<DoublePair> AppropriateScales;

            public DoublePair Get(ExampleBase exampleBase, double minSequentialExecutionTime)
            {
                foreach (DoublePair doublePair in AppropriateScales)
                    if (doublePair.Key == minSequentialExecutionTime)
                        return doublePair;

                DoublePair result = new DoublePair();
                result.Key = minSequentialExecutionTime;
                result.Value = findGoodSize(exampleBase, minSequentialExecutionTime);

                AppropriateScales.Add(result);

                return result;
            }

            private double findGoodSize(ExampleBase example, double minSequentialExecutionTime)
            {
                example.ExecuteOn = Execute.OnSingleCpu;
                double scale = example.FindAppropriateSize(minSequentialExecutionTime);

                Console.WriteLine("Scale " + scale + " for " + example.GetType().Name + "."); // + " for " + executionTime + "s.");

                return scale;
            }
        }

        public static List<Example> Examples;

        private static string m_MachineName;
        public static string MachineName { get { return m_MachineName; } set { m_MachineName = value; } }

        private static Platform m_Platform;
        public static Platform Platform { get { return m_Platform; } set { m_Platform = value; } }

        static SystemCharacteristics()
        {
            Examples = new List<Example>();
            
            MachineName = Environment.MachineName;
            Platform = Hybrid.Scheduler.Platform;
        }

        private static Example Get(string exampleName)
        {
            foreach (Example example in Examples)
                if (example.Name == exampleName)
                    return example;

            Example result = new Example();
            result.Name = exampleName;
            result.AppropriateScales = new List<DoublePair>();

            Examples.Add(result);

            return result;
        }

        public static double GetScale(ExampleBase exampleBase, double minSequentialExecutionTime)
        {
            Example example = Get(exampleBase.GetType().ToString());
            DoublePair pair = example.Get(exampleBase, minSequentialExecutionTime);

            return pair.Value;
        }

        public new static string ToString()
        {
            string result = "";

            result = "Compute Devices found on " + MachineName + ":\r\n";

            foreach (ComputeDevice computeDevice in Platform.ComputeDevices)
                result += " * " + computeDevice.Name + " [performance index: " + computeDevice.PredictPerformance(new AlgorithmCharacteristics()) + "]" + "\r\n";

            return result;
        }
    }
}
