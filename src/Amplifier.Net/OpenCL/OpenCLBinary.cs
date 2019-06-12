using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amplifier.OpenCL
{
    /// <summary>
    /// Serializable class to create binary for the OpenCL compiler. Used for saving and loading of the compiler to save time from kernel compilation. For reusability!!!
    /// </summary>
    public class OpenCLBinary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenCLBinary"/> class.
        /// </summary>
        public OpenCLBinary()
        {
            CompiledInstances = new List<string>();
            Kernels = new List<KernelBin>();
        }

        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        /// <value>
        /// The device identifier.
        /// </value>
        public int DeviceID { get; set; }

        /// <summary>
        /// Gets or sets the compiled instances.
        /// </summary>
        /// <value>
        /// The compiled instances.
        /// </value>
        public List<string> CompiledInstances { get; set; }

        /// <summary>
        /// Gets or sets the kernels.
        /// </summary>
        /// <value>
        /// The kernels.
        /// </value>
        public List<KernelBin> Kernels { get; set; }
    }

    /// <summary>
    /// Kernel binary class to save the program
    /// </summary>
    public class KernelBin
    {
        /// <summary>
        /// Gets or sets the name of the kernel.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the program binaries.
        /// </summary>
        /// <value>
        /// The binaries.
        /// </value>
        public byte[][] Binaries { get; set; }
    }
}
