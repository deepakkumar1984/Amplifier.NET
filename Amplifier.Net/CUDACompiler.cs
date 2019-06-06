using System;
using System.Collections.Generic;
using System.Text;

namespace Amplifier
{
    public class CUDACompiler : BaseCompiler
    {
        public CUDACompiler() : base("CUDA")
        {
        }

        public override List<Device> Devices => throw new NotImplementedException();

        public override List<string> Kernels => throw new NotImplementedException();

        public override void CompileKernel(Type cls)
        {
            throw new NotImplementedException();
        }

        public override void Execute<TSource>(string functionName, params object[] args)
        {
            throw new NotImplementedException();
        }

        public override void UseDevice(int deviceId = 0)
        {
            throw new NotImplementedException();
        }
    }
}
