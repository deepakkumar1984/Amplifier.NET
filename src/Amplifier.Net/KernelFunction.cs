using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amplifier
{
    /// <summary>
    /// Class to hold the kernel method info
    /// </summary>
    public class KernelFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KernelFunction"/> class.
        /// </summary>
        public KernelFunction()
        {
            Parameters = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public Dictionary<string, string> Parameters { get; set; }
    }
}
