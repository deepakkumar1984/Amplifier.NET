using Amplifier;
using AmplifierExamples.Kernels;
using System;

namespace AmplifierExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            var ex = new SaveAndLoadEx();
            ex.Execute();

            Console.ReadLine();
        }
    }
}
