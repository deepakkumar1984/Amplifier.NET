using Amplifier.OpenCL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace AmplifierExamples.Kernels
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SampleStruct
    {
        public float VarA;
        public float VarB;
        public unsafe fixed float Data[10];
    }
}
