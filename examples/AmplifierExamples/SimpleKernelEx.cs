using Amplifier;
using Amplifier.OpenCL;
using AmplifierExamples.Kernels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmplifierExamples
{
    class SimpleKernelEx : IExample
    {
        public void Execute()
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
            var x = new XArray(new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }).Reshape(3, 3);
            var y = new XArray(new float[] { 3, 3 });
            var z = new XArray(new float[] { 3, 3 });

            //Get the execution engine
            var exec = compiler.GetExec();

            //Execute fill kernel method
            exec.Fill(y, 0.5f);
            var r = y.ToArray();

            //Execute AddData kernel method
            exec.AddData(x, y, z);
            r = z.ToArray();
            //Execute AddHalf kernel method
            //var xhalf = Array.ConvertAll(x, v => (half)v);
            //var yhalf = Array.ConvertAll(y, v => (half)v);
            //exec.AddHalf(xhalf, yhalf);
            //z = Array.ConvertAll(yhalf, v => (float)v);

            //Execuete SAXPY kernel method
            exec.SAXPY(x, y, 2f);

            //Print the result
            Console.WriteLine("\nResult----");
            for (int i = 0; i < y.Count; i++)
            {
                Console.Write(y[i] + " ");
            }
        }
    }
}
