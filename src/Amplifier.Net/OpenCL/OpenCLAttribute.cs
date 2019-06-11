using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.Decompiler.TypeSystem;

namespace Amplifier.OpenCL
{
    [System.AttributeUsage(System.AttributeTargets.Parameter)]
    public class GlobalAttribute : System.Attribute
    {
    }

    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class OpenCLKernelAttribute : System.Attribute
    {
        
    }
}
