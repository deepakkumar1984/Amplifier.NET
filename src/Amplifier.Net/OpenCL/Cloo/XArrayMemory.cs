using Amplifier.OpenCL.Cloo.Bindings;
using Amplifier.Decompiler.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Amplifier.OpenCL.Cloo
{
    internal class XArrayMemory : ComputeMemory
    {
        public XArrayMemory(ComputeContext context, ComputeMemoryFlags flags) : base(context, flags)
        {
        }

        public XArrayMemory(ComputeContext context, ComputeMemoryFlags flags, XArray obj) : base(context, flags)
        {
            var hostPtr = IntPtr.Zero;
            if ((flags & (ComputeMemoryFlags.CopyHostPointer | ComputeMemoryFlags.UseHostPointer)) != ComputeMemoryFlags.None)
            {
                hostPtr = obj.NativePtr;
            }

            ComputeErrorCode error = ComputeErrorCode.Success;
            var handle = CL12.CreateBuffer(context.Handle, flags, new IntPtr(obj.Count), hostPtr, out error);
            
            this.Size = obj.Count;
            this.Handle = handle;
        }
    }
}
