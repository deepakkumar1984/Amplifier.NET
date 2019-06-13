/*    
*    Merge.cs
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
    public class Merge: ExampleBase
    {
        int[] input1, input2;
        int[] output;

        protected override void setup()
        {
            if (sizeX > 500000) sizeX = 500000;
            input1 = new int[sizeX];
            input2 = new int[sizeX];
            output = new int[2 * sizeX];

            for (int x = 0; x < sizeX; x++)
            {
                input1[x] = Random.Next();
                input2[x] = Random.Next();
            }
            Array.Sort(input1);
            Array.Sort(input2);
        }

        protected override void printInput()
        {
            printField(input1, sizeX);
            printField(input2, sizeX);
        }

        // TODO find a parallel implementation
        protected override void algorithm()
        {
            int i, j, k;
            i = j = k = 0;

            while (i < sizeX && j < sizeX)
            {
                if (input1[i] < input2[j])
                    output[k++] = input1[i++];
                else if (input1[i] > input2[j])
                    output[k++] = input2[j++];
                else // (input1[i] == input2[j])
                {
                    output[k++] = input1[i++];
                    //j++;
                }
            }

            // add missing elements to output
            while (i < sizeX)
                output[k++] = input1[i++];

            while(j < sizeX)
                output[k++] = input2[j++];
        }

        protected override void printResult()
        {
            printField(output, 2 * sizeX);
        }

        protected override bool isValid()
        {
            // output needs to contain all values of input{1,2}
            // this validation takes forever, because it is in O(n^2)
            for (int x = 0; x < sizeX; x++)
                if (System.Array.IndexOf(output, input1[x])<0 || System.Array.IndexOf(output,input2[x])<0)
                    return false;

            // output needs to be sorted 
            for (int x = 0; x < 2 * sizeX - 1; x++)
                if (output[x] > output[x + 1])
                    return false;

            return true;
        }

        protected override void cleanup()
        {
            input1 = null;
            input2 = null;
            output = null;
        }
    }
}
