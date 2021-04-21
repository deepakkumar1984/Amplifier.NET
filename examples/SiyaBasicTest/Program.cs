using Amplifier;
using Siya;
using System;

namespace SiyaBasicTest
{
    class Program
    {
        static void Main(string[] args)
        {
            sx.use_device(DeviceType.CPU, 0);
            NDArray x = new NDArray(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 });
            NDArray y = new NDArray(new float[] { 3, 3, 3, 3, 3, 3, 3, 3, 3 });
            var z = sx.ones_like(x);
            var r = sx.add(x, y);
            var arr = sx.sqrt(r).ToArray();
        }
    }
}
