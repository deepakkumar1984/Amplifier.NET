# Amplifier.NET

**Write C#. Run on GPU.**

Amplifier.NET is a GPU computing library that lets .NET developers harness the power of parallel processing on Intel, NVIDIA, and AMD hardware—without writing a single line of C or OpenCL kernel code.

[![NuGet](https://img.shields.io/nuget/v/Amplifier.NET.svg)](https://www.nuget.org/packages/Amplifier.NET/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Why Amplifier.NET?

Modern applications demand massive computational power for machine learning, scientific simulations, image processing, and financial modeling. GPUs can process thousands of operations in parallel, but traditionally require specialized knowledge of OpenCL, CUDA, or shader languages.

**Amplifier.NET bridges this gap.** Write your compute kernels in familiar C# syntax, and let Amplifier handle the translation to OpenCL, device management, and memory transfers. Your code runs on any OpenCL-compatible device—from integrated Intel graphics to high-end discrete GPUs.

## Features

- **Pure C# Kernels** — Write GPU compute functions using standard C# syntax
- **Automatic Translation** — C# code is decompiled and translated to OpenCL C99 at runtime
- **OpenCL 3.0 Support** — Full support for the latest OpenCL specification including optional features
- **Cross-Platform** — Works on Windows, Linux, and macOS with any OpenCL driver
- **Multi-Device** — Enumerate and target specific compute devices (CPU, GPU, FPGA)
- **Struct Support** — Pass custom structs between host and device
- **XArray System** — Advanced array types with shape manipulation and automatic memory management

## Quick Start

### Installation

```bash
dotnet add package Amplifier.NET
```

### Your First Kernel

Define a kernel class that extends `OpenCLFunctions`:

```csharp
using Amplifier.OpenCL;

public class MyKernels : OpenCLFunctions
{
    [OpenCLKernel]
    void VectorAdd([Global] float[] a, [Global] float[] b, [Global] float[] result)
    {
        int i = get_global_id(0);
        result[i] = a[i] + b[i];
    }

    [OpenCLKernel]
    void Scale([Global] float[] data, float factor)
    {
        int i = get_global_id(0);
        data[i] *= factor;
    }
}
```

### Execute on GPU

```csharp
using Amplifier;

// Initialize the compiler and select a device
var compiler = new OpenCLCompiler();

Console.WriteLine("Available Devices:");
foreach (var device in compiler.Devices)
    Console.WriteLine($"  {device}");

compiler.UseDevice(0);  // Select first device
compiler.CompileKernel(typeof(MyKernels));

// Prepare data
float[] a = { 1, 2, 3, 4, 5 };
float[] b = { 10, 20, 30, 40, 50 };
float[] result = new float[5];

// Execute kernels
var exec = compiler.GetExec();
exec.VectorAdd(a, b, result);

Console.WriteLine(string.Join(", ", result));
// Output: 11, 22, 33, 44, 55
```

## Working with Structs

Amplifier supports custom structs for complex data types:

```csharp
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct Particle
{
    public float X, Y, Z;
    public float VelocityX, VelocityY, VelocityZ;
    public float Mass;
    public int Active;
}

public class PhysicsKernels : OpenCLFunctions
{
    [OpenCLKernel]
    void Integrate([Global][Struct] Particle[] particles, float deltaTime)
    {
        int i = get_global_id(0);
        if (particles[i].Active == 1)
        {
            particles[i].X += particles[i].VelocityX * deltaTime;
            particles[i].Y += particles[i].VelocityY * deltaTime;
            particles[i].Z += particles[i].VelocityZ * deltaTime;
        }
    }
}

// Compile with struct types
compiler.CompileKernel(typeof(PhysicsKernels), typeof(Particle));
```

## Advanced: XArray for Scientific Computing

The `XArray` system provides NumPy-like array operations with automatic GPU memory management:

```csharp
int M = 1024, N = 1024, K = 512;

var a = new InArray(new long[] { M, K }, DType.Float32);
var b = new InArray(new long[] { K, N }, DType.Float32);
var c = new OutArray(new long[] { M, N }, DType.Float32);

exec.Fill(a, 1.0f);
exec.Fill(b, 2.0f);
exec.MatMul(M, N, K, a, b, c);

float[] result = c.ToArray();
```

## OpenCL Built-in Functions

Kernels have access to all standard OpenCL functions:

| Category | Functions |
|----------|-----------|
| **Work-item** | `get_global_id`, `get_local_id`, `get_group_id`, `get_global_size` |
| **Math** | `sin`, `cos`, `tan`, `exp`, `log`, `pow`, `sqrt`, `fabs`, `fmin`, `fmax` |
| **Geometric** | `dot`, `cross`, `length`, `normalize`, `distance` |
| **Integer** | `abs`, `clamp`, `min`, `max` |
| **Synchronization** | `barrier`, `mem_fence` |

## Performance Tips

1. **Minimize Host-Device Transfers** — Keep data on the GPU between kernel calls
2. **Use Appropriate Work Sizes** — Match your problem dimensions to the kernel's global size
3. **Prefer Float over Double** — Many GPUs have limited double-precision performance
4. **Coalesce Memory Access** — Access contiguous memory addresses for best throughput
5. **Avoid Branching** — Divergent control flow reduces GPU efficiency

## Supported Platforms

| Platform | Status |
|----------|--------|
| Windows (x64) | Fully Supported |
| Linux (x64) | Fully Supported |
| macOS | Supported (Intel/AMD GPUs) |

**Tested Hardware:**
- Intel Iris Xe, UHD Graphics
- NVIDIA GTX/RTX series
- AMD Radeon RX series

## Documentation

- **Getting Started**: [Articles](https://deepakkumar1984.github.io/Amplifier.NET/articles/intro.html)
- **API Reference**: [Documentation](https://deepakkumar1984.github.io/Amplifier.NET/api/Amplifier.html)
- **Examples**: See the [examples](examples/) directory

## Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch
3. Submit a pull request

For bugs or feature requests, please [open an issue](https://github.com/deepakkumar1984/Amplifier.NET/issues).

## License

Amplifier.NET is released under the [MIT License](LICENSE).

---

**Amplifier.NET** — Unlock the power of GPU computing in pure C#.
