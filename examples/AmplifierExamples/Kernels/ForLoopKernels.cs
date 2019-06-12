using Amplifier.OpenCL;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmplifierExamples.Kernels
{
    class ForLoopKernels : OpenCLFunctions
    {
        [OpenCLKernel]
        void Sigmoid([Global]float[] x)
        {
            int i = get_global_id(0);
            x[i] = exp(x[i]) / (exp(x[i]) + 1);
        }

        [OpenCLKernel]
        void Threshold([Global] float[] x, float value)
        {
            int i = get_global_id(0);
            x[i] = x[i] > value ? 1 : 0;
        }
    }
}
