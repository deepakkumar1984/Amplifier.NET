using ICSharpCode.Decompiler.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amplifier.OpenCL
{
    internal class KernelBin
    {
        public string SourceCode { get; set; }

        public List<Type> Instances { get; set; }

        public List<KernelFunction> InstanceMethods { get; set; }
    }
}
