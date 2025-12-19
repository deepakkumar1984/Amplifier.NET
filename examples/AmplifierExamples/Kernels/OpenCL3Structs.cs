using Amplifier.OpenCL;
using System.Runtime.InteropServices;

namespace AmplifierExamples.Kernels
{
    /// <summary>
    /// 3D Vector using float components (no double)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Float3
    {
        public float x;
        public float y;
        public float z;
    }

    /// <summary>
    /// 4x4 Matrix using float components for transformations
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4x4F
    {
        public float M00, M01, M02, M03;
        public float M10, M11, M12, M13;
        public float M20, M21, M22, M23;
        public float M30, M31, M32, M33;
    }

    /// <summary>
    /// Particle structure for physics simulation
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Particle
    {
        public float PosX, PosY, PosZ;
        public float VelX, VelY, VelZ;
        public float Mass;
        public int Active;  // 1 = active, 0 = inactive
    }

    /// <summary>
    /// Color structure with RGBA channels
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorRGBA
    {
        public float R, G, B, A;
    }

    /// <summary>
    /// Complex number for signal processing
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ComplexF
    {
        public float Real;
        public float Imag;
    }

    /// <summary>
    /// Histogram bin for image processing
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct HistogramBin
    {
        public int Count;
        public float NormalizedValue;
        public int MinIndex;
        public int MaxIndex;
    }

    /// <summary>
    /// Bounding box for spatial computations
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BoundingBox
    {
        public float MinX, MinY, MinZ;
        public float MaxX, MaxY, MaxZ;
    }

    /// <summary>
    /// Neural network weight structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct NeuronWeights
    {
        public float W0, W1, W2, W3;
        public float Bias;
        public int LayerIndex;
        public int NeuronIndex;
        public int Padding;  // Alignment padding
    }
}
