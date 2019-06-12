using Amplifier;
using AmplifierExamples.Kernels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmplifierExamples
{
    class SaveAndLoadEx : IExample
    {
        public void Execute()
        {
            //Compile all the kernel and save it to a bin file
            SaveCompiler();

            //Once saved you can reuse the same bin instead of compiling from scratch. Save compilation time. Also the bin file is portable
            var compiler = new OpenCLCompiler();
            compiler.Load("test.bin");

            foreach (var item in compiler.Kernels)
            {
                Console.WriteLine(item);
            }

            //Create variable a, b and r
            Array x = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Array y = new float[9];

            //Get the execution engine
            var exec = compiler.GetExec<float>();

            //Execute fill kernel method
            exec.Fill(y, 0.5f);

            //Execuete SAXPY kernel method
            exec.SAXPY(x, y, 2f);

            //Print the result
            Console.WriteLine("\nResult----");
            for (int i = 0; i < y.Length; i++)
            {
                Console.Write(y.GetValue(i) + " ");
            }
        }

        private void SaveCompiler()
        {
            //Create instance of OpenCL compiler
            var compiler = new OpenCLCompiler();

            //Select a default device
            compiler.UseDevice(0);

            //Compile the sample kernel
            compiler.CompileKernel(typeof(SimpleKernels));
            compiler.Save("test.bin");
        }
    }
}
