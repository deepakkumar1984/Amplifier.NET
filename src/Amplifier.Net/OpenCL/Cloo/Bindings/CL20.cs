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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

namespace Amplifier.OpenCL.Cloo.Bindings
{
    /// <summary>
    /// Contains bindings to the OpenCL 2.0 functions.
    /// </summary>
    /// <remarks> See the OpenCL specification for documentation regarding these functions. </remarks>
    [SuppressUnmanagedCodeSecurity]
    internal class CL20 : CL12
    {
        #region Command Queue

        /// <summary>
        /// Creates a host or device command-queue on a specific device.
        /// </summary>
        [DllImport(libName, EntryPoint = "clCreateCommandQueueWithProperties")]
        public static extern CLCommandQueueHandle CreateCommandQueueWithProperties(
            CLContextHandle context,
            CLDeviceHandle device,
            [MarshalAs(UnmanagedType.LPArray)] IntPtr[] properties,
            out ComputeErrorCode errcode_ret);

        #endregion

        #region Pipes

        /// <summary>
        /// Creates a pipe object.
        /// </summary>
        [DllImport(libName, EntryPoint = "clCreatePipe")]
        public static extern CLMemoryHandle CreatePipe(
            CLContextHandle context,
            ComputeMemoryFlags flags,
            Int32 pipe_packet_size,
            Int32 pipe_max_packets,
            [MarshalAs(UnmanagedType.LPArray)] IntPtr[] properties,
            out ComputeErrorCode errcode_ret);

        /// <summary>
        /// Gets information specific to a pipe object.
        /// </summary>
        [DllImport(libName, EntryPoint = "clGetPipeInfo")]
        public static extern ComputeErrorCode GetPipeInfo(
            CLMemoryHandle pipe,
            ComputePipeInfo param_name,
            IntPtr param_value_size,
            IntPtr param_value,
            out IntPtr param_value_size_ret);

        #endregion

        #region Sampler

        /// <summary>
        /// Creates a sampler object with properties.
        /// </summary>
        [DllImport(libName, EntryPoint = "clCreateSamplerWithProperties")]
        public static extern CLSamplerHandle CreateSamplerWithProperties(
            CLContextHandle context,
            [MarshalAs(UnmanagedType.LPArray)] IntPtr[] sampler_properties,
            out ComputeErrorCode errcode_ret);

        #endregion

        #region Shared Virtual Memory (SVM)

        /// <summary>
        /// Allocates a shared virtual memory buffer.
        /// </summary>
        [DllImport(libName, EntryPoint = "clSVMAlloc")]
        public static extern IntPtr SVMAlloc(
            CLContextHandle context,
            ComputeSVMMemFlags flags,
            IntPtr size,
            Int32 alignment);

        /// <summary>
        /// Frees a shared virtual memory buffer.
        /// </summary>
        [DllImport(libName, EntryPoint = "clSVMFree")]
        public static extern void SVMFree(
            CLContextHandle context,
            IntPtr svm_pointer);

        /// <summary>
        /// Enqueues a command to free shared virtual memory buffers.
        /// </summary>
        [DllImport(libName, EntryPoint = "clEnqueueSVMFree")]
        public static extern ComputeErrorCode EnqueueSVMFree(
            CLCommandQueueHandle command_queue,
            Int32 num_svm_pointers,
            [MarshalAs(UnmanagedType.LPArray)] IntPtr[] svm_pointers,
            IntPtr pfn_free_func,
            IntPtr user_data,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);

        /// <summary>
        /// Enqueues a command to do a memcpy operation on shared virtual memory.
        /// </summary>
        [DllImport(libName, EntryPoint = "clEnqueueSVMMemcpy")]
        public static extern ComputeErrorCode EnqueueSVMMemcpy(
            CLCommandQueueHandle command_queue,
            ComputeBoolean blocking_copy,
            IntPtr dst_ptr,
            IntPtr src_ptr,
            IntPtr size,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);

