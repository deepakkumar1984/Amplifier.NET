using Amplifier;
using AmplifierExamples.Kernels;
using System;

namespace AmplifierExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            var compiler = new OpenCLCompiler();
            foreach (var item in compiler.Devices)
            {
                Console.WriteLine(item);
            }

            compiler.UseDevice(0);
            compiler.CompileKernel(typeof(SimpleKernels));

            Console.WriteLine("\nList Kernels----");
            foreach (var item in compiler.Kernels)
            {
                Console.WriteLine(item);
            }

            Array a = new float[] { 1, 2, 3, 4 };
            Array b = new float[4];
            Array r = new float[4];

            var exec = compiler.GetExec<float>();
            exec.Fill(b, 0.5f);
            exec.add_float(a, b, r);

            Console.ReadLine();
        }
    }
}
