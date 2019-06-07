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
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Catch the execution exception
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class ExecutionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionException"/> class.
        /// </summary>
        public ExecutionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ExecutionException(string message) : base(message)
        {
            
        }

        public ExecutionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExecutionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}