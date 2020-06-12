using Amplifier.OpenCL;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmplifierExamples.Kernels
{
    public class SGEMMKernals : OpenCLFunctions
    {
        [OpenCLKernel]
        void MatMul(int M, int N, int K, [Global]float[] A, [Global]float[] B, [Global]float[] C)
        {
            int globalRow = get_global_id(0);
            int globalCol = get_global_id(1);
            float acc = 0.0f;
            for(int k = 0; k < K; k++)
            {
                acc += A[k * M + globalRow] * B[globalCol * K + k];
            }

            C[globalCol * M + globalRow] = acc;
        }
    }
}
