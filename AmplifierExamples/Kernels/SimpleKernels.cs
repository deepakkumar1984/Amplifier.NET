using Amplifier.OpenCL;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmplifierExamples.Kernels
{
    class SimpleKernels : OpenCLFunctions<float>
    {
        [OpenCLKernel]
        void add_float([Global]float[] a, [Global] float[] b, [Global]float[] r)
        {
            int i = get_global_id(0);
            b[i] = 0.5f * b[i];
            r[i] = a[i] + b[i];
        }

        [OpenCLKernel]
        void acos_double([Global] double[] x, [Global]double[] r)
        {
            int i = get_global_id(0);
            r[i] = acos(x[i]);
        }

        [OpenCLKernel]
        void Fill([Global] float[] x, float value)
        {
            int i = get_global_id(0);
            x[i] = value;
        }
    }
}
