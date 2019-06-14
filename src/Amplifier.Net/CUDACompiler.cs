/*
MIT License

Copyright (c) 2019 Tech Quantum

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 
*/
namespace Amplifier
{
    using System;
    using System.Collections.Generic;

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

        public override void CompileKernel(params Type[] classes)
        {
            throw new NotImplementedException();
        }

        public override void Execute(string functionName, params object[] args)
        {
            throw new NotImplementedException();
        }

        public override void Load(string filePath, int deviceId = 0)
        {
            throw new NotImplementedException();
        }

        public override void Save(string filePath)
        {
            throw new NotImplementedException();
        }

        public override void UseDevice(int deviceId = 0)
        {
            throw new NotImplementedException();
        }
    }
}
