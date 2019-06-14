#region License

/*
 
Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

*/

#endregion

using Amplifier.OpenCL.Cloo.Bindings;

namespace Amplifier.OpenCL.Cloo
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Represents an OpenCL buffer.
    /// </summary>
    /// <typeparam name="T"> The type of the elements of the <see cref="ComputeBuffer{T}"/>. <typeparamref name="T"/> is restricted to value types and <c>struct</c>s containing such types. </typeparam>
    /// <remarks> A memory object that stores a linear collection of bytes. Buffer objects are accessible using a pointer in a kernel executing on a device. </remarks>
    /// <seealso cref="ComputeDevice"/>
    /// <seealso cref="ComputeKernel"/>
    /// <seealso cref="ComputeMemory"/>
    internal class ComputeBuffer<T> : ComputeBufferBase<T> where T : struct
    {
        #region Constructors

        /// <summary>
        /// Creates a new <see cref="ComputeBuffer{T}"/>.
        /// </summary>
        /// <param name="context"> A <see cref="ComputeContext"/> used to create the <see cref="ComputeBuffer{T}"/>. </param>
        /// <param name="flags"> A bit-field that is used to specify allocation and usage information about the <see cref="ComputeBuffer{T}"/>. </param>
        /// <param name="count"> The number of elements of the <see cref="ComputeBuffer{T}"/>. </param>
        public ComputeBuffer(ComputeContext context, ComputeMemoryFlags flags, long count)
            : this(context, flags, count, IntPtr.Zero)
        { }

        /// <summary>
        /// Creates a new <see cref="ComputeBuffer{T}"/>.
        /// </summary>
        /// <param name="context"> A <see cref="ComputeContext"/> used to create the <see cref="ComputeBuffer{T}"/>. </param>
        /// <param name="flags"> A bit-field that is used to specify allocation and usage information about the <see cref="ComputeBuffer{T}"/>. </param>
        /// <param name="count"> The number of elements of the <see cref="ComputeBuffer{T}"/>. </param>
        /// <param name="dataPtr"> A pointer to the data for the <see cref="ComputeBuffer{T}"/>. </param>
        public ComputeBuffer(ComputeContext context, ComputeMemoryFlags flags, long count, IntPtr dataPtr)
            : base(context, flags)
        {
            var size = ComputeTools.SizeOf<T>()*count;

            Handle = CL12.CreateBuffer(context.Handle, flags, new IntPtr(size), dataPtr, out var error);
            ComputeException.ThrowOnError(error);
            Init(size, count);
        }

        /// <summary>
        /// Creates a new <see cref="ComputeBuffer{T}"/>.
        /// </summary>
        /// <param name="context"> A <see cref="ComputeContext"/> used to create the <see cref="ComputeBuffer{T}"/>. </param>
        /// <param name="flags"> A bit-field that is used to specify allocation and usage information about the <see cref="ComputeBuffer{T}"/>. </param>
        /// <param name="data"> The data for the <see cref="ComputeBuffer{T}"/>. </param>
        /// <remarks> Note, that <paramref name="data"/> cannot be an "immediate" parameter, i.e.: <c>new T[100]</c>, because it could be quickly collected by the GC causing Amplifier.OpenCL.Cloo to send and invalid reference to OpenCL. </remarks>
        public ComputeBuffer(ComputeContext context, ComputeMemoryFlags flags, T[] data)
            : base(context, flags)
        {
            var size = ComputeTools.SizeOf<T>()*data.Length;

            GCHandle dataPtr = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                Handle = CL12.CreateBuffer(context.Handle, flags, new IntPtr(size), dataPtr.AddrOfPinnedObject(), out var error);
                ComputeException.ThrowOnError(error);
            }
            finally 
            {
                dataPtr.Free(); 
            }

            Init(size, data.Length);
        }

        private ComputeBuffer(CLMemoryHandle handle, ComputeContext context, ComputeMemoryFlags flags)
            : base(context, flags)
        {
            Handle = handle;
            Init();
        }

        private ComputeBuffer(CLMemoryHandle handle, ComputeContext context, ComputeMemoryFlags flags, long size)
            : base(context, flags)
        {
            Handle = handle;
            Init(size);
        }

        private ComputeBuffer(CLMemoryHandle handle, ComputeContext context, ComputeMemoryFlags flags, long size, long count)
            : base(context, flags)
        {
            Handle = handle;
            Init(size, count);
        }
        
        #endregion

        #region Public methods

        /// <summary>
        /// Creates a new <see cref="ComputeBuffer{T}"/> from an existing OpenGL buffer object.
        /// </summary>
        /// <typeparam name="TDataType"> The type of the elements of the <see cref="ComputeBuffer{T}"/>. <typeparamref name="T"/> should match the type of the elements in the OpenGL buffer. </typeparam>
        /// <param name="context"> A <see cref="ComputeContext"/> with enabled CL/GL sharing. </param>
        /// <param name="flags"> A bit-field that is used to specify usage information about the <see cref="ComputeBuffer{T}"/>. Only <see cref="ComputeMemoryFlags.ReadOnly"/>, <see cref="ComputeMemoryFlags.WriteOnly"/> and <see cref="ComputeMemoryFlags.ReadWrite"/> are allowed. </param>
        /// <param name="bufferId"> The OpenGL buffer object id to use for the creation of the <see cref="ComputeBuffer{T}"/>. </param>
        /// <returns> The created <see cref="ComputeBuffer{T}"/>. </returns>
        public static ComputeBuffer<TDataType> CreateFromGLBuffer<TDataType>(ComputeContext context, ComputeMemoryFlags flags, int bufferId) where TDataType : struct
        {
            CLMemoryHandle handle = CL12.CreateFromGLBuffer(context.Handle, flags, bufferId, out var error);
            ComputeException.ThrowOnError(error);
            return new ComputeBuffer<TDataType>(handle, context, flags);
        }


        #endregion

        /// <summary>
        /// Clones the ComputeBuffer. Because the buffer is retained the cloned buffer as well as the clone have to be disposed
        /// </summary>
        /// <returns>Cloned buffer</returns>
        public override ComputeBufferBase<T> Clone()
        {
            CL10.RetainMemObject(Handle);
            return new ComputeBuffer<T>(Handle, Context, Flags, Size, Count);
        }

        /// <summary>
        /// Creates a new ComputeBuffer from an ComputeMemory. Because the memory is retained the cloned buffer as well as the clone have to be disposed
        /// </summary>
        /// <param name="memory"></param>
        /// <returns></returns>
        public static ComputeBuffer<T> From(ComputeMemory memory)
        {
            CL10.RetainMemObject(memory.Handle);
            return new ComputeBuffer<T>(memory.Handle, memory.Context, memory.Flags, memory.Size);
        }

        /// <summary>
        /// Creates a new ComputeBuffer from external memory handles.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ComputeBuffer<T> From(IntPtr handle, ComputeContext context)
        {
            var memoryHandle = new CLMemoryHandle(handle);

            var flags = (ComputeMemoryFlags)GetInfo<CLMemoryHandle, ComputeMemoryInfo, long>(memoryHandle, ComputeMemoryInfo.Flags, CL12.GetMemObjectInfo);

            return new ComputeBuffer<T>(memoryHandle, context, flags);
        }
    }
}