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
    /// <summary>
    /// Represents an OpenCL sub-buffer.
    /// </summary>
    /// <typeparam name="T"> The type of the elements of the <see cref="ComputeSubBuffer{T}"/>. <typeparamref name="T"/> is restricted to value types and <c>struct</c>s containing such types. </typeparam>
    /// <remarks> A sub-buffer is created from a standard buffer and represents all or part of its data content. <br/> Requires OpenCL 1.1. </remarks>
    internal class ComputeSubBuffer<T> : ComputeBufferBase<T> where T : struct
    {
        #region Constructors

        /// <summary>
        /// Creates a new <see cref="ComputeSubBuffer{T}"/> from a specified <see cref="ComputeBuffer{T}"/>.
        /// </summary>
        /// <param name="buffer"> The buffer to create the <see cref="ComputeSubBuffer{T}"/> from. </param>
        /// <param name="flags"> A bit-field that is used to specify allocation and usage information about the <see cref="ComputeBuffer{T}"/>. </param>
        /// <param name="offset"> The index of the element of <paramref name="buffer"/>, where the <see cref="ComputeSubBuffer{T}"/> starts. </param>
        /// <param name="count"> The number of elements of <paramref name="buffer"/> to include in the <see cref="ComputeSubBuffer{T}"/>. </param>
        public ComputeSubBuffer(ComputeBuffer<T> buffer, ComputeMemoryFlags flags, long offset, long count)
            : base(buffer.Context, flags)
        {
            var sizeofT = ComputeTools.SizeOf<T>();

            SysIntX2 region = new SysIntX2(offset * sizeofT, count * sizeofT);
            Handle = CL11.CreateSubBuffer(buffer.Handle, flags, ComputeBufferCreateType.Region, ref region, out var error);
            ComputeException.ThrowOnError(error);

            Init();
        }

        internal ComputeSubBuffer(ComputeContext context, CLMemoryHandle handle, ComputeMemoryFlags flags)
            : base(context, flags)
        {
            Handle = handle;

            Init();
        }

        #endregion

        /// <summary>
        /// Clones the ComputeBuffer. Because the buffer is retained the cloned buffer as well as the clone have to be disposed
        /// </summary>
        /// <returns>Cloned buffer</returns>
        public override ComputeBufferBase<T> Clone()
        {
            CL10.RetainMemObject(Handle);
            return new ComputeSubBuffer<T>(Context, Handle, Flags);
        }
    }

    
}