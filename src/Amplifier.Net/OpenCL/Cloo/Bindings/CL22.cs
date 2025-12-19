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
    /// Contains bindings to the OpenCL 2.2 functions.
    /// </summary>
    /// <remarks> See the OpenCL specification for documentation regarding these functions. </remarks>
    [SuppressUnmanagedCodeSecurity]
    internal class CL22 : CL21
    {
        #region Program

        /// <summary>
        /// Registers a user callback function with a program object.
        /// </summary>
        [DllImport(libName, EntryPoint = "clSetProgramReleaseCallback")]
        public static extern ComputeErrorCode SetProgramReleaseCallback(
            CLProgramHandle program,
            ComputeProgramReleaseCallback pfn_notify,
            IntPtr user_data);

        /// <summary>
        /// Sets the value of a specialization constant.
        /// </summary>
        [DllImport(libName, EntryPoint = "clSetProgramSpecializationConstant")]
        public static extern ComputeErrorCode SetProgramSpecializationConstant(
            CLProgramHandle program,
            Int32 spec_id,
            IntPtr spec_size,
            IntPtr spec_value);

        #endregion
    }

    /// <summary>
    /// Delegate for program release callback.
    /// </summary>
    internal delegate void ComputeProgramReleaseCallback(CLProgramHandle program, IntPtr user_data);
}
