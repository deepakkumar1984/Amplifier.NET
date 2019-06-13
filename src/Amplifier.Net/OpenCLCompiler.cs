/*
MIT License

Copyright (c) 2019 Tech Quantum

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
namespace Amplifier
{
    using Amplifier.OpenCL;
    using Amplifier.OpenCL.Cloo;
    using ICSharpCode.Decompiler.CSharp;
    using ICSharpCode.Decompiler.TypeSystem;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Compiler for OpenCL which will be used to compile kernel created in C# and execute them.
    /// </summary>
    /// <seealso cref="Amplifier.BaseCompiler" />
    public class OpenCLCompiler : BaseCompiler
    {
        #region Private Variables
        /// <summary>
        /// The device list
        /// </summary>
        private List<ComputeDevice> _devices = new List<ComputeDevice>();

        /// <summary>
        /// The default device for the accelerator
        /// </summary>
        private ComputeDevice _defaultDevice = null;

        /// <summary>
        /// The default platorm for the accelerator
        /// </summary>
        private ComputePlatform _defaultPlatorm = null;

        /// <summary>
        /// List of all compiled kernels
        /// </summary>
        private List<ComputeKernel> _compiledKernels = new List<ComputeKernel>();

        /// <summary>
        /// The computer context with 
        /// </summary>
        private ComputeContext _context = null;

        /// <summary>
        /// The compiled instances
        /// </summary>
        private List<Type> _compiledInstances = new List<Type>();

        /// <summary>
        /// The source code
        /// </summary>
        private string SourceCode = "";

        /// <summary>
        /// The replace empty list
        /// </summary>
        private string[] replaceEmptyList = new string[] { "using Amplifier.OpenCL;", "using System;", "public", "private", "protected", "<float>", "<double>", "<complex>",
                                        "<int>", "<uint>", "<long>", "<byte>", "<sbyte>", "this.", "base." };

        #endregion

        #region Abstract Implementation
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenCLCompiler"/> class.
        /// </summary>
        public OpenCLCompiler() : base("OpenCL")
        {
        }

        /// <summary>
        /// Gets or sets the devices.
        /// </summary>
        /// <value>
        /// The devices.
        /// </value>
        public override List<Device> Devices
        {
            get
            {
                List<Device> result = new List<Device>();
                LoadDevices();

                for (int i = 0; i < _devices.Count; i++)
                {
                    result.Add(new Device()
                    {
                        ID = i,
                        Name = _devices[i].Name,
                        Type = (DeviceType)_devices[i].Type,
                        Vendor = _devices[i].Vendor,
                        Arch = DeviceArch.OpenCL
                    });
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the kernel functions for the compiler.
        /// </summary>
        /// <value>
        /// The kernels.
        /// </value>
        public override List<string> Kernels
        {
            get { return _compiledKernels.Select(x => x.FunctionName).ToList(); }
        }

        /// <summary>
        /// Compiles the kernel.
        /// </summary>
        /// <param name="cls">The CLS.</param>
        public override void CompileKernel(Type cls)
        {
            StringBuilder source = new StringBuilder();
            source.AppendLine("#ifdef cl_khr_fp64");
            source.AppendLine("#pragma OPENCL EXTENSION cl_khr_fp64 : enable");
            source.AppendLine("#endif");
            if (_compiledInstances.Contains(cls))
                throw new CompileException(string.Format("{0} is already compiled", cls.FullName));

            _compiledInstances.Add(cls);
            KernelFunctions.Clear();
            foreach (var item in _compiledInstances)
            {
                source.AppendLine(GetKernelCode(item));
            }
            
            CreateKernels(source.ToString());
            SourceCode = source.ToString();
        }

        /// <summary>
        /// Compiles the kernel with the classes list
        /// </summary>
        /// <param name="classes">The classes.</param>
        public override void CompileKernel(params Type[] classes)
        {
            StringBuilder source = new StringBuilder();
            source.AppendLine("#ifdef cl_khr_fp64");
            source.AppendLine("#pragma OPENCL EXTENSION cl_khr_fp64 : enable");
            source.AppendLine("#endif");
            foreach (var cls in classes)
            {
                if (_compiledInstances.Contains(cls))
                    throw new CompileException(string.Format("{0} is already compiled", cls.FullName));

                _compiledInstances.Add(cls);
            }

            KernelFunctions.Clear();
            foreach (var item in _compiledInstances)
            {
                if (item.IsLayoutSequential)
                    source.AppendLine(GetStructCode(item));
                else if (item.IsClass)
                    source.AppendLine(GetKernelCode(item));
            }

            CreateKernels(source.ToString());
            SourceCode = source.ToString();
        }

        /// <summary>
        /// Executes the specified kernel function name.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="args"></param>
        /// <exception cref="ExecutionException">
        /// </exception>
        public override void Execute<TSource>(string functionName, params object[] args)
        {
            ValidateArgs<TSource>(functionName, args);

            ComputeKernel kernel = _compiledKernels.FirstOrDefault(x => (x.FunctionName == functionName));
            ComputeCommandQueue commands = new ComputeCommandQueue(_context, _defaultDevice, ComputeCommandQueueFlags.None);

            if (kernel == null)
                throw new ExecutionException(string.Format("Kernal function {0} not found", functionName));

            try
            {
                var ndobject = (TSource[])args.FirstOrDefault(x => (x.GetType() == typeof(TSource[])));

                long length = ndobject != null ? ndobject.Length : 1;

                var buffers = BuildKernelArguments<TSource>(args, kernel, length);
                commands.Execute(kernel, null, new long[] { length }, null, null);

                foreach (var item in buffers)
                {
                    TSource[] r = (TSource[])args[item.Key];
                    commands.ReadFromBuffer(item.Value, ref r, true, null);
                    item.Value.Dispose();
                }

                commands.Finish();
            }
            catch (Exception ex)
            {
                throw new ExecutionException(ex.Message);
            }
            finally
            {
                commands.Dispose();
            }
        }

        /// <summary>
        /// Validate the list of arguments
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="args">The arguments.</param>
        /// <exception cref="ExecutionException"></exception>
        private void ValidateArgs<TSource>(string functionName, object[] args) where TSource : struct
        {
            var method = KernelFunctions.FirstOrDefault(x => (x.Name == functionName));

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].GetType().IsPrimitive)
                {
                    args[i] = Convert.ChangeType(args[i], typeof(TSource));
                }
                else if (args[i].GetType().IsArray)
                {
                    var parameter = method.Parameters.ElementAt(i);
                    if (parameter.Value != args[i].GetType().Name)
                        throw new ExecutionException(string.Format("Data type mismatch for parameter {0}. Expected is {1} but got {2}",
                                                        parameter.Key,
                                                        (parameter.Value,
                                                        args[i].GetType().Name)));
                }
            }
        }

        /// <summary>
        /// Saves the compiler to a file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public override void Save(string filePath)
        {
            KernelBin bin = new KernelBin()
            {
                Instances = _compiledInstances,
                InstanceMethods = KernelFunctions,
                SourceCode = SourceCode
            };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(bin, Newtonsoft.Json.Formatting.Indented);
            byte[] data = Encoding.UTF8.GetBytes(json);

            File.WriteAllText(filePath, Convert.ToBase64String(data));
        }

        /// <summary>
        /// Loads the compiler from the saved bin file.
        /// </summary>
        /// <param name="filePath">The file path for the saved binary.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Load(string filePath, int deviceId = 0)
        {
            string base64Data= File.ReadAllText(filePath);
            var data = Convert.FromBase64String(base64Data);
            var json = Encoding.UTF8.GetString(data);
            var bin = Newtonsoft.Json.JsonConvert.DeserializeObject<KernelBin>(json);
            DeviceID = deviceId;
            _compiledInstances = bin.Instances;
            KernelFunctions = bin.InstanceMethods;
            UseDevice(deviceId);
            CreateKernels(bin.SourceCode);
        }

        /// <summary>
        /// Uses the device for the compiler.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        /// <exception cref="System.Exception">No device found. Please invoke Accelerator.Init with a device id to initialize. " +
        ///                     "If you have done the Init and still getting the error please check if OpenCL is installed.</exception>
        /// <exception cref="System.ArgumentException">Device ID out of range.</exception>
        public override void UseDevice(int deviceId = 0)
        {
            _compiledKernels = new List<ComputeKernel>();
            _compiledInstances = new List<Type>();
            LoadDevices();

            if (_devices.Count == 0)
                throw new Exception("No device found. Please invoke Accelerator.Init with a device id to initialize. " +
                    "If you have done the Init and still getting the error please check if OpenCL is installed.");

            if (deviceId >= _devices.Count)
                throw new ArgumentException("Device ID out of range.");

            _defaultDevice = _devices[deviceId];
            _defaultPlatorm = _defaultDevice.Platform;
            ComputeContextPropertyList properties = new ComputeContextPropertyList(_defaultPlatorm);
            _context = new ComputeContext(new ComputeDevice[] { _defaultDevice }, properties, null, IntPtr.Zero);
            Console.WriteLine("Selected device: " + _defaultDevice.Name);
            DeviceID = deviceId;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            foreach (var item in _compiledKernels)
            {
                item.Dispose();
            }

            _context.Dispose();
            base.Dispose();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Loads the devices.
        /// </summary>
        private void LoadDevices()
        {
            if (_devices.Count == 0)
            {
                _devices = new List<ComputeDevice>();
                foreach (var item in ComputePlatform.Platforms)
                {
                    _devices.AddRange(item.Devices);
                }
            }
        }

        /// <summary>
        /// Gets the kernel code.
        /// </summary>
        /// <param name="kernalClass">The kernal class.</param>
        /// <returns></returns>
        private string GetKernelCode(Type kernalClass)
        {
            string assemblyPath = kernalClass.Assembly.Location;
            CSharpDecompiler cSharpDecompiler
                = new CSharpDecompiler(assemblyPath, new ICSharpCode.Decompiler.DecompilerSettings() { ThrowOnAssemblyResolveErrors = false, ForEachStatement = false });
            StringBuilder result = new StringBuilder();
            ITypeDefinition typeInfo = cSharpDecompiler.TypeSystem.MainModule.Compilation.FindType(new FullTypeName(kernalClass.FullName)).GetDefinition();

            List<IMethod> kernelMethods = new List<IMethod>();
            List<IMethod> nonKernelMethods = new List<IMethod>();
            foreach (var item in typeInfo.Methods)
            {
                if (item.IsConstructor)
                    continue;

                if (item.GetAttributes().FirstOrDefault(x => (x.AttributeType.Name == "OpenCLKernelAttribute")) == null)
                {
                    nonKernelMethods.Add(item);
                    continue;
                }

                kernelMethods.Add(item);

                var k = new KernelFunction() { Name = item.Name };
                foreach (var p in item.Parameters)
                {
                    k.Parameters.Add(p.Name, p.Type.Name);
                }

                KernelFunctions.Add(k);
            }

            var kernelHandles = kernelMethods.ToList().Select(x => (x.MetadataToken)).ToList();
            var nonKernelHandles = nonKernelMethods.ToList().Select(x => (x.MetadataToken)).ToList();

            result.AppendLine(cSharpDecompiler.DecompileAsString(nonKernelHandles));
            result.AppendLine(cSharpDecompiler.DecompileAsString(kernelHandles));

            string resultCode = result.ToString();
            resultCode = resultCode
                        .Replace("[OpenCLKernel]", "__kernel")
                        .Replace("[Global]", "global")
                        .Replace("[]", "*")
                        .Replace("@", "v_");

            foreach (var item in replaceEmptyList)
            {
                resultCode = resultCode.Replace(item, "");
            }

            Regex floatRegEx = new Regex(@"(\d+)(\.\d+)*f]?");
            var matches = floatRegEx.Matches(resultCode);
            foreach (Match match in matches)
            {
                resultCode = resultCode.Replace(match.Value, match.Value.Replace("f", ""));
            }

            floatRegEx = new Regex(@"(\d+)(\.\d+)*u]?");
            matches = floatRegEx.Matches(resultCode);
            foreach (Match match in matches)
            {
                resultCode = resultCode.Replace(match.Value, match.Value.Replace("u", ""));
            }

            return resultCode;
        }

        private string GetStructCode(Type structInstance)
        {
            string assemblyPath = structInstance.Assembly.Location;
            CSharpDecompiler cSharpDecompiler
                = new CSharpDecompiler(assemblyPath, new ICSharpCode.Decompiler.DecompilerSettings() { ThrowOnAssemblyResolveErrors = false, ForEachStatement = false });

            var tree = cSharpDecompiler.DecompileType(new FullTypeName(structInstance.FullName));

            return cSharpDecompiler.DecompileTypeAsString(new FullTypeName(structInstance.FullName));
        }

        /// <summary>
        /// Creates the kernels.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="code">The code.</param>
        /// <exception cref="CompileException"></exception>
        private void CreateKernels(string code)
        {
            var program = new ComputeProgram(_context, code);
            try
            {
                program.Build(null, null, null, IntPtr.Zero);
                _compiledKernels.AddRange(program.CreateAllKernels());
            }
            catch (Exception ex)
            {
                string log = program.GetBuildLog(_defaultDevice);
                throw new CompileException(string.Format("Compilation error code: {0} \n Message: {1}", ex.Message, log));
            }
        }

        /// <summary>
        /// Builds the kernel arguments.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="inputs">The inputs.</param>
        /// <param name="kernel">The kernel.</param>
        /// <param name="length">The length.</param>
        /// <param name="returnInputVariable">The return result.</param>
        /// <returns></returns>
        private Dictionary<int, ComputeBuffer<TSource>> BuildKernelArguments<TSource>(object[] inputs, ComputeKernel kernel, long length, int? returnInputVariable = null) where TSource : struct
        {
            int i = 0;
            Dictionary<int, ComputeBuffer<TSource>> result = new Dictionary<int, ComputeBuffer<TSource>>();
            
            foreach (var item in inputs)
            {
                if (item.GetType().IsArray)
                {
                    var datagch = GCHandle.Alloc(item, GCHandleType.Pinned);
                    var buffer = new ComputeBuffer<TSource>(_context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, length, datagch.AddrOfPinnedObject());
                    kernel.SetMemoryArgument(i, buffer);
                    result.Add(i, buffer);
                }
                else if (item.GetType().IsPrimitive)
                {
                    var datagch = GCHandle.Alloc(item, GCHandleType.Pinned);
                    kernel.SetArgument(i, new IntPtr(Marshal.SizeOf(item)), datagch.AddrOfPinnedObject());
                }

                i++;
            }

            return result;
        }
        #endregion
    }
}

