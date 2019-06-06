using Amplifier.OpenCL;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmplifierExamples.Kernels
{
    class SimpleKernels : OpenCLFunctions
    {
        [OpenCLKernel]
        public void add_float([Global]float[] a, [Global] float[] b, [Global]float[] r)
        {
            int i = get_global_id(0);

            r[i] = a[i] + b[i];
        }

        [OpenCLKernel]
        public void acos_double([Global] double[] x, [Global]double[] r)
        {
            int i = get_global_id(0);
            r[i] = acos(x[i]);
        }

        [OpenCLKernel]
        public void Fill([Global] float[] x, float value)
        {
            int i = get_global_id(0);
            x[i] = value;
        }
    }
}
