# Amplifier.NET 2.0 Release Notes

**Release Date:** December 2024

We are excited to announce Amplifier.NET 2.0, a major release that brings full OpenCL 3.0 support, significant improvements to the C#-to-OpenCL code translator, and a comprehensive test suite demonstrating advanced GPU computing patterns.

---

## Highlights

### OpenCL 3.0 Support

Amplifier.NET now includes complete bindings for OpenCL 2.0, 2.1, 2.2, and 3.0, enabling access to the latest GPU computing features:

- **Shared Virtual Memory (SVM)** — Share memory pointers directly between host and device
- **Pipes** — First-in-first-out data structures for kernel-to-kernel communication
- **Device-side Enqueue** — Kernels can enqueue other kernels dynamically
- **SPIR-V Support** — Load pre-compiled SPIR-V shader binaries
- **Subgroups** — Fine-grained work-item synchronization within a subgroup
- **Program Specialization** — Set compile-time constants for kernel optimization

### New OpenCL Binding Files

| File | OpenCL Version | Key Features |
|------|----------------|--------------|
| `CL20.cs` | OpenCL 2.0 | SVM, Pipes, Atomics, `CreateCommandQueueWithProperties` |
| `CL21.cs` | OpenCL 2.1 | `CreateProgramWithIL`, `CloneKernel`, Device/Host Timers |
| `CL22.cs` | OpenCL 2.2 | Program Release Callbacks, Specialization Constants |
| `CL30.cs` | OpenCL 3.0 | `CreateBufferWithProperties`, `CreateImageWithProperties`, Context Destructors |

### Expanded Enumerations

The `Enums.cs` file now includes 60+ new constants covering:

- New error codes (`InvalidPipeSize`, `InvalidDeviceQueue`, `InvalidSpecId`, etc.)
- SVM memory flags and capabilities
- Pipe information queries
- Kernel execution info and subgroup queries
- Atomic and device-enqueue capabilities

---

## Code Translator Improvements

The C#-to-OpenCL translator (`CodeTranslator.cs`) has been significantly enhanced to handle modern C# syntax and edge cases:

### Fixed Issues

| Issue | Before | After |
|-------|--------|-------|
| File-scoped namespaces (C# 10) | `namespace Foo.Bar;` caused malformed output | Properly stripped from generated code |
| Struct spacing | `globalstruct` | `global struct` |
| StructLayout attributes | `[StructLayout(...)]` in output | Removed from OpenCL code |
| Double semicolons | `;;` | `;` |
| Float constants | `float.MinValue` / `float.MaxValue` | `-3.4028234e+38f` / `3.4028234e+38f` |
| Integer float suffix | `2f` (invalid in OpenCL C) | `2` |
| Access modifiers | Partial removal issues | Clean removal with word boundaries |

### Improved Regex Patterns

- Access modifiers (`public`, `private`, etc.) now removed using `\b` word boundaries to prevent partial matches
- Fixed array syntax (`fixed float Data[10]`) properly converted

---

## Decompiler Fix

Fixed a `NullReferenceException` in `DynamicCallSiteTransform.cs` (line 531) that occurred when decompiling certain dynamic call sites. Added null check for `callSiteCacheField.ReturnType`.

---

## Comprehensive Test Suite

Version 2.0 includes a new comprehensive OpenCL 3.0 test example demonstrating 35+ kernels across 7 categories:

### Test Categories

1. **Vector Operations**
   - `VectorAdd`, `VectorFMA`, `VectorMagnitude`, `VectorNormalize`
   - `VectorDot`, `VectorCross`

2. **Matrix Operations**
   - `MatrixVectorMul`, `MatrixMulNxN`, `MatrixTranspose`

3. **Particle Physics Simulation**
   - `ParticleIntegrate`, `ParticleApplyGravity`
   - `NBodyForces`, `ParticleBoundsCheck`

4. **Image Processing**
   - `RgbToGrayscale`, `BrightnessContrast`, `GammaCorrection`
   - `Convolve3x3`, `BoxBlur`

5. **Complex Number Operations**
   - `ComplexMul`, `ComplexMagnitude`, `ComplexPhase`, `PolarToComplex`

6. **Neural Network Operations**
   - `DenseForward`, `ReLU`, `LeakyReLU`, `Sigmoid`, `Tanh`
   - `SoftmaxNormalize`, `BatchNormForward`

7. **Reduction Operations**
   - `ReduceSum`, `ReduceMax`, `ReduceMin`, `ReduceSumSquares`

8. **Utility Kernels**
   - `FillFloat`, `FillInt`, `Scale`, `Abs`, `Clamp`, `Copy`, `Iota`

### New Struct Types

Eight new struct types for testing complex data scenarios:

- `Float3` — 3D vector (x, y, z)
- `Matrix4x4F` — 4x4 transformation matrix
- `Particle` — Physics simulation particle
- `ColorRGBA` — RGBA color channels
- `ComplexF` — Complex number (real, imaginary)
- `HistogramBin` — Image processing histogram
- `BoundingBox` — Spatial bounds
- `NeuronWeights` — Neural network weights

---

## Breaking Changes

None. Version 2.0 is fully backward compatible with 1.x code.

---

## Dependencies Updated

| Package | Previous | New |
|---------|----------|-----|
| ICSharpCode.Decompiler | 7.x | 9.1.0.7988 |
| System.Collections.Immutable | — | 10.0.1 |
| System.Reflection.Metadata | — | 10.0.1 |

---

## Known Limitations

- OpenCL 2.0+ features (SVM, Pipes, etc.) require driver support; check `ComputeDevice` capabilities before use
- The `cl_khr_fp64` extension warning appears on devices without double-precision support (non-breaking)
- Some integrated GPUs may have limited performance with complex kernels

---

## Migration Guide

### From 1.x to 2.0

No code changes required. Simply update the NuGet package:

```bash
dotnet add package Amplifier.NET --version 2.0.0
```

### Using New OpenCL 3.0 Features

Check device capabilities before using optional features:

```csharp
var device = compiler.Devices[0];
// Check for SVM support, pipe support, etc. via device properties
```

---

## Contributors

- Deepak Battini — Core library, OpenCL 3.0 bindings
- Community contributors — Bug reports and testing

---

## What's Next

- CUDA backend support (planned)
- Automatic kernel optimization hints
- Improved error messages with C# line number mapping
- Metal backend for macOS (exploring)

---

**Thank you for using Amplifier.NET!**

For questions or issues, please visit: https://github.com/deepakkumar1984/Amplifier.NET/issues
