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

namespace Amplifier.Extensions
{
    using System;
    using System.Linq;

    /// <summary>
    /// .NET array extension class to add extension methods for amplifier
    /// </summary>
    public static class ArrayExtension
    {
        /// <summary>
        /// For loop for array which will invoke Amplifier code.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="compiler">The compiler.</param>
        /// <param name="kernelName">Name of the kernel.</param>
        /// <param name="args">The arguments.</param>
        public static void AmplifyFor(this Array x, BaseCompiler compiler, string kernelName, params object[] args)
        {
            var arguments = args.ToList();
            arguments.Insert(0, x);
            compiler.Execute(kernelName, arguments.ToArray());
        }
    }
}
