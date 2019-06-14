namespace Amplifier.OpenCL
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    internal class KernelBin
    {
        /// <summary>
        /// Gets or sets the source code.
        /// </summary>
        /// <value>
        /// The source code.
        /// </value>
        public string SourceCode { get; set; }

        /// <summary>
        /// Gets or sets the instances of the kernel class.
        /// </summary>
        /// <value>
        /// The instances.
        /// </value>
        public List<Type> Instances { get; set; }

        /// <summary>
        /// Gets or sets the class instance methods.
        /// </summary>
        /// <value>
        /// The instance methods.
        /// </value>
        public List<KernelFunction> InstanceMethods { get; set; }
    }
}
