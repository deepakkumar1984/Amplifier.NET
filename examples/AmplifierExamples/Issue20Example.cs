using Amplifier;
using AmplifierExamples.Kernels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmplifierExamples
{
    internal class Issue20Example : IExample
    {
        OpenCLCompiler compiler = null;

        public void Execute()
        {
            Init();
            while (true)
            {
                int z;
            }
        }

        void Init()
        {
            if (compiler != null)
                compiler.Dispose();

            compiler = new OpenCLCompiler();
            compiler.UseDevice(0);
            compiler.CompileKernel(typeof(Issue20Kernel));
        }
    }
}
