using Amplifier;
using Amplifier.OpenCL;
using AmplifierExamples.Kernels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmplifierExamples
{
    class ImageExampleCalls : IExample
    {
        public void Execute()
        {
            int pixelCount = 8000000;
            int imageCount = 100;

            int[] imageInt;
            float[] imageFloat;

            var compiler = InitCompiler();
            var exec = compiler.GetExec();

            // Loop to imitate function call on many images
            for (int i = 0; i < imageCount; i++)
            {
                //var compiler = InitCompiler();
                //var exec = compiler.GetExec();

                imageInt = new int[pixelCount];
                imageFloat = new float[pixelCount];

                exec.FillI2F(imageInt, imageFloat);

                // Perfectly I would clear memory here

                //compiler.Dispose();
                Console.WriteLine($"Pass: {i + 1}");
            }

            compiler.Dispose();
            Console.ReadLine();
        }

        static OpenCLCompiler InitCompiler()
        {
            var compiler = new OpenCLCompiler();
            compiler.UseDevice(0);
            compiler.CompileKernel(typeof(SimpleImageKernels));
            return compiler;
        }
    }

    class SimpleImageKernels : OpenCLFunctions
    {
        [OpenCLKernel]
        void FillI2F([Global]int[] sourceImage, [Global]float[] resultImage)
        {
            int i = get_global_id(0);
            resultImage[i] = (float)sourceImage[i] / 255.0f;
        }
    }
}
