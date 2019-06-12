# Saving and loading the compiler

Saving the compiler after compiling the kernel and reusing it is an efficient way of working. This save a lot of compilation time and the saved bin file can be reused in other project.

Below is how you can save a compiler after compiling with kernel class. Simply invoke Save function passing the file name:

```csharp
private void SaveCompiler()
{
    //Create instance of OpenCL compiler
    var compiler = new OpenCLCompiler();

    //Select a default device
    compiler.UseDevice(0);

    //Compile the sample kernel
    compiler.CompileKernel(typeof(SimpleKernels));
    compiler.Save("amp_cl.bin");
}
```

Create another project file, and you can copy the amp_cl.bin file to the new project directory. Below is the code to load and execute, note that you don't have to compile anything here.

```csharp
//Once saved you can reuse the same bin instead of compiling from scratch. Save compilation time. Also the bin file is portable
var compiler = new OpenCLCompiler();
compiler.Load("amp_cl.bin");

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