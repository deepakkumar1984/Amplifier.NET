/*
Amplifier.NET - LGPL 2.1 License
Please consider purchasing a commercial license - it helps development, frees you from LGPL restrictions
and provides you with support.  Thank you!
Copyright (C) 2013 Hybrid DSP Systems
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
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Win32;

namespace Amplifier
{
    public class CompileProperties
    {
        //-I"C:\Program Files (x86)\NVIDIA GPU Computing Toolkit\CUDA\v4.0\include" -m64  -arch=sm_20  -cubin  cudadevrt.lib  cublas_device.lib  -dlink  "C:\Sandbox\HybridDSPSystems\Codeplex\Amplifier\Amplifier.Host.UnitTests\bin\Debug\AmplifierSOURCETEMP.cu"  -o "C:\Sandbox\HybridDSPSystems\Codeplex\Amplifier\Amplifier.Host.UnitTests\bin\Debug\AmplifierSOURCETEMP.ptx"  -ptx
        // -I"C:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v5.0\include" -m64  -arch=sm_13  "C:\Sandbox\HybridDSPSystems\Codeplex\Amplifier\Amplifier.Host.UnitTests\bin\Debug\AmplifierSOURCETEMP.cu"  -o "C:\Sandbox\HybridDSPSystems\Codeplex\Amplifier\Amplifier.Host.UnitTests\bin\Debug\AmplifierSOURCETEMP.ptx"  -ptx

        public CompileProperties()
        {
            CompilerPath = "";
            IncludeDirectoryPath = "";
            WorkingDirectory = "";
            OutputFile = "AmplifierSOURCETEMP.ptx";
            AdditionalInputArgs = "";
            AdditionalOutputArgs = "";
            TimeOut = 20000;
            Platform = ePlatform.Auto;
            Architecture = eArchitecture.sm_20;
            CompileMode = eAmplifierCompileMode.Default;
            InputFile = "AmplifierSOURCETEMP.cu";

        }

        private const string csM64 = "-m64";

        private const string csM32 = "-m32";



        public string CompilerPath { get; set; }
        
        public string IncludeDirectoryPath { get; set; }

        public string WorkingDirectory { get; set; }

        public string InputFile { get; set; }

        private string _outputFile = "";

        public string OutputFile { 
            get{ return _outputFile; }
            set{ ChangeOutputFilename(value);}
        }
                  
        public ePlatform Platform { get; set; }

        public eArchitecture Architecture { get; set; }

        public eLanguage Language
        {
            get
            {
                return (((uint)Architecture & (uint)32768) == (uint)32768) ? eLanguage.OpenCL : eLanguage.Cuda;
                //return Architecture.HasFlag((eArchitecture)32768) ? eLanguage.OpenCL : eLanguage.Cuda;
            }
        }

        public string PlatformArg 
        { 
            get 
            { 
                if(Platform == ePlatform.x64)
                    return csM64;
                else if(Platform == ePlatform.x86)
                    return csM32;
                else
                    return IntPtr.Size == 4 ? csM32 : csM64;;
            }
        }
        
        public eAmplifierCompileMode CompileMode { get; set; }

        public string AdditionalInputArgs { get; set; }

        public string AdditionalOutputArgs { get; set; }

        public bool GenerateDebugInfo { get; set; }

        public bool DeleteGeneratedFiles { get; set; }

        /// <summary>
        /// Gets or sets the time out for compilation.
        /// </summary>
        /// <value>
        /// The time out in milliseconds.
        /// </value>
        public int TimeOut { get; set; }

        //-m64  -arch=sm_20  -cubin  cudadevrt.lib  cublas_device.lib  -dlink 
        public string GetCommandString()
        {
            bool binary = (CompileMode & eAmplifierCompileMode.Binary) == eAmplifierCompileMode.Binary;
            string format = string.Format(@"{0} -I""{1}"" {2} -arch={3} {4} {5} {6}", 
                "", IncludeDirectoryPath, PlatformArg, //0,1,2
                Architecture, GenerateDebugInfo ? "-G" : "", (CompileMode & eAmplifierCompileMode.Binary) == eAmplifierCompileMode.Binary ? "-cubin " : "-ptx",//3,4,5
                AdditionalInputArgs//6
                );

            format += string.Format(@" ""{0}"" ", WorkingDirectory + DSChar + InputFile);

            if (!binary)
                format += string.Format(@" -o ""{0}"" ", WorkingDirectory + DSChar + OutputFile);
            return format;
        }

        private string ChangeOutputFilename(string newname)
        {          
            //if (Path.HasExtension(OutputFile))
            //{
            //    string ext = Path.GetExtension(OutputFile);
            //    newname = Path.GetFileNameWithoutExtension(newname) +  ext;
            //}
            _outputFile = newname;
            return newname;
        }

        private char DSChar
        {
            get { return Path.DirectorySeparatorChar; }
        }

        /// <summary>Extra entries to prepend to the PATH environment variable when launching the compiler.</summary>
        public string[] PathEnvVarExtraEntries { get; set; }
    }

    public class CompilerHelper
    {
        public static eLanguage GetLanguage(eArchitecture arch)
        {
            //return (((uint)arch & (uint)eArchitecture.OpenCL) == (uint)32768) ? eLanguage.OpenCL : eLanguage.Cuda;
            if (arch == eArchitecture.Unknown)
                return AmplifierModes.Language;
            //return arch.HasFlag((eArchitecture)32768) ? eLanguage.OpenCL : eLanguage.Cuda;
            return (((uint)arch & (uint)32768) == (uint)32768) ? eLanguage.OpenCL : eLanguage.Cuda;
        }

        public static eGPUType GetGPUType(eArchitecture arch)
        {
            //return arch.HasFlag((eArchitecture)32768) ? eGPUType.OpenCL : eGPUType.Cuda;
            if (arch == eArchitecture.Emulator)
                return eGPUType.Emulator;
            return (((uint)arch & (uint)32768) == (uint)32768) ? eGPUType.OpenCL : eGPUType.Cuda;
        }

        public static CompileProperties Create(ePlatform platform = ePlatform.Auto, eArchitecture arch = eArchitecture.sm_20, eAmplifierCompileMode mode = eAmplifierCompileMode.Default, string workingDir = null, bool debugInfo = false)
        {
            CompileProperties tp = new CompileProperties();
            eLanguage language = GetLanguage(arch);
            if (language == eLanguage.Cuda)
            {
                string progFiles = Utility.ProgramFiles();

                tp.Architecture = (arch == eArchitecture.Unknown) ? eArchitecture.sm_20 : arch;
                bool binary = ((mode & eAmplifierCompileMode.Binary) == eAmplifierCompileMode.Binary);
                string tempFileName = "AmplifierSOURCETEMP.tmp";
                string cuFileName = tempFileName.Replace(".tmp", ".cu");
                string outputFileName = tempFileName.Replace(".tmp", binary ? ".cubin" : ".ptx");
                tp.InputFile = cuFileName;
                tp.OutputFile = outputFileName;
                if ((mode & eAmplifierCompileMode.DynamicParallelism) == eAmplifierCompileMode.DynamicParallelism)
                {
                    tp.AdditionalInputArgs = "cudadevrt.lib  cublas_device.lib  -dlink";                    
                }
                if (arch == eArchitecture.Emulator)
                    mode = eAmplifierCompileMode.TranslateOnly;
            }
            else
            {
                mode = eAmplifierCompileMode.TranslateOnly;
                tp.Architecture = (arch == eArchitecture.Unknown) ? eArchitecture.OpenCL : arch;
            }
            tp.WorkingDirectory = Directory.Exists(workingDir) ? workingDir : Environment.CurrentDirectory;

            tp.Platform = platform;
            tp.CompileMode = mode;         
            tp.GenerateDebugInfo = debugInfo;
            
            return tp;
        }
    }


    #region Proposed
    public abstract class ProgramModule
    {
        //-I"C:\Program Files (x86)\NVIDIA GPU Computing Toolkit\CUDA\v4.0\include" -m64  -arch=sm_20  -cubin  cudadevrt.lib  cublas_device.lib  -dlink  "C:\Sandbox\HybridDSPSystems\Codeplex\Amplifier\Amplifier.Host.UnitTests\bin\Debug\AmplifierSOURCETEMP.cu"  -o "C:\Sandbox\HybridDSPSystems\Codeplex\Amplifier\Amplifier.Host.UnitTests\bin\Debug\AmplifierSOURCETEMP.ptx"  -ptx
        // -I"C:\Program Files\NVIDIA GPU Computing Toolkit\CUDA\v5.0\include" -m64  -arch=sm_13  "C:\Sandbox\HybridDSPSystems\Codeplex\Amplifier\Amplifier.Host.UnitTests\bin\Debug\AmplifierSOURCETEMP.cu"  -o "C:\Sandbox\HybridDSPSystems\Codeplex\Amplifier\Amplifier.Host.UnitTests\bin\Debug\AmplifierSOURCETEMP.ptx"  -ptx

        protected readonly string csPROGRAMMODULE = "ProgramModule";

        public ProgramModule()
        {
            CompilerPath = "";
            IncludeDirectoryPath = "";
            WorkingDirectory = "";
            AdditionalInputArgs = "";

            TimeOut = 20000;
            Platform = ePlatform.Auto;
            Architecture = eArchitecture.sm_20;

            InputFile = "";
        }

        public string SourceID { get; set; }

        public virtual XElement PopulateXElement(XElement xe)
        {
            xe.Add(new XElement("InputFile", InputFile));
            xe.SetAttributeValue("Type", GetType());
            xe.SetAttributeValue("TimeOut", TimeOut);
            xe.SetAttributeValue("Platform", Platform);
            xe.SetAttributeValue("Architecture", Architecture);
            return xe;
        }

        public void SetFromXElement(XElement xe)
        {
            InputFile = xe.Element("InputFile").Value;
            TimeOut = xe.TryGetAttributeInt32Value("TimeOut").Value;
            Platform = xe.TryGetAttributeEnum<ePlatform>("Platform");
            Architecture = xe.TryGetAttributeEnum<eArchitecture>("Architecture");
        }


        public string CompilerPath { get; set; }

        public string IncludeDirectoryPath { get; set; }

        public string WorkingDirectory { get; set; }

        public string InputFile { get; set; }

        public ePlatform Platform { get; set; }

        public eArchitecture Architecture { get; set; }

        public eLanguage Language
        {
            get
            {
                return ((Architecture & eArchitecture.OpenCL) == (eArchitecture)32768) ? eLanguage.OpenCL : eLanguage.Cuda;
            }
        }

        public bool SupportDouble { get; set; }


        public string AdditionalInputArgs { get; set; }

        public bool GenerateDebugInfo { get; set; }

        /// <summary>
        /// Gets or sets the time out for compilation.
        /// </summary>
        /// <value>
        /// The time out in milliseconds.
        /// </value>
        public int TimeOut { get; set; }

        public abstract string GetCommandString();

        protected char DSChar
        {
            get { return Path.DirectorySeparatorChar; }
        }
    }

    public class OpenCLModule : ProgramModule
    {
        public override string GetCommandString()
        {
            return null;
        }

        //public override XElement PopulateXElement(XElement parent)
        //{
        //    XElement xe = parent.Element(csPROGRAMMODULE);
        //}
    }

    public abstract class CompilableModule : ProgramModule
    {
        public CompilableModule()
        {
            AdditionalOutputArgs = "";
            CompileMode = eAmplifierCompileMode.Default;
        }

        protected byte[] _binary = new byte[0];

        public string AdditionalOutputArgs { get; set; }


        public eAmplifierCompileMode CompileMode { get; set; }

        public void SetFromBase64(string base64)
        {
            _binary = Convert.FromBase64String(base64);
        }

        public void Set(string text)
        {
            _binary = Encoding.Default.GetBytes(text);
        }

        public void Set(byte[] binary)
        {
            _binary = binary;
        }

        public string GetBinaryAsString()
        {
            return Encoding.Default.GetString(_binary);
        }


        protected string ChangeOutputFilename(string newname)
        {
            if (Path.HasExtension(OutputFile))
            {
                string ext = Path.GetExtension(OutputFile);
                newname = newname + "." + ext;
            }
            _outputFile = newname;
            return newname;
        }

        private string _outputFile = "";

        public string OutputFile
        {
            get { return _outputFile; }
            set { ChangeOutputFilename(value); }
        }
    }

    public class CUDAModule : CompilableModule
    {

        private const string csM64 = "-m64";

        private const string csM32 = "-m32";


        public override string GetCommandString()
        {
            bool binary = (CompileMode & eAmplifierCompileMode.Binary) == eAmplifierCompileMode.Binary;
            string format = string.Format(@"{0} -I""{1}"" {2} -arch={3} {4} {5}",
                "", IncludeDirectoryPath, PlatformArg, //0,1,2
                Architecture, GenerateDebugInfo ? "-G" : "", (CompileMode & eAmplifierCompileMode.Binary) == eAmplifierCompileMode.Binary ? "-cubin " : "-ptx",//3,4
                AdditionalInputArgs//5
                );

            format += string.Format(@" ""{0}"" ", WorkingDirectory + DSChar + InputFile);

            if (!binary)
                format += string.Format(@" -o ""{0}"" ", WorkingDirectory + DSChar + OutputFile);
            return format;
        }

        private string PlatformArg
        {
            get
            {
                if (Platform == ePlatform.x64)
                    return csM64;
                else if (Platform == ePlatform.x86)
                    return csM32;
                else
                    return IntPtr.Size == 4 ? csM32 : csM64; ;
            }
        }



    }
    #endregion

}
