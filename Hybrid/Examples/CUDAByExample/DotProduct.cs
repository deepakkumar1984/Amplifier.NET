/*    
*    DotProduct.cs
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
    public class DotProduct : ExampleBase // Based on CUDA By Example by Jason Sanders and Edward Kandrot
    {
        float[] a;
        float[] b;
        float result;

        protected override void setup()
        {
             
            int maximumSize = 134217728;
            if (sizeX * sizeY > maximumSize)
            {
                sizeX = maximumSize / 1024;
                sizeY = 1024;
            }
            a = new float[sizeX * sizeY];
            b = new float[sizeX * sizeY];

            for (int i = 0; i < sizeX * sizeY; i++)
            {
                a[i] = i;
                b[i] = i * 2;
            }
        }

        protected override void printInput()
        {
            printField(a, sizeX * sizeY);
            Console.WriteLine();
            printField(b, sizeX * sizeY);
        }

        protected override void algorithm()
        {
            float[] temp = new float[sizeX];

            // TODO the book also provides a Multi-GPU version
            // How can we support that also? Explicit? Automatic?
            Parallel.For(ExecuteOn, 0, sizeX, delegate(int x)
            {
                temp[x] = 0.0f;
                int tid = x;

                for (int y = 0; y < sizeY; y++)
                {
                    temp[x] += a[tid] * b[tid];
                    tid += sizeX;
                }
            });

            result = 0.0f;

            // apply reduction here
            for (int x = 0; x < sizeX; x++)
                result += temp[x];
        }

        protected override void printResult()
        {
            Console.WriteLine(result);
        }

        protected override bool isValid()
        {
            float result = 0.0f;

            for (int i = 0; i < sizeX * sizeY; i++)
                result += a[i] * b[i];

            if (this.result / result < 0.99999f && this.result - result > 0.0001f)
                return false;
            else
                return true;
        }

        protected override void cleanup()
        {
            a = null;
            b = null;
        }
    }
}
