/*    
*    Histogram.cs
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

namespace Hybrid.Examples.CudaByExample
{
    public class Histogram : ExampleBase // Based on CUDA By Example by Jason Sanders and Edward Kandrot
    {
        byte[] buffer;
        int[] histo;

        protected override void setup()
        {
            if (sizeX > Int32.MaxValue || sizeX < 0)
                sizeX = Int32.MaxValue;

            this.sizeZ = 256;

            histo = new int[sizeZ];

            buffer = new byte[sizeX];
            for (int i = 0; i < sizeX; i++)
                buffer[i] = (byte)Random.Next(0, sizeZ);
        }

        protected override void printInput()
        {
            printField((byte[])buffer, sizeX);
        }

        protected override void algorithm()
        {
            histo = new int[sizeZ];

            int[] temp = new int[sizeZ];

            Parallel.For(ExecuteOn, 0, sizeZ, delegate(int thread_id)
            {
                temp[thread_id] = 0;
            });


            Parallel.For(ExecuteOn, 0, sizeY, delegate(int thread_id)
            {
                int i = thread_id;

                while (i < sizeX)
                {
                    Atomic.Add(ref temp[buffer[i]], 1);
                    i += sizeY;
                }
            });

            Parallel.For(ExecuteOn, 0, sizeZ, delegate(int thread_id)
            {
                Atomic.Add(ref histo[thread_id], temp[thread_id]);
            });
        }

        protected override void printResult()
        {
            printField(histo, sizeZ);
        }

        protected override bool isValid()
        {
            for (int i = 0; i < sizeX; i++)
                histo[buffer[i]]--;

            for (int i = 0; i < sizeZ; i++)
                if (histo[i] != 0)
                    return false;

            return true;
        }

        protected override void cleanup()
        {
            buffer = null;
            histo = null;
        }
    }
}
