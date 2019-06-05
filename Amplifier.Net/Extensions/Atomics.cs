/*
Amplifier.NET - LGPL 2.1 License
Please consider purchasing a commerical license - it helps development, frees you from LGPL restrictions
and provides you with support.  Thank you!
Copyright (C) 2011 Hybrid DSP Systems
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

namespace Amplifier.Atomics
{

    /// <summary>
    /// Extension class containing atomic functions. See the NVIDIA CUDA documentation for more information.
    /// </summary>
    public static class AtomicFunctions
    {
#pragma warning disable 1591
        #region Add

        public static int atomicAdd(this GThread thread, ref int address, int val)
        {
            lock (thread.gridDim)
            {
                int old = address;
                address += val;
                return old;
            }
        }

        public static uint atomicAdd(this GThread thread, ref uint address, uint val)
        {            
            lock (thread.gridDim)
            {
                uint old = address;
                address += val;
                return old;
            }
        }


        /// <summary>
        /// Not supported by OpenCL.
        /// </summary>
        /// <param name="thread">The thread.</param>
        /// <param name="address">The address.</param>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        public static float atomicAdd(this GThread thread, ref float address, float val)
        {
            lock (thread.gridDim)
            {
                float old = address;
                address += val;
                return old;
            }
        }

        #endregion

        #region Subtract

        public static int atomicSub(this GThread thread, ref int address, int val)
        {
            lock (thread.gridDim)
            {
                int old = address;
                address -= val;
                return old;
            }
        }

        public static uint atomicSub(this GThread thread, ref uint address, uint val)
        {
            lock (thread.gridDim)
            {
                uint old = address;
                address -= val;
                return old;
            }
        }

        #endregion

        #region Exchange

        public static int atomicExch(this GThread thread, ref int address, int val)
        {
            return Interlocked.Exchange(ref address, val);
        }

        public static uint atomicExch(this GThread thread, ref uint address, uint val)
        {
            lock (thread.gridDim)
            {
                uint old = address;
                address = val;
                return old;
            }
        }

        public static ulong atomicExch(this GThread thread, ref ulong address, ulong val)
        {
            lock (thread.gridDim)
            {
                ulong old = address;
                address = val;
                return old;
            }
        }

        public static float atomicExch(this GThread thread, ref float address, float val)
        {
            return Interlocked.Exchange(ref address, val);
        }

        #endregion

        #region Min, Max

        public static int atomicMin(this GThread thread, ref int address, int val)
        {
            lock (thread.gridDim)
            {
                int old = address;
                address = Math.Min(address, val);
                return old;
            }
        }

        public static uint atomicMin(this GThread thread, ref uint address, uint val)
        {
            lock (thread.gridDim)
            {
                uint old = address;
                address = Math.Min(address, val);
                return old;
            }
        }

        public static int atomicMax(this GThread thread, ref int address, int val)
        {
            lock (thread.gridDim)
            {
                int old = address;
                address = Math.Max(address, val);
                return old;
            }
        }

        public static uint atomicMax(this GThread thread, ref uint address, uint val)
        {
            lock (thread.gridDim)
            {
                uint old = address;
                address = Math.Max(address, val);
                return old;
            }
        }

        #endregion

        #region Inc, Dec, CAS

        /// <summary>
        /// Supported by both CUDA and OpenCL.
        /// </summary>
        /// <param name="thread">The thread.</param>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static uint atomicIncEx(this GThread thread, ref uint address)
        {
            lock (thread.gridDim)
            {
                uint old = address;
                address = old + 1;
                return old;
            }
        }

        /// <summary>
        /// Supported by both CUDA and OpenCL.
        /// </summary>
        /// <param name="thread">The thread.</param>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static uint atomicDecEx(this GThread thread, ref uint address)
        {
            lock (thread.gridDim)
            {
                uint old = address;
                address = old - 1;
                return old;
            }
        }

        /// <summary>
        /// Not supported by OpenCL.
        /// </summary>
        /// <param name="thread">The thread.</param>
        /// <param name="address">The address.</param>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static uint atomicInc(this GThread thread, ref uint address, uint val)
        {
            lock (thread.gridDim)
            {
                uint old = address;
                address = ((old >= val) ? 0 : (old + 1));
                return old;
            }
        }

        /// <summary>
        /// Not supported by OpenCL.
        /// </summary>
        /// <param name="thread">The thread.</param>
        /// <param name="address">The address.</param>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static uint atomicDec(this GThread thread, ref uint address, uint val)
        {
            lock (thread.gridDim)
            {
                uint old = address;
                address = (((old == 0) | (old > val)) ? val : (old - 1));
                return old;
            }
        }

        public static int atomicCAS(this GThread thread, ref int address, int compare, int val)
        {
            lock (thread.gridDim)
            {
                int old = address;
                address = (old == compare ? val : old);
                return old;
            }
        }

        public static uint atomicCAS(this GThread thread, ref uint address, uint compare, uint val)
        {
            lock (thread.gridDim)
            {
                uint old = address;
                address = (old == compare ? val : old);
                return old;
            }
        }

        public static ulong atomicCAS(this GThread thread, ref ulong address, ulong compare, ulong val)
        {
            lock (thread.gridDim)
            {
                ulong old = address;
                address = (old == compare ? val : old);
                return old;
            }
        }

        #endregion

        #region Bitwise

        public static int atomicAnd(this GThread thread, ref int address, int val)
        {
            lock (thread.gridDim)
            {
                int old = address;
                address = (old & val);
                return old;
            }
        }

        public static uint atomicAnd(this GThread thread, ref uint address, uint val)
        {
            lock (thread.gridDim)
            {
                uint old = address;
                address = (old & val);
                return old;
            }
        }

        public static int atomicOr(this GThread thread, ref int address, int val)
        {
            lock (thread.gridDim)
            {
                int old = address;
                address = (old | val);
                return old;
            }
        }

        public static uint atomicOr(this GThread thread, ref uint address, uint val)
        {
            lock (thread.gridDim)
            {
                uint old = address;
                address = (old | val);
                return old;
            }
        }

        public static int atomicXor(this GThread thread, ref int address, int val)
        {
            lock (thread.gridDim)
            {
                int old = address;
                address = (old ^ val);
                return old;
            }
        }

        public static uint atomicXor(this GThread thread, ref uint address, uint val)
        {
            lock (thread.gridDim)
            {
                uint old = address;
                address = (old ^ val);
                return old;
            }
        }

        #endregion
#pragma warning restore 1591
    }

    //public sealed class LockHolder<T> : IDisposable where T : class
    //{
    //    private T handle;
    //    private bool holdsLock;

    //    public LockHolder(T handle, int milliSecondTimeout)
    //    {
    //        this.handle = handle;
    //        holdsLock = System.Threading.Monitor.TryEnter(
    //            handle, milliSecondTimeout);
    //    }

    //    public bool LockSuccessful
    //    {
    //        get { return holdsLock; }
    //    }

    //    #region IDisposable Members
    //    public void Dispose()
    //    {
    //        if (holdsLock)
    //            System.Threading.Monitor.Exit(handle);
    //        // Don’t unlock twice
    //        holdsLock = false;
    //    }
    //    #endregion
    //}
}
