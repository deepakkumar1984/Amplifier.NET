using Amplifier.OpenCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmplifierExamples.Kernels
{
    public class Issue20Kernel : OpenCLFunctions
    {
        [OpenCLKernel]
        void CalculateChunkLine([Global] int[] counts, int x, [Global] long[] a, [Global] long[] b)
        {
            int thread = get_global_id(0);
            int z = thread - 1875000;
            long chunckSeed = ((x * 0x4F9939F508 + z * 0x1EF1565BD5) ^ 0x5DEECE66D) & 0xFFFFFFFFFFFF;
            int vx = x >> 4;
            int vz = z >> 4;
            for(byte y = 1; y < 5; y++)
            {
                for(int bx = vx; bx < vz + 16; bx++)
                {
                    for(int bz = vz; bz < vz + 16; bz++)
                    {
                        int pi = ((((bz & 0xF) * 0x10) + (bx & 0xF)) << 2) + (3 - (y - 1));
                        if (((((chunckSeed * a[pi] + b[pi]) & 0xFFFFFFFFFFFF) >> 0x11) % 5) >= y)
                            counts[thread] += 1;
                    }
                }
            }
        }
    }
}
