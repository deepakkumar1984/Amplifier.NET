using System;
using System.Collections.Generic;
using System.Text;

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
