/*    
*    RayTracing.cs
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
    public class RayTracing : ExampleBase // Based on CUDA By Example by Jason Sanders and Edward Kandrot
    {
        struct Sphere
        {
            public float r, g, b;
            public float radius;
            public float x, y, z;

            public void print()
            {
                //Console.Write("[(" + r + "," + g + "," + b + ")/" + radius + "/(" + x + "," + y + "," + z + ")]");
                Console.Write("[(" + r + ")/" + radius + "/(" + x + "," + y + "," + z + ")]");
            }
        }

        Sphere[] sphere;
        float[,] bitmap;

        protected override void setup()
        {
            // for nice rendering of the examples
            //this.sizeX = Math.Max(this.sizeX, 40);
            //this.sizeY = Math.Max(this.sizeY, 80);
            //this.sizeZ = Math.Max(this.sizeZ, 100);

            bitmap = new float[sizeX, sizeY];

            sphere = new Sphere[sizeZ];

            for (int i = 0; i < sizeZ; i++)
            {
                sphere[i] = new Sphere();

                sphere[i].r = (float)Random.NextDouble();
                sphere[i].g = (float)Random.NextDouble();
                sphere[i].b = (float)Random.NextDouble();

                sphere[i].x = (float)(Random.NextDouble() * 100.0f - 50.0f);
                sphere[i].y = (float)(Random.NextDouble() * 100.0f - 50.0f);
                sphere[i].z = (float)(Random.NextDouble() * 100.0f - 50.0f);

                sphere[i].radius = (float)(Random.NextDouble() * 10.0f + 2.0f);
            }
        }

        protected override void printInput()
        {
            for (int i = 0; i < Math.Min(sizeZ, 10); i++)
                sphere[i].print();

            Console.WriteLine();
        }

        public float hit(float x, float y, float z, float radius, float ox, float oy, ref float n)
        {
            float dx = ox - x;
            float dy = oy - y;

            if (dx * dx + dy * dy < radius * radius)
            {
                float dz = (float)Math.Sqrt(radius * radius - dx * dx - dy * dy);
                n = (float)(dz / Math.Sqrt(radius * radius));
                return dz + z;
            }

            return (float)-3.40282347E+38; // float.MinValue;
        }

        protected override void algorithm()
        {
            Parallel.For(ExecuteOn, 0, sizeX, 0, sizeY, delegate(int x, int y)
            {
                float ox = x - sizeX / 2.0f;
                float oy = y - sizeY / 2.0f;

                float r = 0.0f;
                float g = 0.0f;
                float b = 0.0f;

                float maxz = float.MinValue;

                for (int i = 0; i < sizeZ; i++)
                {
                    float n = 0.0f;
                    float t = hit(sphere[i].x, sphere[i].y, sphere[i].z, sphere[i].radius, ox, oy, ref n);

                    if (t > maxz)
                    {
                        float fscale = n;
                        r = sphere[i].r * fscale;
                        g = sphere[i].g * fscale;
                        b = sphere[i].b * fscale;
                    }
                }

                bitmap[x, y] = r * 256 *256 * 256 + g * 256 + b;
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
                    float ox = x - sizeX / 2.0f;
                    float oy = y - sizeY / 2.0f;

                    float r = 0.0f;
                    float g = 0.0f;
                    float b = 0.0f;

                    float maxz = float.MinValue;

                    for (int i = 0; i < sizeZ; i++)
                    {
                        float n = 0.0f;
                        //float t = sphere[i].hit(ox, oy, ref n);
                        float t = hit(sphere[i].x, sphere[i].y, sphere[i].z, sphere[i].radius, ox, oy, ref n);

                        if (t > maxz)
                        {
                            float fscale = n;
                            r = sphere[i].r * fscale;
                            g = sphere[i].g * fscale;
                            b = sphere[i].b * fscale;
                        }
                    }

                    if (Math.Abs(bitmap[x, y] - (r * 256 *256 * 256 + g * 256 + b)) > 1)
                        return false;
                }

            return true;
        }

        protected override void cleanup()
        {
            sphere = null;
            bitmap = null;
        }
    }
}
