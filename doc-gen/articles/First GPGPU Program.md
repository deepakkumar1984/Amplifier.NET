# First GPGPU Program

GPU Programing is not that scary, only if you understood the basic term called kernel and know how to modularize your code to simple funtions.

## What is a kernel?
Code that gets executed on a device is called a kernel in OpenCL. 
The kernels are written in a C dialect, which is mostly straightforward C with a lot of built-in functions and additional data types. 
For instance, 4-component vectors are a built-in type just as integers. For example, we’ll be implementing a simple SAXPY kernel.

SAXPY stands for “Single-Precision A·X Plus Y”.  It is a function in the standard Basic Linear Algebra Subroutines (BLAS)library. 
SAXPY is a combination of scalar multiplication and vector addition, and it’s very simple: it takes as input two vectors of 32-bit floats X and Y with N elements each, and a scalar value A. 
It multiplies each element X[i] by A and adds the result to Y[i]

* Create a new C# .NET Console project.
* Go to Nuget and add reference of Amplifier.NET
* Create a new class file "SampleKernel.cs" which will hold the kernel function. You can create as many class files.
* Edit SampleKernel.cs and inherit the class with OpenCLFunctions

```csharp
class SimpleKernels : OpenCLFunctions
```

* Now in the class body add your first kernel SAXPY.

```csharp
[OpenCLKernel]
void SAXPY([Global]float[] x, [Global] float[] y, float a)
{
    int i = get_global_id(0);

    y[i] += a * x[i];
}
```

The **[OpenCLKernel]** tells the compiler that its a OpenCL kernel and need to convert this to C99 format which will be executed in the CPU/GPU hardware.

The **[Global]** indicates that this is global memory or simply memory allocated on the device. OpenCL supports different address spaces; in this sample, we’ll be using only global memory. You wonder probably where the SAXPY loop went; this is one of the key design decisions behind OpenCL, so let’s understand first how the code is executed.

In order to identify the kernel instance, the runtime environment provides an id. Inside the kernel, we use 
**get_global_id** which returns the id of the current work item in the first dimension. We will start as many instances as there are elements in our vector, and each work item will process exactly one entry.

