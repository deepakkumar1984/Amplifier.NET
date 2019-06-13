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

            Console.WriteLine("---------------------Basic example---------------------------");
            example = new SimpleKernelEx();
            example.Execute();
            Console.WriteLine("\n---------------------Basic example---------------------------");

            Console.WriteLine("---------------------Array Loop example---------------------");
            example = new ArrayForLoopEx();
            example.Execute();
            Console.WriteLine("\n---------------------Array Loop example---------------------");

            Console.WriteLine("--------------------Simple kernel calls----------------------");
            example = new SimpleKernelCalls();
            example.Execute();
            Console.WriteLine("\n--------------------Simple kernel calls----------------------");

            Console.WriteLine("--------------------Save and load example-------------------");
            example = new SaveAndLoadEx();
            example.Execute();
            Console.WriteLine("\n--------------------Save and load example-------------------");

            Console.ReadLine();
        }
    }
}
