using Amplifier;
using AmplifierExamples.Kernels;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace AmplifierExamples
{
    class WithStructEx : IExample
    {
        public void Execute()
        {
            //Create instance of OpenCL compiler
            var compiler = new OpenCLCompiler();

            //Select a default device
            compiler.UseDevice(0);

            //Compile the sample kernel
            compiler.CompileKernel(typeof(WithStructKernel), typeof(SampleStruct));
            var exec = compiler.GetExec();
            SampleStruct[] x = new SampleStruct[5];

            exec.Fill(x, 2.5);

            foreach (var item in x)
            {
                Console.WriteLine("VarA: {0}, VarB: {1}", item.VarA, item.VarB);
            }
        }
    }
}
