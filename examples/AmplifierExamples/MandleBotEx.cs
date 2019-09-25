using Amplifier.OpenCL.Cloo;
using AmplifierExamples.Kernels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmplifierExamples
{
    public class MandleBotEx : IExample
    {
        public void Execute()
        {
            //Get the instance of the Simple Kernel build with Device 0 which is in my case is GPU
            var kernal = new MandleBotKernal()[deviceId: 0];
            
            kernal.Generate();
        }
    }
}
