using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
namespace Amplifier
{
    /// <summary>
    /// Extensions to the Assembly class for handling related Amplifier Modules
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Determines whether the assembly has a Amplifier module embedded.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>
        ///   <c>true</c> if it has Amplifier module; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAmplifierModule(this Assembly assembly)
        {
            return AmplifierModule.HasAmplifierModule(assembly);
        }

        /// <summary>
        /// Gets the embedded Amplifier module from the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Amplifier module.</returns>
        public static AmplifierModule GetAmplifierModule(this Assembly assembly)
        {
            return AmplifierModule.GetFromAssembly(assembly);
        }

        /// <summary>
        /// Cudafies the assembly producing a *.cdfy file with same name as assembly. Architecture is 2.0.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="arch">The architecture.</param>
        /// <returns>Output messages of the Amplifiercl.exe process.</returns>
        public static string Amplifier(this Assembly assembly, eArchitecture arch = eArchitecture.sm_20)
        {
            string messages;
            if(!TryAmplifier(assembly, out messages, arch))
                throw new AmplifierCompileException(AmplifierCompileException.csCOMPILATION_ERROR_X, messages);
            return messages;
        }

        /// <summary>
        /// Tries Amplifiering the assembly producing a *.cdfy file with same name as assembly. Architecture is 2.0.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="arch">The architecture.</param>
        /// <returns>
        ///   <c>true</c> if successful; otherwise, <c>false</c>.
        /// </returns>
        public static bool TryAmplifier(this Assembly assembly, eArchitecture arch = eArchitecture.sm_20)
        {
            string messages;
            return TryAmplifier(assembly, out messages, arch);

        }
        /// <summary>
        /// Tries Amplifiering the assembly producing a *.cdfy file with same name as assembly. Architecture is 2.0.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="messages">Output messages of the Amplifiercl.exe process.</param>
        /// <param name="arch">The architecture.</param>
        /// <returns>
        ///   <c>true</c> if successful; otherwise, <c>false</c>.
        /// </returns>
        public static bool TryAmplifier(this Assembly assembly, out string messages, eArchitecture arch = eArchitecture.sm_20)
        {
            var assemblyName = assembly.Location;
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName = "Amplifiercl.exe";
            StringBuilder sb = new StringBuilder();
            process.StartInfo.Arguments = string.Format("{0} -arch={1} -cdfy", assemblyName, arch);
            process.Start();
            while (!process.HasExited)
                System.Threading.Thread.Sleep(10);
            if (process.ExitCode != 0)
            {
                messages = process.StandardError.ReadToEnd() + "\r\n";
                messages += process.StandardOutput.ReadToEnd();
                return false;
            }
            else
            {
                messages = process.StandardOutput.ReadToEnd();
                return true;
            }
        }
    }
}
