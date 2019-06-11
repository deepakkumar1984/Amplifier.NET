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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    /// <summary>
    /// Represents an OpenCL context.
    /// </summary>
    /// <remarks> The environment within which the kernels execute and the domain in which synchronization and memory management is defined. </remarks>
    /// <br/>
    /// <example> 
    /// This example shows how to create a <see cref="ComputeContext"/> that is able to share data with an OpenGL context in a Microsoft Windows OS:
    /// <code>
    /// <![CDATA[
    /// 
    /// // NOTE: If you see some non C# bits surrounding this code section, ignore them. They're not part of the code.
    /// 
    /// // We will need the device context, which is obtained through an OS specific function.
    /// [DllImport("opengl32.dll")]
    /// extern static IntPtr wglGetCurrentDC();
    /// 
    /// // Query the device context.
    /// IntPtr deviceContextHandle = wglGetCurrentDC();
    /// 
    /// // Select a platform which is capable of OpenCL/OpenGL interop.
    /// ComputePlatform platform = ComputePlatform.GetByName(name);
    /// 
    /// // Create the context property list and populate it.
    /// ComputeContextProperty p1 = new ComputeContextProperty(ComputeContextPropertyName.Platform, platform.Handle.Value);
    /// ComputeContextProperty p2 = new ComputeContextProperty(ComputeContextPropertyName.CL_GL_CONTEXT_KHR, openGLContextHandle);
    /// ComputeContextProperty p3 = new ComputeContextProperty(ComputeContextPropertyName.CL_WGL_HDC_KHR, deviceContextHandle);
    /// ComputeContextPropertyList cpl = new ComputeContextPropertyList(new ComputeContextProperty[] { p1, p2, p3 });
    /// 
    /// // Create the context. Usually, you'll want this on a GPU but other options might be available as well.
    /// ComputeContext context = new ComputeContext(ComputeDeviceTypes.Gpu, cpl, null, IntPtr.Zero);
    /// 
    /// // Create a shared OpenCL/OpenGL buffer.
    /// // The generic type should match the type of data that the buffer contains.
    /// // glBufferId is an existing OpenGL buffer identifier.
    /// ComputeBuffer<float> clglBuffer = ComputeBuffer.CreateFromGLBuffer<float>(context, ComputeMemoryFlags.ReadWrite, glBufferId);
    /// 
    /// ]]>
    /// </code>
    /// Before working with the <c>clglBuffer</c> you should make sure of two things:<br/>
    /// 1) OpenGL isn't using <c>glBufferId</c>. You can achieve this by calling <c>glFinish</c>.<br/>
    /// 2) Make it available to OpenCL through the <see cref="ComputeCommandQueue.AcquireGLObjects"/> method.<br/>
    /// When finished, you should wait until <c>clglBuffer</c> isn't used any longer by OpenCL. After that, call <see cref="ComputeCommandQueue.ReleaseGLObjects"/> to make the buffer available to OpenGL again.
    /// </example>
    /// <seealso cref="ComputeDevice"/>
    /// <seealso cref="ComputePlatform"/>
    internal class ComputeContext : ComputeResource
    {
        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ReadOnlyCollection<ComputeDevice> _devices;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ComputePlatform _platform;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ComputeContextPropertyList _properties;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ComputeContextNotifier _callback;

        private CLContextHandle _handle;

        #endregion

        #region Properties

        /// <summary>
        /// The handle of the <see cref="ComputeContext"/>.
        /// </summary>
        public CLContextHandle Handle
        {
            get => _handle;
            protected set => _handle = value;
        }

        /// <summary>
        /// Gets a read-only collection of the <see cref="ComputeDevice"/>s of the <see cref="ComputeContext"/>.
        /// </summary>
        /// <value> A read-only collection of the <see cref="ComputeDevice"/>s of the <see cref="ComputeContext"/>. </value>
        public ReadOnlyCollection<ComputeDevice> Devices => _devices;

        /// <summary>
        /// Gets the <see cref="ComputePlatform"/> of the <see cref="ComputeContext"/>.
        /// </summary>
        /// <value> The <see cref="ComputePlatform"/> of the <see cref="ComputeContext"/>. </value>
        public ComputePlatform Platform => _platform;

        /// <summary>
        /// Gets a collection of <see cref="ComputeContextProperty"/>s of the <see cref="ComputeContext"/>.
        /// </summary>
        /// <value> A collection of <see cref="ComputeContextProperty"/>s of the <see cref="ComputeContext"/>. </value>
        public ComputeContextPropertyList Properties => _properties;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="ComputeContext"/> on a collection of <see cref="ComputeDevice"/>s.
        /// </summary>
        /// <param name="devices"> A collection of <see cref="ComputeDevice"/>s to associate with the <see cref="ComputeContext"/>. </param>
        /// <param name="properties"> A <see cref="ComputeContextPropertyList"/> of the <see cref="ComputeContext"/>. </param>
        /// <param name="notify"> A delegate instance that refers to a notification routine. This routine is a callback function that will be used by the OpenCL implementation to report information on errors that occur in the <see cref="ComputeContext"/>. The callback function may be called asynchronously by the OpenCL implementation. It is the application's responsibility to ensure that the callback function is thread-safe and that the delegate instance doesn't get collected by the Garbage Collector until <see cref="ComputeContext"/> is disposed. If <paramref name="notify"/> is <c>null</c>, no callback function is registered. </param>
        /// <param name="notifyDataPtr"> Optional user data that will be passed to <paramref name="notify"/>. </param>
        public ComputeContext(ICollection<ComputeDevice> devices, ComputeContextPropertyList properties, ComputeContextNotifier notify, IntPtr notifyDataPtr)
        {
            CLDeviceHandle[] deviceHandles = ComputeTools.ExtractHandles(devices, out var handleCount);
            IntPtr[] propertyArray = properties?.ToIntPtrArray();
            _callback = notify;

            Handle = CL12.CreateContext(propertyArray, handleCount, deviceHandles, notify, notifyDataPtr, out var error);
            ComputeException.ThrowOnError(error);
            
            SetID(Handle.Value);
            
            _properties = properties;
            ComputeContextProperty platformProperty = properties.GetByName(ComputeContextPropertyName.Platform);
            _platform = ComputePlatform.GetByHandle(platformProperty.Value);
            _devices = GetDevices();

            //Debug.WriteLine("Create " + this + " in Thread(" + Thread.CurrentThread.ManagedThreadId + ").", "Information");
        }

        /// <summary>
        /// Creates a new <see cref="ComputeContext"/> on all the <see cref="ComputeDevice"/>s that match the specified <see cref="ComputeDeviceTypes"/>.
        /// </summary>
        /// <param name="deviceType"> A bit-field that identifies the type of <see cref="ComputeDevice"/> to associate with the <see cref="ComputeContext"/>. </param>
        /// <param name="properties"> A <see cref="ComputeContextPropertyList"/> of the <see cref="ComputeContext"/>. </param>
        /// <param name="notify"> A delegate instance that refers to a notification routine. This routine is a callback function that will be used by the OpenCL implementation to report information on errors that occur in the <see cref="ComputeContext"/>. The callback function may be called asynchronously by the OpenCL implementation. It is the application's responsibility to ensure that the callback function is thread-safe and that the delegate instance doesn't get collected by the Garbage Collector until <see cref="ComputeContext"/> is disposed. If <paramref name="notify"/> is <c>null</c>, no callback function is registered. </param>
        /// <param name="userDataPtr"> Optional user data that will be passed to <paramref name="notify"/>. </param>
        public ComputeContext(ComputeDeviceTypes deviceType, ComputeContextPropertyList properties, ComputeContextNotifier notify, IntPtr userDataPtr)
        {
            IntPtr[] propertyArray = properties?.ToIntPtrArray();
            _callback = notify;

            Handle = CL12.CreateContextFromType(propertyArray, deviceType, notify, userDataPtr, out var error);
            ComputeException.ThrowOnError(error);

            SetID(Handle.Value);

            _properties = properties;
            ComputeContextProperty platformProperty = properties.GetByName(ComputeContextPropertyName.Platform);
            _platform = ComputePlatform.GetByHandle(platformProperty.Value);
            _devices = GetDevices();

            //Debug.WriteLine("Create " + this + " in Thread(" + Thread.CurrentThread.ManagedThreadId + ").", "Information");
        }

        /*private struct ComputeContextPropertyTuple
        {
            public ComputeContextPropertyName Name;
            public IntPtr Value;
        }*/

        /// <summary>
        /// Creates a <see cref="ComputeContext"/> from an external handle>.
        /// </summary>
        /// <param name="externalHandle"></param>
        public ComputeContext(IntPtr externalHandle)
        {
            Handle = new CLContextHandle(externalHandle);
            
            ComputeErrorCode error = CL12.RetainContext(Handle);
            ComputeException.ThrowOnError(error);

            SetID(Handle.Value);

            //var p1 = new List<ComputeContextPropertyTuple>(GetArrayInfo<CLContextHandle, ComputeContextInfo, ComputeContextPropertyTuple>(Handle, ComputeContextInfo.Properties, CL12.GetContextInfo));
            var p2 = new List<ComputeContextProperty>();
            //foreach (var p in p1) p2.Add(new ComputeContextProperty(p.Name, p.Value));

            _properties = new ComputeContextPropertyList(p2);
            ComputeContextProperty platformProperty = _properties.GetByName(ComputeContextPropertyName.Platform);
            _platform = ComputePlatform.GetByHandle(platformProperty.Value);
            _devices = GetDevices();

            //Debug.WriteLine("Create " + this + " in Thread(" + Thread.CurrentThread.ManagedThreadId + ").", "Information");
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Releases the associated OpenCL object.
        /// </summary>
        /// <param name="manual"> Specifies the operation mode of this method. </param>
        /// <remarks> <paramref name="manual"/> must be <c>true</c> if this method is invoked directly by the application. </remarks>
        protected override void Dispose(bool manual)
        {
            if (manual)
            {
                //free managed resources
            }

            // free native resources
            if (Handle.IsValid)
            {
                //Debug.WriteLine("Dispose " + this + " in Thread(" + Thread.CurrentThread.ManagedThreadId + ").", "Information");
                CL12.ReleaseContext(Handle);
                _handle.Invalidate();
            }
        }

        #endregion

        #region Private methods

        private ReadOnlyCollection<ComputeDevice> GetDevices()
        {
            List<CLDeviceHandle> deviceHandles = new List<CLDeviceHandle>(GetArrayInfo<CLContextHandle, ComputeContextInfo, CLDeviceHandle>(Handle, ComputeContextInfo.Devices, CL12.GetContextInfo));
            List<ComputeDevice> devices = new List<ComputeDevice>();
            foreach (ComputePlatform platform in ComputePlatform.Platforms)
            {
                foreach (ComputeDevice device in platform.Devices)
                    if (deviceHandles.Contains(device.Handle))
                        devices.Add(device);
            }
            return new ReadOnlyCollection<ComputeDevice>(devices);
        }

        #endregion
    }
}