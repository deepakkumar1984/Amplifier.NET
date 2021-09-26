
using Amplifier.OpenCL;

namespace AmplifierExamples.Kernels
{
    class WithStructKernel : OpenCLFunctions
    {
        [OpenCLKernel]
        void Fill([Global][Struct] SampleStruct[] x, float value)
        {
            int i = get_global_id(0);
            x[i].VarA = value;
            x[i].VarB = GetVar(value);

            //float[] flist = new float[4] { 1, 2, 3, 4 };
            //float sum = 0;
            //foreach (var item in flist)
            //{
            //    sum += item;
            //}
        }

        private float GetVar(float v)
        {
            return v * 2;
        }

        [OpenCLKernel]
        public void Compute([Global][Struct] Vecter3D[] x, [Global] double[] y, double a)
        {
            int i = get_global_id(0);
            y[i] = a * x[i].x + a * x[i].y + a * x[i].z;
        }
    }
}
