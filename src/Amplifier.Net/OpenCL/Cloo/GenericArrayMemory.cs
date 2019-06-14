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
    internal class GenericArrayMemory : ComputeMemory
    {
        public GenericArrayMemory(ComputeContext context, ComputeMemoryFlags flags) : base(context, flags)
        {
        }

        public GenericArrayMemory(ComputeContext context, ComputeMemoryFlags flags, object obj) : base(context, flags)
        {
            Array array = (Array)obj;
            
            if (array.Length == 0)
                return;

            int size = Marshal.SizeOf(array.GetValue(0).GetType()) * array.Length;
            var datagch = GCHandle.Alloc(obj, GCHandleType.Pinned);
            ComputeErrorCode error = ComputeErrorCode.Success;
            var handle = CL12.CreateBuffer(context.Handle, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, new IntPtr(size), datagch.AddrOfPinnedObject(), out error);
            
            this.Size = size;
            this.Handle = handle;
        }
    }
}
