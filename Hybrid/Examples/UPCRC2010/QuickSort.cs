/*    
*    QuickSort.cs
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

namespace Hybrid.Examples.Upcrc2010
{
    public class QuickSort : ExampleBase // http://myssa.upcrc.illinois.edu/files/Lab_OpenMP_Assignments/
    {
        int[] A;
        int threashold = 200;

        protected override void setup()
        {
            A = new int[sizeX];

            int i, x, y, t;

            for (i = 0; i < sizeX; i++)
                A[i] = i;

            for (i = 0; i < sizeX; i++)
            {
                x = Random.Next(sizeX);
                y = Random.Next(sizeX);

                t = A[y];
                A[y] = A[x];
                A[x] = t;
            }
        }

        protected override void printInput()
        {
            printField(A, sizeX);
        }

        protected override void algorithm()
        {
            quickSort(0, sizeX - 1);
        }

        private void quickSort(int p, int r)
        {
            if (r - p < threashold)
                quickSortSerial(p, r);
            else
            {
                int q = partition(p, r);

                Parallel.Invoke(
                    delegate() { quickSort(p, q - 1); },
                    delegate() { quickSort(q + 1, r); }
                );
            }
        }

        void quickSortSerial(int p, int r)
        {
            if (p < r)
            {
                int q = partition(p, r);
                quickSortSerial(p, q - 1);
                quickSortSerial(q + 1, r);
            }
        }

        int partition(int p, int r)
        {
            int x = A[p];
            int k = p;
            int l = r + 1;
            int t;

            while (true)
            {
                do k++;
                while ((A[k] <= x) && (k < r));

                do l--;
                while (A[l] > x);

                while (k < l)
                {
                    t = A[k]; A[k] = A[l]; A[l] = t;
                    do k++; while (A[k] <= x);
                    do l--; while (A[l] > x);
                }
                t = A[p]; A[p] = A[l]; A[l] = t;
                return l;
            }
        }

        protected override void printResult()
        {
            printField(A, sizeX);
        }

        protected override bool isValid()
        {
            for (int i = 0; i < sizeX - 1; i++)
                if (A[i] > A[i + 1])
                    return false;

            return true;
        }

        protected override void cleanup()
        {
            A = null;
        }
    }
}
