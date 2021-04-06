using Amplifier;
using DarknetOpencl;
using System;

namespace DarknetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            amp.LoadKernels();
            XArray x = new XArray(new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }).Reshape(3, 3);
            var act = new Activation(Activations.TANH);
            x = act.Forward(x);
        }
    }
}
