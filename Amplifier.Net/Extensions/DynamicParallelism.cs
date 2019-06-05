/*
Amplifier.NET - LGPL 2.1 License
Please consider purchasing a commerical license - it helps development, frees you from LGPL restrictions
and provides you with support.  Thank you!
Copyright (C) 2013 Hybrid DSP Systems
http://www.hybriddsp.com

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Amplifier.DynamicParallelism
{
    /// <summary>
    /// Extension methods for dynamic parallelism.  Compute 3.5 or higher.
    /// </summary>
    public static class DynamicParallelismFunctions
    {
        private const string csErrorMsg = "Emulation of dynamic parallelism.";

        private static void ThrowNotSupported()
        {
            throw new NotSupportedException(csErrorMsg);
        }
        
        /// <summary>
        ///  NOTE: Compute Capability 3.5 and later only. Dynamic parallelism. Call from a single thread.
        ///  Not supported by emulator.
        /// </summary>
        /// <param name="gridSize">Size of grid.</param>
        /// <param name="blockSize">Size of block.</param>
        /// <param name="functionName">Name of function to launch.</param>
        /// <param name="args">Arguments.</param>
        public static int Launch(this GThread thread, dim3 gridSize, dim3 blockSize, string functionName, params object[] args)
        {
            ThrowNotSupported();
            return 0;
        }

        /// <summary>
        /// Synchronizes threads.
        /// </summary>
        /// <returns></returns>
        public static int SynchronizeDevice(this GThread thread)
        {
            ThrowNotSupported();
            return 0;
        }

        /// <summary>
        /// Gets the last error.
        /// </summary>
        /// <param name="thread"></param>
        /// <returns>Int32 representation of last error.</returns>
        public static int GetLastError(this GThread thread)
        {
            ThrowNotSupported();
            return 0;
        }

        //public string GetLastErrorString(this GThread thread)
        //{
        //    ThrowNotSupported();
        //    return string.Empty;
        //}

        /// <summary>
        /// Gets the number of devices.
        /// </summary>
        /// <param name="thread"></param>
        /// <param name="count">Number of devices.</param>
        /// <returns></returns>
        public static int GetDeviceCount(this GThread thread, ref int count)
        {
            ThrowNotSupported();
            return 0;
        }

        /// <summary>
        /// Gets the current device ID.
        /// </summary>
        /// <param name="thread"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetDeviceID(this GThread thread, ref int id)
        {
            ThrowNotSupported();
            return 0;
        }

        //cudaMemcpyAsync

        //cudaMemsetAsync

        //cudaRuntimeGetVersion

        //cudaMalloc cudaError_t cudaMalloc ( void** devPtr, size_t size )

        //cudaFree
    }
}
