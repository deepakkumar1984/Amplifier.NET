/*    
*    ParGrep.cs
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

namespace Hybrid.Examples
{
    public class ParGrep: ExampleBase
    {
        byte needle;
        byte[] haystack;
        int[] positions;

        protected override void setup()
        {
            haystack = new byte[sizeX];
            positions = new int[sizeX];

            for (int x = 0; x < sizeX; x++)
                haystack[x] = (byte)Random.Next(256);

            needle = (byte)Random.Next(256);
        }

        protected override void printInput()
        {
            printField(haystack, sizeX);
        }

        protected override void algorithm()
        {
            /* Serial Implementation:
             * 
             * for (int x = 0; x < sizeX; x++)
             *    if (haystack[x] == needle)
             *        positions[k++] = x;
             *        
             */

            int k = 0;
            
            Parallel.For(ExecuteOn, 0, sizeX, delegate(int x)
            {
                if (haystack[x] == needle)
                {
                    positions[k] = x;
                    
                    // TODO postpone incrementing of k.
                    // TODO create local results first and insert them afterwards.
                    Atomic.Add(ref k, 1);
                }
            });
        }

        protected override void printResult()
        {
            printField(positions, sizeX);
        }

        protected override bool isValid()
        {
            for (int x = 0; x < sizeX; x++)
            {
                if (positions[x] == 0)
                    break; // reached the end of the results

                if (haystack[positions[x]] != needle)
                    return false;
            }

            return true;
        }

        protected override void cleanup()
        {
            haystack = null;
            positions = null;
        }
    }
}
