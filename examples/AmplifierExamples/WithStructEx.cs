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
            compiler.CompileKernel(typeof(WithStructKernel), typeof(SampleStruct), typeof(Vecter3D));
            var exec = compiler.GetExec();
            SampleStruct[] x = new SampleStruct[5];

            exec.Fill(x, 2.5);

            Vecter3D[] vecter3Ds = new Vecter3D[2];
            vecter3Ds[0] = new Vecter3D { x = 2, y = 4, z = 1 };
            vecter3Ds[1] = new Vecter3D { x = 4.5, y = 1, z = 0 };

            double[] y = new double[2];

            exec.Compute(vecter3Ds, y, 5.5);

            foreach (var item in x)
            {
                Console.WriteLine("VarA: {0}, VarB: {1}", item.VarA, item.VarB);
            }
        }
    }
}
