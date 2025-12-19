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
    /// Contains bindings to the OpenCL 2.1 functions.
    /// </summary>
    /// <remarks> See the OpenCL specification for documentation regarding these functions. </remarks>
    [SuppressUnmanagedCodeSecurity]
    internal class CL21 : CL20
    {
        #region Kernel

        /// <summary>
        /// Makes a shallow copy of a kernel object.
        /// </summary>
        [DllImport(libName, EntryPoint = "clCloneKernel")]
        public static extern CLKernelHandle CloneKernel(
            CLKernelHandle source_kernel,
            out ComputeErrorCode errcode_ret);

        /// <summary>
        /// Returns information about the kernel object that may be specific to a device.
        /// </summary>
        [DllImport(libName, EntryPoint = "clGetKernelSubGroupInfo")]
        public static extern ComputeErrorCode GetKernelSubGroupInfo(
            CLKernelHandle kernel,
            CLDeviceHandle device,
            ComputeKernelSubGroupInfo param_name,
            IntPtr input_value_size,
            IntPtr input_value,
            IntPtr param_value_size,
            IntPtr param_value,
            out IntPtr param_value_size_ret);

        #endregion

        #region Program

        /// <summary>
        /// Creates a program object for a context, and loads the IL (SPIR-V) into the program object.
        /// </summary>
        [DllImport(libName, EntryPoint = "clCreateProgramWithIL")]
        public static extern CLProgramHandle CreateProgramWithIL(
            CLContextHandle context,
            IntPtr il,
            IntPtr length,
            out ComputeErrorCode errcode_ret);

        #endregion

        #region Device

        /// <summary>
        /// Returns a reasonably synchronized pair of timestamps from the device timer and the host timer.
        /// </summary>
        [DllImport(libName, EntryPoint = "clGetDeviceAndHostTimer")]
        public static extern ComputeErrorCode GetDeviceAndHostTimer(
            CLDeviceHandle device,
            out UInt64 device_timestamp,
            out UInt64 host_timestamp);

        /// <summary>
        /// Returns the current value of the host clock as seen by device.
        /// </summary>
        [DllImport(libName, EntryPoint = "clGetHostTimer")]
        public static extern ComputeErrorCode GetHostTimer(
            CLDeviceHandle device,
            out UInt64 host_timestamp);

        #endregion

        #region Command Queue

        /// <summary>
        /// Replaces a default device command queue created with clCreateCommandQueueWithProperties.
        /// </summary>
        [DllImport(libName, EntryPoint = "clSetDefaultDeviceCommandQueue")]
        public static extern ComputeErrorCode SetDefaultDeviceCommandQueue(
            CLContextHandle context,
            CLDeviceHandle device,
            CLCommandQueueHandle command_queue);

        #endregion

        #region SVM

        /// <summary>
        /// Enqueues a command to indicate which device a set of ranges of SVM allocations should be associated with.
        /// </summary>
        [DllImport(libName, EntryPoint = "clEnqueueSVMMigrateMem")]
        public static extern ComputeErrorCode EnqueueSVMMigrateMem(
            CLCommandQueueHandle command_queue,
            Int32 num_svm_pointers,
            [MarshalAs(UnmanagedType.LPArray)] IntPtr[] svm_pointers,
            [MarshalAs(UnmanagedType.LPArray)] IntPtr[] sizes,
            ComputeMemMigrationFlags flags,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);

        #endregion
    }
}
