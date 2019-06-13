/*    
*    Wator.cs
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
    public class Wator: ExampleBase
    {
        // 0 empty " " (= plancton)
        // 1 shark "s"
        // 2 fish  "f"

        const byte empty = 0;
        const byte fish = 1;
        const byte shark = 2;
        const int breedTimeFish = 2;
        const int breedTimeShark = 4;
        const int sharkStarveTime = 8;

        byte[,] state1;
        int[,] age;
        int[,] starve;
        byte[,] state2;

        protected override void setup()
        {
            this.sizeZ = 10;

            state1 = new byte[sizeX, sizeY];
            state2 = new byte[sizeX, sizeY];
            age = new int[sizeX, sizeY];
            starve = new int[sizeX, sizeY];

            for (int y = 1; y < sizeY - 1; y++)
                for (int x = 1; x < sizeX - 1; x++)
                {
                    state1[x, y] = (byte)Random.Next(3);

                    if (state1[x, y] != 0)
                        age[x, y] = Random.Next(10);
                }
        }

        private void printField(byte[,] fields)
        {
            printField(fields, sizeX, sizeY, delegate(int x, int y)
            {
                if (fields[x,y] == 0)
                    Console.Write(" ");
                else if (fields[x,y] == 1)
                    Console.Write("s");
                else 
                   Console.Write("f");
            });
        }

        protected override void printInput()
        {
            printField(state1);
        }

        protected override void algorithm()
        {
            for (int round = 0; round < sizeZ; round++)
            {
                Parallel.For(ExecuteOn, 1, sizeX - 1, 1, sizeY - 1, delegate(int x, int y)
                {
                    if (state1[x, y] == fish)
                    {
                        if (age[x, y] == breedTimeFish)
                        { // breed another fish, stay where you are
                            breed(x, y, fish);
                        }
                        else
                        { // swim to neighbor field
                            int randNeighbor = Random.Next(4);

                            switch (randNeighbor)
                            {
                                case 0:
                                    swimToField(x, y, x-1, y, fish);
                                    break;
                                case 1:
                                    swimToField(x, y, x, y-1, fish);
                                    break;
                                case 2:
                                    swimToField(x, y, x+1, y, fish);
                                    break;
                                case 3:
                                    swimToField(x, y, x, y+1, fish);
                                    break;
                            }
                        }
                    }
                    else if (state1[x, y] == shark)
                    {
                        if (age[x, y] == breedTimeShark)
                        { // breed another shark, stay where you are
                            breed(x, y, shark);
                        }
                        else
                        {
                            bool ate = false;

                            for (int neighbor = 0; neighbor < 4; neighbor++)
                            {
                                if (neighbor == 0)
                                    ate = tryToEatAndSwim(x, y, x - 1, y);
                                else if (neighbor == 1)
                                    ate = tryToEatAndSwim(x, y, x, y-1);
                                else if (neighbor == 2)
                                    ate = tryToEatAndSwim(x, y, x + 1, y);
                                else if (neighbor == 3)
                                    ate = tryToEatAndSwim(x, y, x, y + 1);

                                if (ate)
                                    break;
                            }

                            if (!ate)
                            {
                                starve[x, y]++; // We have nothing to eat-. So we starve (again).

                                int randNeighbor = Random.Next(4);

                                switch (randNeighbor)
                                {
                                    case 0:
                                        swimToField(x, y, x - 1, y, shark);
                                        break;
                                    case 1:
                                        swimToField(x, y, x, y - 1, shark);
                                        break;
                                    case 2:
                                        swimToField(x, y, x + 1, y, shark);
                                        break;
                                    case 3:
                                        swimToField(x, y, x, y + 1, shark);
                                        break;
                                }
                            }


                        }
                    }
                });

                swap(ref state1, ref state2);
            }
        }

        private bool tryToEatAndSwim(int x, int y, int eatX, int eatY)
        {
            if (state1[eatX, eatY] == fish)
            {
                state2[eatX, eatY] = shark;
                age[eatX, eatY] = age[x, y];
                starve[eatX, eatY] = starve[x, y];

                state2[x, y] = empty;
                age[x, y] = 0;
                starve[x, y] = 0;

                return true;
            }
            else
            {
                return false;
            }
        }
        
        void swimToField(int x, int y, int newX, int newY, byte type)
        {
            if (state1[newX, newY] == empty)
            {
                state2[x, y] = empty;               // leave current field
                state2[newX, newY] = type;          // move fish to new field
                age[newX, newY] = age[x, y] + 1;    // set age of fish to old age + 1
                age[x, y] = 0;                      // an empty field has no age

                if (type == shark)
                { // copy starve time for sharks
                    starve[newX, newY] = starve[x, y];
                    starve[x, y] = 0;
                }
            }
            else
            {
                age[x, y]++;    // stay where you are since selected field is not empty
            }
        }

        private void breed(int x, int y, byte type)
        {
            int breedPosition = Random.Next(4);

            switch (breedPosition)
            {
                case 0:
                    state2[x - 1, y] = type;
                    break;
                case 1:
                    state2[x, y - 1] = type;
                    break;
                case 2:
                    state2[x + 1, y] = type;
                    break;
                case 3:
                    state2[x, y + 1] = type;
                    break;
            }

            age[x, y]++; // after breeding you get older
        }

        protected override void printResult()
        {
            printField(state2);
        }

        protected override bool isValid()
        {
            return true;
        }

        protected override void cleanup()
        {
            state1 = null;
            state2 = null;
            age = null;
            starve = null;
        }
    }
}
