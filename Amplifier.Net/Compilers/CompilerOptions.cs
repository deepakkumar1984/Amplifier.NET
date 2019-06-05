/*
Amplifier.NET - LGPL 2.1 License
Please consider purchasing a commerical license - it helps development, frees you from LGPL restrictions
and provides you with support.  Thank you!
Copyright (C) 2011 Hybrid DSP Systems
http://www.hybriddsp.com

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace Amplifier.Compilers
{
    /// <summary>
    /// Abstract base class for options.
    /// </summary>
    public abstract class CompilerOptions : ICompilerOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompilerOptions"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="compilerPath">The compiler path.</param>
        /// <param name="includeDirectory">The include directory.</param>
        /// <param name="compilerVersion">Compiler/toolkit version (e.g. CUDA V5.0).</param>
        protected CompilerOptions(string name, string compilerPath, string includeDirectory, Version compilerVersion, ePlatform platform )
        {
            _options = new List<string>();
            _sources = new List<string>();
            _outputs = new List<string>();
            Name = name;
            CompilerPath = compilerPath;
            //Directory = string.Empty;
            Include = includeDirectory;
            GenerateDebugInfo = false;
            Version = compilerVersion;
            Platform = platform;
            CompileMode = eAmplifierCompileMode.Default;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the compiler path.
        /// </summary>
        /// <value>
        /// The compiler path.
        /// </value>
        public string CompilerPath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the platform.
        /// </summary>
        public ePlatform Platform { get; protected set; }

        /// <summary>
        /// Gets the architecture.
        /// </summary>
        public eArchitecture Architecture { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether to generate debug info.
        /// </summary>
        /// <value>
        ///   <c>true</c> if generate debug info; otherwise, <c>false</c>.
        /// </value>
        public bool GenerateDebugInfo { get; set; }

        //public string Directory
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Gets or sets the include path.
        /// </summary>
        /// <value>
        /// The include path.
        /// </value>
        public string Include
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the version of the compiler.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public Version Version
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets the name of the compiler file.
        /// </summary>
        /// <returns></returns>
        public string GetFileName()
        {
            string filename = CompilerPath;
            return filename;
        }

        private List<string> _options;

        /// <summary>
        /// Gets the options.
        /// </summary>
        public IEnumerable<string> Options
        {
            get { return _options; }
        }

        /// <summary>
        /// Adds an option.
        /// </summary>
        /// <param name="opt">The opt.</param>
        public void AddOption(string opt)
        {
            _options.Add(opt);
        }

        /// <summary>
        /// Clears the options.
        /// </summary>
        public void ClearOptions()
        {
            _options.Clear();
        }

        private List<string> _sources;

        /// <summary>
        /// Gets the sources.
        /// </summary>
        public IEnumerable<string> Sources
        {
            get { return _sources; }
        }

        /// <summary>
        /// Adds a source.
        /// </summary>
        /// <param name="src">The source file.</param>
        public void AddSource(string src)
        {
            _sources.Add(src);
        }

        /// <summary>
        /// Clears the sources.
        /// </summary>
        public void ClearSources()
        {
            _sources.Clear();
        }

        private List<string> _outputs;

        /// <summary>
        /// Gets the outputs.
        /// </summary>
        public IEnumerable<string> Outputs
        {
            get { return _outputs; }
        }

        /// <summary>
        /// Adds an output.
        /// </summary>
        /// <param name="output">The output file.</param>
        public void AddOutput(string output)
        {
            _outputs.Add(output);
        }

        /// <summary>
        /// Clears the outputs.
        /// </summary>
        public void ClearOutputs()
        {
            _outputs.Clear();
        }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <returns></returns>
        public virtual string GetArguments()
        {
            throw new NotImplementedException();
        }
#warning Try calling nvcc.exe
        /// <summary>
        /// Checks if include directory exists.
        /// </summary>
        /// <returns>True if exists, else false.</returns>
        public virtual bool TryTest()
        {
            if (!string.IsNullOrEmpty(Include))
                if (!System.IO.Directory.Exists(Include))
                    return false;
            //if (!string.IsNullOrEmpty(Directory))
            //    if (!System.IO.Directory.Exists(Directory))
            //        return false;
            //string path = CompilerPath);
            //if (!string.IsNullOrEmpty(path))
            //    if (!System.IO.File.Exists(path))
            //        return false;
            return true;
        }


        /// <summary>
        /// Checks if include directory exists.
        /// </summary>
        /// <exception cref="AmplifierCompileException">File or directory not found.</exception>
        public virtual void Test()
        {
            try
            {
                if (!string.IsNullOrEmpty(Include))
                    if (!System.IO.Directory.Exists(Include))
                        throw new DirectoryNotFoundException(Include);
                //if (!string.IsNullOrEmpty(Directory))
                //    if (!System.IO.Directory.Exists(Directory))
                //        throw new DirectoryNotFoundException(Directory);
                //string path = Path.Combine(Directory, CompilerPath);
                //if (!string.IsNullOrEmpty(path))
                //    if (!System.IO.File.Exists(path))
                //        throw new FileNotFoundException(path);
            }
            catch (FileNotFoundException ex)
            {
                throw new AmplifierCompileException(AmplifierCompileException.csCOMPILATION_ERROR_X, ex.Message);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new AmplifierCompileException(AmplifierCompileException.csCOMPILATION_ERROR_X, ex.Message);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Can edit.
        /// </summary>
        protected bool _canEdit = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance can edit.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can edit; otherwise, <c>false</c>.
        /// </value>
        public virtual bool CanEdit
        {
            get { return _canEdit; }
            set { _canEdit = value; }
        }

        /// <summary>
        /// Gets the summary.
        /// </summary>
        /// <returns></returns>
        public string GetSummary()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var s in Options)
                sb.Append(string.Format("{0}, ", s));
            sb.Append(string.Format(" Platform: {0},", Platform));
            return sb.ToString();
        }

        /// <summary>
        /// Gets or sets the time out for compilation.
        /// </summary>
        /// <value>
        /// The time out in milliseconds.
        /// </value>
        public int TimeOut { get; set; }

        /// <summary>
        /// Gets a flag indicating whether the compiler generates binary.
        /// </summary>
        public eAmplifierCompileMode CompileMode { get; set; }
    }
}
