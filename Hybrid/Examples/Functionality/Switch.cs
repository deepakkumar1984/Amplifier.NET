/*    
*    Switch.cs
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
    // Simple test for switch statement
    // Just fill an input array with random values,
    // Iterate across it and "identify" 0s and 1s using a switch statement.
    public class Switch: ExampleBase
    {
        int[] input;
        int[] output;

        protected override void setup()
        {
            if (sizeX > 16777216)
                sizeX = 16777216;

            input = new int[sizeX];
            output = new int[sizeX];

            for (int i = 0; i < sizeX; i++)
                input[i] = Random.Next(10);
        }

        protected override void printInput()
        {
            printField(input, sizeX);
        }

        protected override void algorithm()
        {
            Parallel.For(ExecuteOn, 0, sizeX, delegate(int x)
            {
                switch (input[x])
                {
                    case 0:
                        output[x] = -1;
                        break;
                    case 1:
                        output[x] = +1;
                        break;
                    default:
                        output[x] = 0;
                        break;
                }
            });
        }

        protected override void printResult()
        {
            printField(output, sizeX);
        }

        protected override bool isValid()
        {
            for (int x = 0; x < sizeX; x++)
            {
                switch (input[x])
                {
                    case 0:
                        if (output[x] != -1)
                            return false;
                        break;
                    case 1:
                        if (output[x] != +1)
                            return false;
                        break;
                    default:
                        if (output[x] != 0)
                            return false;
                        break;
                }
            }
            return true;
        }

        protected override void cleanup()
        {
            input = null;
            output = null;
        }
    }
}
