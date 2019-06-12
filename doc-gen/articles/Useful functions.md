# Useful functions

Following are the list of useful functions to build and run the kernel

1. **Devices:** Get list of all availabe devices in your PC/Laptop. 
2. **UseDevice**: Select a device to compile and run the kernel. The function has one parameter: deviceId
3. **Compile**: Function which takes class as input. Compile method will find all the kernel methods having attribute "OpenCLKernel" and convert them to C99 format code.
4. **Kernels**: Property to list all the compiled kernels. Usuful method to see all the names while invoking.
5. **Execute**: Execute the kernel function. Takes two parameters:
   1. functionName: Name of the kernel method
   2. args: List of arguments array or scalar values.

6. **GetExecuter**: Get the executer for the compiler which can seemlesly invoke the kernel methods. Internally it calls the compiler Execute function.

7. **Save**: Save the compiler in form of binary for reusablibity.

8. **Load**: Load the saved bin compiler file. Avoids recompilation of kernel.
