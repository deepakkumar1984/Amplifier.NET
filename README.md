# Amplifier.NET
Amplifier allows .NET developers to easily run complex applications with intensive mathematical computation on Intel CPU/GPU, NVIDIA, AMD without writing any additional C kernel code. Write your function in .NET and Amplifier will take care of running it on your favorite hardware.

Below is the sample Kernel you can write in CSharp

```csharp
[OpenCLKernel]
void add_float([Global]float[] a, [Global] float[] b, [Global]float[] r)
{
    int i = get_global_id(0);
    b[i] = 0.5f * b[i];
    r[i] = a[i] + b[i];
}

[OpenCLKernel]
void Fill([Global] float[] x, float value)
{
    int i = get_global_id(0);

    x[i] = value;
}
```

Now this kernel will be converted to C99 format which is specific instruction for OpenCL. Let's do some magic to execute the kernel using OpenCL

1. Create an instance of OpenCL compiler. You can list all the available devices.
```csharp
var compiler = new OpenCLCompiler();
Console.WriteLine("\nList Devices----");
foreach (var item in compiler.Devices)
{
    Console.WriteLine(item);
}
```

2. Select a device by id and load the Sample kernel created.
```csharp
compiler.UseDevice(0);
compiler.CompileKernel(typeof(SimpleKernels));

Console.WriteLine("\nList Kernels----");
foreach (var item in compiler.Kernels)
{
    Console.WriteLine(item);
}
```

3. Declare variable and do some operation which will run on any hardware selected like Intel CPU/GPU, NVIDIA, AMD etc.
```csharp
Array a = new float[] { 1, 2, 3, 4 };
Array b = new float[4];
Array r = new float[4];

var exec = compiler.GetExec<float>();
exec.Fill(b, 0.5f);
exec.add_float(a, b, r);

Console.WriteLine("\nResult----");
for(int i = 0;i<r.Length;i++)
{
    Console.Write(r.GetValue(i) + " ");
}
```

Result:

![](https://i.ibb.co/5KgvH3D/amplifier-sample.jpg)


## Documentation
* Base: https://deepakkumar1984.github.io/Amplifier.NET/
* Articles: https://deepakkumar1984.github.io/Amplifier.NET/articles/intro.html
* API Reference: https://deepakkumar1984.github.io/Amplifier.NET/api/Amplifier.html

## Any contribution is welcome
Please fork the code and suggest improvements by raising PR. Raise issues so that I can make this library robust.
