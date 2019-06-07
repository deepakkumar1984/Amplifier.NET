using System;
using System.Collections.Generic;
using System.Text;

namespace Amplifier.OpenCL
{
    /// <summary>
    /// Please download the full method specification from here: https://www.khronos.org/registry/OpenCL/specs/2.2/pdf/OpenCL_C.pdf
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class OpenCLFunctions
    {
        public T shuffle<T>(T x, dynamic mask) where T : struct
        {
            return x;
        }

        public T shuffle2<T>(T x, dynamic mask) where T : struct
        {
            return x;
        }

        public int vec_step<T>(T a) where T : struct
        {
            return 0;
        }
    }
}
