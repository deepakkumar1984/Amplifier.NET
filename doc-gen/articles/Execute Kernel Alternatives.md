# Alternate methods to execute kernels

In the "Run your first kernel" guide, you have used the executer instance to run the compiler in a simple fashion. Here are more ways to execute and you can choose the way you like

## Using Array For Loop

Let's try to implement some simple neural network activation kernel as below

```csharp
class ForLoopKernels : OpenCLFunctions
{
    [OpenCLKernel]
    void Sigmoid([Global]float[] x)
    {
        int i = get_global_id(0);
        x[i] = exp(x[i]) / (exp(x[i]) + 1);
    }

    [OpenCLKernel]
    void Threshold([Global] float[] x, float value)
    {
        int i = get_global_id(0);
        x[i] = x[i] > value ? 1 : 0;
    }
}
```

And if we need to invoke using AmplifyFor loop which is an extension method for an array, here is the below code:

```csharp
public void Execute()
{
    //Create instance of OpenCL compiler and use device
    var compiler1 = new OpenCLCompiler();
    compiler1.UseDevice(0);

    var compiler2 = new OpenCLCompiler();
    compiler1.UseDevice(1);

    compiler1.CompileKernel(typeof(ForLoopKernels));
    compiler2.CompileKernel(typeof(ForLoopKernels));

    float[] x = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    x.AmplifyFor(compiler1, "Sigmoid");
    PrintArray(x);

    Console.WriteLine();

    x.AmplifyFor(compiler2, "Threshold", 0.85f);
    PrintArray(x);
}

private void PrintArray(float[] data)
{
    for(int i=0;i<data.Length;i++)
    {
        Console.Write(data[i] + " ");
    }
}
```

## Using compiler execute function

The compiler execute function is the base for all the other kernel invoke. Below is the sample implementation for the same example we have above:

```csharp
public void Execute()
{
    //Create instance of OpenCL compiler and use device
    var compiler1 = new OpenCLCompiler();
    compiler1.UseDevice(0);

    var compiler2 = new OpenCLCompiler();
    compiler1.UseDevice(1);

    compiler1.CompileKernel(typeof(ForLoopKernels));
    compiler2.CompileKernel(typeof(ForLoopKernels));

    float[] x = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    compiler1.Execute<float>("Sigmoid", x);
    PrintArray(x);

    Console.WriteLine();

    compiler2.Execute<float>("Threshold", x, 0.85f);
    PrintArray(x);
}

private void PrintArray(float[] data)
{
    for(int i=0;i<data.Length;i++)
    {
        Console.Write(data[i] + " ");
    }
}
```