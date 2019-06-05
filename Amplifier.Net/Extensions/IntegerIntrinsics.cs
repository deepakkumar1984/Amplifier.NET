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
#if !NET35
using System.Numerics;
#endif
namespace Amplifier.IntegerIntrinsics
{

    /// <summary>
    /// Extension class containing Integer Intrinsics functions. 
    /// </summary>
    public static class IntegerIntrinsicsFunctions
    {

        /// <summary>
        /// Count the number of bits that are set to 1 in x.
        /// </summary>
        /// <param name="thread">The thread.</param>
        /// <param name="val">The value.</param>
        /// <returns>Returns a value between 0 and 32 inclusive representing the number of set bits.</returns>
        public static int popcount(this GThread thread, uint val)
        {
            int c = 0;
            for (; val > 0; val &= val - 1) c++;
            return c;
        }

        /// <summary>
        /// Count the number of bits that are set to 1 in x.
        /// </summary>
        /// <param name="thread">The thread.</param>
        /// <param name="val">The value.</param>
        /// <returns>Returns a value between 0 and 64 inclusive representing the number of set bits.</returns>
        public static int popcountll(this GThread thread, ulong val)
        {
            int c = 0;
            for (; val > 0; val &= val - 1) c++;
            return c;
        }


        /// <summary>
        /// Count the number of consecutive leading zero bits, starting at the most significant bit (bit 31) of x.
        /// </summary>
        /// <param name="thread">The thread.</param>
        /// <param name="val">The value.</param>
        /// <returns>Returns a value between 0 and 32 inclusive representing the number of zero bits.</returns>
        public static int clz(this GThread thread, int val)
        {
            int leadingZeros = 0;
            while (val != 0)
            {
                val = val >> 1;
                leadingZeros++;
            }
            return (32 - leadingZeros);
        }

        /// <summary>
        /// Count the number of consecutive leading zero bits, starting at the most significant bit (bit 63) of x.
        /// </summary>
        /// <param name="thread">The thread.</param>
        /// <param name="val">The value.</param>
        /// <returns>Returns a value between 0 and 64 inclusive representing the number of zero bits.</returns>
        public static int clzll(this GThread thread, long val)
        {
            int leadingZeros = 0;
            while (val != 0)
            {
                val = val >> 1;
                leadingZeros++;
            }
            return (64 - leadingZeros);
        }
#warning TODO Not working for all cases
        /// <summary>
        /// Calculate the least significant 32 bits of the product of the least significant 24 bits of x and y. The high order 8 bits of x and y are ignored.
        /// </summary>
        /// <param name="thread">The thread.</param>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        public static int mul24(this GThread thread, int x, int y)
        {
            bool xsigned = ((0x800000 & x) == 0x800000);
            bool ysigned = ((0x800000 & y) == 0x800000);
            x = (0x7FFFFF & x) * (xsigned ? -1 : 1);
            y = (0x7FFFFF & y) * (ysigned ? -1 : 1);
            long product = x * y;
            long truncproduct = product & 0x7FFFFF;
            return (int)truncproduct;
            //int product = (int)(0x7FFFFF & x) * (int)(0x7FFFFF & y);
            //int p2 = (product & 0x7FFFFFFF);
            //if (product < 0)
            //    p2 = p2 * -1;
            //return (int)p2;
        }

                /// <summary>
        /// Calculate the least significant 32 bits of the product of the least significant 24 bits of x and y. The high order 8 bits of x and y are ignored.
        /// </summary>
        /// <param name="thread">The thread.</param>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        public static uint umul24(this GThread thread, uint x, uint y)
        {
            uint product = (uint)(0xFFFFFF & x) * (uint)(0xFFFFFF & y);
            return product;
        }

        /// <summary>
        /// Calculate the most significant 64 bits of the 128-bit product x * y, where x and y are 64-bit integers.
        /// </summary>
        /// <param name="thread">The thread.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>Returns the most significant 64 bits of the product x * y.</returns>
        public static long mul64hi(this GThread thread, long x, long y)
        {
#if !NET35
            BigInteger product = BigInteger.Multiply(x, y);
            product = product >> 64;
            long l = (long)product;
            return l;
#else
            throw new NotSupportedException();
#endif
        }

        /// <summary>
        /// Calculate the most significant 32 bits of the 64-bit product x * y, where x and y are 32-bit integers.
        /// </summary>
        /// <param name="thread">The thread.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>Returns the most significant 32 bits of the product x * y.</returns>
        public static int mulhi(this GThread thread, int x, int y)
        {
            long product = (long)x * (long)y;
            product = product >> 32;
            long l = (long)product;
            return (int)l;
        }

        /// <summary>
        /// Calculate the most significant 64 bits of the 128-bit product x * y, where x and y are 64-bit integers.
        /// </summary>
        /// <param name="thread">The thread.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>Returns the most significant 64 bits of the product x * y.</returns>
        public static ulong umul64hi(this GThread thread, ulong x, ulong y)
        {
#if !NET35
            BigInteger product = BigInteger.Multiply(x, y);
            product = product >> 64;
            ulong l = (ulong)product;
            return l;
#else
            throw new NotSupportedException();
#endif
        }

        /// <summary>
        /// Calculate the most significant 32 bits of the 64-bit product x * y, where x and y are 32-bit integers.
        /// </summary>
        /// <param name="thread">The thread.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>Returns the most significant 32 bits of the product x * y.</returns>
        public static uint umulhi(this GThread thread, uint x, uint y)
        {
            ulong product = (ulong)x * (ulong)y;
            product = product >> 32;
            ulong l = (ulong)product;
            return (uint)l;
        }
    }
}