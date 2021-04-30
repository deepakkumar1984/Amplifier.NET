using System;
using System.Collections.Generic;
using System.Text;

namespace Amplifier
{
    public class OpenCLVars
    {
        public static bool Enabled { get; set; }

        public static long[] GlobalWorkOffset { get; set; }

        public static long[] GlobalWorkSize { get; set; }

        public static long[] LocalWorkSize { get; set; }
    }

    public class ExecuteOptions : IDisposable
    {
        public ExecuteOptions(LongTuple global_work_offset, LongTuple global_work_size, LongTuple local_work_size)
        {
            OpenCLVars.GlobalWorkOffset = global_work_offset.data;
            OpenCLVars.GlobalWorkSize = global_work_size.data;
            OpenCLVars.LocalWorkSize = local_work_size.data;
            OpenCLVars.Enabled = true;
        }

        public void Dispose()
        {
            OpenCLVars.GlobalWorkOffset = null;
            OpenCLVars.GlobalWorkSize = null;
            OpenCLVars.LocalWorkSize = null;
            OpenCLVars.Enabled = false;
        }
    }
}
