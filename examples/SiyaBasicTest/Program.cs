using Amplifier;
using Siya;
using System;

namespace SiyaBasicTest
{
    class Program
    {
        static void Main(string[] args)
        {
            nd.use_device(1);
            DateTime start = DateTime.Now;
            //NDArray x = nd.full(new Shape(30000, 9000), 3);
            //NDArray y = nd.full(new Shape(30000, 9000), 3);
            //var z = 0.5 * nd.sqrt(x) + nd.sin(y) * nd.log(x) - nd.exp(y);
            //var data = z.ToArray().GetValue(0, 0);
            //Console.WriteLine($"{data} -> {(DateTime.Now - start).TotalMilliseconds / 1000} sec");

            var x = new NDArray(new float[] { 1, 2, 3, 3, 2, 1 }).reshape((3, 2));
            var y = new NDArray(new float[] { 1, 2, 3, 3, 2, 1 }).reshape((2, 3));
            var z = nd.dot(x, y);
            var data = z.data;
        }
    }
}
