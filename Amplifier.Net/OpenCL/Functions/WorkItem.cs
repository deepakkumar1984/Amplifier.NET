using System;
using System.Collections.Generic;
using System.Text;
using size_t = System.Int32;
using ptrdiff_t = System.Int32;
using intptr_t = System.Int32;
using uintptr_t = System.UInt32;

namespace Amplifier.OpenCL
{
    public partial class OpenCLFunctions
    {
        /// <summary>
        /// Global work item ID value
        /// </summary>
        /// <param name="dimindx">The dimindx.</param>
        /// <returns></returns>
        public size_t get_global_id(int dimindx) { return 0; }

        /// <summary>
        /// Number of dimensions in use
        /// </summary>
        /// <returns></returns>
        public uint get_work_dim() { return 0; }

        /// <summary>
        /// Number of global work items
        /// </summary>
        /// <param name="dimindx">The dimindx.</param>
        /// <returns></returns>
        public size_t get_global_size(int dimindx) { return 0; }

        /// <summary>
        /// Local work item ID
        /// </summary>
        /// <param name="dimindx">The dimindx.</param>
        /// <returns></returns>
        public size_t get_local_id(int dimindx) { return 0; }

        /// <summary>
        /// Number of local work items
        /// </summary>
        /// <param name="dimindx">The dimindx.</param>
        /// <returns></returns>
        public size_t get_local_size(int dimindx) { return 0; }

        /// <summary>
        /// Number of work groups
        /// </summary>
        /// <param name="dimindx">The dimindx.</param>
        /// <returns></returns>
        public size_t get_num_groups(int dimindx) { return 0; }

        /// <summary>
        /// Work group ID
        /// </summary>
        /// <param name="dimindx">The dimindx.</param>
        /// <returns></returns>
        public size_t get_group_id(int dimindx) { return 0; }

        /// <summary>
        /// Returns the offset values specified in global_work_offset argument to clEnqueueNDRangeKernel. Valid values of dimindx are 0 to get_work_dim() - 1. For other values, get_global_offset() returns 0.
        /// For clEnqueueTask, this returns 0.
        /// </summary>
        /// <param name="dimindx">The dimindx.</param>
        /// <returns></returns>
        public size_t get_global_offset(int dimindx) { return 0; }
    }
}
