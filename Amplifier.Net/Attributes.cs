using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplifier
{
    /// <summary>
    /// Static methods, static fields and structures to be converted to CUDA C should be decorated with this attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Enum | AttributeTargets.Class)]
    public class AmplifierAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplifierAttribute"/> class with type set to eAmplifierType.Auto.
        /// </summary>
        public AmplifierAttribute()
        {
            AmplifierType = eAmplifierType.Auto;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmplifierAttribute"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public AmplifierAttribute(eAmplifierType type)
        {
            AmplifierType = type;
        }

        /// <summary>
        /// Gets the type of the Amplifier attribute.
        /// </summary>
        /// <value>
        /// The type of the Amplifier.
        /// </value>
        public eAmplifierType AmplifierType { get; private set; }
     
    }

    /// <summary>
    /// Methods, structures and fields that already have an equivalent in Cuda C should be decorated with this attribute.
    /// The item should have the same name and be in a CUDA C (.cu) file of the same name unless specified.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Struct | AttributeTargets.Field)]
    public class AmplifierDummyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplifierDummyAttribute"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="behaviour">If set to Suppress then do not include CUDA C file of the same name.</param>
        public AmplifierDummyAttribute(eAmplifierType type, eAmplifierDummyBehaviour behaviour = eAmplifierDummyBehaviour.Default)
        {
            AmplifierType = type;
            SupportsEmulation = true;
            Behaviour = behaviour;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmplifierDummyAttribute"/> class.
        /// </summary>
        public AmplifierDummyAttribute()
        {
            AmplifierType = eAmplifierType.Auto;
            SupportsEmulation = true;
            Behaviour = eAmplifierDummyBehaviour.Default;
        }
        
        ///// <summary>
        ///// Initializes a new instance of the <see cref="AmplifierDummyAttribute"/> class.
        ///// </summary>
        //public AmplifierDummyAttribute(bool supportsEmulation = true)
        //    : this(eAmplifierType.Auto, supportsEmulation)
        //{
        //}

        ///// <summary>
        ///// Initializes a new instance of the <see cref="AmplifierDummyAttribute"/> class.
        ///// </summary>
        ///// <param name="type">The type.</param>
        //public AmplifierDummyAttribute(eAmplifierType type, bool supportsEmulation = true)
        //{
        //    AmplifierType = type;
        //    SupportsEmulation = supportsEmulation;
        //    //SourceFile = string.Empty;
        //}

        //public AmplifierDummyAttribute(eAmplifierType type, string sourceFile)
        //{
        //    AmplifierType = type;
        //    SourceFile = sourceFile;
        //}

        //public string SourceFile { get; private set; }

        /// <summary>
        /// Gets the type of the Amplifier attribute.
        /// </summary>
        /// <value>
        /// The type of the Amplifier.
        /// </value>
        public eAmplifierType AmplifierType { get; private set; }

        /// <summary>
        /// Gets the behaviour.
        /// </summary>
        public eAmplifierDummyBehaviour Behaviour { get; private set; }

        /// <summary>
        /// Gets a value indicating whether supports emulation.
        /// </summary>
        /// <value>
        ///   <c>true</c> if supports emulation; otherwise, <c>false</c>.
        /// </value>
        public bool SupportsEmulation { get; private set; }
    }

    /// <summary>
    /// Informs the AmplifierTranslator to ignore the member of a struct.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor)]
    public class AmplifierIgnoreAttribute : Attribute
    {
    }


    /// <summary>
    /// Placed on parameters to indicate the OpenCL address space. Note that if not specified then arrays will
    /// automatically be marked global. Ignored when translating to CUDA.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class AmplifierAddressSpaceAttribute : Attribute
    {
        public AmplifierAddressSpaceAttribute(eAmplifierAddressSpace qualifier)
        {
            Qualifier = qualifier;
        }
        
        public eAmplifierAddressSpace Qualifier { get; private set; }
    }

    /// <summary>
    /// Optionally placed on methods to indicate whether the method should be inlined or not.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AmplifierInlineAttribute : Attribute
    {
        public AmplifierInlineAttribute()
        {
            Mode = eAmplifierInlineMode.Force;
        }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mode"></param>
        public AmplifierInlineAttribute(eAmplifierInlineMode mode)
        {
            Mode = mode;
        }

        /// <summary>
        /// Gets the inline mode.
        /// </summary>
        public eAmplifierInlineMode Mode { get; private set; }
    }
}
