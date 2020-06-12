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

        public GenericArrayMemory(ComputeContext context, ComputeMemoryFlags flags, Array array) : base(context, flags)
        {
            if (array.Length == 0)
                return;
            
            int size = Marshal.SizeOf(array.GetValue(0).GetType()) * array.Length;
            var hostPtr = IntPtr.Zero;
            if ((flags & (ComputeMemoryFlags.CopyHostPointer | ComputeMemoryFlags.UseHostPointer)) != ComputeMemoryFlags.None)
            {
                var datagch = GCHandle.Alloc(array, GCHandleType.Pinned);
                hostPtr = datagch.AddrOfPinnedObject();
            }

            ComputeErrorCode error = ComputeErrorCode.Success;
            var handle = CL12.CreateBuffer(context.Handle, flags, new IntPtr(size), hostPtr, out error);
            
            this.Size = size;
            this.Handle = handle;
        }

        public GenericArrayMemory(ComputeContext context, ComputeMemoryFlags flags, XArray obj) : base(context, flags)
        {
            var hostPtr = IntPtr.Zero;
            long size = obj.DataType.Size() * obj.Count;
            if ((flags & (ComputeMemoryFlags.CopyHostPointer | ComputeMemoryFlags.UseHostPointer)) != ComputeMemoryFlags.None)
            {
                hostPtr = obj.NativePtr;
            }

            ComputeErrorCode error = ComputeErrorCode.Success;
            var handle = CL12.CreateBuffer(context.Handle, flags, new IntPtr(size), hostPtr, out error);

            this.Size = size;
            this.Handle = handle;
        }
    }
}
