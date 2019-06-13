/*    
*    SummingVectors.cs
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
    public class SummingVectors : ExampleBase // Based on CUDA By Example by Jason Sanders and Edward Kandrot
    {
        int[] a;
        int[] b;
        int[] c;

        protected override void setup()
        {
            if (sizeX > 16777216 || sizeX < 0)
                sizeX = 16777216;

            a = new int[sizeX];
            b = new int[sizeX];
            c = new int[sizeX];

            for (int i = 0; i < sizeX; i++)
            {
                a[i] = -i;
                b[i] = i * i;
            }
        }

        protected override void printInput()
        {
            printField(a, sizeX);
            Console.WriteLine();
            printField(b, sizeX);
        }

        protected override void algorithm()
        {
            Parallel.For(ExecuteOn, 0, sizeX, delegate(int tid)
            {
                c[tid] = a[tid] + b[tid];
            });
        }

        protected override void printResult()
        {
            printField(c, sizeX);

        }

        protected override bool isValid()
        {
            for (int i = 0; i < sizeX; i++)
                if (c[i] != a[i] + b[i])
                    return false;

            return true;
        }

        protected override void cleanup()
        {
            a = null;
            b = null;
            c = null;
        }
    }
}
