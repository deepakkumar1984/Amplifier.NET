using Amplifier.OpenCL;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmplifierExamples.Kernels
{
    class SimpleKernels : OpenCLFunctions
    {
        [OpenCLKernel]
        void AddData([Global, Input]float[] a, [Global]float[] b, [Global, Output]float[] r)
        {
            int i = get_global_id(0);
            b[i] = 0.5f * b[i];
            r[i] = a[i] + b[i];
            a[i] += 2; // result will not copy out
        }

        [OpenCLKernel]
        void AddHalf([Global, Input]half[] a, [Global]half[] b)
        {
            int i = get_global_id(0);
            float af = vload_half(i, a);
            float bf = vload_half(i, b);
            vstore_half(af + bf, i, b);
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
