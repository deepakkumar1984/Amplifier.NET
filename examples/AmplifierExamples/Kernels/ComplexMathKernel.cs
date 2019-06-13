using Amplifier.OpenCL;

namespace AmplifierExamples.Kernels
{
    class ComplexMathKernel : OpenCLFunctions
    {
        void Fill([Global]Complex[] x, float value)
        {
            int i = get_global_id(0);
            x[i].Real = value;
            x[i].Imag = 0;

            float[] flist = new float[4] { 1, 2, 3, 4 };
            float sum = 0;
            foreach (var item in flist)
            {
                sum += item;
            }
        }
    }
}
