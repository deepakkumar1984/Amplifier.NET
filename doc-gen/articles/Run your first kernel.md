# Run your first kernel

Let's try to run our first kernel SAXPY. If you see the method clealy, there are 3 parameters: float array of "x", float array of "y" and scalar float "a"

1. So lets define our 3 parameter using .NET standard declaration:

```csharp
//Create variable a, b and r
Array x = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
Array Y = new float[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 };
float a = 0.5f;
```

2. Create an instance of OpenCL compiler. You can see all the available devices supported by Open CL

```csharp
//Create instance of OpenCL compiler
var compiler = new OpenCLCompiler();

//Get the available device list
Console.WriteLine("\nList Devices----");
foreach (var item in compiler.Devices)
{
    Console.WriteLine(item);
}
```

If more than one device is found, then the system supports OpenCL. Now a days almost all the hardware and GPU cards supports OpenCL. You can go through the link for the list of supported hardwar list: https://www.khronos.org/conformance/adopters/conformant-products/opencl

3. For now lets select one device which can be done by specifiying the device id like below:

```csharp
//Select a default device
compiler.UseDevice(0);
```

4. Compile the kernel class with the selected device.  You can invoke the Compile method for other kernel class as well. With the Kernels property you can see all the defined functions

```csharp
//Compile the sample kernel
compiler.CompileKernel(typeof(SampleKernels));

//See all the kernel methods
Console.WriteLine("\nList Kernels----");
foreach (var item in compiler.Kernels)
{
    Console.WriteLine(item);
}
```

5. Get the execution engine for the compiler and invoke the function.

```csharp
//Get the execution engine
var exec = compiler.GetExec<float>();

//Execuete SAXPY kernel method
exec.SAXPY(x, y, a);

//Print the result
Console.WriteLine("\nResult----");
for (int i = 0; i < y.Length; i++)
{
    Console.Write(y.GetValue(i) + " ");
}
```
