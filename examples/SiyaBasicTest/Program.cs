using Amplifier;
using Siya;
using System;

namespace SiyaBasicTest
{
    class Program
    {
        static void Main(string[] args)
        {
            sx.use_device(0);
            DateTime start = DateTime.Now;
            NDArray x = sx.full(new Shape(3000, 3000), 3);
            NDArray y = sx.full(new Shape(3000, 3000), 3);
            var z = 0.5 * sx.sqrt(x) + sx.sin(y) * sx.log(x) - sx.exp(y);
            var data = z.ToArray().GetValue(0, 0);
            Console.WriteLine($"{data} -> {(DateTime.Now - start).TotalMilliseconds / 1000} sec");
        }
    }
}
