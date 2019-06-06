using System;
using System.Collections.Generic;
using System.Text;

namespace Amplifier
{
    public abstract class BaseCompiler : IDisposable
    {
        /// <summary>
        /// Gets or sets the name of the compiler.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

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
        /// Executes the specified kernel function name.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="inputs">The inputs.</param>
        /// <param name="returnInputVariable">The return result.</param>
        /// <returns></returns>
        public abstract void Execute<TSource>(string functionName, params object[] args) where TSource : struct;

        /// <summary>
        /// Gets the execute.
        /// </summary>
        /// <returns></returns>
        public dynamic GetExec<T>() where T : struct
        {
            return new Executer<T>(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            
        }
    }
}
