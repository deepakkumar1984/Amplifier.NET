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

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Amplifier.OpenCL.Cloo.Bindings
{
    /// <summary>
    /// Contains bindings to the OpenCL 3.0 functions.
    /// </summary>
    /// <remarks> See the OpenCL specification for documentation regarding these functions. </remarks>
    [SuppressUnmanagedCodeSecurity]
    internal class CL30 : CL22
    {
        #region Buffer

        /// <summary>
        /// Creates a buffer object with properties.
        /// </summary>
        [DllImport(libName, EntryPoint = "clCreateBufferWithProperties")]
        public static extern CLMemoryHandle CreateBufferWithProperties(
            CLContextHandle context,
            [MarshalAs(UnmanagedType.LPArray)] IntPtr[] properties,
            ComputeMemoryFlags flags,
            IntPtr size,
            IntPtr host_ptr,
            out ComputeErrorCode errcode_ret);

        #endregion

        #region Image

        /// <summary>
        /// Creates an image object with properties.
        /// </summary>
        [DllImport(libName, EntryPoint = "clCreateImageWithProperties")]
        public static extern CLMemoryHandle CreateImageWithProperties(
            CLContextHandle context,
            [MarshalAs(UnmanagedType.LPArray)] IntPtr[] properties,
            ComputeMemoryFlags flags,
            ref ComputeImageFormat image_format,
            ref ComputeImageDescription image_desc,
            IntPtr host_ptr,
            out ComputeErrorCode errcode_ret);

        #endregion

        #region Context

        /// <summary>
        /// Registers a user callback function with a context that is called when the context is destroyed.
        /// </summary>
        [DllImport(libName, EntryPoint = "clSetContextDestructorCallback")]
        public static extern ComputeErrorCode SetContextDestructorCallback(
            CLContextHandle context,
            ComputeContextDestructorCallback pfn_notify,
            IntPtr user_data);

        #endregion
    }

    /// <summary>
    /// Delegate for context destructor callback.
    /// </summary>
    internal delegate void ComputeContextDestructorCallback(CLContextHandle context, IntPtr user_data);

    /// <summary>
    /// OpenCL image description structure for OpenCL 1.2+.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ComputeImageDescription
    {
        /// <summary>
        /// Describes the image type.
        /// </summary>
        public ComputeMemoryType ImageType;

        /// <summary>
        /// Width of the image in pixels.
        /// </summary>
        public IntPtr ImageWidth;

        /// <summary>
        /// Height of the image in pixels (for 2D/3D images).
        /// </summary>
        public IntPtr ImageHeight;

        /// <summary>
        /// Depth of the image in pixels (for 3D images).
        /// </summary>
        public IntPtr ImageDepth;

        /// <summary>
        /// Number of images in the image array.
        /// </summary>
        public IntPtr ImageArraySize;

        /// <summary>
        /// Scan-line pitch in bytes.
        /// </summary>
        public IntPtr ImageRowPitch;

        /// <summary>
        /// Size in bytes of each 2D slice in the 3D image.
        /// </summary>
        public IntPtr ImageSlicePitch;

        /// <summary>
        /// Must be 0.
        /// </summary>
        public Int32 NumMipLevels;

        /// <summary>
        /// Must be 0.
        /// </summary>
        public Int32 NumSamples;

        /// <summary>
        /// Buffer from which to create the image (for buffer-backed images).
        /// </summary>
        public CLMemoryHandle Buffer;
    }
}
