namespace Amplifier.OpenCL
{
    /// <summary>
    /// Open CL extension functions
    /// </summary>
    public partial class OpenCLFunctions
    {
        /// <summary>
        /// Get the instance of the kernel class comoiled with the specified device identifier.
        /// </summary>
        /// <value>
        /// The <see cref="dynamic"/>.
        /// </value>
        /// <param name="deviceId">The device identifier.</param>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        public dynamic this[int deviceId]
        {
            get
            {
                var compiler = new OpenCLCompiler();
                compiler.UseDevice(deviceId);
                compiler.CompileKernel(this.GetType());

                return compiler.GetExec();
            }
        }
    }
}
