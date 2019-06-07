using Amplifier;
using AmplifierExamples.Kernels;
using System;

namespace AmplifierExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create instance of OpenCL compiler
            var compiler = new OpenCLCompiler();

            //Get the available device list
            Console.WriteLine("\nList Devices----");
            foreach (var item in compiler.Devices)
            {
                Console.WriteLine(item);
            }

            //Select a default device
            compiler.UseDevice(0);

            //Compile the sample kernel
            compiler.CompileKernel(typeof(SimpleKernels));

            //See all the kernel methods
            Console.WriteLine("\nList Kernels----");
            foreach (var item in compiler.Kernels)
            {
                Console.WriteLine(item);
            }

            //Create variable a, b and r
            Array a = new float[] { 1, 2, 3, 4 };
            Array b = new float[4];
            Array r = new float[4];

            //Get the execution engine
            var exec = compiler.GetExec<float>();

            //Execute fill kernel method
            exec.Fill(b, 0.5f);

            //Execuete add_float kermet method
            exec.add_float(a, b, r);

            //Print the result
            Console.WriteLine("\nResult----");
            for(int i = 0;i<r.Length;i++)
            {
                Console.Write(r.GetValue(i) + " ");
            }

            Console.ReadLine();
        }
    }
}
