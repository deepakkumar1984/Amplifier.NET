/*    
*    Random.cs
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
using Hybrid.MsilToOpenCL;

namespace Hybrid
{
    public class Random
    {
        [ThreadStatic]
        static System.Random staticRandom = null;

        public static void Seed(int seed)
        {
            staticRandom = new System.Random(seed);
        }

        public int InstanceNext()
        {
            return Next();
        }

        public static int Next()
        {
            return (int)NextUInt();
        }

        public static int Next(int maxValue)
        {
            return (int)(NextDouble() * maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return (int)(minValue + (maxValue-minValue)*NextDouble());
        }

        public static void NextBytes(byte[] array)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = (byte)(NextDouble() * 255);
        }

        public static double NextDouble()
        {
            return (double)Random.NextUInt()/((double)(uint.MaxValue)+1.0);
        }

        [OpenClAlias("MWC64X")]
        public static uint NextUInt()
        {
            return (uint)GetStaticRandom().Next();
        }

        private static System.Random GetStaticRandom()
        {
            System.Random rnd = staticRandom;
            if (object.ReferenceEquals(rnd, null))
            {
                // This is adapted from
                // http://blog.codeeffects.com/Article/Generate-Random-Numbers-And-Strings-C-Sharp

                byte[] b = new byte[4];
                new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
                int seed = (b[0] & 0x7f) << 24 | b[1] << 16 | b[2] << 8 | b[3];
                rnd = staticRandom = new System.Random(seed);
            }
            return rnd;
        }


        // The following method is adapted from 
        // http://blog.codeeffects.com/Article/Generate-Random-Numbers-And-Strings-C-Sharp

        public static int GetRandomNumber(int maxNumber)
        {
            if (maxNumber < 1)
                throw new System.Exception("The maxNumber value should be greater than 1");
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            int seed = (b[0] & 0x7f) << 24 | b[1] << 16 | b[2] << 8 | b[3];
            System.Random r = new System.Random(seed);
            return r.Next(1, maxNumber);
        }
    }
}
