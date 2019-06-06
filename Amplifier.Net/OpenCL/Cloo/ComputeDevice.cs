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

using Amplifier.OpenCL.Cloo.Bindings;

namespace Amplifier.OpenCL.Cloo
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    /// <summary>
    /// Represents an OpenCL device.
    /// </summary>
    /// <value> A device is a collection of compute units. A command queue is used to queue commands to a device. Examples of commands include executing kernels, or reading and writing memory objects. OpenCL devices typically correspond to a GPU, a multi-core CPU, and other processors such as DSPs and the Cell/B.E. processor. </value>
    /// <seealso cref="ComputeCommandQueue"/>
    /// <seealso cref="ComputeKernel"/>
    /// <seealso cref="ComputeMemory"/>
    /// <seealso cref="ComputePlatform"/>
    internal class ComputeDevice : ComputeObject
    {
        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _addressBits;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly bool _available;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly bool _compilerAvailable;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly string _driverVersion;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly bool _endianLittle;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly bool _errorCorrectionSupport;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly ComputeDeviceExecutionCapabilities _executionCapabilities;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly ReadOnlyCollection<string> _extensions;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _globalMemoryCachelineSize;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _globalMemoryCacheSize;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly ComputeDeviceMemoryCacheType _globalMemoryCacheType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _globalMemorySize;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly bool _imageSupport;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _image2DMaxHeight;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _image2DMaxWidth;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _image3DMaxDepth;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _image3DMaxHeight;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _image3DMaxWidth;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _localMemorySize;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly ComputeDeviceLocalMemoryType _localMemoryType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _maxClockFrequency;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _maxComputeUnits;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _maxConstantArguments;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _maxConstantBufferSize;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _maxMemAllocSize;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _maxParameterSize;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _maxReadImageArgs;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _maxSamplers;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _maxWorkGroupSize;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _maxWorkItemDimensions;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly ReadOnlyCollection<long> _maxWorkItemSizes;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _maxWriteImageArgs;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _memBaseAddrAlign;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _minDataTypeAlignSize;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly string _name;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly ComputePlatform _platform;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _preferredVectorWidthChar;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _preferredVectorWidthFloat;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _preferredVectorWidthInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _preferredVectorWidthLong;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _preferredVectorWidthShort;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly string _profile;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _profilingTimerResolution;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly ComputeCommandQueueFlags _queueProperties;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly ComputeDeviceSingleCapabilities _singleCapabilities;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly ComputeDeviceTypes _type;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly string _vendor;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly long _vendorId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly string _version;

        #endregion

        #region Properties

        /// <summary>
        /// The handle of the <see cref="ComputeDevice"/>.
        /// </summary>
        public CLDeviceHandle Handle
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the default <see cref="ComputeDevice"/> address space size in bits.
        /// </summary>
        /// <value> Currently supported values are 32 or 64 bits. </value>
        public long AddressBits => _addressBits;

        /// <summary>
        /// Gets the availability state of the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> Is <c>true</c> if the <see cref="ComputeDevice"/> is available and <c>false</c> otherwise. </value>
        public bool Available => _available;

        /// <summary>
        /// Gets the <see cref="ComputeCommandQueueFlags"/> supported by the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> The <see cref="ComputeCommandQueueFlags"/> supported by the <see cref="ComputeDevice"/>. </value>
        public ComputeCommandQueueFlags CommandQueueFlags => _queueProperties;

        /// <summary>
        /// Gets the availability state of the OpenCL compiler of the <see cref="ComputeDevice.Platform"/>.
        /// </summary>
        /// <value> Is <c>true</c> if the implementation has a compiler available to compile the program source and <c>false</c> otherwise. This can be <c>false</c> for the embededed platform profile only. </value>
        public bool CompilerAvailable => _compilerAvailable;

        /// <summary>
        /// Gets the OpenCL software driver version string of the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> The version string in the form <c>major_number.minor_number</c>. </value>
        public string DriverVersion => _driverVersion;

        /// <summary>
        /// Gets the endianness of the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> Is <c>true</c> if the <see cref="ComputeDevice"/> is a little endian device and <c>false</c> otherwise. </value>
        public bool EndianLittle => _endianLittle;

        /// <summary>
        /// Gets the error correction support state of the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> Is <c>true</c> if the <see cref="ComputeDevice"/> implements error correction for the memories, caches, registers etc. Is <c>false</c> if the <see cref="ComputeDevice"/> does not implement error correction. This can be a requirement for certain clients of OpenCL. </value>
        public bool ErrorCorrectionSupport => _errorCorrectionSupport;

        /// <summary>
        /// Gets the <see cref="ComputeDeviceExecutionCapabilities"/> of the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> The <see cref="ComputeDeviceExecutionCapabilities"/> of the <see cref="ComputeDevice"/>. </value>
        public ComputeDeviceExecutionCapabilities ExecutionCapabilities => _executionCapabilities;

        /// <summary>
        /// Gets a read-only collection of names of extensions that the <see cref="ComputeDevice"/> supports.
        /// </summary>
        /// <value> A read-only collection of names of extensions that the <see cref="ComputeDevice"/> supports. </value>
        public ReadOnlyCollection<string> Extensions => _extensions;

        /// <summary>
        /// Gets the size of the global <see cref="ComputeDevice"/> memory cache line in bytes.
        /// </summary>
        /// <value> The size of the global <see cref="ComputeDevice"/> memory cache line in bytes. </value>
        public long GlobalMemoryCacheLineSize => _globalMemoryCachelineSize;

        /// <summary>
        /// Gets the size of the global <see cref="ComputeDevice"/> memory cache in bytes.
        /// </summary>
        /// <value> The size of the global <see cref="ComputeDevice"/> memory cache in bytes. </value>
        public long GlobalMemoryCacheSize => _globalMemoryCacheSize;

        /// <summary>
        /// Gets the <see cref="ComputeDeviceMemoryCacheType"/> of the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> The <see cref="ComputeDeviceMemoryCacheType"/> of the <see cref="ComputeDevice"/>. </value>
        public ComputeDeviceMemoryCacheType GlobalMemoryCacheType => _globalMemoryCacheType;

        /// <summary>
        /// Gets the size of the global <see cref="ComputeDevice"/> memory in bytes.
        /// </summary>
        /// <value> The size of the global <see cref="ComputeDevice"/> memory in bytes. </value>
        public long GlobalMemorySize => _globalMemorySize;

        /// <summary>
        /// Gets the maximum <see cref="ComputeImage.Height"/> value for <see cref="ComputeImage2D"/> that the <see cref="ComputeDevice"/> supports in pixels.
        /// </summary>
        /// <value> The minimum value is 8192 if <see cref="ComputeDevice.ImageSupport"/> is <c>true</c>. </value>
        public long Image2DMaxHeight => _image2DMaxHeight;

        /// <summary>
        /// Gets the maximum <see cref="ComputeImage.Width"/> value for <see cref="ComputeImage2D"/> that the <see cref="ComputeDevice"/> supports in pixels.
        /// </summary>
        /// <value> The minimum value is 8192 if <see cref="ComputeDevice.ImageSupport"/> is <c>true</c>. </value>
        public long Image2DMaxWidth => _image2DMaxWidth;

        /// <summary>
        /// Gets the maximum <see cref="ComputeImage.Depth"/> value for <see cref="ComputeImage3D"/> that the <see cref="ComputeDevice"/> supports in pixels.
        /// </summary>
        /// <value> The minimum value is 2048 if <see cref="ComputeDevice.ImageSupport"/> is <c>true</c>. </value>
        public long Image3DMaxDepth => _image3DMaxDepth;

        /// <summary>
        /// Gets the maximum <see cref="ComputeImage.Height"/> value for <see cref="ComputeImage3D"/> that the <see cref="ComputeDevice"/> supports in pixels.
        /// </summary>
        /// <value> The minimum value is 2048 if <see cref="ComputeDevice.ImageSupport"/> is <c>true</c>. </value>
        public long Image3DMaxHeight => _image3DMaxHeight;

        /// <summary>
        /// Gets the maximum <see cref="ComputeImage.Width"/> value for <see cref="ComputeImage3D"/> that the <see cref="ComputeDevice"/> supports in pixels.
        /// </summary>
        /// <value> The minimum value is 2048 if <see cref="ComputeDevice.ImageSupport"/> is <c>true</c>. </value>
        public long Image3DMaxWidth => _image3DMaxWidth;

        /// <summary>
        /// Gets the state of image support of the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> Is <c>true</c> if <see cref="ComputeImage"/>s are supported by the <see cref="ComputeDevice"/> and <c>false</c> otherwise. </value>
        public bool ImageSupport => _imageSupport;

        /// <summary>
        /// Gets the size of local memory are of the <see cref="ComputeDevice"/> in bytes.
        /// </summary>
        /// <value> The minimum value is 16 KB (OpenCL 1.0) or 32 KB (OpenCL 1.1). </value>
        public long LocalMemorySize => _localMemorySize;

        /// <summary>
        /// Gets the <see cref="ComputeDeviceLocalMemoryType"/> that is supported on the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> The <see cref="ComputeDeviceLocalMemoryType"/> that is supported on the <see cref="ComputeDevice"/>. </value>
        public ComputeDeviceLocalMemoryType LocalMemoryType => _localMemoryType;

        /// <summary>
        /// Gets the maximum configured clock frequency of the <see cref="ComputeDevice"/> in MHz.
        /// </summary>
        /// <value> The maximum configured clock frequency of the <see cref="ComputeDevice"/> in MHz. </value>
        public long MaxClockFrequency => _maxClockFrequency;

        /// <summary>
        /// Gets the number of parallel compute cores on the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> The minimum value is 1. </value>
        public long MaxComputeUnits => _maxComputeUnits;

        /// <summary>
        /// Gets the maximum number of arguments declared with the <c>__constant</c> or <c>constant</c> qualifier in a <see cref="ComputeKernel"/> executing in the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> The minimum value is 8. </value>
        public long MaxConstantArguments => _maxConstantArguments;

        /// <summary>
        /// Gets the maximum size in bytes of a constant buffer allocation in the <see cref="ComputeDevice"/> memory.
        /// </summary>
        /// <value> The minimum value is 64 KB. </value>
        public long MaxConstantBufferSize => _maxConstantBufferSize;

        /// <summary>
        /// Gets the maximum size of memory object allocation in the <see cref="ComputeDevice"/> memory in bytes.
        /// </summary>
        /// <value> The minimum value is <c>max(<see cref="ComputeDevice.GlobalMemorySize"/>/4, 128*1024*1024)</c>. </value>
        public long MaxMemoryAllocationSize => _maxMemAllocSize;

        /// <summary>
        /// Gets the maximum size in bytes of the arguments that can be passed to a <see cref="ComputeKernel"/> executing in the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> The minimum value is 256 (OpenCL 1.0) or 1024 (OpenCL 1.1). </value>
        public long MaxParameterSize => _maxParameterSize;

        /// <summary>
        /// Gets the maximum number of simultaneous <see cref="ComputeImage"/>s that can be read by a <see cref="ComputeKernel"/> executing in the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> The minimum value is 128 if <see cref="ComputeDevice.ImageSupport"/> is <c>true</c>. </value>
        public long MaxReadImageArguments => _maxReadImageArgs;

        /// <summary>
        /// Gets the maximum number of <see cref="ComputeSampler"/>s that can be used in a <see cref="ComputeKernel"/>.
        /// </summary>
        /// <value> The minimum value is 16 if <see cref="ComputeDevice.ImageSupport"/> is <c>true</c>. </value>
        public long MaxSamplers => _maxSamplers;

        /// <summary>
        /// Gets the maximum number of work-items in a work-group executing a <see cref="ComputeKernel"/> in a <see cref="ComputeDevice"/> using the data parallel execution model.
        /// </summary>
        /// <value> The minimum value is 1. </value>
        public long MaxWorkGroupSize => _maxWorkGroupSize;

        /// <summary>
        /// Gets the maximum number of dimensions that specify the global and local work-item IDs used by the data parallel execution model.
        /// </summary>
        /// <value> The minimum value is 3. </value>
        public long MaxWorkItemDimensions => _maxWorkItemDimensions;

        /// <summary>
        /// Gets the maximum number of work-items that can be specified in each dimension of the globalWorkSize argument of <see cref="ComputeCommandQueue.Execute"/>.
        /// </summary>
        /// <value> The maximum number of work-items that can be specified in each dimension of the globalWorkSize argument of <see cref="ComputeCommandQueue.Execute"/>. </value>
        public ReadOnlyCollection<long> MaxWorkItemSizes => _maxWorkItemSizes;

        /// <summary>
        /// Gets the maximum number of simultaneous <see cref="ComputeImage"/>s that can be written to by a <see cref="ComputeKernel"/> executing in the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> The minimum value is 8 if <see cref="ComputeDevice.ImageSupport"/> is <c>true</c>. </value>
        public long MaxWriteImageArguments => _maxWriteImageArgs;

        /// <summary>
        /// Gets the alignment in bits of the base address of any <see cref="ComputeMemory"/> allocated in the <see cref="ComputeDevice"/> memory.
        /// </summary>
        /// <value> The alignment in bits of the base address of any <see cref="ComputeMemory"/> allocated in the <see cref="ComputeDevice"/> memory. </value>
        public long MemoryBaseAddressAlignment => _memBaseAddrAlign;

        /// <summary>
        /// Gets the smallest alignment in bytes which can be used for any data type allocated in the <see cref="ComputeDevice"/> memory.
        /// </summary>
        /// <value> The smallest alignment in bytes which can be used for any data type allocated in the <see cref="ComputeDevice"/> memory. </value>
        public long MinDataTypeAlignmentSize => _minDataTypeAlignSize;

        /// <summary>
        /// Gets the name of the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> The name of the <see cref="ComputeDevice"/>. </value>
        public string Name => _name;

        /// <summary>
        /// Gets the <see cref="ComputePlatform"/> associated with the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> The <see cref="ComputePlatform"/> associated with the <see cref="ComputeDevice"/>. </value>
        public ComputePlatform Platform => _platform;

        /// <summary>
        /// Gets the <see cref="ComputeDevice"/>'s preferred native vector width size for vector of <c>char</c>s.
        /// </summary>
        /// <value> The <see cref="ComputeDevice"/>'s preferred native vector width size for vector of <c>char</c>s. </value>
        /// <remarks> The vector width is defined as the number of scalar elements that can be stored in the vector. </remarks>
        public long PreferredVectorWidthChar => _preferredVectorWidthChar;

        /// <summary>
        /// Gets the <see cref="ComputeDevice"/>'s preferred native vector width size for vector of <c>double</c>s or 0 if the <c>cl_khr_fp64</c> format is not supported.
        /// </summary>
        /// <value> The <see cref="ComputeDevice"/>'s preferred native vector width size for vector of <c>double</c>s or 0 if the <c>cl_khr_fp64</c> format is not supported. </value>
        /// <remarks> The vector width is defined as the number of scalar elements that can be stored in the vector. </remarks>
        public long PreferredVectorWidthDouble => GetInfo<uint>(ComputeDeviceInfo.PreferredVectorWidthDouble);

        /// <summary>
        /// Gets the <see cref="ComputeDevice"/>'s preferred native vector width size for vector of <c>float</c>s.
        /// </summary>
        /// <value> The <see cref="ComputeDevice"/>'s preferred native vector width size for vector of <c>float</c>s. </value>
        /// <remarks> The vector width is defined as the number of scalar elements that can be stored in the vector. </remarks>
        public long PreferredVectorWidthFloat => _preferredVectorWidthFloat;

        /// <summary>
        /// Gets the <see cref="ComputeDevice"/>'s preferred native vector width size for vector of <c>half</c>s or 0 if the <c>cl_khr_fp16</c> format is not supported.
        /// </summary>
        /// <value> The <see cref="ComputeDevice"/>'s preferred native vector width size for vector of <c>half</c>s or 0 if the <c>cl_khr_fp16</c> format is not supported. </value>
        /// <remarks> The vector width is defined as the number of scalar elements that can be stored in the vector. </remarks>
        public long PreferredVectorWidthHalf => GetInfo<uint>(ComputeDeviceInfo.PreferredVectorWidthHalf);

        /// <summary>
        /// Gets the <see cref="ComputeDevice"/>'s preferred native vector width size for vector of <c>int</c>s.
        /// </summary>
        /// <value> The <see cref="ComputeDevice"/>'s preferred native vector width size for vector of <c>int</c>s. </value>
        /// <remarks> The vector width is defined as the number of scalar elements that can be stored in the vector. </remarks>
        public long PreferredVectorWidthInt => _preferredVectorWidthInt;

        /// <summary>
        /// Gets the <see cref="ComputeDevice"/>'s preferred native vector width size for vector of <c>long</c>s.
        /// </summary>
        /// <value> The <see cref="ComputeDevice"/>'s preferred native vector width size for vector of <c>long</c>s. </value>
        /// <remarks> The vector width is defined as the number of scalar elements that can be stored in the vector. </remarks>
        public long PreferredVectorWidthLong => _preferredVectorWidthLong;

        /// <summary>
        /// Gets the <see cref="ComputeDevice"/>'s preferred native vector width size for vector of <c>short</c>s.
        /// </summary>
        /// <value> The <see cref="ComputeDevice"/>'s preferred native vector width size for vector of <c>short</c>s. </value>
        /// <remarks> The vector width is defined as the number of scalar elements that can be stored in the vector. </remarks>
        public long PreferredVectorWidthShort => _preferredVectorWidthShort;

        /// <summary>
        /// Gets the OpenCL profile name supported by the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> 
        /// The profile name returned can be one of the following strings:
        /// <list type="bullets">
        /// <item>
        ///     <term> FULL_PROFILE </term>
        ///     <description> The <see cref="ComputeDevice"/> supports the OpenCL specification (functionality defined as part of the core specification and does not require any extensions to be supported). </description>
        /// </item>
        /// <item>
        ///     <term> EMBEDDED_PROFILE </term>
        ///     <description> The <see cref="ComputeDevice"/> supports the OpenCL embedded profile. </description>
        /// </item>
        /// </list>
        /// </value>
        public string Profile => _profile;

        /// <summary>
        /// Gets the resolution of the <see cref="ComputeDevice"/> timer in nanoseconds.
        /// </summary>
        /// <value> The resolution of the <see cref="ComputeDevice"/> timer in nanoseconds. </value>
        public long ProfilingTimerResolution => _profilingTimerResolution;

        /// <summary>
        /// Gets the <see cref="ComputeDeviceSingleCapabilities"/> of the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> The <see cref="ComputeDeviceSingleCapabilities"/> of the <see cref="ComputeDevice"/>. </value>
        public ComputeDeviceSingleCapabilities SingleCapabilites => _singleCapabilities;

        /// <summary>
        /// Gets the <see cref="ComputeDeviceTypes"/> of the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> The <see cref="ComputeDeviceTypes"/> of the <see cref="ComputeDevice"/>. </value>
        public ComputeDeviceTypes Type => _type;

        /// <summary>
        /// Gets the <see cref="ComputeDevice"/> vendor name string.
        /// </summary>
        /// <value> The <see cref="ComputeDevice"/> vendor name string. </value>
        public string Vendor => _vendor;

        /// <summary>
        /// Gets a unique <see cref="ComputeDevice"/> vendor identifier.
        /// </summary>
        /// <value> A unique <see cref="ComputeDevice"/> vendor identifier. </value>
        /// <remarks> An example of a unique device identifier could be the PCIe ID. </remarks>
        public long VendorId => _vendorId;

        /// <summary>
        /// Gets the OpenCL version supported by the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> The OpenCL version supported by the <see cref="ComputeDevice"/>. </value>
        public Version Version => ComputeTools.ParseVersionString(VersionString, 1);

        /// <summary>
        /// Gets the OpenCL version string supported by the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> The version string has the following format: <c>OpenCL[space][major_version].[minor_version][space][vendor-specific information]</c>. </value>
        public string VersionString => _version;

        //////////////////////////////////
        // OpenCL 1.1 device properties //
        //////////////////////////////////

        /// <summary>
        /// Gets information about the presence of the unified memory subsystem.
        /// </summary>
        /// <value> Is <c>true</c> if the <see cref="ComputeDevice"/> and the host have a unified memory subsystem and <c>false</c> otherwise. </value>
        /// <remarks> Requires OpenCL 1.1 </remarks>
        public bool HostUnifiedMemory => GetBoolInfo(ComputeDeviceInfo.HostUnifiedMemory);

        /// <summary>
        /// Gets the native ISA vector width size for vector of <c>char</c>s.
        /// </summary>
        /// <value> The native ISA vector width size for vector of <c>char</c>s. </value>
        /// <remarks> 
        ///     <para> The vector width is defined as the number of scalar elements that can be stored in the vector. </para>
        ///     <para> Requires OpenCL 1.1 </para>
        /// </remarks>
        public long NativeVectorWidthChar => GetInfo<long>(ComputeDeviceInfo.NativeVectorWidthChar);

        /// <summary>
        /// Gets the native ISA vector width size for vector of <c>double</c>s or 0 if the <c>cl_khr_fp64</c> format is not supported.
        /// </summary>
        /// <value> The native ISA vector width size for vector of <c>double</c>s or 0 if the <c>cl_khr_fp64</c> format is not supported. </value>
        /// <remarks> 
        ///     <para> The vector width is defined as the number of scalar elements that can be stored in the vector. </para>
        ///     <para> Requires OpenCL 1.1 </para>
        /// </remarks>
        public long NativeVectorWidthDouble => GetInfo<long>(ComputeDeviceInfo.NativeVectorWidthDouble);

        /// <summary>
        /// Gets the native ISA vector width size for vector of <c>float</c>s.
        /// </summary>
        /// <value> The native ISA vector width size for vector of <c>float</c>s. </value>
        /// <remarks> 
        ///     <para> The vector width is defined as the number of scalar elements that can be stored in the vector. </para>
        ///     <para> Requires OpenCL 1.1 </para>
        /// </remarks>
        public long NativeVectorWidthFloat => GetInfo<long>(ComputeDeviceInfo.NativeVectorWidthFloat);

        /// <summary>
        /// Gets the native ISA vector width size for vector of <c>half</c>s or 0 if the <c>cl_khr_fp16</c> format is not supported.
        /// </summary>
        /// <value> The native ISA vector width size for vector of <c>half</c>s or 0 if the <c>cl_khr_fp16</c> format is not supported. </value>
        /// <remarks> 
        ///     <para> The vector width is defined as the number of scalar elements that can be stored in the vector. </para>
        ///     <para> Requires OpenCL 1.1 </para>
        /// </remarks>
        public long NativeVectorWidthHalf => GetInfo<long>(ComputeDeviceInfo.NativeVectorWidthHalf);

        /// <summary>
        /// Gets the native ISA vector width size for vector of <c>int</c>s.
        /// </summary>
        /// <value> The native ISA vector width size for vector of <c>int</c>s. </value>
        /// <remarks>
        ///     <para> The vector width is defined as the number of scalar elements that can be stored in the vector. </para>
        ///     <para> Requires OpenCL 1.1 </para>
        /// </remarks>
        public long NativeVectorWidthInt => GetInfo<long>(ComputeDeviceInfo.NativeVectorWidthInt);

        /// <summary>
        /// Gets the native ISA vector width size for vector of <c>long</c>s.
        /// </summary>
        /// <value> The native ISA vector width size for vector of <c>long</c>s. </value>
        /// <remarks>
        ///     <para> The vector width is defined as the number of scalar elements that can be stored in the vector. </para>
        ///     <para> Requires OpenCL 1.1 </para>
        /// </remarks>
        public long NativeVectorWidthLong => GetInfo<long>(ComputeDeviceInfo.NativeVectorWidthLong);

        /// <summary>
        /// Gets the native ISA vector width size for vector of <c>short</c>s.
        /// </summary>
        /// <value> The native ISA vector width size for vector of <c>short</c>s. </value>
        /// <remarks> 
        ///     <para> The vector width is defined as the number of scalar elements that can be stored in the vector. </para>
        ///     <para> Requires OpenCL 1.1 </para>
        /// </remarks>
        public long NativeVectorWidthShort => GetInfo<long>(ComputeDeviceInfo.NativeVectorWidthShort);

        /// <summary>
        /// Gets the OpenCL C version supported by the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> Is <c>1.1</c> if <see cref="ComputeDevice.Version"/> is <c>1.1</c>. Is <c>1.0</c> or <c>1.1</c> if <see cref="ComputeDevice.Version"/> is <c>1.0</c>. </value>
        /// <remarks> Requires OpenCL 1.1. </remarks>
        public Version OpenCLCVersion => ComputeTools.ParseVersionString(OpenCLCVersionString, 2);

        /// <summary>
        /// Gets the OpenCL C version string supported by the <see cref="ComputeDevice"/>.
        /// </summary>
        /// <value> The OpenCL C version string supported by the <see cref="ComputeDevice"/>. The version string has the following format: <c>OpenCL[space]C[space][major_version].[minor_version][space][vendor-specific information]</c>. </value>
        /// <remarks> Requires OpenCL 1.1. </remarks>
        public string OpenCLCVersionString => GetStringInfo(ComputeDeviceInfo.OpenCLCVersion);

        #endregion

        #region Constructors

        internal ComputeDevice(ComputePlatform platform, CLDeviceHandle handle)
        {
            Handle = handle;
            SetID(Handle.Value);

            _addressBits = GetInfo<uint>(ComputeDeviceInfo.AddressBits);
            _available = GetBoolInfo(ComputeDeviceInfo.Available);
            _compilerAvailable = GetBoolInfo(ComputeDeviceInfo.CompilerAvailable);
            _driverVersion = GetStringInfo(ComputeDeviceInfo.DriverVersion);
            _endianLittle = GetBoolInfo(ComputeDeviceInfo.EndianLittle);
            _errorCorrectionSupport = GetBoolInfo(ComputeDeviceInfo.ErrorCorrectionSupport);
            _executionCapabilities = (ComputeDeviceExecutionCapabilities)GetInfo<long>(ComputeDeviceInfo.ExecutionCapabilities);

            string extensionString = GetStringInfo(ComputeDeviceInfo.Extensions);
            _extensions = new ReadOnlyCollection<string>(extensionString.Split(new [] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            _globalMemoryCachelineSize = GetInfo<uint>(ComputeDeviceInfo.GlobalMemoryCachelineSize);
            _globalMemoryCacheSize = (long)GetInfo<ulong>(ComputeDeviceInfo.GlobalMemoryCacheSize);
            _globalMemoryCacheType = (ComputeDeviceMemoryCacheType)GetInfo<long>(ComputeDeviceInfo.GlobalMemoryCacheType);
            _globalMemorySize = (long)GetInfo<ulong>(ComputeDeviceInfo.GlobalMemorySize);
            _image2DMaxHeight = (long)GetInfo<IntPtr>(ComputeDeviceInfo.Image2DMaxHeight);
            _image2DMaxWidth = (long)GetInfo<IntPtr>(ComputeDeviceInfo.Image2DMaxWidth);
            _image3DMaxDepth = (long)GetInfo<IntPtr>(ComputeDeviceInfo.Image3DMaxDepth);
            _image3DMaxHeight = (long)GetInfo<IntPtr>(ComputeDeviceInfo.Image3DMaxHeight);
            _image3DMaxWidth = (long)GetInfo<IntPtr>(ComputeDeviceInfo.Image3DMaxWidth);
            _imageSupport = GetBoolInfo(ComputeDeviceInfo.ImageSupport);
            _localMemorySize = (long)GetInfo<ulong>(ComputeDeviceInfo.LocalMemorySize);
            _localMemoryType = (ComputeDeviceLocalMemoryType)GetInfo<long>(ComputeDeviceInfo.LocalMemoryType);
            _maxClockFrequency = GetInfo<uint>(ComputeDeviceInfo.MaxClockFrequency);
            _maxComputeUnits = GetInfo<uint>(ComputeDeviceInfo.MaxComputeUnits);
            _maxConstantArguments = GetInfo<uint>(ComputeDeviceInfo.MaxConstantArguments);
            _maxConstantBufferSize = (long)GetInfo<ulong>(ComputeDeviceInfo.MaxConstantBufferSize);
            _maxMemAllocSize = (long)GetInfo<ulong>(ComputeDeviceInfo.MaxMemoryAllocationSize);
            _maxParameterSize = (long)GetInfo<IntPtr>(ComputeDeviceInfo.MaxParameterSize);
            _maxReadImageArgs = GetInfo<uint>(ComputeDeviceInfo.MaxReadImageArguments);
            _maxSamplers = GetInfo<uint>(ComputeDeviceInfo.MaxSamplers);
            _maxWorkGroupSize = (long)GetInfo<IntPtr>(ComputeDeviceInfo.MaxWorkGroupSize);
            _maxWorkItemDimensions = GetInfo<uint>(ComputeDeviceInfo.MaxWorkItemDimensions);
            _maxWorkItemSizes = new ReadOnlyCollection<long>(ComputeTools.ConvertArray(GetArrayInfo<CLDeviceHandle, ComputeDeviceInfo, IntPtr>(Handle, ComputeDeviceInfo.MaxWorkItemSizes, CL12.GetDeviceInfo)));
            _maxWriteImageArgs = GetInfo<uint>(ComputeDeviceInfo.MaxWriteImageArguments);
            _memBaseAddrAlign = GetInfo<uint>(ComputeDeviceInfo.MemoryBaseAddressAlignment);
            _minDataTypeAlignSize = GetInfo<uint>(ComputeDeviceInfo.MinDataTypeAlignmentSize);
            _name = GetStringInfo(ComputeDeviceInfo.Name);
            _platform = platform;
            _preferredVectorWidthChar = GetInfo<uint>(ComputeDeviceInfo.PreferredVectorWidthChar);
            _preferredVectorWidthFloat = GetInfo<uint>(ComputeDeviceInfo.PreferredVectorWidthFloat);
            _preferredVectorWidthInt = GetInfo<uint>(ComputeDeviceInfo.PreferredVectorWidthInt);
            _preferredVectorWidthLong = GetInfo<uint>(ComputeDeviceInfo.PreferredVectorWidthLong);
            _preferredVectorWidthShort = GetInfo<uint>(ComputeDeviceInfo.PreferredVectorWidthShort);
            _profile = GetStringInfo(ComputeDeviceInfo.Profile);
            _profilingTimerResolution = (long)GetInfo<IntPtr>(ComputeDeviceInfo.ProfilingTimerResolution);
            _queueProperties = (ComputeCommandQueueFlags)GetInfo<long>(ComputeDeviceInfo.CommandQueueProperties);
            _singleCapabilities = (ComputeDeviceSingleCapabilities)GetInfo<long>(ComputeDeviceInfo.SingleFPConfig);
            _type = (ComputeDeviceTypes)GetInfo<long>(ComputeDeviceInfo.Type);
            _vendor = GetStringInfo(ComputeDeviceInfo.Vendor);
            _vendorId = GetInfo<uint>(ComputeDeviceInfo.VendorId);
            _version = GetStringInfo(ComputeDeviceInfo.Version);
        }

        #endregion

        #region Private methods

        private bool GetBoolInfo(ComputeDeviceInfo paramName)
        {
            return GetBoolInfo(Handle, paramName, CL12.GetDeviceInfo);
        }

        private TNativeType GetInfo<TNativeType>(ComputeDeviceInfo paramName) where TNativeType : struct
        {
            return GetInfo<CLDeviceHandle, ComputeDeviceInfo, TNativeType>(Handle, paramName, CL12.GetDeviceInfo);
        }

        private string GetStringInfo(ComputeDeviceInfo paramName)
        {
            return GetStringInfo(Handle, paramName, CL12.GetDeviceInfo);
        }

        #endregion
    }
}