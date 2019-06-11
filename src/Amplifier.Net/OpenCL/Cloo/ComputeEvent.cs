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
    using System.Runtime.InteropServices;

    /// <summary>
    /// Represents an OpenCL event.
    /// </summary>
    /// <remarks> An event encapsulates the status of an operation such as a command. It can be used to synchronize operations in a context. </remarks>
    /// <seealso cref="ComputeUserEvent"/>
    /// <seealso cref="ComputeCommandQueue"/>
    /// <seealso cref="ComputeContext"/>
    internal class ComputeEvent : ComputeEventBase
    {
        #region Properties

        /// <summary>
        /// Gets the <see cref="ComputeCommandQueue"/> associated with the <see cref="ComputeEvent"/>.
        /// </summary>
        /// <value> The <see cref="ComputeCommandQueue"/> associated with the <see cref="ComputeEvent"/>. </value>
        public ComputeCommandQueue CommandQueue { get; }

        #endregion

        #region Constructors

        internal ComputeEvent(CLEventHandle handle, ComputeCommandQueue queue) : this (handle, queue, 0)
        {
            Type = (ComputeCommandType)GetInfo<CLEventHandle, ComputeEventInfo, int>(Handle, ComputeEventInfo.CommandType, CL12.GetEventInfo);
        }

        internal ComputeEvent(CLEventHandle handle, ComputeCommandQueue queue, ComputeCommandType type)
        {
            Handle = handle;
            SetID(Handle.Value);

            CommandQueue = queue;
            Type = type;
            Context = queue.Context;

            //if (ComputeTools.ParseVersionString(CommandQueue.Device.Platform.Version, 1) > new Version(1, 0))
            //    HookNotifier();

            //Debug.WriteLine("Create " + this + " in Thread(" + Thread.CurrentThread.ManagedThreadId + ").", "Information");
        }



        #endregion

        #region Internal methods

        internal void TrackGCHandle(GCHandle gcHandle)
        {
            var freeDelegate = new ComputeCommandStatusChanged((s, e) =>
            {
                if (gcHandle.IsAllocated && gcHandle.Target != null) gcHandle.Free();
            });

            Completed += freeDelegate;
            Aborted += freeDelegate;
        }

        #endregion
        
        /// <summary>
        /// Clones the event. Because the event is retained the cloned event as well as the clone have to be disposed
        /// </summary>
        /// <returns>Cloned event</returns>
        public override ComputeEventBase Clone()
        {
            CL10.RetainEvent(Handle);
            return new ComputeEvent(Handle, CommandQueue, Type);
        }
    }
}