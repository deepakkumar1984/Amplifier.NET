using Amplifier;
using AmplifierExamples.Kernels;
using System;

namespace AmplifierExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            IExample example = null;

            //Console.WriteLine("---------------------Basic example---------------------------");
            //example = new SimpleKernelEx();
            //example.Execute();
            //Console.WriteLine("\n---------------------Basic example---------------------------");

            PrintThreeEmptyLines();

            Console.WriteLine("---------------------Array Loop example---------------------");
            example = new ArrayForLoopEx();
            example.Execute();
            Console.WriteLine("\n---------------------Array Loop example---------------------");

            //PrintThreeEmptyLines();

            //Console.WriteLine("--------------------Simple kernel calls----------------------");
            //example = new SimpleKernelCalls();
            //example.Execute();
            //Console.WriteLine("\n--------------------Simple kernel calls----------------------");

            //PrintThreeEmptyLines();

            //Console.WriteLine("--------------------Save and load example-------------------");
            //example = new SaveAndLoadEx();
            //example.Execute();
            //Console.WriteLine("\n--------------------Save and load example-------------------");

            //PrintThreeEmptyLines();

            //Console.WriteLine("--------------------Compiler execute example-------------------");
            //example = new CompilerExecuteEx();
            //example.Execute();
            //Console.WriteLine("\n--------------------Compiler execute example-------------------");

            //PrintThreeEmptyLines();

            //Console.WriteLine("---------------------Complex math with struct example---------------------------");
            //example = new WithStructEx();
            //example.Execute();


            Console.ReadLine();
        }

        private static void PrintThreeEmptyLines()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
