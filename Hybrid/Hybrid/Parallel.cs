/*    
*    Parallel.cs
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

namespace Hybrid
{
    public class Parallel
    {
        // TODO: uncomment in final version
        //
        //public static void For(int fromInclusive, int toExclusive, Action<int> action)
        //{
        //    For(fromInclusive, toExclusive, action, Execute.OnEverythingAvailable);
        //}

        public static void For(Execute mode, int fromInclusive, int toExclusive, Action<int> action)
        {
            if (fromInclusive >= toExclusive)
                return;

            switch (mode)
            {
                case Execute.OnAllCpus:
                    Cpu.CpuComputeDevice.CpuParallelFor(fromInclusive, toExclusive, action);
                    break;

                case Execute.OnSingleGpu:
                    Gpu.GpuComputeDevice.GpuParallelFor(fromInclusive, toExclusive, action);
                    break;

                case Execute.OnSingleCpu:
                    for (int i = fromInclusive; i < toExclusive; i++)
                        action(i);
                    break;

                case Execute.OnEverythingAvailable:
                    Scheduler.AutomaticFor(fromInclusive, toExclusive, action);
                    break;

                default:
                    throw new NotImplementedException("Execution mode " + mode.ToString() + " is not supported.");
            }
        }

        // TODO: uncomment in final version
        //
        //public static void For(int fromInclusiveX, int toExclusiveX, int fromInclusiveY, int toExclusiveY, Action<int, int> action)
        //{
        //    For(fromInclusiveX, toExclusiveX, fromInclusiveY, toExclusiveY, action, Execute.OnEverythingAvailable);
        //}

        public static void For(Execute mode, int fromInclusiveX, int toExclusiveX, int fromInclusiveY, int toExclusiveY, Action<int, int> action)
        {
            if (fromInclusiveX >= toExclusiveX || fromInclusiveY >= toExclusiveY)
                return;

            switch (mode)
            {
                case Execute.OnAllCpus:
                    Cpu.CpuComputeDevice.CpuParallelFor(fromInclusiveX, toExclusiveX, fromInclusiveY, toExclusiveY, action);
                    break;

                case Execute.OnSingleGpu:
                    Gpu.GpuComputeDevice.GpuParallelFor(fromInclusiveX, toExclusiveX, fromInclusiveY, toExclusiveY, action);
                    break;

                case Execute.OnSingleCpu:
                    for (int x = fromInclusiveX; x < toExclusiveX; x++)
                        for (int y = fromInclusiveY; y < toExclusiveY; y++)
                            action(x, y);
                    break;

                case Execute.OnEverythingAvailable:
                    Scheduler.AutomaticFor(fromInclusiveX, toExclusiveX, fromInclusiveY, toExclusiveY, action);
                    break;

                default:
                    throw new NotImplementedException("Execution mode " + mode.ToString() + " is not supported.");
            }
        }

        public static void Invoke(params Action[] actions)
        {
            Invoke(Execute.OnAllCpus, actions);
        }

        public static void Invoke( Execute mode, params Action[] actions)
        {
            switch (mode)
            {
                case Execute.OnAllCpus:
                    System.Threading.Tasks.Parallel.Invoke(actions);
                    break;

                case Execute.OnSingleGpu:
                    Gpu.GpuComputeDevice.GpuParallelInvoke(actions);
                    break;

                case Execute.OnSingleCpu:
                    foreach (Action action in actions)
                        action();
                    break;

                case Execute.OnEverythingAvailable:
                    Scheduler.AutomaticInvoke(actions);
                    break;

                default:
                    throw new NotImplementedException("Execution mode " + mode.ToString() + " is not supported.");
            }
        }

        public static void ReInitialize()
        {
            Gpu.GpuComputeDevice.GpuReInitialize();
        }


#if !NET20
        public static void For(Execute mode, int fromInclusive, int toExclusive, System.Action<int, System.Threading.Tasks.ParallelLoopState> body)
        {
            System.Threading.Tasks.Parallel.For(fromInclusive, toExclusive, body);
        }

        public static void For(Execute mode, int fromInclusive, int toExclusive, System.Threading.Tasks.ParallelOptions parallelOptions, System.Action<int, System.Threading.Tasks.ParallelLoopState> body)
        {
            System.Threading.Tasks.Parallel.For(fromInclusive, toExclusive, parallelOptions, body);
        }

        public static void For(Execute mode, int fromInclusive, int toExclusive, System.Threading.Tasks.ParallelOptions parallelOptions, System.Action<int> body)
        {
            System.Threading.Tasks.Parallel.For(fromInclusive, toExclusive, parallelOptions, body);
        }

        public static void For(Execute mode, long fromInclusive, long toExclusive, System.Action<long, System.Threading.Tasks.ParallelLoopState> body)
        {
            System.Threading.Tasks.Parallel.For(fromInclusive, toExclusive, body);
        }

        public static void For(Execute mode, long fromInclusive, long toExclusive, System.Action<long> body)
        {
            System.Threading.Tasks.Parallel.For(fromInclusive, toExclusive, body);
        }

        public static void For(Execute mode, long fromInclusive, long toExclusive, System.Threading.Tasks.ParallelOptions parallelOptions, System.Action<long, System.Threading.Tasks.ParallelLoopState> body)
        {
            System.Threading.Tasks.Parallel.For(fromInclusive, toExclusive, parallelOptions, body);
        }

        public static void For(Execute mode, long fromInclusive, long toExclusive, System.Threading.Tasks.ParallelOptions parallelOptions, System.Action<long> body)
        {
            System.Threading.Tasks.Parallel.For(fromInclusive, toExclusive, parallelOptions, body);
        }

        public static void For<TLocal>(Execute mode, int fromInclusive, int toExclusive, System.Func<TLocal> localInit, System.Func<int, System.Threading.Tasks.ParallelLoopState, TLocal, TLocal> body, System.Action<TLocal> localFinally)
        {
            System.Threading.Tasks.Parallel.For<TLocal>(fromInclusive, toExclusive, localInit, body, localFinally);
        }

        public static void For<TLocal>(Execute mode, int fromInclusive, int toExclusive, System.Threading.Tasks.ParallelOptions parallelOptions, System.Func<TLocal> localInit, System.Func<int, System.Threading.Tasks.ParallelLoopState, TLocal, TLocal> body, System.Action<TLocal> localFinally)
        {
            System.Threading.Tasks.Parallel.For<TLocal>(fromInclusive, toExclusive, parallelOptions, localInit, body, localFinally);
        }

        public static void For<TLocal>(Execute mode, long fromInclusive, long toExclusive, System.Func<TLocal> localInit, System.Func<long, System.Threading.Tasks.ParallelLoopState, TLocal, TLocal> body, System.Action<TLocal> localFinally)
        {
            System.Threading.Tasks.Parallel.For<TLocal>(fromInclusive, toExclusive, localInit, body, localFinally);
        }

        public static void For<TLocal>(Execute mode, long fromInclusive, long toExclusive, System.Threading.Tasks.ParallelOptions parallelOptions, System.Func<TLocal> localInit, System.Func<long, System.Threading.Tasks.ParallelLoopState, TLocal, TLocal> body, System.Action<TLocal> localFinally)
        {
            System.Threading.Tasks.Parallel.For<TLocal>(fromInclusive, toExclusive, parallelOptions, localInit, body, localFinally);
        }

        public static void ForEach<TSource, TLocal>(Execute mode, System.Collections.Concurrent.OrderablePartitioner<TSource> source, System.Func<TLocal> localInit, System.Func<TSource, System.Threading.Tasks.ParallelLoopState, long, TLocal, TLocal> body, System.Action<TLocal> localFinally)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource, TLocal>(source, localInit, body, localFinally);
        }

        public static void ForEach<TSource, TLocal>(Execute mode, System.Collections.Concurrent.OrderablePartitioner<TSource> source, System.Threading.Tasks.ParallelOptions parallelOptions, System.Func<TLocal> localInit, System.Func<TSource, System.Threading.Tasks.ParallelLoopState, long, TLocal, TLocal> body, System.Action<TLocal> localFinally)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource, TLocal>(source, parallelOptions, localInit, body, localFinally);
        }

        public static void ForEach<TSource, TLocal>(Execute mode, System.Collections.Concurrent.Partitioner<TSource> source, System.Func<TLocal> localInit, System.Func<TSource, System.Threading.Tasks.ParallelLoopState, TLocal, TLocal> body, System.Action<TLocal> localFinally)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource, TLocal>(source, localInit, body, localFinally);
        }

        public static void ForEach<TSource, TLocal>(Execute mode, System.Collections.Concurrent.Partitioner<TSource> source, System.Threading.Tasks.ParallelOptions parallelOptions, System.Func<TLocal> localInit, System.Func<TSource, System.Threading.Tasks.ParallelLoopState, TLocal, TLocal> body, System.Action<TLocal> localFinally)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource, TLocal>(source, parallelOptions, localInit, body, localFinally);
        }

        public static void ForEach<TSource, TLocal>(Execute mode, System.Collections.Generic.IEnumerable<TSource> source, System.Func<TLocal> localInit, System.Func<TSource, System.Threading.Tasks.ParallelLoopState, long, TLocal, TLocal> body, System.Action<TLocal> localFinally)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource, TLocal>(source, localInit, body, localFinally);
        }

        public static void ForEach<TSource, TLocal>(Execute mode, System.Collections.Generic.IEnumerable<TSource> source, System.Func<TLocal> localInit, System.Func<TSource, System.Threading.Tasks.ParallelLoopState, TLocal, TLocal> body, System.Action<TLocal> localFinally)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource, TLocal>(source, localInit, body, localFinally);
        }

        public static void ForEach<TSource, TLocal>(Execute mode, System.Collections.Generic.IEnumerable<TSource> source, System.Threading.Tasks.ParallelOptions parallelOptions, System.Func<TLocal> localInit, System.Func<TSource, System.Threading.Tasks.ParallelLoopState, long, TLocal, TLocal> body, System.Action<TLocal> localFinally)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource, TLocal>(source, parallelOptions, localInit, body, localFinally);
        }

        public static void ForEach<TSource, TLocal>(Execute mode, System.Collections.Generic.IEnumerable<TSource> source, System.Threading.Tasks.ParallelOptions parallelOptions, System.Func<TLocal> localInit, System.Func<TSource, System.Threading.Tasks.ParallelLoopState, TLocal, TLocal> body, System.Action<TLocal> localFinally)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource, TLocal>(source, parallelOptions, localInit, body, localFinally);
        }

        public static void ForEach<TSource>(Execute mode, System.Collections.Concurrent.OrderablePartitioner<TSource> source, System.Action<TSource, System.Threading.Tasks.ParallelLoopState, long> body)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource>(source, body);
        }

        public static void ForEach<TSource>(Execute mode, System.Collections.Concurrent.OrderablePartitioner<TSource> source, System.Threading.Tasks.ParallelOptions parallelOptions, System.Action<TSource, System.Threading.Tasks.ParallelLoopState, long> body)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource>(source, parallelOptions, body);
        }

        public static void ForEach<TSource>(Execute mode, System.Collections.Concurrent.Partitioner<TSource> source, System.Action<TSource, System.Threading.Tasks.ParallelLoopState> body)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource>(source, body);
        }

        public static void ForEach<TSource>(Execute mode, System.Collections.Concurrent.Partitioner<TSource> source, System.Action<TSource> body)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource>(source, body);
        }

        public static void ForEach<TSource>(Execute mode, System.Collections.Concurrent.Partitioner<TSource> source, System.Threading.Tasks.ParallelOptions parallelOptions, System.Action<TSource, System.Threading.Tasks.ParallelLoopState> body)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource>(source, parallelOptions, body);
        }

        public static void ForEach<TSource>(Execute mode, System.Collections.Concurrent.Partitioner<TSource> source, System.Threading.Tasks.ParallelOptions parallelOptions, System.Action<TSource> body)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource>(source, parallelOptions, body);
        }

        public static void ForEach<TSource>(Execute mode, System.Collections.Generic.IEnumerable<TSource> source, System.Action<TSource, System.Threading.Tasks.ParallelLoopState, long> body)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource>(source, body);
        }

        public static void ForEach<TSource>(Execute mode, System.Collections.Generic.IEnumerable<TSource> source, System.Action<TSource, System.Threading.Tasks.ParallelLoopState> body)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource>(source, body);
        }

        public static void ForEach<TSource>(Execute mode, System.Collections.Generic.IEnumerable<TSource> source, System.Action<TSource> body)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource>(source, body);
        }

        public static void ForEach<TSource>(Execute mode, System.Collections.Generic.IEnumerable<TSource> source, System.Threading.Tasks.ParallelOptions parallelOptions, System.Action<TSource, System.Threading.Tasks.ParallelLoopState, long> body)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource>(source, parallelOptions, body);
        }

        public static void ForEach<TSource>(Execute mode, System.Collections.Generic.IEnumerable<TSource> source, System.Threading.Tasks.ParallelOptions parallelOptions, System.Action<TSource, System.Threading.Tasks.ParallelLoopState> body)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource>(source, parallelOptions, body);
        }

        public static void ForEach<TSource>(Execute mode, System.Collections.Generic.IEnumerable<TSource> source, System.Threading.Tasks.ParallelOptions parallelOptions, System.Action<TSource> body)
        {
            System.Threading.Tasks.Parallel.ForEach<TSource>(source, parallelOptions, body);
        }
#endif





























    }
}


#if NET20
namespace System.Threading.Tasks {
	internal class Parallel {
		public static void For(int fromInclusive, int toExclusive, Action<int> action) {
			for (int i = fromInclusive; i < toExclusive; i++) {
				action(i);
			}
		}

		public static void Invoke(Action[] actions) {
			foreach (Action action in actions) {
				action();
			}
		}

		public static void ForEach<T>(IEnumerable<T> l, Action<T> action) {
			foreach (T t in l)
				action(t);
		}
	}
}
#endif