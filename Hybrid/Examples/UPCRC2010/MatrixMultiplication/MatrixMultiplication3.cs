/*    
*    MatrixMultiplication3.cs
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
    public class MatrixMultiplication3 : MatrixMultiplicationBase
    {
        protected override void algorithm() // http://myssa.upcrc.illinois.edu/files/Lab_OpenMP_Assignments/
        {
            int ITILE2 = 32;
            int JTILE2 = 32;

            Parallel.For(ExecuteOn, 0, sizeX, delegate(int ii)
            {
                for (int jj = 0; jj < sizeY; jj += JTILE2)
                {
                    int il = Math.Min((ii * ITILE2) + ITILE2, sizeX);
                    int jl = Math.Min(jj + JTILE2, sizeY);
                    for (int i = (ii * ITILE2); i < il; i++)
                        for (int j = jj; j < jl; j++)
                        {
                            c[i, j] = 0;
                            for (int k = 0; k < sizeZ; k++)
                                c[i, j] += a[i, k] * b[k, j];
                        }
                }
            });
        }
    }
}