        /// <summary>
        /// Enqueues a command to fill a region in memory with a pattern.
        /// </summary>
        [DllImport(libName, EntryPoint = "clEnqueueSVMMemFill")]
        public static extern ComputeErrorCode EnqueueSVMMemFill(
            CLCommandQueueHandle command_queue,
            IntPtr svm_ptr,
            IntPtr pattern,
            IntPtr pattern_size,
            IntPtr size,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);

        /// <summary>
        /// Enqueues a command that will allow the host to update a region of a SVM buffer.
        /// </summary>
        [DllImport(libName, EntryPoint = "clEnqueueSVMMap")]
        public static extern ComputeErrorCode EnqueueSVMMap(
            CLCommandQueueHandle command_queue,
            ComputeBoolean blocking_map,
            ComputeMemoryMappingFlags flags,
            IntPtr svm_ptr,
            IntPtr size,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);

        /// <summary>
        /// Enqueues a command to indicate that the host has completed updating a region.
        /// </summary>
        [DllImport(libName, EntryPoint = "clEnqueueSVMUnmap")]
        public static extern ComputeErrorCode EnqueueSVMUnmap(
            CLCommandQueueHandle command_queue,
            IntPtr svm_ptr,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event);

        /// <summary>
        /// Sets the argument value for a specific argument of a kernel to a SVM pointer.
        /// </summary>
        [DllImport(libName, EntryPoint = "clSetKernelArgSVMPointer")]
        public static extern ComputeErrorCode SetKernelArgSVMPointer(
            CLKernelHandle kernel,
            Int32 arg_index,
            IntPtr arg_value);

        /// <summary>
        /// Used to pass additional information other than argument values to a kernel.
        /// </summary>
        [DllImport(libName, EntryPoint = "clSetKernelExecInfo")]
        public static extern ComputeErrorCode SetKernelExecInfo(
            CLKernelHandle kernel,
            ComputeKernelExecInfo param_name,
            IntPtr param_value_size,
            IntPtr param_value);

        #endregion

        #region Deprecated functions

        /// <summary>
        /// See the OpenCL specification.
        /// </summary>
        [Obsolete("Deprecated in OpenCL 2.0. Use CreateCommandQueueWithProperties instead.")]
        public new static CLCommandQueueHandle CreateCommandQueue(
            CLContextHandle context,
            CLDeviceHandle device,
            ComputeCommandQueueFlags properties,
            out ComputeErrorCode errcode_ret)
        {
            Debug.WriteLine("WARNING! clCreateCommandQueue has been deprecated in OpenCL 2.0.");
            return CL12.CreateCommandQueue(context, device, properties, out errcode_ret);
        }

        /// <summary>
        /// See the OpenCL specification.
        /// </summary>
        [Obsolete("Deprecated in OpenCL 2.0. Use CreateSamplerWithProperties instead.")]
        public new static CLSamplerHandle CreateSampler(
            CLContextHandle context,
            [MarshalAs(UnmanagedType.Bool)] bool normalized_coords,
            ComputeImageAddressing addressing_mode,
            ComputeImageFiltering filter_mode,
            out ComputeErrorCode errcode_ret)
        {
            Debug.WriteLine("WARNING! clCreateSampler has been deprecated in OpenCL 2.0.");
            return CL12.CreateSampler(context, normalized_coords, addressing_mode, filter_mode, out errcode_ret);
        }

        /// <summary>
        /// See the OpenCL specification.
        /// </summary>
        [Obsolete("Deprecated in OpenCL 2.0.")]
        public new static ComputeErrorCode EnqueueTask(
            CLCommandQueueHandle command_queue,
            CLKernelHandle kernel,
            Int32 num_events_in_wait_list,
            [MarshalAs(UnmanagedType.LPArray)] CLEventHandle[] event_wait_list,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] CLEventHandle[] new_event)
        {
            Debug.WriteLine("WARNING! clEnqueueTask has been deprecated in OpenCL 2.0.");
            return CL12.EnqueueTask(command_queue, kernel, num_events_in_wait_list, event_wait_list, new_event);
        }

        #endregion
    }
}
