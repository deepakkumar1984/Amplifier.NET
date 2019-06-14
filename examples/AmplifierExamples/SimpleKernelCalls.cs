using Amplifier;
using AmplifierExamples.Kernels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmplifierExamples
{
    class SimpleKernelCalls : IExample
    {
        public void Execute()
        {
            //Get the instance of the Simple Kernel build with Device 0 which is in my case is GPU
            var dev0 = new SimpleKernels()[deviceId: 0];
            
            //Get the instance of the neural activation Kernel build with Device 1 which is in my case is CPU
            var dev1 = new NNActivationKernels()[1];

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
        }
    }
}
