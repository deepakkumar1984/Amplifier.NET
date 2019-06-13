/*    
*    ExampleBase.cs
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
using System.IO;

namespace Hybrid.Examples
{
    public abstract class ExampleBase
    {
        public class RunResult
        {
            public string Name;
            public Execute ExecutedOn;

            public double SizeX;
            public double SizeY;
            public double SizeZ;

            public bool Valid;
            public double ElapsedTotalSeconds;

            public double RelativeExecutionTime(double other)
            {
                if (!Valid)
                    return -1;

                return other / this.ElapsedTotalSeconds;
            }
        }

        public Execute ExecuteOn = Execute.OnEverythingAvailable;

        protected Random random = new Random();

        protected int sizeX = 0;
        protected int sizeY = 0;
        protected int sizeZ = 0;

        public string Name { get { return this.GetType().Name; } }

        public RunResult Run(double preferredExecutionTime, int rounds, int warmupRounds)
        {
            double size = FindAppropriateSize(preferredExecutionTime);
            return Run(size, size, size, false, rounds, warmupRounds);
        }

        Dictionary<Execute, Dictionary<double, double>> FindAppropriateSize_Cache = new Dictionary<Execute, Dictionary<double, double>>();

        public double FindAppropriateSize(double preferredExecutionTime)
        {
            if (!FindAppropriateSize_Cache.ContainsKey(ExecuteOn))
                FindAppropriateSize_Cache.Add(ExecuteOn, new Dictionary<double, double>());

            if (!FindAppropriateSize_Cache[ExecuteOn].ContainsKey(preferredExecutionTime))
                FindAppropriateSize_Cache[ExecuteOn][preferredExecutionTime] = findApppropriateSize(preferredExecutionTime);

            return FindAppropriateSize_Cache[ExecuteOn][preferredExecutionTime];
        }

        private double findApppropriateSize(double preferredExecutionTime)
        {
            double scale = 5;

            double executionTime = 0;
            double executionTime2 = 0;

            while(executionTime >= executionTime2)
            {
                executionTime = Run(scale, scale, scale, false, 20, 5).ElapsedTotalSeconds;
                while (executionTime <= 0.001)
                {
                    scale *= 2;
                    executionTime = Run(scale, scale, scale, false, 20, 5).ElapsedTotalSeconds;
                }

                executionTime2 = Run(scale * 2, scale * 2, scale * 2, false, 20, 5).ElapsedTotalSeconds;
            }
            double log = Math.Log(executionTime2 / executionTime, 2);
            scale = Math.Pow(preferredExecutionTime / executionTime, 1 / log) * scale;

            // executionTime = example.Run(scale, scale, scale, false, 20, 5).ElapsedTotalSeconds;

            return scale;
        }

        public RunResult Run(double size, bool print, int rounds)
        {
            return Run(size, size, size, print, rounds, 0);
        }

        public RunResult Run(double size, bool print, int rounds, int warmupRounds)
        {
            return Run(size, size, size, print, rounds, warmupRounds);
        }


        public RunResult Run(double sizeX, double sizeY, double sizeZ, bool print, int rounds, int warmupRounds)
        {
            Console.Write("Running " + this.GetType().Name);

            if (sizeX > int.MaxValue)
                sizeX = int.MaxValue;
            else
                this.sizeX = (int)sizeX;

            if (sizeY > int.MaxValue)
                sizeY = int.MaxValue;
            else
                this.sizeY = (int)sizeY;

            if (sizeZ > int.MaxValue)
                sizeZ = int.MaxValue;
            else
                this.sizeZ = (int)sizeZ;

            setup();

            Console.Write("(" + this.sizeX + " / " + this.sizeY + " / " + this.sizeZ + ")...");

            if (print)
            {
                Console.WriteLine();
                Console.WriteLine("Input:");
                printInput();
            }

            for (int warmupRound = 0; warmupRound < warmupRounds; warmupRound++)
                algorithm();

            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            for (int round = 0; round < rounds; round++)
                algorithm();
            watch.Stop();

            for (int warmupRound = 0; warmupRound < warmupRounds; warmupRound++)
                algorithm();

            if (print)
            {
                Console.WriteLine();
                Console.WriteLine("Result:");
                printResult();
            }

            bool valid = checkResult(print);

            cleanup();

            Console.WriteLine("Done in " + watch.Elapsed.TotalSeconds + "s. " + (valid ? "SUCCESS" : "<!!! FAILED !!!>"));

            RunResult result = new RunResult();
			result.Name = this.GetType().Name;
			result.ExecutedOn = ExecuteOn;
			result.SizeX = this.sizeX;
            result.SizeY = this.sizeY;
            result.SizeZ = this.sizeZ;
            result.ElapsedTotalSeconds = watch.Elapsed.TotalSeconds;
            result.Valid = valid;
			return result;
        }

        protected abstract void setup();
        protected abstract void printInput();
        protected abstract void algorithm();
        protected abstract void printResult();
        protected abstract void cleanup();

        protected bool checkResult(bool throwOnError)
        {
            if (!isValid())
            {
                if (throwOnError)
                {
                    throw new Exception("Calculated Result is not valid.");  // this line gets annoying after some time...
                }
                return false;
            }

            return true;
        }

        protected abstract bool isValid();

        protected string doubleToString(double value)
        {
            return String.Format("{0:0.00}", value).Substring(0, 4);
        }

        protected void swap(ref byte[,] a, ref byte[,] b)
        {
            byte[,] tmp = a;
            a = b;
            b = tmp;
        }

        protected void printField(double[,] field, int sizeX, int sizeY)
        {
            for (int i = -1; i <= sizeX; i++)
            {
                for (int j = -1; j <= sizeY; j++)
                {
                    if (j == -1 || j == sizeY || i == -1 || i == sizeX)
                        Console.Write("**** ");
                    else
                        Console.Write(doubleToString(field[i, j]) + " ");
                }
                Console.WriteLine();
            }
        }

        protected void printField(byte[,] field, int sizeX, int sizeY)
        {
            for (int i = -1; i <= sizeX; i++)
            {
                for (int j = -1; j <= sizeY; j++)
                {
                    if (j == -1 || j == sizeY || i == -1 || i == sizeX)
                        Console.Write("**** ");
                    else
                        Console.Write(""+ field[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        protected void printField(byte[,] fields, int sizeX, int sizeY, Action<int, int> printAction)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                    printAction.Invoke(x, y);
                Console.WriteLine();
            }
        }

        protected void printField(float[] field, int sizeX)
        {
            for (int i = 0; i < Math.Min(sizeX, 80); i++)
                Console.Write(doubleToString(field[i]) + " ");

            if (sizeX > 80)
                Console.Write("...");

            Console.WriteLine();
        }

        protected void printField(byte[] field, int sizeX)
        {
            int[] intArray = new int[sizeX];

            for (int i = 0; i < sizeX; i++)
                intArray[i] = field[i];

            printField(intArray, sizeX);
        }

        protected void printField(int[] field, int sizeX)
        {
            for (int i = 0; i < Math.Min(sizeX, 80); i++)
                Console.Write(field[i] + " ");

            if (sizeX > 80)
                Console.Write("...");

            Console.WriteLine();
        }

        protected void paintField(float[,] bitmap)
        {
            for (int x = 0; x < sizeX; x++)
                for (int y = 0; y < sizeY; y++)
                {
                    paintValue(bitmap[x, y], 0.0f, 32.0f, ' ');
                    paintValue(bitmap[x, y], 32.0f, 64.0f, '.');
                    paintValue(bitmap[x, y], 64.0f, 96.0f, '°');
                    paintValue(bitmap[x, y], 96.0f, 128.0f, ':');
                    paintValue(bitmap[x, y], 128.0f, 160.0f, '+');
                    paintValue(bitmap[x, y], 160.0f, 192.0f, '*');
                    paintValue(bitmap[x, y], 192.0f, 224.0f, '#');
                    paintValue(bitmap[x, y], 224.0f, 256.0f, '8');
                }
        }

        protected void paintValue(double value, double fromInclusive, double toExclusive, char output)
        {
            if (value >= fromInclusive && value < toExclusive)
                Console.Write(output);
        }

        public override bool Equals(object obj)
        {
            return this.GetType() == obj.GetType();
        }

        public override int GetHashCode()
        {
            return this.GetType().GetHashCode();
        }

        public override string ToString()
        {
            return this.GetType().ToString();
        }
    }
}
