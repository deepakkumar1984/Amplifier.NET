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
            NDArray x = new NDArray(new long[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 });
            NDArray y = new NDArray(new long[] { 3, 3, 3, 3, 3, 3, 3, 3, 3 });

            var r = sx.add(x, y);
            r = sx.abs(r);
        }
    }
}
