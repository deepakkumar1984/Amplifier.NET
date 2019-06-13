/*    
*    MatrixMultiplication4.cs
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
    public class MatrixMultiplication4 : MatrixMultiplicationBase
    {
        protected override void algorithm() // http://myssa.upcrc.illinois.edu/files/Lab_OpenMP_Assignments/
        {
            int m = sizeX;
            int n = sizeY;
            int p = sizeZ;

            int ITILE3 = 32;
            int JTILE3 = 32;
            int KTILE3 = 32;

            Parallel.For(ExecuteOn, 0, m, delegate(int i)
            {
                for (int j = 0; j < n; j++)
                    c[i, j] = 0;
            });

			/*
            for (int ii = 0; ii < m; ii += ITILE3)
            {
                int il = Math.Min(ii + ITILE3, m);
                for (int jj = 0; jj < n; jj += JTILE3)
                {
                    int jl = Math.Min(jj + JTILE3, n);

                    Parallel.For(ExecuteOn, 0, (p + KTILE3 - 1) / KTILE3, delegate(int kk)
                    {
                        int kl = Math.Min((kk * KTILE3) + KTILE3, p);

                        for (int i = ii; i < il; i++)
                            for (int j = jj; j < jl; j++)
                                for (int k = (kk * KTILE3); k < kl; k++)
                                    c[i, j] += a[i, k] * b[k, j];
                    });
                }
            }
			*/

			Parallel.For(ExecuteOn, 0, (m + ITILE3 - 1) / ITILE3, 0, (n + JTILE3 - 1) / JTILE3, delegate(int idx_ii, int idx_jj)
			{
				//for (int idx_ii = 0; idx_ii < (m + ITILE3 - 1) / ITILE3; idx_ii++) {
				//  for (int idx_jj = 0; idx_jj < (n + JTILE3 - 1) / JTILE3; idx_jj++) {
				int ii = idx_ii * ITILE3;
				int il = Math.Min(ii + ITILE3, m);

				int jj = idx_jj * JTILE3;
				int jl = Math.Min(jj + JTILE3, n);

				//System.Console.WriteLine("i=[{0:X2},{1:X2})   j=[{2:X2},{3:X2})", ii, il, jj, jl);
				for (int kk = 0; kk < p; kk += KTILE3) {
					int kl = Math.Min(kk + KTILE3, p);

					for (int i = ii; i < il; i++)
						for (int j = jj; j < jl; j++)
							for (int k = kk; k < kl; k++)
								c[i, j] += a[i, k] * b[k, j];
				}
			});
        }
    }
}
