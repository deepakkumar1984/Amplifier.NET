using Amplifier;
using System;
using System.IO;
using System.Text;

namespace DarknetOpencl
{
    public class amp
    {
        private static OpenCLCompiler compiler = new OpenCLCompiler();

        private static dynamic exec;

        public static dynamic Ops
        {
            get
            {
                if(exec == null)
                    LoadKernels();

                return exec;
            }
        }

        public static void LoadKernels()
         {
            compiler.UseDevice(0);
            var clfiles = new DirectoryInfo("./kernels").GetFiles();
            StringBuilder sb = new StringBuilder();
            foreach (var f in clfiles)
            {
                sb.AppendLine(File.ReadAllText(f.FullName));
                sb.AppendLine();
            }

            compiler.CompileKernel(sb.ToString());
            exec = compiler.GetExec();
        }
    }
}
