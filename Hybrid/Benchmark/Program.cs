/*    
*    Program.cs
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

using Hybrid.Examples;
using Hybrid.Examples.CudaByExample;
using Hybrid.Examples.Upcrc2010;
using Hybrid.Examples.Upcrc2010.MatrixMultiplication;
using Hybrid.Examples.Functionality;
using System.Threading;
using System.Globalization;

namespace Hybrid.Benchmark
{
    public class Program
    {
        int rounds;
        int warmup_rounds;

        bool print;

        TextWriter csv;
        TextWriter log;

        static void Main(string[] args)
        {
            Environment.SetEnvironmentVariable("CL_LOG_ERRORS", "stdout");

            new Program().RunBenchmark();

            Console.WriteLine("Press ENTER to quit...");
            Console.ReadLine();
        }

        private void logWrite(string text)
        {
            Console.Write(text);
            log.Write(text);
            log.Flush();
        }

        private void logWriteLine()
        {
            logWriteLine("");
        }

        private void logWriteLine(string text)
        {
            Console.WriteLine(text);
            log.WriteLine(text);
            log.Flush();
        }

        static TextWriter errorLog()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

            return File.CreateText(uniqueFileName() + ".log");
        }

        static TextWriter evaluationLog()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

            TextWriter tw = File.CreateText(uniqueFileName() + ".csv");
            tw.WriteLine("alogrithm;scaleX;scaleY;scaleZ;AbsSerial;AbsCPUs;AbsGPU;AbsAll;SpdUpSerial;SpdUpCPUs;SpdUpGPU;SpdUpAll;");

            return tw;
        }

        private static string uniqueFileName()
        {
            return "Evaluation_"
                + System.Environment.MachineName + "_"
                + DateTime.Now.ToShortDateString() + "_"
                + DateTime.Now.ToShortTimeString().Replace(":", ".") + "."
                + DateTime.Now.Second;
        }

        static List<ExampleBase> examples()
        {
            return new List<ExampleBase>(new ExampleBase[]{
               
 
                //CUDAByExample
                //-----------------------------------------
                //new Average(),
                //new DotProduct(),
                //new HeatTransfer(),
                //new Histogram(),
                
                // not reliable
                //new JuliaSet(),

                // no gpu-support (subcall)
                //new RayTracing(),
                   
                //new Ripple(),
                //new SummingVectors(),

                //-----------------------------------------

                //Function Tests
                //-----------------------------------------
                
                //new AtomicExample(),
                //new Lists(),
                //new LocalFunctionCall(),
                //new StaticFunctionCall(),
                //new RandomExample(),
                //new Switch(),
                //new ForLoop(),
                //new MathematicalFunctions(),
                
                //-----------------------------------------

                //FurtherExamples
                //-----------------------------------------
                
                //Sudoku Validator
                //new SudokuValidator(),
                //new SudokuValidator2D(),
                
                //new SudokuValidatorInvalidColumn(),
                //new SudokuValidatorInvalidNumbers(),
                //new SudokuValidatorInvalidRow(),
                //new SudokuValidatorInvalidSubfield(),
                
                
                //new Sum(),
                
                // Big Examples
                
                //new Crypt3(),
                //new GameOfLife(),
                //new Merge(),
                //new ParGrep(),
                //new Wator(),
                
                

                //-----------------------------------------

                //UPCRC2010
                //-----------------------------------------
                
                //Matrix Multiplication
                
                new MatrixMultiplication0(),
                //new MatrixMultiplication1(),
                //new MatrixMultiplication2(),
                //new MatrixMultiplication3(),
                //new MatrixMultiplication4(),
                //new MatrixMultiplication5(),
               

                //Other Examples
                
                //new Convolution(),
                //new MatrixVectorMultiplication(),
                //new MinimumSpanningTree(),
                //new PrefixScan(),
                //new ConvolutionNPP(),

                // not Gpu-enabled
                //new QuickSort(),

                             
            });
        }

        private void RunBenchmark()
        {
            csv = evaluationLog();
            log = errorLog();
            try
            {
                logWriteLine(SystemCharacteristics.ToString());

                //for (int i = 3; i < 10; i++)
                //    benchmark(4);
                    benchmark(10);
            }
            catch (TypeInitializationException e)
            {
                Exception handledException = e;
                while (handledException.InnerException != null)
                {
                    handledException = handledException.InnerException;
                }
                logWriteLine(handledException.GetType().ToString() + ":" + handledException.Message);
 
            }
            csv.Close();
            log.Close();
        }

        private void benchmark(double minSequentialExecutionTime)
        {
            rounds = 5;
            warmup_rounds = 5;

            print = false;

            


            foreach (ExampleBase example in examples())
            {
                double sizeFactor = 0.0;
                try
                {
                    sizeFactor = SystemCharacteristics.GetScale(example, minSequentialExecutionTime);
                }
                catch (Exception exception)
                {
                    logWriteLine(exception.StackTrace);
                    logWriteLine(example.Name + "is skipped due to a scale error.");
                    continue;
                }
                runExample(example, sizeFactor);
                //runExampleGpuAndAutomaticOnly(example, minSequentialExecutionTime);

            }
                
        }

        private void runExampleGpuAndAutomaticOnly(ExampleBase example, double sizeFactor)
        {
            ExampleBase.RunResult runResultSerial = new ExampleBase.RunResult();
            {
                runResultSerial.Valid = true;
                runResultSerial.ElapsedTotalSeconds = 1;
                runResultSerial.SizeX = sizeFactor;
                runResultSerial.SizeY = sizeFactor;
                runResultSerial.SizeZ = sizeFactor;
                runResultSerial.Name = example.Name;
            }; //executeSerial(example, sizeFactor);
            ExampleBase.RunResult runResultParallel = executeParallel(example, sizeFactor);
            ExampleBase.RunResult runResultGPU = executeGpu(example, sizeFactor);
            ExampleBase.RunResult runResultAutomatic = new ExampleBase.RunResult();
            {
                runResultAutomatic.Valid = false;
                runResultAutomatic.ElapsedTotalSeconds = -1;
            }; //executeAutomatic(example, sizeFactor);

            logWriteLine();

            writeOutputs(example, runResultSerial, runResultParallel, runResultGPU, runResultAutomatic);
        }

        private void runExample(ExampleBase example, double sizeFactor)
        {
            ExampleBase.RunResult runResultSerial = executeSerial(example, sizeFactor);
            ExampleBase.RunResult runResultParallel = executeParallel(example, sizeFactor);
            ExampleBase.RunResult runResultGPU = executeGpu(example, sizeFactor);
            ExampleBase.RunResult runResultAutomatic = executeAutomatic(example, sizeFactor);

            logWriteLine();

            writeOutputs(example, runResultSerial, runResultParallel, runResultGPU, runResultAutomatic);
            //writeOutputs(example, runResultGPU, runResultGPU, runResultGPU, runResultGPU);
        }

        private ExampleBase.RunResult executeAutomatic(ExampleBase example, double sizeFactor)
        {
            logWrite("[Automatic] ");
            ExampleBase.RunResult runResultAutomatic = runExample(example, Execute.OnEverythingAvailable, sizeFactor);
            Parallel.ReInitialize();

            if (runResultAutomatic == null)
            {
                runResultAutomatic = new ExampleBase.RunResult();
                {
                    runResultAutomatic.Valid = false;
                    runResultAutomatic.ElapsedTotalSeconds = -1;
                };
            }
            return runResultAutomatic;
        }

        private ExampleBase.RunResult executeGpu(ExampleBase example, double sizeFactor)
        {
            const bool CpuViaOpenCL = true;

            ExampleBase.RunResult runResultGPU = null;
            if (!SystemCharacteristics.Platform.ContainsAGpu && !CpuViaOpenCL)
                logWriteLine("[GPU]       No GPUs available!");
            else
            {
                logWrite("[GPU]       ");
                runResultGPU = runExample(example, Execute.OnSingleGpu, sizeFactor);
                Parallel.ReInitialize();
            }

            if (runResultGPU == null)
            {
                runResultGPU = new ExampleBase.RunResult();
                {
                    runResultGPU.Valid = false;
                    runResultGPU.ElapsedTotalSeconds = -1;
                };
            }
            return runResultGPU;
        }

        private ExampleBase.RunResult executeParallel(ExampleBase example, double sizeFactor)
        {
            logWrite("[Parallel]  ");
            ExampleBase.RunResult runResultParallel = runExample(example, Execute.OnAllCpus, sizeFactor);

            if (runResultParallel == null) {
                runResultParallel = new ExampleBase.RunResult();
                {
                    runResultParallel.Valid = false;
                    runResultParallel.ElapsedTotalSeconds = -1;
                };
            }
            return runResultParallel;
        }

        private ExampleBase.RunResult executeSerial(ExampleBase example, double sizeFactor)
        {
            logWrite("[Serial]    ");
            ExampleBase.RunResult runResultSerial = runExample(example, Execute.OnSingleCpu, sizeFactor);

            if (runResultSerial == null) {
                runResultSerial = new ExampleBase.RunResult();
                {
                    runResultSerial.Valid = false;
                    runResultSerial.ElapsedTotalSeconds = -1;
                };
            }
            return runResultSerial;
        }

        private void writeOutputs(ExampleBase example, ExampleBase.RunResult runResultSerial, ExampleBase.RunResult runResultParallel, ExampleBase.RunResult runResultGPU, ExampleBase.RunResult runResultAutomatic)
        {
            reasonablyEqual(runResultSerial, runResultParallel);
            reasonablyEqual(runResultSerial, runResultGPU);
            reasonablyEqual(runResultSerial, runResultAutomatic);

            double relSerial = runResultSerial.RelativeExecutionTime(runResultSerial.ElapsedTotalSeconds);
            double relCPUs = runResultParallel.RelativeExecutionTime(runResultSerial.ElapsedTotalSeconds);
            double relGPU = runResultGPU.RelativeExecutionTime(runResultSerial.ElapsedTotalSeconds);
            double relAll = runResultAutomatic.RelativeExecutionTime(runResultSerial.ElapsedTotalSeconds);

            csv.WriteLine(example.Name + ";" + runResultSerial.SizeX + ";" + runResultSerial.SizeY + ";" + runResultSerial.SizeZ + ";"
                + runResultSerial.ElapsedTotalSeconds + ";" + runResultParallel.ElapsedTotalSeconds + ";" + runResultGPU.ElapsedTotalSeconds + ";" + runResultAutomatic.ElapsedTotalSeconds + ";"
                + relSerial + ";" + relCPUs + ";" + relGPU + ";" + relAll + ";");
            csv.Flush();
        }

        private void reasonablyEqual(ExampleBase.RunResult one, ExampleBase.RunResult other)
        {
            if (!other.Valid)
                return;

            if (!one.Valid)
            {
                logWriteLine("!!!!!!!!!!!!!!!!!!!!!!");
                logWriteLine("!!!!ONE IS INVALID!!!!");
                logWriteLine("!!!!!!!!!!!!!!!!!!!!!!");
            }

            if (one.SizeX != other.SizeX || one.SizeY != other.SizeY || one.SizeZ != other.SizeZ || one.Name != other.Name)
            {
                logWriteLine("!!!!!!!!!!!!!!!!!!!!!");
                logWriteLine("!!!!INVALID STATE!!!!");
                logWriteLine("!!!!!!!!!!!!!!!!!!!!!");
                throw new Exception("Invalid state!!");
            }
        }

        private ExampleBase.RunResult runExample(ExampleBase example, Execute mode, double sizeFactor)
        {
            example.ExecuteOn = mode;

            System.Threading.Thread.Sleep(100);

            try
            {
                return example.Run(sizeFactor, sizeFactor, sizeFactor, print, rounds, warmup_rounds);
            }
            catch (Exception exception)
            {
                logWriteLine(exception.ToString());
                
                return null;
            }
        }
    }
}
