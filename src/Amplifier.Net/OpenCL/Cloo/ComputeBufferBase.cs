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

    /// <summary>
    /// Represents the parent type to any Cloo buffer types.
    /// </summary>
    /// <typeparam name="T"> The type of the elements of the buffer. </typeparam>
    internal abstract class ComputeBufferBase<T> : ComputeMemory where T : struct
    {
        #region Properties

        /// <summary>
        /// Gets the number of elements in the <see cref="ComputeBufferBase{T}"/>.
        /// </summary>
        /// <value> The number of elements in the <see cref="ComputeBufferBase{T}"/>. </value>
        public long Count { get; private set; }

        /// <summary>
        /// Gets the current reference count of the <see cref="ComputeBufferBase{T}"/>.
        /// </summary>
        /// <value> The current reference count of the <see cref="ComputeBufferBase{T}"/>. </value>
        public uint ReferenceCount => GetInfo<CLMemoryHandle, ComputeMemoryInfo, uint>(Handle, ComputeMemoryInfo.ReferenceCount, CL12.GetMemObjectInfo);

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="flags"></param>
        protected ComputeBufferBase(ComputeContext context, ComputeMemoryFlags flags)
            : base(context, flags)
        { }

        #endregion

        #region Protected methods

        /// <summary>
        /// 
        /// </summary>
        protected void Init(long size)
        {
            SetID(Handle.Value);

            Size = size;
            Count = Size / ComputeTools.SizeOf<T>();

            //Debug.WriteLine("Create " + this + " in Thread(" + Thread.CurrentThread.ManagedThreadId + ").", "Information");
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Init(long size, long count)
        {
            SetID(Handle.Value);

            Size = size;
            Count = count;

            //Debug.WriteLine("Create " + this + " in Thread(" + Thread.CurrentThread.ManagedThreadId + ").", "Information");
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Init()
        {
            SetID(Handle.Value);

            Size = (long)GetInfo<CLMemoryHandle, ComputeMemoryInfo, IntPtr>(Handle, ComputeMemoryInfo.Size, CL12.GetMemObjectInfo);
            Count = Size / ComputeTools.SizeOf<T>();

            //Debug.WriteLine("Create " + this + " in Thread(" + Thread.CurrentThread.ManagedThreadId + ").", "Information");
        }

        #endregion

        /// <summary>
        /// Clones the ComputeBuffer. Because the buffer is retained the cloned buffer as well as the clone have to be disposed
        /// </summary>
        /// <returns>Cloned buffer</returns>
        public abstract ComputeBufferBase<T> Clone();
    }
}