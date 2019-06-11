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

using System.Runtime.InteropServices;
using Amplifier.OpenCL.Cloo.Bindings;

namespace Amplifier.OpenCL.Cloo
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Represents the parent type to any Cloo event types.
    /// </summary>
    /// <seealso cref="ComputeEvent"/>
    /// <seealso cref="ComputeUserEvent"/>
    internal abstract class ComputeEventBase : ComputeResource
    {
        #region Fields

        private event ComputeCommandStatusChanged AbortedInternal;        
        private event ComputeCommandStatusChanged CompletedInternal;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ComputeCommandStatusArgs _status;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _notifierHooked;

        private readonly object _statusLockObject = new object();
        private CLEventHandle _handle;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the command associated with the event is abnormally terminated.
        /// </summary>
        /// <remarks> Requires OpenCL 1.1. </remarks>
        public event ComputeCommandStatusChanged Aborted
        {
            add
            {
                lock (_statusLockObject)
                {
                    if (!_notifierHooked) HookNotifier();

                    if (_status != null && _status.Status != ComputeCommandExecutionStatus.Complete)
                        value.Invoke(this, _status);

                    AbortedInternal += value;
                }
            }
            remove => AbortedInternal -= value;
        }

        /// <summary>
        /// Occurs when <c>ComputeEventBase.Status</c> changes to <c>ComputeCommandExecutionStatus.Complete</c>.
        /// </summary>
        /// <remarks> Requires OpenCL 1.1. </remarks>
        public event ComputeCommandStatusChanged Completed
        {
            add
            {
                lock (_statusLockObject)
                {
                    if (!_notifierHooked) HookNotifier();
                    
                    if (_status != null && _status.Status == ComputeCommandExecutionStatus.Complete)
                        value.Invoke(this, _status);

                    CompletedInternal += value;
                }
            }
            remove => CompletedInternal -= value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The handle of the <see cref="ComputeEventBase"/>.
        /// </summary>
        public CLEventHandle Handle
        {
            get => _handle;
            protected set => _handle = value;
        }

        /// <summary>
        /// Gets the <see cref="ComputeContext"/> associated with the <see cref="ComputeEventBase"/>.
        /// </summary>
        /// <value> The <see cref="ComputeContext"/> associated with the <see cref="ComputeEventBase"/>. </value>
        public ComputeContext Context { get; protected set; }

        /// <summary>
        /// Gets the <see cref="ComputeDevice"/> time counter in nanoseconds when the associated command has finished execution.
        /// </summary>
        /// <value> The <see cref="ComputeDevice"/> time counter in nanoseconds when the associated command has finished execution. </value>
        public ulong FinishTime => GetInfo<CLEventHandle, ComputeCommandProfilingInfo, ulong>(Handle, ComputeCommandProfilingInfo.Ended, CL12.GetEventProfilingInfo);

        /// <summary>
        /// Gets the <see cref="ComputeDevice"/> time counter in nanoseconds when the associated command is enqueued in the <see cref="ComputeCommandQueue"/> by the host.
        /// </summary>
        /// <value> The <see cref="ComputeDevice"/> time counter in nanoseconds when the associated command is enqueued in the <see cref="ComputeCommandQueue"/> by the host. </value>
        public ulong EnqueueTime => GetInfo<CLEventHandle, ComputeCommandProfilingInfo, ulong>(Handle, ComputeCommandProfilingInfo.Queued, CL12.GetEventProfilingInfo);

        /// <summary>
        /// Gets the execution status of the associated command.
        /// </summary>
        /// <value> The execution status of the associated command or a negative value if the execution was abnormally terminated. </value>
        public ComputeCommandExecutionStatus Status => (ComputeCommandExecutionStatus)GetInfo<CLEventHandle, ComputeEventInfo, int>(Handle, ComputeEventInfo.ExecutionStatus, CL12.GetEventInfo);

        /// <summary>
        /// Gets the <see cref="ComputeDevice"/> time counter in nanoseconds when the associated command starts execution.
        /// </summary>
        /// <value> The <see cref="ComputeDevice"/> time counter in nanoseconds when the associated command starts execution. </value>
        public ulong StartTime => GetInfo<CLEventHandle, ComputeCommandProfilingInfo, ulong>(Handle, ComputeCommandProfilingInfo.Started, CL12.GetEventProfilingInfo);

        /// <summary>
        /// Gets the <see cref="ComputeDevice"/> time counter in nanoseconds when the associated command that has been enqueued is submitted by the host to the device.
        /// </summary>
        /// <value> The <see cref="ComputeDevice"/> time counter in nanoseconds when the associated command that has been enqueued is submitted by the host to the device. </value>
        public ulong SubmitTime => GetInfo<CLEventHandle, ComputeCommandProfilingInfo, ulong>(Handle, ComputeCommandProfilingInfo.Submitted, CL12.GetEventProfilingInfo);

        /// <summary>
        /// Gets the <see cref="ComputeCommandType"/> associated with the event.
        /// </summary>
        /// <value> The <see cref="ComputeCommandType"/> associated with the event. </value>
        public ComputeCommandType Type { get; protected set; }

        #endregion
        
        #region Protected methods

        /// <summary>
        /// Releases the associated OpenCL object.
        /// </summary>
        /// <param name="manual"> Specifies the operation mode of this method. </param>
        /// <remarks> <paramref name="manual"/> must be <c>true</c> if this method is invoked directly by the application. </remarks>
        protected override void Dispose(bool manual)
        {
            if (Handle.IsValid)
            {
                //Debug.WriteLine("Dispose " + this + " in Thread(" + Thread.CurrentThread.ManagedThreadId + ").", "Information");
                CL12.ReleaseEvent(Handle);
                _handle.Invalidate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void HookNotifier()
        {
            _notifierHooked = true;

            var callback = new ComputeEventCallback(StatusNotify);
            var handle = GCHandle.Alloc(callback);
            var handlePointer = GCHandle.ToIntPtr(handle);

            ComputeErrorCode error = CL11.SetEventCallback(Handle, (int)ComputeCommandExecutionStatus.Complete, callback, handlePointer);
            ComputeException.ThrowOnError(error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="evArgs"></param>
        protected virtual void OnCompleted(object sender, ComputeCommandStatusArgs evArgs)
        {
            //Debug.WriteLine("Complete " + Type + " operation of " + this + ".", "Information");
            CompletedInternal?.Invoke(sender, evArgs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="evArgs"></param>
        protected virtual void OnAborted(object sender, ComputeCommandStatusArgs evArgs)
        {
            //Debug.WriteLine("Abort " + Type + " operation of " + this + ".", "Information");
            AbortedInternal?.Invoke(sender, evArgs);
        }

        #endregion

        #region Private methods

        private void StatusNotify(CLEventHandle eventHandle, int cmdExecStatusOrErr, IntPtr userData)
        {
            lock (_statusLockObject)
            {
                _status = new ComputeCommandStatusArgs(this, (ComputeCommandExecutionStatus)cmdExecStatusOrErr);
                switch (cmdExecStatusOrErr)
                {
                    case (int)ComputeCommandExecutionStatus.Complete:
                        OnCompleted(this, _status);
                        break;
                    default:
                        OnAborted(this, _status);
                        break;
                }
            }
            
            var handle = GCHandle.FromIntPtr(userData);
            handle.Free();
        }

        #endregion

        /// <summary>
        /// Clones the event. Because the event is retained the cloned event as well as the clone have to be disposed
        /// </summary>
        /// <returns>Cloned event</returns>
        public abstract ComputeEventBase Clone();
    }

    /// <summary>
    /// Represents the arguments of a command status change.
    /// </summary>
    internal class ComputeCommandStatusArgs : EventArgs
    {
        /// <summary>
        /// Gets the event associated with the command that had its status changed.
        /// </summary>
        public ComputeEventBase Event { get; }

        /// <summary>
        /// Gets the execution status of the command represented by the event.
        /// </summary>
        /// <remarks> Returns a negative integer if the command was abnormally terminated. </remarks>
        public ComputeCommandExecutionStatus Status { get; }

        /// <summary>
        /// Creates a new <c>ComputeCommandStatusArgs</c> instance.
        /// </summary>
        /// <param name="ev"> The event representing the command that had its status changed. </param>
        /// <param name="status"> The status of the command. </param>
        public ComputeCommandStatusArgs(ComputeEventBase ev, ComputeCommandExecutionStatus status)
        {
            Event = ev;
            Status = status;
        }

        /// <summary>
        /// Creates a new <c>ComputeCommandStatusArgs</c> instance.
        /// </summary>
        /// <param name="ev"> The event of the command that had its status changed. </param>
        /// <param name="status"> The status of the command. </param>
        public ComputeCommandStatusArgs(ComputeEventBase ev, int status)
            : this(ev, (ComputeCommandExecutionStatus)status)
        { }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    internal delegate void ComputeCommandStatusChanged(object sender, ComputeCommandStatusArgs args);
}