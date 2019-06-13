using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amplifier.OpenCL
{
    public partial class OpenCLFunctions
    {
        public dynamic Instance<T>(int deviceId = 0) where T : struct
        {
            var compiler = new OpenCLCompiler();
            compiler.UseDevice(deviceId);
            compiler.CompileKernel(this.GetType());

            return compiler.GetExec<T>();
        }
    }
}
