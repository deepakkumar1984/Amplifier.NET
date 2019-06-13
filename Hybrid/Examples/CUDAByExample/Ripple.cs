/*    
*    Ripple.cs
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

namespace Hybrid.Examples.CudaByExample
{
    public class Ripple : ExampleBase // Based on CUDA By Example by Jason Sanders and Edward Kandrot
    {
        float[,] bitmap;

        protected override void setup()
        {
            int maximumSize = 67108864;
            while (sizeX * sizeY > maximumSize)
            {
                sizeX = sizeX / 2;
                sizeY = sizeY / 2;
            }
            bitmap = new float[sizeX, sizeY];
        }

        protected override void printInput()
        {
            Console.WriteLine("SizeX:" + sizeX + " SizeY:" + sizeY);
        }

        protected override void algorithm()
        {
            Parallel.For(ExecuteOn, 0, sizeX, 0, sizeY, delegate(int x, int y)
            {
                float fx = x - sizeX / 2.0f;
                float fy = y - sizeY / 2.0f;

                float d = (float)Math.Sqrt(fx * fx + fy * fy);

                int ticks = 1;
                float grey = (float)(128.0f + 127.0f * Math.Cos(d / 10.0f - ticks / 7.0f) / (d / 10.0f + 1.0f));

                bitmap[x, y] = grey;
            });
        }

        protected override void printResult()
        {
            paintField(bitmap);
        }

        protected override bool isValid()
        {
            for (int x = 0; x < sizeX; x++)
                for (int y = 0; y < sizeY; y++)
                {
                    float fx = x - sizeX / 2.0f;
                    float fy = y - sizeY / 2.0f;

                    float d = (float)Math.Sqrt(fx * fx + fy * fy);

                    int ticks = 1;
                    float grey = (float)(128.0f + 127.0f * Math.Cos(d / 10.0f - ticks / 7.0f) / (d / 10.0f + 1.0f));

					if (Math.Abs(bitmap[x, y] - grey) > 0.00005f)
                        return false;
                }

            return true;
        }

        protected override void cleanup()
        {
            bitmap = null;
        }
    }
}
