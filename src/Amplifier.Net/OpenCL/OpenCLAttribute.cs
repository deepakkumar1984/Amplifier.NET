using System;
using System.Collections.Generic;
using System.Text;
using Amplifier.Decompiler.TypeSystem;

namespace Amplifier.OpenCL
{
    [System.AttributeUsage(System.AttributeTargets.Parameter)]
    public class GlobalAttribute : System.Attribute
    {
    }

    [System.AttributeUsage(System.AttributeTargets.Parameter)]
    public class InputAttribute : System.Attribute
    {
    }

    [System.AttributeUsage(System.AttributeTargets.Parameter)]
    public class OutputAttribute : System.Attribute
    {
    }

    [System.AttributeUsage(System.AttributeTargets.Parameter)]
    public class StructAttribute : System.Attribute
    {
    }

    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class OpenCLKernelAttribute : System.Attribute
    {
        
    }

    [System.AttributeUsage(System.AttributeTargets.Struct)]
    public class OpenCLStructAttribute : System.Attribute
    {

    }
}
