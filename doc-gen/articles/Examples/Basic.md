# Basic example

Create a kernel class with following methods:

```csharp
class SimpleKernels : OpenCLFunctions
{
    [OpenCLKernel]
    void AddData([Global]float[] a, [Global] float[] b, [Global]float[] r)
    {
        int i = get_global_id(0);
        b[i] = 0.5f * b[i];
        r[i] = a[i] + b[i];
    }

    [OpenCLKernel]
    void Fill([Global] float[] x, float value)
    {
        int i = get_global_id(0);
        
        x[i] = (float)value;
    }

    [OpenCLKernel]
    void SAXPY([Global]float[] x, [Global] float[] y, float a)
    {
        int i = get_global_id(0);

        y[i] += a * x[i];
    }
}
```

Add the following code in the Program.Main method

```csharp
//Create instance of OpenCL compiler
var compiler = new OpenCLCompiler();

//Get the available device list
Console.WriteLine("\nList Devices----");
foreach (var item in compiler.Devices)
{
    Console.WriteLine(item);
}

//Select a default device
compiler.UseDevice(0);

//Compile the sample kernel
compiler.CompileKernel(typeof(SimpleKernels));

//See all the kernel methods
Console.WriteLine("\nList Kernels----");
foreach (var item in compiler.Kernels)
{
    Console.WriteLine(item);
}

//Create variable a, b and r
Array x = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
Array y = new float[9];

//Get the execution engine
var exec = compiler.GetExec<float>();

//Execute fill kernel method
exec.Fill(y, 0.5f);

//Execuete SAXPY kernel method
exec.SAXPY(x, y, 2f);

//Print the result
Console.WriteLine("\nResult----");
for (int i = 0; i < y.Length; i++)
{
    Console.Write(y.GetValue(i) + " ");
}
```