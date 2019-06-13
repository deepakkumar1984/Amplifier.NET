/*    
*    MatrixMultiplicationBase.cs
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

namespace Hybrid.Examples.Upcrc2010.MatrixMultiplication
{
    public abstract class MatrixMultiplicationBase : ExampleBase
    {
        protected double[,] a;
        protected double[,] b;

        protected double[,] c;

        protected override void setup()
        {
            a = new double[sizeX, sizeZ];
            
            for (int i = 0; i < sizeX; i++)
                for (int j = 0; j < sizeZ; j++)
                    a[i, j] = Random.NextDouble();

            b = new double[sizeZ, sizeY];

            for (int i = 0; i < sizeZ; i++)
                for (int j = 0; j < sizeY; j++)
                    b[i, j] = Random.NextDouble();

            c = new double[sizeX, sizeY];
        }

        protected override void printInput()
        {
            printField(a, sizeX, sizeZ);
            printField(b, sizeZ, sizeY);
        }

        protected override void printResult()
        {
            printField(c, sizeX, sizeY);
        }

        protected override bool isValid()
        {
            // http://www.codeproject.com/KB/cs/aforge_parallel.aspx

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    double v = 0;

                    for (int k = 0; k < sizeZ; k++)
                        v += a[i, k] * b[k, j];

                    if (Math.Abs(c[i, j] - v) > 0.00000000005f)
                        return false;
                }
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
