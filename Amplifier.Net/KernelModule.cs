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
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using Amplifier.Compilers;
//using GASS.CUDA.Types;
namespace Amplifier
{
    /// <summary>
    /// Flags for compilers.
    /// </summary>
    [Flags]
    public enum eGPUCompiler
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,
        /// <summary>
        /// Nvcc Cuda compiler.
        /// </summary>
        CudaNvcc = 1,
        /// <summary>
        /// Compile for all targets.
        /// </summary>
        All = 255
    };

    public abstract class ProgramModuleBase
    {
        /// <summary>
        /// Gets the platform.
        /// </summary>
        public ePlatform Platform { get; internal set; }
        /// <summary>
        /// Gets the architecture.
        /// </summary>
        public eArchitecture Architecture { get; internal set; }

        public string SourceCodeID { get; internal set; }
    }

    /// <summary>
    /// Internal use.
    /// </summary>
    public class PTXModule : ProgramModuleBase
    {
        
        
        ///// <summary>
        ///// Gets the platform.
        ///// </summary>
        //public ePlatform Platform { get; internal set; }
#if DEBUG
        public string PTX { get; set; }
#else
        /// <summary>
        /// Gets the PTX.
        /// </summary>
        public string PTX { get; internal set; }

#endif
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Platform.ToString();
        }
    }

    /// <summary>
    /// Internal use.
    /// </summary>
    public class BinaryModule : ProgramModuleBase
    {

#if DEBUG
        public byte[] Binary { get; set; }
#else
        /// <summary>
        /// Gets the binary (e.g. cubin)
        /// </summary>   
        public byte[] Binary { get; internal set; }
#endif
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "binary-"+Platform.ToString() + "-"+Architecture.ToString();
        }
    }

    public class SourceCodeFile
    {
        public SourceCodeFile()
        {
            ID = Guid.NewGuid().ToString();
            Source = "";
            Language = eLanguage.Cuda;
            Architecture = eArchitecture.Unknown;
        }

        public SourceCodeFile(string source, eLanguage language, eArchitecture arch) : this()
        {
            Source = source;
            Language = language;
            Architecture = arch;
        }
        
        public string ID { get; internal set; }

        public string Source { get; internal set; }

        public eLanguage Language { get; internal set; }

        public eArchitecture Architecture { get; internal set; }
    }


    /// <summary>
    /// Amplifier module.
    /// </summary>
    public class AmplifierModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplifierModule"/> class.
        /// </summary>
        public AmplifierModule()
        {
            _is64bit = IntPtr.Size == 8;
            //SourceCode = string.Empty;
            Name = "Amplifiermodule";
            CompilerOutput = string.Empty;
            CompilerArguments = string.Empty;
            TimeOut = 60000;
            _options = new List<CompilerOptions>();
            _PTXModules = new List<PTXModule>();
            _BinaryModules = new List<BinaryModule>();
            _sourceCodes = new List<SourceCodeFile>();
            Reset();
        }

        private bool _is64bit;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets optional extra data (CUmodule).
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public object Tag { get; set; }

        /// <summary>
        /// Gets the functions.
        /// </summary>
        public Dictionary<string, KernelMethodInfo> Functions { get; internal set; }

        /// <summary>
        /// Gets the constants.
        /// </summary>
        public Dictionary<string, KernelConstantInfo> Constants { get; internal set; }

        /// <summary>
        /// Gets the types.
        /// </summary>
        public Dictionary<string, KernelTypeInfo> Types { get; internal set; }

        /// <summary>
        /// Gets the member names.
        /// </summary>
        public IEnumerable<string> GetMemberNames()
        {
            foreach (var v in Functions)
                yield return v.Key;
            foreach (var v in Constants)
                yield return v.Key;
            foreach (var v in Types)
                yield return v.Key;
        }

        /// <summary>
        /// NOT IMPLEMENTED YET. Gets or sets a value indicating whether this instance can print to console.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can print; otherwise, <c>false</c>.
        /// </value>
        public bool CanPrint { get; set; }

        //public string Architecture { get; set; }

        private List<PTXModule> _PTXModules;

        private List<BinaryModule> _BinaryModules;

        private List<SourceCodeFile> _sourceCodes;

        /// <summary>
        /// Gets the PTX modules.
        /// </summary>
        public PTXModule[] PTXModules
        {
            get { return _PTXModules.ToArray(); }
        }

        public BinaryModule[] BinaryModules
        {
            get { return _BinaryModules.ToArray(); }
        }

        /// <summary>
        /// Removes the PTX modules.
        /// </summary>
        public void RemovePTXModules()
        {
            _PTXModules.Clear();
        }

        /// <summary>
        /// Removes the binary modules.
        /// </summary>
        public void RemoveBinaryModules()
        {
            _BinaryModules.Clear();
        }

        /// <summary>
        /// Gets the current platform.
        /// </summary>
        public ePlatform CurrentPlatform
        {
            get { return _is64bit ? ePlatform.x64 : ePlatform.x86; }
        }

        /// <summary>
        /// Gets the first PTX suitable for the current platform.
        /// </summary>
        public PTXModule PTX
        {
            get { return _PTXModules.Where(ptx => ptx.Platform == CurrentPlatform).FirstOrDefault(); }
        }

        /// <summary>
        /// Gets the first PTX suitable for the current platform.
        /// </summary>
        public BinaryModule Binary
        {
            get { return _BinaryModules.Where(b => b.Platform == CurrentPlatform).FirstOrDefault(); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has suitable PTX.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has PTX; otherwise, <c>false</c>.
        /// </value>
        public bool HasSuitablePTX
        {
            get { return _PTXModules.Count(ptx => ptx.Platform == CurrentPlatform) > 0; }
        }

        ///// <summary>
        ///// Gets a value indicating whether this instance has suitable binary.
        ///// </summary>
        ///// <value>
        /////   <c>true</c> if this instance has binary; otherwise, <c>false</c>.
        ///// </value>
        //public bool HasSuitableBinary
        //{
        //    get { return _BinaryModules.Count(b => b.Platform == CurrentPlatform) > 0; }
        //}

        ///// <summary>
        ///// Gets a value indicating whether this instance has suitable binary or PTX.
        ///// </summary>
        ///// <value>
        /////   <c>true</c> if this instance has binary; otherwise, <c>false</c>.
        ///// </value>
        //public bool HasSuitableProgramFile
        //{
        //    get { return HasSuitableBinary || HasSuitablePTX; }
        //}

        /// <summary>
        /// Gets a value indicating whether this instance has one or more PTX.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has PTX; otherwise, <c>false</c>.
        /// </value>
        public bool HasPTX
        {
            get { return _PTXModules.Count > 0; }
        }

        /// <summary>
        /// Determines whether module has binary for the specified platform and architecture.
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <param name="arch">The architecture.</param>
        /// <returns>
        ///   <c>true</c> if module has binary for the specified platform and an architecture equal or less than that specified; otherwise, <c>false</c>.
        /// </returns>
        public bool HasPTXForPlatform(ePlatform platform, eArchitecture arch)
        {
            ePlatform currPlatform = platform == ePlatform.Auto ? CurrentPlatform : platform;
            return _PTXModules.Count(b => b.Platform == currPlatform && b.Architecture <= arch) > 0;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has one or more binary modules.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has a binary module; otherwise, <c>false</c>.
        /// </value>
        public bool HasBinary
        {
            get { return _BinaryModules.Count > 0; }
        }

        /// <summary>
        /// Determines whether module has PTX for the specified platform.
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <returns>
        ///   <c>true</c> if module has PTX for the specified platform; otherwise, <c>false</c>.
        /// </returns>
        public bool HasPTXForPlatform(ePlatform platform)
        {
            ePlatform currPlatform = platform == ePlatform.Auto ? CurrentPlatform : platform;
            return _PTXModules.Count(ptx => ptx.Platform == currPlatform) > 0;
        }

        /// <summary>
        /// Determines whether module has PTX or binary for the specified platform.
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <param name="arch">The architecture.</param>
        /// <returns>
        ///   <c>true</c> if module has module for the specified values; otherwise, <c>false</c>.
        /// </returns>
        public bool HasProgramModuleForPlatform(ePlatform platform, eArchitecture arch)
        {
            return HasPTXForPlatform(platform, arch) || HasBinaryForPlatform(platform, arch);
        }

        /// <summary>
        /// Determines whether module has PTX or binary for the specified platform.
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <returns>
        ///   <c>true</c> if module has module for the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool HasProgramModuleForPlatform(ePlatform platform)
        {
            return HasPTXForPlatform(platform) || HasBinaryForPlatform(platform);
        }

        ///// <summary>
        ///// Determines whether module has binary for the specified platform.
        ///// </summary>
        ///// <param name="platform">The platform.</param>
        ///// <returns>
        /////   <c>true</c> if module has binary for the specified platform; otherwise, <c>false</c>.
        ///// </returns>
        //public bool HasBinaryForPlatform(ePlatform platform)
        //{
        //    return _BinaryModules.Count(b => b.Platform == platform) > 0;
        //}

        /// <summary>
        /// Determines whether module has binary for the specified platform and architecture.
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <param name="arch">The architecture.</param>
        /// <returns>
        ///   <c>true</c> if module has binary for the specified platform and architecture; otherwise, <c>false</c>.
        /// </returns>
        public bool HasBinaryForPlatform(ePlatform platform, eArchitecture arch)
        {
            ePlatform currPlatform = platform == ePlatform.Auto ? CurrentPlatform : platform;
            return _BinaryModules.Count(b => b.Platform == currPlatform && b.Architecture == arch) > 0;
        }

        /// <summary>
        /// Determines whether module has binary for the specified platform and architecture.
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <returns>
        ///   <c>true</c> if module has binary for the specified platform; otherwise, <c>false</c>.
        /// </returns>
        public bool HasBinaryForPlatform(ePlatform platform)
        {
            ePlatform currPlatform = platform == ePlatform.Auto ? CurrentPlatform : platform;
            return _BinaryModules.Count(b => b.Platform == currPlatform) > 0;
        }

        internal void StorePTXFile(string sourceCodeFileId, ePlatform platform, eArchitecture arch, string path)
        {
            using (StreamReader sr = File.OpenText(path))
            {
                string ptx = sr.ReadToEnd();
                _PTXModules.Add(new PTXModule() { Platform = platform, PTX = ptx, Architecture = arch, SourceCodeID = sourceCodeFileId });
            }
        }

        internal void StoreBinaryFile(string sourceCodeFileId, ePlatform platform, eArchitecture arch, string path)
        {
            if(!File.Exists(path))
                path = "a_dlink.cubin";            
            byte[] bytes = File.ReadAllBytes(path);

            _BinaryModules.Add(new BinaryModule() { Platform = platform, Binary = bytes, Architecture = arch, SourceCodeID = sourceCodeFileId });
        }

        /// <summary>
        /// Gets or sets the CUDA or OpenCL source code.
        /// </summary>
        /// <value>
        /// The cuda source code.
        /// </value>
        [Obsolete("Use SourceCode instead")]
        public string CudaSourceCode
        {
            get { return SourceCode; }
            set { SourceCode = value; }
        }

        /// <summary>
        /// Gets or sets the CUDA or OpenCL source code.
        /// </summary>
        /// <value>
        /// The source code.
        /// </value>
        public string SourceCode 
        {
            get { var sc = _sourceCodes.FirstOrDefault(); if (sc == null) return null; return sc.Source; }
            set 
            { 
                _sourceCodes.Clear(); 
                _sourceCodes.Add(new SourceCodeFile() 
                { 
                    ID = Guid.NewGuid().ToString(), Language = eLanguage.Cuda, Source = value, Architecture = eArchitecture.Unknown 
                }); 
            }
        }

        public void AddSourceCodeFile(SourceCodeFile scf)
        {
            if (_sourceCodes.Any(s => s.ID == scf.ID))
                throw new ArgumentException("Module already contains a source code with this ID.");
            _sourceCodes.Add(scf);
        }


        /// <summary>
        /// Gets a value indicating whether this instance has cuda source code.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has cuda source code; otherwise, <c>false</c>.
        /// </value>
        [Obsolete("Use HasSourceCode instead")]
        public bool HasCudaSourceCode
        {
            get { return HasSourceCode; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has source code.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has source code; otherwise, <c>false</c>.
        /// </value>
        public bool HasSourceCode
        {
            get { return !string.IsNullOrEmpty(SourceCode)  ||  _sourceCodes.Count == 0; }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            Functions = new Dictionary<string, KernelMethodInfo>();
            Constants = new Dictionary<string, KernelConstantInfo>();
            Types = new Dictionary<string, KernelTypeInfo>();
            _PTXModules.Clear();
            CanPrint = false;
            CompilerOptionsList.Clear();
        }

        private const string csAmplifierMODULE = "AmplifierModule";
        private const string csVERSION = "Version";
        private const string csCUDASOURCECODE = "CudaSourceCode";
        private const string csSOURCECODES = "SourceCodes";
        private const string csSOURCECODEFILE = "SourceCodeFile";
        private const string csID = "ID";
        private const string csSOURCECODE = "SourceCode";
        private const string csLANGUAGE = "Language";
        private const string csHASCUDASOURCECODE = "HasCudaSourceCode";
        private const string csPTX = "PTX";
        private const string csBINARY = "Binary";
        private const string csPTXMODULES = "PTXMODULES";
        private const string csPTXMODULE = "PTXMODULE";
        private const string csBINARYMODULES = "BinaryModules";
        private const string csBINARYMODULE = "BinaryModule";
        private const string csHASPTX = "HasPTX";
        private const string csHASBINARY = "HasBinary";
        private const string csFUNCTIONS = "Functions";
        private const string csCONSTANTS = "Constants";
        private const string csCONSTANT = "Constant";
        private const string csTYPES = "Types";
        private const string csFILEEXT = ".cdfy";
        private const string csPLATFORM = "Platform";
        private const string csARCH = "Arch";
        private const string csNAME = "Name";
        private const string csDEBUGINFO = "DebugInfo";

        /// <summary>
        /// Trues to serialize this instance to file based on Name.
        /// </summary>
        /// <returns>True if successful, else false.</returns>
        public bool TrySerialize()
        {
            try
            {
                Serialize();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
#if DEBUG
                throw;
#endif
            }
            return false;
        }

        /// <summary>
        /// Serializes this instance to file based on Name.
        /// </summary>
        public void Serialize()
        {
            Serialize(Name);
        }

        /// <summary>
        /// Serializes the module to the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public void Serialize(string filename)
        {
            if (!Path.HasExtension(filename))
                filename += csFILEEXT;
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                Serialize(fs);
            }
        }

        /// <summary>
        /// Serializes the module to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public void Serialize(Stream stream)
        {
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", null));

            byte[] cudaSrcBa = UnicodeEncoding.ASCII.GetBytes(SourceCode);
            string cudaSrcB64 = Convert.ToBase64String(cudaSrcBa);

            XElement root = new XElement(csAmplifierMODULE);
            root.SetAttributeValue(csVERSION, this.GetType().Assembly.GetName().Version.ToString());
            root.SetAttributeValue(csNAME, Name == null ? "Amplifiermodule" : Name);
            root.SetAttributeValue(csDEBUGINFO, GenerateDebug);
            
            XElement cudaSrc = new XElement(csCUDASOURCECODE, string.Empty);//cudaSrcB64);
            root.SetAttributeValue(csHASCUDASOURCECODE, XmlConvert.ToString(false));//XmlConvert.ToString(HasSourceCode));
            root.Add(cudaSrc);
            
            root.SetAttributeValue(csHASPTX, XmlConvert.ToString(_PTXModules.Count > 0));
            XElement xe = new XElement(csPTXMODULES);
            foreach (var ptxMod in _PTXModules)
            {
                byte[] ba = UnicodeEncoding.ASCII.GetBytes(ptxMod.PTX);
                string b64 = Convert.ToBase64String(ba);
                XElement ptxXe = new XElement(csPTXMODULE, b64);
                ptxXe.SetAttributeValue(csPLATFORM, ptxMod.Platform);
                ptxXe.SetAttributeValue(csARCH, ptxMod.Architecture);
                ptxXe.SetAttributeValue(csSOURCECODEFILE, ptxMod.SourceCodeID);
                xe.Add(ptxXe);
            }
            root.Add(xe);

            root.SetAttributeValue(csHASBINARY, XmlConvert.ToString(_BinaryModules.Count > 0));
            xe = new XElement(csBINARYMODULES);
            foreach (var binMod in _BinaryModules)
            {
                byte[] ba = binMod.Binary;
                string b64 = Convert.ToBase64String(ba);
                XElement binXe = new XElement(csBINARYMODULES, b64);
                binXe.SetAttributeValue(csPLATFORM, binMod.Platform);
                binXe.SetAttributeValue(csARCH, binMod.Architecture);
                binXe.SetAttributeValue(csSOURCECODEFILE, binMod.SourceCodeID);
                xe.Add(binXe);
            }
            root.Add(xe);

            xe = new XElement(csSOURCECODES);
            foreach (var scf in _sourceCodes)
            {
                byte[] ba = UnicodeEncoding.ASCII.GetBytes(scf.Source);
                string b64 = Convert.ToBase64String(ba);
                XElement scfxe = new XElement(csSOURCECODEFILE, b64);
                scfxe.SetAttributeValue(csID, scf.ID);
                scfxe.SetAttributeValue(csLANGUAGE, scf.Language);
                scfxe.SetAttributeValue(csARCH, scf.Architecture);
                xe.Add(scfxe);
            }
            root.Add(xe);
          
            XElement funcs = new XElement(csFUNCTIONS);
            root.Add(funcs);
            foreach (var kvp in Functions)
            {
                xe = kvp.Value.GetXElement();
                funcs.Add(xe);
            }

            XElement constants = new XElement(csCONSTANTS);
            root.Add(constants);
            foreach (var kvp in Constants)
            {
                xe = kvp.Value.GetXElement();
                constants.Add(xe);
            }

            XElement types = new XElement(csTYPES);
            root.Add(types);
            foreach (var kvp in Types)
            {
                xe = kvp.Value.GetXElement();
                types.Add(xe);
            }

            doc.Add(root);
            //if (!Path.HasExtension(filename))
            //    filename += csFILEEXT;
            //doc.Save(filename);
            doc.SaveStream(stream);
        }

        /// <summary>
        /// Deletes the specified filename (with or without default .cdfy extension).
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>True if file was deleted else false.</returns>
        public static bool Clean(string filename)
        {
            filename = GetFilename(filename);
            bool exists = File.Exists(filename);
            if (exists)
                File.Delete(filename);
            return exists;
        }

        private static string GetFilename(string filename)
        {
            if (!File.Exists(filename))
                filename += csFILEEXT;
            return filename;
        }

        /// <summary>
        /// Gets the dummy struct includes.
        /// </summary>
        /// <returns>Strings representing the Cuda include files.</returns>
        public IEnumerable<string> GetDummyStructIncludes()
        {
            foreach (var kvp in Types.Where(k => k.Value.IsDummy))
                yield return kvp.Value.GetDummyInclude();
        }

        /// <summary>
        /// Gets the dummy function includes.
        /// </summary>
        /// <returns>Strings representing the Cuda include files.</returns>
        public IEnumerable<string> GetDummyIncludes()
        {
            foreach (var kvp in Functions.Where(k => k.Value.IsDummy))
                yield return kvp.Value.GetDummyInclude();
        }

        /// <summary>
        /// Gets the dummy defines.
        /// </summary>
        /// <returns>Strings representing the Cuda defines files.</returns>
        public IEnumerable<string> GetDummyDefines()
        {
            foreach (var kvp in Constants.Where(k => k.Value.IsDummy))
                yield return kvp.Value.GetDummyDefine();
        }

        /// <summary>
        /// Tries to deserialize from a file with the same name as the calling type.
        /// </summary>
        /// <returns>Amplifier module or null if failed.</returns>
        public static AmplifierModule TryDeserialize()
        {
            StackTrace stackTrace = new StackTrace();
            Type type = stackTrace.GetFrame(1).GetMethod().ReflectedType;
            return TryDeserialize(type.Name);
        }

        /// <summary>
        /// Tries to deserialize from the specified file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>Amplifier module or null if failed.</returns>
        public static AmplifierModule TryDeserialize(string filename)
        {
            string ts;
            return TryDeserialize(filename, out ts);
        }

        /// <summary>
        /// Tries to deserialize from the specified file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="errorMsg">The error message if fails, else empty string.</param>
        /// <returns>Amplifier module or null if failed.</returns>
        public static AmplifierModule TryDeserialize(string filename, out string errorMsg)
        {
            errorMsg = string.Empty;
            filename = GetFilename(filename);
            if (!File.Exists(filename))
                return null;
            AmplifierModule km = null;
            try
            {
                km = Deserialize(filename);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return km;
        }

        /// <summary>
        /// Determines whether there is a Amplifier module in the calling assembly.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if calling assembly has Amplifier module; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAmplifierModuleInAssembly()
        {
            StackTrace stackTrace = new StackTrace();
            Type type = stackTrace.GetFrame(1).GetMethod().ReflectedType;
            return HasAmplifierModule(type.Assembly);
        }

        /// <summary>
        /// Determines whether there is a Amplifier module in the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>
        ///   <c>true</c> if assembly has a Amplifier module; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAmplifierModule(Assembly assembly)
        {
            var assemblyName = assembly.GetName().Name;
            var resourceName = assemblyName + ".cdfy";
            return assembly.GetManifestResourceNames().Contains(resourceName);
        }

        /// <summary>
        /// Gets a Amplifier module that was stored as a resource in the calling assembly.
        /// </summary>
        /// <returns>The stored Amplifier module.</returns>
        /// <exception cref="AmplifierException">Resource not found.</exception>
        public static AmplifierModule GetFromAssembly()
        {
            var cm = TryGetFromAssembly();
            if (cm == null)
                throw new AmplifierException(AmplifierException.csRESOURCE_NOT_FOUND);
            return cm;
        }

        /// <summary>
        /// Gets a Amplifier module that was stored as a resource in the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The stored Amplifier module.</returns>
        /// <exception cref="AmplifierException">Resource not found.</exception>
        public static AmplifierModule GetFromAssembly(Assembly assembly)
        {
            var cm = TryGetFromAssembly(assembly);
            if (cm == null)
                throw new AmplifierException(AmplifierException.csRESOURCE_NOT_FOUND);
            return cm;
        }

        /// <summary>
        /// Tries to get a Amplifier module that was stored as a resource in the calling assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The stored Amplifier module, or null if not present.</returns>
        public static AmplifierModule TryGetFromAssembly()
        {
            StackTrace stackTrace = new StackTrace();
            Type type = stackTrace.GetFrame(1).GetMethod().ReflectedType;
            return TryGetFromAssembly(type.Assembly);
        }

        /// <summary>
        /// Tries to get a Amplifier module that was stored as a resource in the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The stored Amplifier module, or null if not present.</returns>
        public static AmplifierModule TryGetFromAssembly(Assembly assembly)
        {
            var assemblyName = assembly.GetName().Name;
            var resourceName = assemblyName + ".cdfy";
            if (assembly.GetManifestResourceNames().Contains(resourceName))
            {
                var stream = assembly.GetManifestResourceStream(resourceName);
                return AmplifierModule.Deserialize(stream);
            }
            return null;
        }

        /// <summary>
        /// Deserializes from a file with the same name as the calling type.
        /// </summary>
        /// <returns>Amplifier module.</returns>
        public static AmplifierModule Deserialize()
        {
            StackTrace stackTrace = new StackTrace();
            Type type = stackTrace.GetFrame(1).GetMethod().ReflectedType;
            return Deserialize(type.Name);
        }
#warning TODO http://www.codeproject.com/KB/dotnet/AppDomain_quick_start.aspx
        /// <summary>
        /// Deserializes the specified file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>Amplifier module.</returns>
        public static AmplifierModule Deserialize(string filename)
        {
            AmplifierModule km = new AmplifierModule();
            if (!File.Exists(filename))
                filename += csFILEEXT;

            XDocument doc = XDocument.Load(filename);

            string path = Path.GetDirectoryName(filename);

            return Deserialize(km, doc, path);
        }

        public static AmplifierModule Deserialize(Stream stream)
        {
            AmplifierModule km = new AmplifierModule();
#if NET35
            XmlReader reader = XmlReader.Create(stream);
            XDocument doc = XDocument.Load(reader);
#else
            XDocument doc = XDocument.Load(stream);
#endif
            return Deserialize(km, doc, null);
        }

        private static AmplifierModule Deserialize(AmplifierModule km, XDocument doc, string path)
        {
            XElement root = doc.Element(csAmplifierMODULE);
            if (root == null)
                throw new XmlException(string.Format(GES.csELEMENT_X_NOT_FOUND, csAmplifierMODULE));

            string vStr = root.GetAttributeValue(csVERSION);
            Version version = new Version(vStr);
            Version curVers = typeof(AmplifierModule).Assembly.GetName().Version;
            if (version < new Version(1, 25))
                throw new AmplifierException(AmplifierException.csVERSION_MISMATCH_EXPECTED_X_GOT_X, "1.25 or later", version);
            if (version.Major != curVers.Major || version.Minor > curVers.Minor)
                throw new AmplifierException(AmplifierException.csVERSION_MISMATCH_EXPECTED_X_GOT_X, curVers, version);

            string name = root.TryGetAttributeValue(csNAME);
            km.Name = name != null ? name : km.Name;

            // Cuda Source
            bool? hasCudaSrc = root.TryGetAttributeBoolValue(csHASCUDASOURCECODE);
            XElement scse = root.Element(csSOURCECODES);
            if (hasCudaSrc == true && scse == null) // Legacy support
            {
                km.SourceCode = root.TryGetElementBase64(csCUDASOURCECODE); 
            }
            // else if sourcecodes node
            else if (scse != null)
            {
                foreach (var sce in scse.Elements(csSOURCECODEFILE))
                {
                    SourceCodeFile scf = new SourceCodeFile();
                    scf.ID = sce.TryGetAttributeValue(csID);
                    scf.Language = sce.TryGetAttributeEnum<eLanguage>(csLANGUAGE);
                    scf.Source = UnicodeEncoding.ASCII.GetString(Convert.FromBase64String(sce.Value));
                    scf.Architecture = sce.TryGetAttributeEnum<eArchitecture>(csARCH);
                    km._sourceCodes.Add(scf);              
                }
            }
            else
            {
                km.SourceCode = string.Empty;
            }

            bool? hasDebug = root.TryGetAttributeBoolValue(csDEBUGINFO);
            km.GenerateDebug = hasDebug.HasValue && hasDebug.Value;

            // PTX
            bool? hasPtx = root.TryGetAttributeBoolValue(csHASPTX);
            if (hasPtx == true && root.Element(csPTX) != null) // legacy support V0.3 or less
            {
                string ptx = root.Element(csPTX).Value;
                byte[] ba = Convert.FromBase64String(ptx);
                km._PTXModules.Add(new PTXModule() { PTX = UnicodeEncoding.ASCII.GetString(ba), Platform = km.CurrentPlatform, Architecture = eArchitecture.Unknown });
            }
            else if (hasPtx == true)
            {
                XElement ptxsxe = root.Element(csPTXMODULES);
                foreach (XElement xe in ptxsxe.Elements(csPTXMODULE))
                {
                    string ptx = xe.Value;  //xe.Element(csPTX).Value;
                    string platformStr = xe.GetAttributeValue(csPLATFORM);
                    ePlatform platform = (ePlatform)Enum.Parse(typeof(ePlatform), platformStr);
                    string archStr = xe.TryGetAttributeValue(csARCH);
                    eArchitecture arch = eArchitecture.Unknown;
                    string sourcecodeId = xe.TryGetAttributeValue(csSOURCECODEFILE);
                    if (archStr != null)
                        arch = (eArchitecture)Enum.Parse(typeof(eArchitecture), archStr);
                    byte[] ba = Convert.FromBase64String(ptx);
                    km._PTXModules.Add(new PTXModule() { PTX = UnicodeEncoding.ASCII.GetString(ba), Platform = platform, Architecture = arch, SourceCodeID = sourcecodeId });
                }
            }

            // Binary
            bool? hasBinary = root.TryGetAttributeBoolValue(csHASBINARY);
            if (hasBinary == true)
            {
                XElement binsxe = root.Element(csBINARYMODULES);
                foreach (XElement xe in binsxe.Elements(csBINARYMODULE))
                {
                    string bin = xe.Value; //xe.Element(csBINARY).Value;
                    string platformStr = xe.GetAttributeValue(csPLATFORM);
                    string archStr = xe.GetAttributeValue(csARCH);
                    ePlatform platform = (ePlatform)Enum.Parse(typeof(ePlatform), platformStr);
                    eArchitecture arch = (eArchitecture)Enum.Parse(typeof(eArchitecture), archStr);
                    string sourcecodeId = xe.TryGetAttributeValue(csSOURCECODEFILE);
                    byte[] ba = Convert.FromBase64String(bin);
                    km._BinaryModules.Add(new BinaryModule() { Binary = ba, Platform = platform, Architecture = arch, SourceCodeID = sourcecodeId });
                }
            }

            // Functions
            XElement funcs = root.Element(csFUNCTIONS);
            if (funcs != null)
            {
                foreach (var xe in funcs.Elements(KernelMethodInfo.csAmplifierKERNELMETHOD))
                {
                    KernelMethodInfo kmi = KernelMethodInfo.Deserialize(xe, km, path);
                    km.Functions.Add(kmi.Method.Name, kmi);
                }
            }

            // Constants
            XElement constants = root.Element(csCONSTANTS);
            if (constants != null)
            {
                foreach (var xe in constants.Elements(KernelConstantInfo.csAmplifierCONSTANTINFO))
                {
                    KernelConstantInfo kci = KernelConstantInfo.Deserialize(xe, path);
                    km.Constants.Add(kci.Name, kci);
                }
            }

            // Types
            XElement types = root.Element(csTYPES);
            if (constants != null)
            {
                foreach (var xe in types.Elements(KernelTypeInfo.csAmplifierTYPE))
                {
                    KernelTypeInfo kti = KernelTypeInfo.Deserialize(xe, path);
                    km.Types.Add(kti.Name, kti);
                }
            }

            return km;
        }

        /// <summary>
        /// Verifies the checksums of all functions, constants and types.
        /// </summary>
        /// <exception cref="AmplifierException">Check sums don't match or total number of members is less than one, .</exception>
        public void VerifyChecksums()
        {
            if (GetTotalMembers() == 0)
                throw new AmplifierException(AmplifierException.csNO_MEMBERS_FOUND);
            if (!HasSuitablePTX)
                throw new AmplifierException(AmplifierException.csNO_SUITABLE_X_PRESENT_IN_Amplifier_MODULE, "PTX");
            foreach (var kvp in Functions)
                kvp.Value.VerifyChecksums();
            foreach (var kvp in Constants)
                kvp.Value.VerifyChecksums();
            foreach (var kvp in Types)
                kvp.Value.VerifyChecksums();       
        }

        /// <summary>
        /// Verifies the checksums of all functions, constants and types.
        /// </summary>
        /// <returns>True if checksums match and total number of members is greater than one, else false.</returns>
        public bool TryVerifyChecksums()
        {
            return TryVerifyChecksums(CurrentPlatform, eArchitecture.Unknown);
        }

        /// <summary>
        /// Verifies the checksums of all functions, constants and types.
        /// </summary>
        /// <param name="platform">Platform.</param>
        /// <param name="arch">Architecture.</param>
        /// <returns>True if checksums match and total number of members is greater than one, else false.</returns>
        public bool TryVerifyChecksums(ePlatform platform, eArchitecture arch)
        {
            if (GetTotalMembers() == 0) 
                return false;
            if (arch != eArchitecture.Unknown && !HasProgramModuleForPlatform(platform, arch))
                return false;
            foreach (var kvp in Functions)
                if (kvp.Value.TryVerifyChecksums() == false)
                    return false;
            foreach (var kvp in Constants)
                if (kvp.Value.TryVerifyChecksums() == false)
                    return false;
            foreach (var kvp in Types)
                if (kvp.Value.TryVerifyChecksums() == false)
                    return false;
            return true;
        }

        /// <summary>
        /// Clones the module. Useful for loading the same module to multiple GPUs.
        /// </summary>
        /// <returns>Cloned module.</returns>
        public AmplifierModule Clone()
        {
            MemoryStream ms = new MemoryStream();
            this.Serialize(ms);
            ms.Position = 0;
            var cm = AmplifierModule.Deserialize(ms);
            return cm;
        }

        private int GetTotalMembers()
        {
            return Functions.Count + Constants.Count + Types.Count;
        }

        /// <summary>
        /// Gets the compiler options.
        /// </summary>
        /// <value>
        /// The compiler options.
        /// </value>
        public List<CompilerOptions> CompilerOptionsList 
        { 
            get 
            { 
                return _options; 
            } 
        }

        //public void AddCompilerOptions(CompilerOptions co)
        //{
        //    var existingOpt = _options.Where(opt => opt.
        //}


        private List<CompilerOptions> _options;

        /// <summary>
        /// Gets or sets the compiler output.
        /// </summary>
        public string CompilerOutput { get; set; }

        /// <summary>
        /// Gets the last arguments passed to compiler.
        /// </summary>
        public string CompilerArguments { get; private set; }

        private string _workingDirectory;

        /// <summary>
        /// Gets or sets the working directory for the compiler.
        /// </summary>
        public string WorkingDirectory
        {
            get { return (string.IsNullOrEmpty(_workingDirectory) || !Directory.Exists(_workingDirectory)) ? Environment.CurrentDirectory : _workingDirectory; }
            set { _workingDirectory = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to compile for debug.
        /// </summary>
        /// <value>
        ///   <c>true</c> if compile for debug; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateDebug { get; set; }

        /// <summary>
        /// Gets or sets the time out for compilation.
        /// </summary>
        /// <value>
        /// The time out in milliseconds.
        /// </value>
        public int TimeOut { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to start the compilation in a new window.
        /// </summary>
        /// <value>
        ///   <c>true</c> if suppress a new window; otherwise, <c>false</c>.
        /// </value>
        public bool SuppressWindow { get; set; }

        StringBuilder standardOutput = new StringBuilder();
        StringBuilder standardError = new StringBuilder();
        void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
                standardError.Append(e.Data + Environment.NewLine);
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
                standardOutput.Append(e.Data + Environment.NewLine);
        }

        /// <summary>
        /// Compiles the module based on current Cuda source code and options.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="deleteGeneratedCode">if set to <c>true</c> delete generated code on success.</param>
        /// <param name="binary">Compile to binary if true.</param>
        /// <returns>The compile arguments.</returns>
        /// <exception cref="AmplifierCompileException">No source code or compilation error.</exception>
        public string Compile(eGPUCompiler mode, bool deleteGeneratedCode = false, eAmplifierCompileMode compileMode = eAmplifierCompileMode.Default)
        {
            string ts = string.Empty;
            if ((mode & eGPUCompiler.CudaNvcc) == eGPUCompiler.CudaNvcc)
            {
                CompilerOutput = string.Empty;
                _PTXModules.Clear();
                _BinaryModules.Clear();
                if (!HasSourceCode)
                    throw new AmplifierCompileException(AmplifierCompileException.csNO_X_SOURCE_CODE_PRESENT_IN_Amplifier_MODULE, "CUDA");

                bool binary = (compileMode & eAmplifierCompileMode.Binary) == eAmplifierCompileMode.Binary;
                // Write to temp file
                string tempFileName = "AmplifierSOURCETEMP.tmp";
                string cuFileName = WorkingDirectory + Path.DirectorySeparatorChar + tempFileName.Replace(".tmp", ".cu");
                string ptxFileName = WorkingDirectory + Path.DirectorySeparatorChar + tempFileName.Replace(".tmp", binary ? ".cubin" : ".ptx");
                File.WriteAllText(cuFileName, SourceCode, Encoding.Default);

                foreach (CompilerOptions co in CompilerOptionsList)
                {
                    co.GenerateDebugInfo = GenerateDebug;
                    co.TimeOut = TimeOut;
                    co.ClearSources();
                    co.AddSource(cuFileName);

                    co.ClearOutputs();
                    co.AddOutput(ptxFileName);

                    CompilerOutput += "\r\n" + co.GetSummary();
                    CompilerOutput += "\r\n" + co.GetArguments();

                    // Convert to ptx
                    Process process = new Process();
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.CreateNoWindow = SuppressWindow;//WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    process.StartInfo.FileName = co.GetFileName();
                    process.StartInfo.Arguments = co.GetArguments();
                    CompilerArguments = process.StartInfo.Arguments;
                    process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
                    process.ErrorDataReceived += new DataReceivedEventHandler(process_ErrorDataReceived);
                    Debug.WriteLine(process.StartInfo.FileName);
                    Debug.WriteLine(CompilerArguments);
                    standardError.Clear(); standardOutput.Clear();

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.Start();
                    

                    //while (!process.HasExited)
                    //    Thread.Sleep(10);
                    int waitCounter = 0;
                    bool procTimedOut = false;
                    int timeout = co.TimeOut;
                    while (!process.HasExited && !(procTimedOut = ++waitCounter >= timeout)) // 1m timeout
                        //Thread.Sleep(10);
                        process.WaitForExit(10);
                    if (procTimedOut)
                        throw new AmplifierCompileException(AmplifierCompileException.csCOMPILATION_ERROR_X, "Process timed out");

                    if (process.ExitCode != 0)
                    {
                        string s = standardError.ToString(); //process.StandardError.ReadToEnd();
                        
                        CompilerOutput += "\r\n" + s;
                        if (s.Contains("Cannot find compiler 'cl.exe' in PATH"))
                            CompilerOutput += "\r\nPlease add the Visual Studio VC bin directory to PATH in Environment Variables.";
                        Debug.WriteLine(s);
                        throw new AmplifierCompileException(AmplifierCompileException.csCOMPILATION_ERROR_X, s);
                    }
                    else
                    {
                        string s = standardError.ToString() + "\r\n" + standardOutput.ToString();
                        CompilerOutput += "\r\n" + s;
                        Debug.WriteLine(s);
                    }

                    // Load ptx file
                    if ((compileMode & eAmplifierCompileMode.Binary) == eAmplifierCompileMode.Binary)
                        this.StoreBinaryFile("na", co.Platform, co.Architecture, ptxFileName);                        
                    else
                        this.StorePTXFile("na", co.Platform, co.Architecture, ptxFileName);
#if DEBUG

#else
                    if (deleteGeneratedCode)
                        Delete(cuFileName, ptxFileName);
#endif
                }
            }
            return CompilerArguments;
        }

        private eLanguage GetLanguageFromArchitecture(eArchitecture arch)
        {
            return arch == eArchitecture.OpenCL ? eLanguage.OpenCL : eLanguage.Cuda;
        }

        public SourceCodeFile GetSourceCodeFile(eArchitecture arch = eArchitecture.Unknown)
        {
            eLanguage language = GetLanguageFromArchitecture(arch);
            var file = _sourceCodes.Where(scf => scf.Architecture <= arch && scf.Language == language).OrderByDescending(scf => scf.Architecture).FirstOrDefault();
            return file;
        }

        public void Compile(CompileProperties p)
        {
            Compile(new[] { p });
        }

        public void Compile(CompileProperties[] props)
        {
            string ts = string.Empty;
            CompilerOutput = string.Empty;
            _PTXModules.Clear();
            _BinaryModules.Clear();

            foreach (CompileProperties p in props)
            {
                if ((p.CompileMode & eAmplifierCompileMode.TranslateOnly) != eAmplifierCompileMode.TranslateOnly)
                {
                    bool binary = (p.CompileMode & eAmplifierCompileMode.Binary) == eAmplifierCompileMode.Binary;
                    
                    // Write to temp file
                    string cuFileName = p.WorkingDirectory + Path.DirectorySeparatorChar + p.InputFile;
                    SourceCodeFile srcCodeFile = GetSourceCodeFile(p.Architecture);
                    if (srcCodeFile == null)
                        throw new AmplifierCompileException(AmplifierCompileException.csNO_X_SOURCE_CODE_PRESENT_IN_Amplifier_MODULE_FOR_X,
                            GetLanguageFromArchitecture(p.Architecture), p.Architecture);
                    File.WriteAllText(cuFileName, srcCodeFile.Source, Encoding.Default);
                    var originalPlatformSetting = p.Platform;
                    var platforms = new List<ePlatform>();
                    if (p.Platform == ePlatform.All)
                        platforms.AddRange(new[] { ePlatform.x64, ePlatform.x86 });
                    else if (p.Platform == ePlatform.Auto)
                        platforms.Add(CurrentPlatform);
                    else
                        platforms.Add(p.Platform);
                    foreach (ePlatform platform in platforms)
                    {
                        p.Platform = platform;

                        // Call nvcc
                        Process process = new Process();
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;
                        process.StartInfo.CreateNoWindow = SuppressWindow;//WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        process.StartInfo.FileName = string.Format(@"""{0}""", p.CompilerPath);
                        process.StartInfo.Arguments = p.GetCommandString();
                        p.Platform = originalPlatformSetting;
                        CompilerArguments = process.StartInfo.Arguments;
                        process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
                        process.ErrorDataReceived += new DataReceivedEventHandler(process_ErrorDataReceived);
                        Debug.WriteLine(process.StartInfo.FileName);
                        Debug.WriteLine(CompilerArguments);
                        standardError.Clear();
                        standardOutput.Clear();
                        process.Start();
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        int waitCounter = 0;
                        bool procTimedOut = false;
                        int timeout = p.TimeOut;
                        while (!process.HasExited && !(procTimedOut = ++waitCounter >= timeout)) // 1m timeout
                            Thread.Sleep(10);
                        if (procTimedOut)
                            throw new AmplifierCompileException(AmplifierCompileException.csCOMPILATION_ERROR_X, "Process timed out");

                        if (process.ExitCode != 0)
                        {
                            string s = standardOutput.ToString() + "\r\n" + standardError.ToString();

                            CompilerOutput += "\r\n" + s;
                            if (s.Contains("Cannot find compiler 'cl.exe' in PATH"))
                                CompilerOutput += "\r\nPlease add the Visual Studio VC bin directory to PATH in Environment Variables.";
                            Debug.WriteLine(CompilerOutput);
                            throw new AmplifierCompileException(AmplifierCompileException.csCOMPILATION_ERROR_X, CompilerOutput);
                        }
                        else
                        {
                            string s = standardError.ToString() + "\r\n" + standardOutput.ToString();
                            CompilerOutput += "\r\n" + s;
                            Debug.WriteLine(s);
                        }

                        // Load ptx file
                        if (binary)
                            this.StoreBinaryFile(srcCodeFile.ID, platform, p.Architecture, p.WorkingDirectory + Path.DirectorySeparatorChar + p.OutputFile);
                        else
                            this.StorePTXFile(srcCodeFile.ID, platform, p.Architecture, p.WorkingDirectory + Path.DirectorySeparatorChar + p.OutputFile);
#if DEBUG

#else
                    if (p.DeleteGeneratedFiles)
                        Delete(p.InputFile, p.OutputFile);
#endif
                    }
                }
            }
        }

        private static void Delete(string cuFileName, string ptxFileName)
        {
            // Delete files
            try
            {
                if(File.Exists(cuFileName))
                    File.Delete(cuFileName);
                if(File.Exists(ptxFileName))
                    File.Delete(ptxFileName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
