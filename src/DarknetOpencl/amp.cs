using Amplifier;
using DarknetOpencl.kernels;
using System;
using System.IO;

namespace DarknetOpencl
{
    public class amp
    {
        public static OpenCLCompiler compiler = new OpenCLCompiler();

        public static dynamic exec;

        public static void LoadKernels()
         {
            compiler.UseDevice(0);
            string activation_kernels = File.ReadAllText("./kernels/activation_kernels.cl");
            compiler.CompileKernel(typeof(Activations), typeof(ActivationKernels));
            exec = compiler.GetExec();
        }
    }
}
