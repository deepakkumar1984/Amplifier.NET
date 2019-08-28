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
    using Amplifier.Decompiler.TypeSystem;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Abstract class for OpenCL and CUDA compiler
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public abstract class BaseCompiler : IDisposable
    {
        /// <summary>
        /// Gets or sets the kernel functions.
        /// </summary>
        /// <value>
        /// The kernel functions.
        /// </value>
        public List<KernelFunction> KernelFunctions { get; set; } = new List<KernelFunction>();

        /// <summary>
        /// Gets or sets the name of the compiler.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        /// <value>
        /// The device identifier.
        /// </value>
        public int DeviceID { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCompiler"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public BaseCompiler(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets the devices.
        /// </summary>
        /// <value>
        /// The devices.
        /// </value>
        public abstract List<Device> Devices { get; }

        /// <summary>
        /// Gets the kernel functions for the compiler.
        /// </summary>
        /// <value>
        /// The kernels.
        /// </value>
        public abstract List<string> Kernels { get; }

        /// <summary>
        /// Uses the device for the compiler.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        public abstract void UseDevice(int deviceId = 0);

        /// <summary>
        /// Compiles the kernel.
        /// </summary>
        /// <param name="cls">The CLS.</param>
        public abstract void CompileKernel(Type cls);

        /// <summary>
        /// Compiles the kernel with the classes list
        /// </summary>
        /// <param name="classes">The classes.</param>
        public abstract void CompileKernel(params Type[] classes);

        /// <summary>
        /// Gets the default executer with float as base data type.
        /// </summary>
        /// <value>
        /// The execute.
        /// </value>
        public dynamic Exec
        {
            get
            {
                return new Executer(this);
            }
        }

        /// <summary>
        /// Executes the specified kernel function name.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="inputs">The inputs.</param>
        /// <param name="returnInputVariable">The return result.</param>
        /// <returns></returns>
        public abstract void Execute(string functionName, params object[] args);

        /// <summary>
        /// Saves the compiler to a file.
        /// </summary>
        /// <param name="filePath">The file path to save the compiled binary.</param>
        public abstract void Save(string filePath);

        /// <summary>
        /// Loads the compiler from the saved bin file.
        /// </summary>
        /// <param name="filePath">The file path for the saved binary.</param>
        /// <param name="deviceId">The device identifier.</param>
        public abstract void Load(string filePath, int deviceId = 0);

        /// <summary>
        /// Gets the execute.
        /// </summary>
        /// <returns></returns>
        public dynamic GetExec()
        {
            return new Executer(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            
        }
    }
}
