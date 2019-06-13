# Simple kernel calls

With the "Run your first GPU Program" guide, you can compile all the kernel class in one execution engine. The compile method excepts multiple kernel class as well. 
Another approach to invoke kernel is from the class itself. 

Let's take two kernel class and compile with 2 different devices and run on same variable

Create instance of first kernel on GPU
```csharp
//Get the instance of the Simple Kernel build with Device 0 which is in my case is GPU
var dev0 = new SimpleKernels().Instance<float>(deviceId: 0);
```

Create instance of seconf kernel on CPU
```csharp
//Get the instance of the neural activation Kernel build with Device 1 which is in my case is CPU
var dev1 = new NNActivationKernels().Instance<float>(deviceId: 1);
```

Please check the device id's which may differ in your case.
Now let's invoke methods from both the kernels and print the result.

```csharp
Array x = new float[9];
//Execute fill kernel method
dev0.Fill(x, 1.5f);

//Execute sigmoid activation kernel method
dev1.Sigmoid(x);

//Print the result
Console.WriteLine("\nResult----");
for (int i = 0; i < x.Length; i++)
{
    Console.Write(x.GetValue(i) + " ");
}
```