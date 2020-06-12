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
    using Amplifier.OpenCL.Cloo.Bindings;
    using Amplifier.Decompiler.CSharp;
    using Amplifier.Decompiler.TypeSystem;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
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
            source.AppendLine("#define NELEMS(x)  (sizeof(x) / sizeof((x)[0]))");
            source.AppendLine("typedef unsigned char byte;");
            source.AppendLine("typedef char sbyte;");
            if (_compiledInstances.Contains(cls))
                throw new CompileException(string.Format("{0} is already compiled", cls.FullName));

            _compiledInstances.Add(cls);
            _compiledInstances = _compiledInstances.OrderByDescending(x => (x.IsLayoutSequential)).ToList();
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
        /// Compiles the kernel with the classes list
        /// </summary>
        /// <param name="classes">The classes.</param>
        public override void CompileKernel(params Type[] classes)
        {
            StringBuilder source = new StringBuilder();
            source.AppendLine("#ifdef cl_khr_fp64");
            source.AppendLine("#pragma OPENCL EXTENSION cl_khr_fp64 : enable");
            source.AppendLine("#endif");
            source.AppendLine("#define NELEMS(x)  (sizeof(x) / sizeof((x)[0]))");
            source.AppendLine("typedef unsigned char byte;");
            source.AppendLine("typedef char sbyte;");
            foreach (var cls in classes)
            {
                if (_compiledInstances.Contains(cls))
                    throw new CompileException(string.Format("{0} is already compiled", cls.FullName));

                _compiledInstances.Add(cls);
            }

            _compiledInstances = _compiledInstances.OrderByDescending(x => (x.IsLayoutSequential)).ToList();

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
        /// Compiles the kernel directly from the source.
        /// </summary>
        /// <param name="source">The source code.</param>
        public override void CompileKernel(string source)
        {
            CreateKernels(source);
            SourceCode = source;
        }

        /// <summary>
        /// Executes the specified kernel function name.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="args"></param>
        /// <exception cref="ExecutionException">
        /// </exception>
        public override void Execute(string functionName, params object[] args)
        {
            ValidateArgs(functionName, args);

            ComputeKernel kernel = _compiledKernels.FirstOrDefault(x => (x.FunctionName == functionName));
            ComputeCommandQueue commands = new ComputeCommandQueue(_context, _defaultDevice, ComputeCommandQueueFlags.None);

            if (kernel == null)
                throw new ExecutionException(string.Format("Kernal function {0} not found", functionName));

            try
            {
                Array ndobject = (Array)args.FirstOrDefault(x => (x.GetType().IsArray));
                List<long> length = new List<long>();
                long totalLength = 0;
                if (ndobject == null)
                {
                    var xarrayList = args.Where(x => (x.GetType().Name == "XArray" || x.GetType().BaseType.Name == "XArray")).ToList();
                    foreach (var item in xarrayList)
                    {
                        var xarrayobj = (XArray)item;
                        if (xarrayobj.Direction == Direction.Output)
                        {
                            totalLength = xarrayobj.Count;
                            if (!xarrayobj.IsElementWise)
                                length = xarrayobj.Sizes.ToList();
                            else
                                length.Add(totalLength);
                        }
                    }
                   
                    if(totalLength == 0)
                    {
                        var xarrayobj = (XArray)xarrayList[0];
                        totalLength = xarrayobj.Count;
                        if (!xarrayobj.IsElementWise)
                            length = xarrayobj.Sizes.ToList();
                        else
                            length.Add(totalLength);
                    }
                }
                else
                {
                    totalLength = ndobject.Length;
                    for (int i = 0; i < ndobject.Rank; i++)
                    {
                        length.Add(ndobject.GetLength(i));
                    }
                }

                var method = KernelFunctions.FirstOrDefault(x => (x.Name == functionName));

                var buffers = BuildKernelArguments(method, args, kernel, totalLength);
                commands.Execute(kernel, null, length.ToArray(), null, null);

                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].GetType().IsArray)
                    {
                        var ioMode = method.Parameters.ElementAt(i).Value.IOMode;
                        if (ioMode == IOMode.InOut || ioMode == IOMode.Out)
                        {
                            Array r = (Array)args[i];
                            commands.ReadFromMemory(buffers[i], ref r, true, 0, null);
                        }

                        buffers[i].Dispose();
                    }
                    else if (args[i].GetType().Name == "XArray" || args[i].GetType().BaseType.Name == "XArray")
                    {
                        var ioMode = method.Parameters.ElementAt(i).Value.IOMode;
                        if (ioMode == IOMode.InOut || ioMode == IOMode.Out)
                        {
                            XArray r = (XArray)args[i];
                            commands.ReadFromMemory(buffers[i], ref r, true, 0, null);
                        }

                        buffers[i].Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ExecutionException(ex.Message);
            }
            finally
            {
                commands.Finish();
                commands.Dispose();
            }
        }

        private int GetSizeOfObject(object obj)
        {
            object val = null;
            int size = 0;
            Type type = obj.GetType();
            PropertyInfo[] info = type.GetProperties();
            var datagch = GCHandle.Alloc(obj, GCHandleType.Pinned);
            
            foreach (PropertyInfo property in info)
            {
                val = property.GetValue(obj, null);
                //size += sizeof(float);
            }
            return size;
        }

        /// <summary>
        /// Validate the list of arguments
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="args">The arguments.</param>
        /// <exception cref="ExecutionException"></exception>
        private void ValidateArgs(string functionName, object[] args)
        {
            var method = KernelFunctions.FirstOrDefault(x => (x.Name == functionName));

            for (int i = 0; i < args.Length; i++)
            {
                var parameter = method.Parameters.ElementAt(i);

                if (args[i].GetType().IsPrimitive)
                {
                    args[i] = Convert.ChangeType(args[i], Type.GetType(parameter.Value.TypeName));
                }
                else if (args[i].GetType().IsArray)
                {
                    if (parameter.Value.TypeName != args[i].GetType().FullName)
                        throw new ExecutionException(string.Format("Data type mismatch for parameter {0}. Expected is {1} but got {2}",
                                                        parameter.Key,
                                                        (parameter.Value,
                                                        args[i].GetType().FullName)));
                }
                else if (args[i].GetType().Name == "XArray" || args[i].GetType().BaseType.Name == "XArray")
                {
                    XArray array = (XArray)args[i];
                    if (!parameter.Value.TypeName.Contains(array.DataType.ToCLRType().Name))
                        throw new ExecutionException(string.Format("Data type mismatch for parameter {0}. Expected is {1} but got {2}",
                                                        parameter.Key,
                                                        (parameter.Value,
                                                        array.DataType.ToCLRType().Name)));
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
                = new CSharpDecompiler(assemblyPath, new Amplifier.Decompiler.DecompilerSettings() { ThrowOnAssemblyResolveErrors = false, ForEachStatement = false });
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
                    var isInput = p.GetAttributes().Any(x => x.AttributeType.Name == "InputAttribute");
                    var isOutput = p.GetAttributes().Any(x => x.AttributeType.Name == "OutputAttribute");
                    var mode = IOMode.InOut;
                    if (isInput)
                        mode = IOMode.In;
                    if (isOutput)
                        mode = IOMode.Out;
                    if (isInput && isOutput)
                        mode = IOMode.InOut;
                    k.Parameters.Add(p.Name, new FunctionParameter
                    {
                        TypeName = p.Type.FullName,
                        IOMode = mode
                    });
                }

                KernelFunctions.Add(k);
            }

            var kernelHandles = kernelMethods.ToList().Select(x => (x.MetadataToken)).ToList();
            var nonKernelHandles = nonKernelMethods.ToList().Select(x => (x.MetadataToken)).ToList();

            result.AppendLine(cSharpDecompiler.DecompileAsString(nonKernelHandles));
            result.AppendLine(cSharpDecompiler.DecompileAsString(kernelHandles));

            return CodeTranslator.Translate(result.ToString());
        }

        private string GetStructCode(Type structInstance)
        {
            string assemblyPath = structInstance.Assembly.Location;
            CSharpDecompiler cSharpDecompiler
                = new CSharpDecompiler(assemblyPath, new Amplifier.Decompiler.DecompilerSettings() { ThrowOnAssemblyResolveErrors = false, ForEachStatement = false });

            var tree = cSharpDecompiler.DecompileType(new FullTypeName(structInstance.FullName));

            string code = cSharpDecompiler.DecompileTypeAsString(new FullTypeName(structInstance.FullName));
            return CodeTranslator.Translate(code).Trim() + ";";
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
        /// <param name="method">The method.</param>
        /// <param name="inputs">The inputs.</param>
        /// <param name="kernel">The kernel.</param>
        /// <param name="length">The length.</param>
        /// <param name="returnInputVariable">The return result.</param>
        /// <returns></returns>
        private Dictionary<int, GenericArrayMemory> BuildKernelArguments(KernelFunction method, object[] inputs, ComputeKernel kernel, long length, int? returnInputVariable = null)
        {
            int i = 0;
            Dictionary<int, GenericArrayMemory> result = new Dictionary<int, GenericArrayMemory>();

            foreach (var item in inputs)
            {
                int size = 0;
                if(item.GetType().IsArray)
                {
                    var mode = method.Parameters.ElementAt(i).Value.IOMode;
                    var flag = ComputeMemoryFlags.ReadWrite;
                    if (mode == IOMode.Out)
                        flag |= ComputeMemoryFlags.AllocateHostPointer;
                    else
                        flag |= ComputeMemoryFlags.CopyHostPointer;
                    GenericArrayMemory mem = new GenericArrayMemory(_context, flag, (Array)item);
                    kernel.SetMemoryArgument(i, mem);
                    result.Add(i, mem);
                }
                else if (item.GetType().Name == "XArray" || item.GetType().BaseType.Name == "XArray")
                {
                    var mode = method.Parameters.ElementAt(i).Value.IOMode;
                    var flag = ComputeMemoryFlags.ReadWrite;
                    if (mode == IOMode.Out)
                        flag |= ComputeMemoryFlags.AllocateHostPointer;
                    else
                        flag |= ComputeMemoryFlags.CopyHostPointer;
                    GenericArrayMemory mem = new GenericArrayMemory(_context, flag, (XArray)item);
                    kernel.SetMemoryArgument(i, mem);
                    result.Add(i, mem);
                }
                else
                {
                    size = Marshal.SizeOf(item);
                    var datagch = GCHandle.Alloc(item, GCHandleType.Pinned);
                    kernel.SetArgument(i, new IntPtr(size), datagch.AddrOfPinnedObject());
                }

                i++;
            }

            return result;
        }

       
        #endregion
    }
}

