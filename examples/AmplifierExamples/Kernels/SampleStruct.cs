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

    [StructLayout(LayoutKind.Sequential)]
    public struct Vecter3D
    {
        public double x;
        public double y;
        public double z;
    }
}
