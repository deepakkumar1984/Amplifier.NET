using Amplifier;
using AmplifierExamples.Kernels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmplifierExamples
{
    public class MatrixMulExample : IExample
    {
        public void Execute()
        {
            //Create instance of OpenCL compiler
            var compiler = new OpenCLCompiler();

            //Select a default device
            compiler.UseDevice(0);

            //Compile the sample kernel
            compiler.CompileKernel(typeof(SGEMMKernals), typeof(SimpleKernels));

            //Create variable a, b and r
            int M = 30;
            int N = 30;
            int K = 20;

            var x = new InArray(new long[] { M, K }, DType.Float32);
            var y = new InArray(new long[] { K, M }, DType.Float32);
            var z = new OutArray(new long[] { M, N }, DType.Float32) { IsElementWise = false };

            //Get the execution engine
            var exec = compiler.GetExec();

            exec.Fill(x, 2);
            exec.Fill(y, 3);
            var r = y.ToArray();
            
            exec.MatMul(M, N, K, x, y, z);
            r = z.ToArray();
            //Print the result
            Console.WriteLine("\nResult----");
            for (int i = 0; i < z.Count; i++)
            {
                Console.Write(z[i] + " ");
            }
        }
    }
}
