/*    
*    Average.cs
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
    public class Average : ExampleBase // Based on CUDA By Example by Jason Sanders and Edward Kandrot
    {
        float[] a;
        float[] b;
        float[] c;

        protected override void setup()
        {
            if (sizeX > Int32.MaxValue || sizeX < 0)
                sizeX = Int32.MaxValue;
          
            a = new float[sizeX];
            for (int i = 0; i < sizeX; i++)
                a[i] = Random.Next(0, 1000);

            b = new float[sizeX];
            for (int i = 0; i < sizeX; i++)
                b[i] = Random.Next(0, 1000);

            c = new float[sizeX];
        }

        protected override void printInput()
        {
            printField(a, sizeX);
            Console.WriteLine();
            printField(b, sizeX);
        }

        protected override void algorithm()
        {
            Parallel.For(ExecuteOn, 0, sizeX, delegate(int idx)
            {
                int idx1 = (idx + 1) % sizeX;
                int idx2 = (idx + 2) % sizeX;

                float _as = (a[idx] + a[idx1] + a[idx2])/3.0f;
                float bs = (b[idx] + b[idx1] + b[idx2])/3.0f;

                c[idx] = (_as + bs) / 2.0f;
            });
        }

        protected override void printResult()
        {
            printField(c, sizeX);
        }

        protected override bool isValid()
        {
            for (int idx = 0; idx < sizeX; idx++)
            {
                int idx1 = (idx + 1) % sizeX;
                int idx2 = (idx + 2) % sizeX;

                float _as = (a[idx] + a[idx1] + a[idx2]) / 3.0f;
                float bs = (b[idx] + b[idx1] + b[idx2]) / 3.0f;

                if(c[idx] - (_as + bs) / 2.0f > 0.0001f) // TODO Understand where the difference comes from
                    return false;
            }

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
