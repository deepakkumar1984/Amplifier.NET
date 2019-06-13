/*    
*    MinimumSpanningTree.cs
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
    public class MinimumSpanningTree : ExampleBase // http://myssa.upcrc.illinois.edu/files/Lab_OpenMP_Assignments/
    {
        float[,] W;
        int[,] MSTree;

        protected override void setup()
        {
            W = new float[sizeX, sizeX];
            MSTree = new int[sizeX, 2];

            // create random, undirected weight matrix
            int i, j, k;
            for (i = 0; i < sizeX; i++)
            {
                for (j = 0; j < sizeX; j++)
                {
                    W[i, j] = float.MaxValue;
                    W[j, i] = W[i, j];
                }
            }
            for (k = 0; k < 5 * sizeX; k++)
            {
                i = Random.Next(sizeX);
                j = Random.Next(sizeX);
                if (i != j)
                {
                    W[i, j] = (float)(Random.NextDouble() * 10.0);
                    W[j, i] = W[i, j];
                }
            }
        }

        protected override void printInput()
        {
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeX; j++)
                {
                    if (W[i, j] == float.MaxValue)
                        Console.Write("**** ");
                    else
                        Console.Write(doubleToString(W[i, j]) + " ");
                }
                Console.WriteLine();
            }
        }

        protected override void algorithm()
        {
            int[] nearNode = new int[sizeX];
            float[] minDist = new float[sizeX];

            // initialize the minDist array with nodes's distance to first node in tree, node 0
            Parallel.For(ExecuteOn, 1, sizeX, delegate(int i)
            {
                nearNode[i] = 0;
                minDist[i] = W[i, 0];
            });

            int nodeIdx = 0;

            // Add new node, one per iteration, to the existing minimum tree
            for (int i = 0; i < sizeX - 1; i++)
            {
                float min = float.MaxValue;

                // Find node that has smallest distance to a node in the tree
                for (int j = 1; j < sizeX; j++)
                {
                    if (0 <= minDist[j] && minDist[j] < min)
                    {
                        min = minDist[j];
                        nodeIdx = j;
                    }
                }
                MSTree[i, 0] = nearNode[nodeIdx];
                MSTree[i, 1] = nodeIdx;
                minDist[nodeIdx] = -1;   // mark node as being in tree

                // update distances to tree by considering distance to just added node
                for (int j = 1; j < sizeX; j++)
                    if (W[j, nodeIdx] < minDist[j])
                    {
                        minDist[j] = W[j, nodeIdx];
                        nearNode[j] = nodeIdx;
                    }
            }
        }

        protected override void printResult()
        {
            float val = 0.0f;

            Console.WriteLine("Edges in MSTree:");

            for (int i = 0; i < sizeX - 1; i++)
            {
                int j = MSTree[i, 0];
                int k = MSTree[i, 1];
                val += W[j, k];
                Console.Write("(" + i + " " + k + ") ");
            }
            Console.WriteLine("Val = " + val);
        }

        protected override bool isValid()
        {
            // TODO: check result
            return true;
        }

        protected override void cleanup()
        {
            W = null;
            MSTree = null;
        }
    }
}
