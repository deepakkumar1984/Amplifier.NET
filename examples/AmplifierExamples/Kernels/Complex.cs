using Amplifier.OpenCL;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmplifierExamples.Kernels
{
    public struct Complex
    {
        public float Real;
        public float Imag;

        public float Magnitude()
        {
            return Real + Imag;
        }
    }
}
