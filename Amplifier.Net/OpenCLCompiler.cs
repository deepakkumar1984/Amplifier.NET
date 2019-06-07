using Amplifier.OpenCL.Cloo;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Amplifier
{
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
        private static List<ComputeDevice> _devices = new List<ComputeDevice>();

        /// <summary>
        /// The default device for the accelerator
        /// </summary>
        private static ComputeDevice _defaultDevice = null;

        /// <summary>
        /// The default platorm for the accelerator
        /// </summary>
        private static ComputePlatform _defaultPlatorm = null;

        /// <summary>
        /// List of all compiled kernels
        /// </summary>
        private static List<ComputeKernel> _compiledKernels = new List<ComputeKernel>();

        /// <summary>
        /// The computer context with 
        /// </summary>
        private static ComputeContext _context = null;
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
            string code = GetKernelCode(cls);

            CreateKernels(cls.Name, code);
        }

        /// <summary>
        /// Executes the specified kernel function name.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="inputs">The inputs.</param>
        /// <param name="returnInputVariable">The return result.</param>
        /// <returns></returns>
        /// <exception cref="ExecutionException">
        /// </exception>
        public override void Execute<TSource>(string functionName, params object[] args)
        {
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
                    //args[item.Key] = r;
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
        /// Uses the device for the compiler.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        /// <exception cref="System.Exception">No device found. Please invoke Accelerator.Init with a device id to initialize. " +
        ///                     "If you have done the Init and still getting the error please check if OpenCL is installed.</exception>
        /// <exception cref="System.ArgumentException">Device ID out of range.</exception>
        public override void UseDevice(int deviceId = 0)
        {
            _compiledKernels = new List<ComputeKernel>();
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
        private static void LoadDevices()
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
        private static string GetKernelCode(Type kernalClass)
        {
            string assemblyPath = kernalClass.Assembly.Location;
            CSharpDecompiler cSharpDecompiler
                = new CSharpDecompiler(assemblyPath, new ICSharpCode.Decompiler.DecompilerSettings() { ThrowOnAssemblyResolveErrors = false });
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
            }

            var kernelHandles = kernelMethods.ToList().Select(x => (x.MetadataToken)).ToList();
            var nonKernelHandles = nonKernelMethods.ToList().Select(x => (x.MetadataToken)).ToList();
            result.AppendLine("#ifdef cl_khr_fp64");
            result.AppendLine("#pragma OPENCL EXTENSION cl_khr_fp64 : enable");
            result.AppendLine("#endif");
            result.AppendLine(cSharpDecompiler.DecompileAsString(kernelHandles));

            result.AppendLine(cSharpDecompiler.DecompileAsString(nonKernelHandles));

            string resultCode = result.ToString();
            resultCode = resultCode.Replace("using Amplifier.OpenCL;", "")
                        .Replace("using System;", "")
                        .Replace("[OpenCLKernel]", "__kernel")
                        .Replace("public", "")
                        .Replace("[Global]", "global")
                        .Replace("[]", "*")
                        .Replace("@", "v_");

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

        /// <summary>
        /// Creates the kernels.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="code">The code.</param>
        /// <exception cref="CompileException"></exception>
        private static void CreateKernels(string name, string code)
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
                throw new CompileException(string.Format("Failed building {0} with error code: {1} \n Message: {2}", name, ex.Message, log));
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
        private static Dictionary<int, ComputeBuffer<TSource>> BuildKernelArguments<TSource>(object[] inputs, ComputeKernel kernel, long length, int? returnInputVariable = null) where TSource : struct
        {
            int i = 0;
            Dictionary<int, ComputeBuffer<TSource>> result = new Dictionary<int, ComputeBuffer<TSource>>();
            foreach (var item in inputs)
            {
                if (item.GetType() == typeof(TSource[]))
                {
                    var buffer = new ComputeBuffer<TSource>(_context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, ((TSource[])item));
                    kernel.SetMemoryArgument(i, buffer);
                    result.Add(i, buffer);
                }
                else if (item.GetType().IsPrimitive)
                    kernel.SetValueArgument(i, (TSource)item);

                i++;
            }

            return result;
        }
        #endregion
    }
}
