using Amplifier.OpenCL;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmplifierExamples.Kernels
{
    class SimpleKernels : OpenCLFunctions
    {
        [OpenCLKernel]
        void AddData([Global]float[] a, [Global] float[] b, [Global]float[] r)
        {
            int i = get_global_id(0);
            b[i] = 0.5f * b[i];
            r[i] = a[i] + b[i];
        }

        [OpenCLKernel]
        void Fill([Global] float[] x, float value)
        {
            int i = get_global_id(0);
            
            x[i] = value;
        }

        [OpenCLKernel]
        void SAXPY([Global]float[] x, [Global] float[] y, float a)
        {
            int i = get_global_id(0);

            y[i] += a * x[i];
        }
    }
}
