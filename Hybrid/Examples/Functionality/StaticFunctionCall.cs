/*    
*    StaticFunctionCall.cs
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

namespace Hybrid.Examples.Functionality
{
    public class StaticFunctionCall: ExampleBase
    {
        int[] a;
        int[] b;
        int[] c;

        protected override void setup()
        {
            a = new int[sizeX];
            for (int x = 0; x < sizeX; x++)
                a[x] = Random.Next(100);

            b = new int[sizeX];
            for (int x = 0; x < sizeX; x++)
                b[x] = Random.Next(100);

            c = new int[sizeX];
        }

        protected override void printInput()
        {
            printField(a, sizeX);
            printField(b, sizeX);
        }

        protected override void algorithm()
        {
            Parallel.For(ExecuteOn, 0, sizeX, delegate(int i)
            {
                c[i] = theFunction(a[i], b[i]);
            });
        }

        static int theFunction(int a, int b)
        {
            return a + b;
        }

        protected override void printResult()
        {
            printField(c, sizeX);
        }

        protected override bool isValid()
        {
            for (int x = 0; x < sizeX; x++)
                if (a[x] + b[x] != c[x])
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
