using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Amplifier
{
    /// <summary>
    /// Struct TensorRef64
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct XArrayRef
    {
        /// <summary>
        /// The buffer
        /// </summary>
        public IntPtr buffer;
        /// <summary>
        /// The sizes
        /// </summary>
        public IntPtr sizes;
        /// <summary>
        /// The strides
        /// </summary>
        public IntPtr strides;
        /// <summary>
        /// The dim count
        /// </summary>
        public int dimCount;
        /// <summary>
        /// The element type
        /// </summary>
        public DType elementType;
    }
}
