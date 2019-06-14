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
    using System.Dynamic;

    /// <summary>
    /// Execution engine for the kernels
    /// </summary>
    /// <seealso cref="System.Dynamic.DynamicObject" />
    public class Executer : System.Dynamic.DynamicObject
    {
        /// <summary>
        /// The compiler
        /// </summary>
        private BaseCompiler Compiler;

        /// <summary>
        /// Initializes a new instance of the <see cref="Executer{T}"/> class.
        /// </summary>
        /// <param name="compiler">The compiler.</param>
        public Executer(BaseCompiler compiler)
        {
            Compiler = compiler;
        }

        /// <summary>
        /// Provides the implementation for operations that invoke a member. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations such as calling a method.
        /// </summary>
        /// <param name="binder">Provides information about the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the statement sampleObject.SampleMethod(100), where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Name returns "SampleMethod". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="args">The arguments that are passed to the object member during the invoke operation. For example, for the statement sampleObject.SampleMethod(100), where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, <paramref name="args[0]" /> is equal to 100.</param>
        /// <param name="result">The result of the member invocation.</param>
        /// <returns>
        ///   <see langword="true" /> if the operation is successful; otherwise, <see langword="false" />. If this method returns <see langword="false" />, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)
        /// </returns>
        /// <exception cref="ExecutionException"></exception>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            try
            {
                if (!Compiler.Kernels.Contains(binder.Name))
                    throw new ExecutionException(string.Format("Method {0} not found!", binder.Name));

                Compiler.Execute(binder.Name, args);
                result = args[args.Length - 1];
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
