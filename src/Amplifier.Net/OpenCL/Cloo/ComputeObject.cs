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

namespace Amplifier.OpenCL.Cloo
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Represents an OpenCL object.
    /// </summary>
    /// <remarks> An OpenCL object is an object that is identified by its handle in the OpenCL environment. </remarks>
    internal abstract class ComputeObject : IEquatable<ComputeObject>
    {
        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IntPtr _handle;

        #endregion

        #region Public methods

        /// <summary>
        /// Checks if two <c>object</c>s are equal. These <c>object</c>s must be cast from <see cref="ComputeObject"/>s.
        /// </summary>
        /// <param name="objA"> The first <c>object</c> to compare. </param>
        /// <param name="objB"> The second <c>object</c> to compare. </param>
        /// <returns> <c>true</c> if the <c>object</c>s are equal otherwise <c>false</c>. </returns>
        public new static bool Equals(object objA, object objB)
        {
            if (objA == objB) return true;
            if (objA == null || objB == null) return false;
            return objA.Equals(objB);
        }

        /// <summary>
        /// Checks if the <see cref="ComputeObject"/> is equal to a specified <see cref="ComputeObject"/> cast to an <c>object</c>.
        /// </summary>
        /// <param name="obj"> The specified <c>object</c> to compare the <see cref="ComputeObject"/> with. </param>
        /// <returns> <c>true</c> if the <see cref="ComputeObject"/> is equal with <paramref name="obj"/> otherwise <c>false</c>. </returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is ComputeObject)) return false;
            return Equals((ComputeObject) obj);
        }

        /// <summary>
        /// Checks if the <see cref="ComputeObject"/> is equal to a specified <see cref="ComputeObject"/>.
        /// </summary>
        /// <param name="obj"> The specified <see cref="ComputeObject"/> to compare the <see cref="ComputeObject"/> with. </param>
        /// <returns> <c>true</c> if the <see cref="ComputeObject"/> is equal with <paramref name="obj"/> otherwise <c>false</c>. </returns>
        public bool Equals(ComputeObject obj)
        {
            if (obj == null) return false;
            if (!_handle.Equals(obj._handle)) return false;
            return true;
        }

        /// <summary>
        /// Gets the hash code of the <see cref="ComputeObject"/>.
        /// </summary>
        /// <returns> The hash code of the <see cref="ComputeObject"/>. </returns>
        public override int GetHashCode()
        {
            return _handle.GetHashCode();
        }

        /// <summary>
        /// Gets the string representation of the <see cref="ComputeObject"/>.
        /// </summary>
        /// <returns> The string representation of the <see cref="ComputeObject"/>. </returns>
        public override string ToString()
        {
            return GetType().Name + "(" + _handle.ToString() + ")";
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="THandleType"></typeparam>
        /// <typeparam name="TInfoType"></typeparam>
        /// <typeparam name="TQueriedType"></typeparam>
        /// <param name="handle"></param>
        /// <param name="paramName"></param>
        /// <param name="getInfoDelegate"></param>
        /// <returns></returns>
        protected static TQueriedType[] GetArrayInfo<THandleType, TInfoType, TQueriedType>
            (THandleType handle, TInfoType paramName, GetInfoDelegate<THandleType, TInfoType> getInfoDelegate)
        {
            var error = getInfoDelegate(handle, paramName, IntPtr.Zero, IntPtr.Zero, out var bufferSizeRet);
            ComputeException.ThrowOnError(error);
            var buffer = new TQueriedType[bufferSizeRet.ToInt64() / Marshal.SizeOf(typeof(TQueriedType))];
            GCHandle gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                error = getInfoDelegate(handle, paramName, bufferSizeRet, gcHandle.AddrOfPinnedObject(), out bufferSizeRet);
                ComputeException.ThrowOnError(error);
            }
            finally
            {
                gcHandle.Free();
            }
            return buffer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMainHandleType"></typeparam>
        /// <typeparam name="TSecondHandleType"></typeparam>
        /// <typeparam name="TInfoType"></typeparam>
        /// <typeparam name="TQueriedType"></typeparam>
        /// <param name="mainHandle"></param>
        /// <param name="secondHandle"></param>
        /// <param name="paramName"></param>
        /// <param name="getInfoDelegate"></param>
        /// <returns></returns>
        protected static TQueriedType[] GetArrayInfo<TMainHandleType, TSecondHandleType, TInfoType, TQueriedType>
            (TMainHandleType mainHandle, TSecondHandleType secondHandle, TInfoType paramName, GetInfoDelegateEx<TMainHandleType, TSecondHandleType, TInfoType> getInfoDelegate)
        {
            var error = getInfoDelegate(mainHandle, secondHandle, paramName, IntPtr.Zero, IntPtr.Zero, out var bufferSizeRet);
            ComputeException.ThrowOnError(error);
            var buffer = new TQueriedType[bufferSizeRet.ToInt64() / Marshal.SizeOf(typeof(TQueriedType))];
            GCHandle gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                error = getInfoDelegate(mainHandle, secondHandle, paramName, bufferSizeRet, gcHandle.AddrOfPinnedObject(), out bufferSizeRet);
                ComputeException.ThrowOnError(error);
            }
            finally
            {
                gcHandle.Free();
            }
            return buffer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="THandleType"></typeparam>
        /// <typeparam name="TInfoType"></typeparam>
        /// <param name="handle"></param>
        /// <param name="paramName"></param>
        /// <param name="getInfoDelegate"></param>
        /// <returns></returns>
        protected static bool GetBoolInfo<THandleType, TInfoType>
            (THandleType handle, TInfoType paramName, GetInfoDelegate<THandleType, TInfoType> getInfoDelegate)
        {
            int result = GetInfo<THandleType, TInfoType, int>(handle, paramName, getInfoDelegate);
            return result == (int)ComputeBoolean.True;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="THandleType"></typeparam>
        /// <typeparam name="TInfoType"></typeparam>
        /// <typeparam name="TQueriedType"></typeparam>
        /// <param name="handle"></param>
        /// <param name="paramName"></param>
        /// <param name="getInfoDelegate"></param>
        /// <returns></returns>
        protected static TQueriedType GetInfo<THandleType, TInfoType, TQueriedType>
            (THandleType handle, TInfoType paramName, GetInfoDelegate<THandleType, TInfoType> getInfoDelegate) 
            where TQueriedType : struct
        {
            TQueriedType result = new TQueriedType();
            GCHandle gcHandle = GCHandle.Alloc(result, GCHandleType.Pinned);
            try
            {
                ComputeErrorCode error = getInfoDelegate(handle, paramName, (IntPtr)Marshal.SizeOf(result), gcHandle.AddrOfPinnedObject(), out _);
                ComputeException.ThrowOnError(error);
            }
            finally
            {
                result = (TQueriedType)gcHandle.Target;
                gcHandle.Free();
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMainHandleType"></typeparam>
        /// <typeparam name="TSecondHandleType"></typeparam>
        /// <typeparam name="TInfoType"></typeparam>
        /// <typeparam name="TQueriedType"></typeparam>
        /// <param name="mainHandle"></param>
        /// <param name="secondHandle"></param>
        /// <param name="paramName"></param>
        /// <param name="getInfoDelegate"></param>
        /// <returns></returns>
        protected static TQueriedType GetInfo<TMainHandleType, TSecondHandleType, TInfoType, TQueriedType>
            (TMainHandleType mainHandle, TSecondHandleType secondHandle, TInfoType paramName, GetInfoDelegateEx<TMainHandleType, TSecondHandleType, TInfoType> getInfoDelegate)
            where TQueriedType : struct
        {
            TQueriedType result = new TQueriedType();
            GCHandle gcHandle = GCHandle.Alloc(result, GCHandleType.Pinned);
            try
            {
                ComputeErrorCode error = getInfoDelegate(mainHandle, secondHandle, paramName, new IntPtr(Marshal.SizeOf(result)), gcHandle.AddrOfPinnedObject(), out _);
                ComputeException.ThrowOnError(error);
            }
            finally
            {
                result = (TQueriedType)gcHandle.Target;
                gcHandle.Free();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="THandleType"></typeparam>
        /// <typeparam name="TInfoType"></typeparam>
        /// <param name="handle"></param>
        /// <param name="paramName"></param>
        /// <param name="getInfoDelegate"></param>
        /// <returns></returns>
        protected static string GetStringInfo<THandleType, TInfoType>
            (THandleType handle, TInfoType paramName, GetInfoDelegate<THandleType, TInfoType> getInfoDelegate)
        {
            byte[] buffer = GetArrayInfo<THandleType, TInfoType, byte>(handle, paramName, getInfoDelegate);
            char[] chars = Encoding.ASCII.GetChars(buffer, 0, buffer.Length);
            return new string(chars).TrimEnd('\0');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMainHandleType"></typeparam>
        /// <typeparam name="TSecondHandleType"></typeparam>
        /// <typeparam name="TInfoType"></typeparam>
        /// <param name="mainHandle"></param>
        /// <param name="secondHandle"></param>
        /// <param name="paramName"></param>
        /// <param name="getInfoDelegate"></param>
        /// <returns></returns>
        protected static string GetStringInfo<TMainHandleType, TSecondHandleType, TInfoType>
            (TMainHandleType mainHandle, TSecondHandleType secondHandle, TInfoType paramName, GetInfoDelegateEx<TMainHandleType, TSecondHandleType, TInfoType> getInfoDelegate)
        {
            byte[] buffer = GetArrayInfo<TMainHandleType, TSecondHandleType, TInfoType, byte>(mainHandle, secondHandle, paramName, getInfoDelegate);
            char[] chars = Encoding.ASCII.GetChars(buffer, 0, buffer.Length);
            return new string(chars).TrimEnd('\0');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        protected void SetID(IntPtr id)
        {
            _handle = id;
        }

        #endregion

        #region Delegates

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="THandleType"></typeparam>
        /// <typeparam name="TInfoType"></typeparam>
        /// <param name="objectHandle"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValueSize"></param>
        /// <param name="paramValue"></param>
        /// <param name="paramValueSizeRet"></param>
        /// <returns></returns>
        protected delegate ComputeErrorCode GetInfoDelegate<THandleType, TInfoType>
            (
                THandleType objectHandle,
                TInfoType paramName,
                IntPtr paramValueSize,
                IntPtr paramValue,
                out IntPtr paramValueSizeRet
            );

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMainHandleType"></typeparam>
        /// <typeparam name="TSecondHandleType"></typeparam>
        /// <typeparam name="TInfoType"></typeparam>
        /// <param name="mainObjectHandle"></param>
        /// <param name="secondaryObjectHandle"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValueSize"></param>
        /// <param name="paramValue"></param>
        /// <param name="paramValueSizeRet"></param>
        /// <returns></returns>
        protected delegate ComputeErrorCode GetInfoDelegateEx<TMainHandleType, TSecondHandleType, TInfoType>
            (
                TMainHandleType mainObjectHandle,
                TSecondHandleType secondaryObjectHandle,
                TInfoType paramName,
                IntPtr paramValueSize,
                IntPtr paramValue,
                out IntPtr paramValueSizeRet
            );

        #endregion
    }
}