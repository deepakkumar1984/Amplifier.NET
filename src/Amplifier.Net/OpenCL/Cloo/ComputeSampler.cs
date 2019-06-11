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
    using System.Diagnostics;

    /// <summary>
    /// Represents an OpenCL sampler.
    /// </summary>
    /// <remarks> An object that describes how to sample an image when the image is read in the kernel. The image read functions take a sampler as an argument. The sampler specifies the image addressing-mode i.e. how out-of-range image coordinates are handled, the filtering mode, and whether the input image coordinate is a normalized or unnormalized value. </remarks>
    /// <seealso cref="ComputeImage"/>
    internal class ComputeSampler : ComputeResource
    {
        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ComputeContext _context;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ComputeImageAddressing _addressing;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ComputeImageFiltering _filtering;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly bool _normalizedCoords;

        private CLSamplerHandle _handle;

        #endregion

        #region Properties

        /// <summary>
        /// The handle of the <see cref="ComputeSampler"/>.
        /// </summary>
        public CLSamplerHandle Handle
        {
            get => _handle;
            protected set => _handle = value;
        }

        /// <summary>
        /// Gets the <see cref="ComputeContext"/> of the <see cref="ComputeSampler"/>.
        /// </summary>
        /// <value> The <see cref="ComputeContext"/> of the <see cref="ComputeSampler"/>. </value>
        public ComputeContext Context => _context;

        /// <summary>
        /// Gets the <see cref="ComputeImageAddressing"/> mode of the <see cref="ComputeSampler"/>.
        /// </summary>
        /// <value> The <see cref="ComputeImageAddressing"/> mode of the <see cref="ComputeSampler"/>. </value>
        public ComputeImageAddressing Addressing => _addressing;

        /// <summary>
        /// Gets the <see cref="ComputeImageFiltering"/> mode of the <see cref="ComputeSampler"/>.
        /// </summary>
        /// <value> The <see cref="ComputeImageFiltering"/> mode of the <see cref="ComputeSampler"/>. </value>
        public ComputeImageFiltering Filtering => _filtering;

        /// <summary>
        /// Gets the state of usage of normalized x, y and z coordinates when accessing a <see cref="ComputeImage"/> in a <see cref="ComputeKernel"/> through the <see cref="ComputeSampler"/>.
        /// </summary>
        /// <value> The state of usage of normalized x, y and z coordinates when accessing a <see cref="ComputeImage"/> in a <see cref="ComputeKernel"/> through the <see cref="ComputeSampler"/>. </value>
        public bool NormalizedCoords => _normalizedCoords;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="ComputeSampler"/>.
        /// </summary>
        /// <param name="context"> A <see cref="ComputeContext"/>. </param>
        /// <param name="normalizedCoords"> The usage state of normalized coordinates when accessing a <see cref="ComputeImage"/> in a <see cref="ComputeKernel"/>. </param>
        /// <param name="addressing"> The <see cref="ComputeImageAddressing"/> mode of the <see cref="ComputeSampler"/>. Specifies how out-of-range image coordinates are handled while reading. </param>
        /// <param name="filtering"> The <see cref="ComputeImageFiltering"/> mode of the <see cref="ComputeSampler"/>. Specifies the type of filter that must be applied when reading data from an image. </param>
        public ComputeSampler(ComputeContext context, bool normalizedCoords, ComputeImageAddressing addressing, ComputeImageFiltering filtering)
        {
            Handle = CL12.CreateSampler(context.Handle, normalizedCoords, addressing, filtering, out var error);
            ComputeException.ThrowOnError(error);

            SetID(Handle.Value);
            
            _addressing = addressing;
            _context = context;
            _filtering = filtering;
            _normalizedCoords = normalizedCoords;

            //Debug.WriteLine("Create " + this + " in Thread(" + Thread.CurrentThread.ManagedThreadId + ").", "Information");
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Releases the associated OpenCL object.
        /// </summary>
        /// <param name="manual"> Specifies the operation mode of this method. </param>
        /// <remarks> <paramref name="manual"/> must be <c>true</c> if this method is invoked directly by the application. </remarks>
        protected override void Dispose(bool manual)
        {
            if (Handle.IsValid)
            {
                //Debug.WriteLine("Dispose " + this + " in Thread(" + Thread.CurrentThread.ManagedThreadId + ").", "Information");
                CL12.ReleaseSampler(Handle);
                _handle.Invalidate();
            }
        }

        #endregion
    }
}