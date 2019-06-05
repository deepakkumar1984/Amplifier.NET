/*
Amplifier.NET - LGPL 2.1 License
Please consider purchasing a commerical license - it helps development, frees you from LGPL restrictions
and provides you with support.  Thank you!
Copyright (C) 2011 Hybrid DSP Systems
http://www.hybriddsp.com

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplifier
{

    ///// <summary>
    ///// Flag for code generator.
    ///// </summary>
    //[Flags]
    //public enum eGPUCodeGenerator
    //{
    //    /// <summary>
    //    /// None selected.
    //    /// </summary>
    //    None = 0,
    //    /// <summary>
    //    /// Cuda C code.
    //    /// </summary>
    //    CudaC = 1,
    //    /// <summary>
    //    /// Generate code for all.
    //    /// </summary>
    //    All = 255
    //};

    /// <summary>
    /// GPU target type.
    /// </summary>
    public enum eGPUType
    {
        /// <summary>
        /// Target GPU kernel emulator.
        /// </summary>
        Emulator,
        /// <summary>
        /// Target a Cuda GPU.
        /// </summary>
        Cuda,
        /// <summary>
        /// Target an OpenCL Device
        /// </summary>
        OpenCL
    }

    /// <summary>
    /// Language type.
    /// </summary>
    public enum eLanguage
    {

        /// <summary>
        /// NVIDIA CUDA C
        /// </summary> 
        Cuda,

        /// <summary>
        /// OpenCL C
        /// </summary>
        OpenCL
    }

    /// <summary>
    /// High level enumerator encapsulation eGPUType and eGPUCodeGenerator.
    /// </summary>
    public enum eAmplifierQuickMode
    {
        /// <summary>
        /// Cuda emulator.
        /// </summary>
        CudaEmulate,
        /// <summary>
        /// Cuda.
        /// </summary>
        Cuda
    }

    /// <summary>
    /// Convenience class for storing device settings.
    /// </summary>
    public class AmplifierModes
    {
        /// <summary>
        /// Gets or sets the device id.
        /// </summary>
        /// <value>
        /// The device id.
        /// </value>
        public static int DeviceId { get; set; }
        
        /// <summary>
        /// Target GPU.
        /// </summary>
        public static eGPUType Target;

        /// <summary>
        /// Target compiler.
        /// </summary>
        public static eGPUCompiler Compiler;

        /// <summary>
        /// Target architecture.
        /// </summary>
        public static eArchitecture Architecture;

        ///// <summary>
        ///// Target code generator.
        ///// </summary>
        //public static eGPUCodeGenerator CodeGen;

        /// <summary>
        /// Language
        /// </summary>
        public static eLanguage Language;

        /// <summary>
        /// Quick mode.
        /// </summary>
        public static eAmplifierQuickMode Mode;

        /// <summary>
        /// Warning message if CRC check fails.
        /// </summary>
        public static string csCRCWARNING = "The Amplifier module was created from a different version of the .NET assembly.";

        /// <summary>
        /// Static constructor for the <see cref="AmplifierModes"/> class.
        /// Sets CodeGen to CudaC, Compiler to CudaNvcc, Target to Cuda and Mode to Cuda.
        /// </summary>
        static AmplifierModes()
        {
            //CodeGen = eGPUCodeGenerator.CudaC;
            Compiler = eGPUCompiler.CudaNvcc;
            Target = eGPUType.Cuda;
            Mode = eAmplifierQuickMode.Cuda;
            DeviceId = 0;
        }
    }

    /// <summary>
    /// Enumerator for the type of AmplifierAttribute.
    /// </summary>
    public enum eAmplifierType
    {
        /// <summary>
        /// Auto. The code generator will determine it.
        /// </summary>
        Auto,
        /// <summary>
        /// Used to indicate a method that should be made into a Cuda C device function.
        /// </summary>
        Device,
        /// <summary>
        /// Used to indicate a method that should be made into a Cuda C global function.
        /// </summary>
        Global,
        /// <summary>
        /// Used to indicate a structure that should be converted to Cuda C.
        /// </summary>
        Struct,
        /// <summary>
        /// Used to indicate a static field that should be converted to Cuda C.
        /// </summary>
        Constant
    };


    /// <summary>
    /// Target platform.
    /// </summary>
    public enum ePlatform
    {
        /// <summary>
        /// None selected.
        /// </summary>
        Auto = 0,
        
        /// <summary>
        /// x86
        /// </summary>
        x86 = 1,
        /// <summary>
        /// x64
        /// </summary>
        x64 = 2,



        /// <summary>
        /// Both x86 and x64
        /// </summary>
        All = 3
    }


    /// <summary>
    /// CUDA or OpenCL Architecture
    /// </summary>
    public enum eArchitecture : uint
    {
                /// <summary>
        /// Unspecified architecture.
        /// </summary>
        Unknown = 0,//0xFFFFFFFF,

        // Emulator has bit 4 set (8)

        /// <summary>
        /// CUDA Emulator
        /// </summary>
        Emulator = 8,
        
        // CUDA has bit 7 set (256)
        // OpenCL start has bit 15 set (32768), CUDA no flag
        
        /// <summary>
        /// CUDA sm_10
        /// </summary>
        sm_10 = 266,
        
        /// <summary>
        /// CUDA sm_11
        /// </summary>
        sm_11 = 267, 
        /// <summary>
        /// CUDA sm_12
        /// </summary>
        sm_12 = 268,
        /// <summary>
        /// CUDA sm_13
        /// </summary>
        sm_13 = 269,
        /// <summary>
        /// CUDA sm_20
        /// </summary>
        sm_20 = 276,
        /// <summary>
        /// CUDA sm_21
        /// </summary>
        sm_21 = 277,
        /// <summary>
        /// CUDA sm_30
        /// </summary>
        sm_30 = 286,
        /// <summary>
        /// CUDA sm_35
        /// </summary>
        sm_35 = 291,
        /// <summary>
        /// CUDA sm_37
        /// </summary>
        sm_37 = 293,
        /// <summary>
        /// CUDA sm_50
        /// </summary>
        sm_50 = 306,
        /// <summary>
        /// CUDA sm_52
        /// </summary>
        sm_52 = 308,
        /// <summary>
        /// OpenCL 1.0
        /// </summary>
        OpenCL = 32778,
        /// <summary>
        /// OpenCL 1.1
        /// </summary>
        OpenCL11 = 32779, 
        /// <summary>
        /// OpenCL 1.2
        /// </summary>
        OpenCL12 = 32780,

    }

    /// <summary>
    /// OpenCL address space 
    /// </summary>
    public enum eAmplifierAddressSpace
    {
        /// <summary>
        /// Prevent automatic placement of an address space qualifier.
        /// </summary>
        None = 0,

        /// <summary>
        /// Variable is in global memory.
        /// </summary>
        Global = 1,

        /// <summary>
        /// Variable is in constant memory.
        /// </summary>
        Constant = 2,

        /// <summary>
        /// Variable is in shared (local) memory.
        /// </summary>
        Shared = 4,

        /// <summary>
        /// Variable is in private/register memory.
        /// </summary>
        Private = 8 
    }


    /// <summary>
    /// Use to specify the behaviour of the AmplifierDummyAttribute.
    /// </summary>
    public enum eAmplifierDummyBehaviour
    {
        /// <summary>
        /// Default
        /// </summary>
        Default = 0,


        /// <summary>
        /// Do not write the include statements for dummy types in the generated CUDA C file.
        /// </summary>
        SuppressInclude = 1
    }

    /// <summary>
    /// Controls the type of compilation.
    /// </summary>
    public enum eAmplifierCompileMode
    {
        /// <summary>
        /// Default (PTX for CUDA). You will get a module for a minimum architecture.
        /// </summary>
        Default = 1,
        /// <summary>
        /// Binary (cubin for CUDA). You will get a module for a specific architecture.
        /// </summary>
        Binary = 2,
        /// <summary>
        /// Binary (cubin for CUDA) and includes relevant library files.
        /// </summary>
        DynamicParallelism = 6,
        /// <summary>
        /// Translate but do not compile.
        /// </summary>
        TranslateOnly = 8
    }
    
    /// <summary>
    /// Controls the inline type.
    /// </summary>
    public enum eAmplifierInlineMode 
    { 
        /// <summary>
        /// Default - let the compiler choose.
        /// </summary>
        Auto,
        /// <summary>
        /// __noinline__
        /// </summary>
        No, 
        /// <summary>
        /// __forceinline__
        /// </summary>
        Force
    };
}
