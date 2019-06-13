/*    
*    Scheduler.cs
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
using System.Threading;

namespace Hybrid
{
    public class Scheduler
    {
        public static Platform Platform = new Platform();

        public static void AutomaticFor(int fromInclusive, int toExclusive, Action<int> action)
        {
            executeFarmerWorker(fromInclusive, toExclusive, action);
        }

        static Queue<ExecuteInfo> executionInfos = new Queue<ExecuteInfo>();

        private static void executeFarmerWorker(int fromInclusive, int toExclusive, Action<int> action)
        {
            if(Platform.ComputeDevices.Count > 1)
                Platform.ComputeDevices.RemoveAt(1);

            int workPackages = 10;
            int count = toExclusive - fromInclusive;
            int workshare = count / workPackages;
            int moreWorkCount = count - workshare * workPackages;

            for (int i = 0; i < workPackages; i++)
                executionInfos.Enqueue(new ExecuteInfo(fromInclusive + i * workshare, fromInclusive + (i+1) * workshare, action));

            if(moreWorkCount > 0)
                executionInfos.Enqueue(new ExecuteInfo(workshare * workPackages, toExclusive, action));

            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < Platform.ComputeDevices.Count; i++)
            {
                Thread thread = new Thread(worker);
                threads.Add(thread);
                thread.Start(Platform.ComputeDevices[i]);
            }

            for (int i = 0; i < Platform.ComputeDevices.Count; i++)
                threads[i].Join();
        }

        private static void worker(object device)
        {
            while (true)
            {
                ExecuteInfo executionInfo;
                lock (executionInfos)
                {
                    if (executionInfos.Count == 0)
                        return;

                    executionInfo = executionInfos.Dequeue();
                }
                try
                {
                    executionInfo.Execute(device as ComputeDevice);
                }
                catch(Exception exception)
                {
                    // TODO 
                    executionInfos.Enqueue(executionInfo);
                }
            }
        }

        private static void executeEvenDistributed(int fromInclusive, int toExclusive, Action<int> action)
        {
            List<ExecuteInfo> executeInfos = new List<ExecuteInfo>();

            int count = toExclusive - fromInclusive;
            int computeDeviceCount = Platform.ComputeDevices.Count;
            int workshare = count / computeDeviceCount;
            int moreWorkCount = count - workshare * computeDeviceCount;

            AlgorithmCharacteristics algorithmCharacteristics = new AlgorithmCharacteristics(action);
            // TODO: do something clever with algochars

            for (int i = 0; i < moreWorkCount; i++)
            {
                int from = fromInclusive + i * (workshare + 1);
                int to = from + (workshare + 1);
                executeInfos.Add(new ExecuteInfo(from, to, action, Platform.ComputeDevices[i]));
            }

            fromInclusive = fromInclusive + moreWorkCount * (workshare + 1);

            for (int i = 0; i < computeDeviceCount - moreWorkCount; i++)
            {
                int from = fromInclusive + i * workshare;
                int to = from + workshare;
                executeInfos.Add(new ExecuteInfo(from, to, action, Platform.ComputeDevices[i]));
            }
            
            System.Threading.Tasks.Parallel.ForEach(executeInfos, delegate(ExecuteInfo executeInfo) { executeInfo.Execute(); });
        }

        public static void AutomaticFor(int fromInclusiveX, int toExclusiveX, int fromInclusiveY, int toExclusiveY, Action<int, int> action)
        {
            executeEvenDistributedByColumn(fromInclusiveX, toExclusiveX, fromInclusiveY, toExclusiveY, action);
        }

        private static void executeEvenDistributedByColumn(int fromInclusiveX, int toExclusiveX, int fromInclusiveY, int toExclusiveY, Action<int,int> action)
        {
            int count = toExclusiveX - fromInclusiveX;
            int computeDeviceCount = Platform.ComputeDevices.Count;
            int workshare = count / computeDeviceCount;
            int moreWorkCount = count - workshare * computeDeviceCount;

            for (int i = 0; i < moreWorkCount; i++)
            {
                int from = fromInclusiveX + i * (workshare + 1);
                int to = from + (workshare + 1);
                execute(from, to, fromInclusiveY, toExclusiveY, action, Platform.ComputeDevices[i]);
            }

            fromInclusiveX = fromInclusiveX + moreWorkCount * (workshare + 1);

            for (int i = 0; i < computeDeviceCount - moreWorkCount; i++)
            {
                int from = fromInclusiveX + i * workshare;
                int to = from + workshare;
                execute(from, to, fromInclusiveY, toExclusiveY, action, Platform.ComputeDevices[i]);
            }
        }

        private static void execute(int fromInclusiveX, int toExclusiveX, int fromInclusiveY, int toExclusiveY, Action<int,int> action, ComputeDevice computeDevice)
        {
            computeDevice.ParallelFor(fromInclusiveX, toExclusiveX, fromInclusiveY, toExclusiveY, action);
        }

        public static void AutomaticInvoke(Action[] actions)
        {
            // Todo Implement me
            foreach (Action action in actions)
                action();
        }
    }
}