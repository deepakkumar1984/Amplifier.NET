using Amplifier;
using AmplifierExamples.Kernels;
using System;
using System.Diagnostics;

namespace AmplifierExamples
{
    /// <summary>
    /// Comprehensive OpenCL 3.0 test example.
    /// Tests vector operations, matrix math, particle physics, image processing,
    /// signal processing, neural network ops, and reduction operations.
    /// Uses only float and int types (no double).
    /// </summary>
    public class OpenCL3Example : IExample
    {
        private OpenCLCompiler compiler;
        private dynamic exec;

        public void Execute()
        {
            Console.WriteLine("=== OpenCL 3.0 Comprehensive Test ===\n");

            // Initialize compiler
            compiler = new OpenCLCompiler();
            compiler.UseDevice(0);

            Console.WriteLine($"Device: {compiler.Devices[0].Name}");
            Console.WriteLine($"Device Vendor: {compiler.Devices[0].Vendor}\n");

            // Compile all kernels and structs
            compiler.CompileKernel(
                typeof(OpenCL3Kernels),
                typeof(Float3),
                typeof(Matrix4x4F),
                typeof(Particle),
                typeof(ColorRGBA),
                typeof(ComplexF),
                typeof(HistogramBin),
                typeof(BoundingBox),
                typeof(NeuronWeights)
            );

            exec = compiler.GetExec();

            // Run all tests
            TestVectorOperations();
            TestMatrixOperations();
            TestParticlePhysics();
            TestImageProcessing();
            TestComplexNumbers();
            TestNeuralNetworkOps();
            TestReductions();
            TestUtilityKernels();

            Console.WriteLine("\n=== All OpenCL 3.0 Tests Completed ===");
        }

        private void TestVectorOperations()
        {
            Console.WriteLine("--- Vector Operations ---");
            int n = 1000;

            // Test VectorAdd
            float[] a = new float[n];
            float[] b = new float[n];
            float[] c = new float[n];
            for (int i = 0; i < n; i++)
            {
                a[i] = i * 0.1f;
                b[i] = i * 0.2f;
            }

            exec.VectorAdd(a, b, c);
            Console.WriteLine($"  VectorAdd: a[10]={a[10]:F2} + b[10]={b[10]:F2} = c[10]={c[10]:F2}");

            // Test VectorFMA
            float[] fmaResult = new float[n];
            exec.VectorFMA(a, b, c, fmaResult);
            Console.WriteLine($"  VectorFMA: a[5]*b[5]+c[5] = {fmaResult[5]:F4}");

            // Test Vector3D operations
            Float3[] vectors = new Float3[100];
            Float3[] normalized = new Float3[100];
            float[] magnitudes = new float[100];

            for (int i = 0; i < 100; i++)
            {
                vectors[i] = new Float3 { x = i * 1.0f, y = i * 2.0f, z = i * 3.0f };
            }

            exec.VectorMagnitude(vectors, magnitudes);
            Console.WriteLine($"  VectorMagnitude: |({vectors[10].x}, {vectors[10].y}, {vectors[10].z})| = {magnitudes[10]:F4}");

            exec.VectorNormalize(vectors, normalized);
            Console.WriteLine($"  VectorNormalize: ({normalized[10].x:F4}, {normalized[10].y:F4}, {normalized[10].z:F4})");

            // Dot product
            Float3[] vecA = new Float3[50];
            Float3[] vecB = new Float3[50];
            float[] dotResults = new float[50];

            for (int i = 0; i < 50; i++)
            {
                vecA[i] = new Float3 { x = 1, y = 0, z = 0 };
                vecB[i] = new Float3 { x = 0, y = 1, z = 0 };
            }
            exec.VectorDot(vecA, vecB, dotResults);
            Console.WriteLine($"  VectorDot: (1,0,0) . (0,1,0) = {dotResults[0]:F4}");

            // Cross product
            Float3[] crossResults = new Float3[50];
            exec.VectorCross(vecA, vecB, crossResults);
            Console.WriteLine($"  VectorCross: (1,0,0) x (0,1,0) = ({crossResults[0].x:F4}, {crossResults[0].y:F4}, {crossResults[0].z:F4})");

            Console.WriteLine();
        }

        private void TestMatrixOperations()
        {
            Console.WriteLine("--- Matrix Operations ---");

            // Matrix multiplication NxN
            int N = 16;
            float[] matA = new float[N * N];
            float[] matB = new float[N * N];
            float[] matC = new float[N * N];

            // Initialize with identity-like matrices
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    matA[i * N + j] = (i == j) ? 1.0f : 0.0f;
                    matB[i * N + j] = i + j * 0.1f;
                }
            }

            exec.MatrixMulNxN(matA, matB, matC, N);
            Console.WriteLine($"  MatrixMulNxN: Identity * B = B, C[0,0]={matC[0]:F2}, C[5,5]={matC[5 * N + 5]:F2}");

            // Matrix transpose
            float[] transposed = new float[N * N];
            exec.MatrixTranspose(matB, transposed, N, N);
            Console.WriteLine($"  MatrixTranspose: B[1,2]={matB[1 * N + 2]:F2} -> T[2,1]={transposed[2 * N + 1]:F2}");

            Console.WriteLine();
        }

        private void TestParticlePhysics()
        {
            Console.WriteLine("--- Particle Physics Simulation ---");

            int numParticles = 100;
            Particle[] particles = new Particle[numParticles];
            BoundingBox[] bounds = new BoundingBox[1];

            // Initialize particles
            Random rng = new Random(42);
            for (int i = 0; i < numParticles; i++)
            {
                particles[i] = new Particle
                {
                    PosX = (float)(rng.NextDouble() * 10 - 5),
                    PosY = (float)(rng.NextDouble() * 10 - 5),
                    PosZ = (float)(rng.NextDouble() * 10 - 5),
                    VelX = (float)(rng.NextDouble() * 2 - 1),
                    VelY = (float)(rng.NextDouble() * 2 - 1),
                    VelZ = (float)(rng.NextDouble() * 2 - 1),
                    Mass = 1.0f,
                    Active = 1
                };
            }

            bounds[0] = new BoundingBox
            {
                MinX = -10, MinY = -10, MinZ = -10,
                MaxX = 10, MaxY = 10, MaxZ = 10
            };

            Console.WriteLine($"  Initial: Particle[0] pos=({particles[0].PosX:F2}, {particles[0].PosY:F2}, {particles[0].PosZ:F2})");

            // Simulate physics for several steps
            float dt = 0.1f;
            for (int step = 0; step < 10; step++)
            {
                exec.ParticleApplyGravity(particles, -9.8f, dt);
                exec.ParticleIntegrate(particles, dt);
                exec.ParticleBoundsCheck(particles, bounds, 0.8f);
            }

            Console.WriteLine($"  After 10 steps: Particle[0] pos=({particles[0].PosX:F2}, {particles[0].PosY:F2}, {particles[0].PosZ:F2})");

            // Test N-body forces
            float[] forceX = new float[numParticles];
            float[] forceY = new float[numParticles];
            float[] forceZ = new float[numParticles];

            exec.NBodyForces(particles, forceX, forceY, forceZ, numParticles, 1.0f, 0.1f);
            Console.WriteLine($"  N-Body forces on particle[0]: ({forceX[0]:F4}, {forceY[0]:F4}, {forceZ[0]:F4})");

            Console.WriteLine();
        }

        private void TestImageProcessing()
        {
            Console.WriteLine("--- Image Processing ---");

            int width = 64;
            int height = 64;
            int size = width * height;

            ColorRGBA[] pixels = new ColorRGBA[size];
            float[] grayscale = new float[size];

            // Create gradient image
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int idx = y * width + x;
                    pixels[idx] = new ColorRGBA
                    {
                        R = x / (float)width,
                        G = y / (float)height,
                        B = 0.5f,
                        A = 1.0f
                    };
                }
            }

            // RGB to Grayscale
            exec.RgbToGrayscale(pixels, grayscale);
            Console.WriteLine($"  RgbToGrayscale: pixel[32,32] RGB=({pixels[32 * width + 32].R:F2}, {pixels[32 * width + 32].G:F2}, {pixels[32 * width + 32].B:F2}) -> gray={grayscale[32 * width + 32]:F4}");

            // Brightness/Contrast
            exec.BrightnessContrast(pixels, 20.0f, 50.0f);
            Console.WriteLine($"  BrightnessContrast applied: pixel[32,32] R={pixels[32 * width + 32].R:F4}");

            // Gamma correction
            exec.GammaCorrection(pixels, 2.2f);
            Console.WriteLine($"  GammaCorrection (2.2) applied: pixel[32,32] R={pixels[32 * width + 32].R:F4}");

            // Box blur
            float[] blurInput = new float[size];
            float[] blurOutput = new float[size];
            for (int i = 0; i < size; i++) blurInput[i] = grayscale[i];

            exec.BoxBlur(blurInput, blurOutput, width, height, 2);
            Console.WriteLine($"  BoxBlur (radius=2): center value = {blurOutput[32 * width + 32]:F4}");

            // 3x3 Convolution (edge detection kernel)
            float[] kernel = new float[] {
                -1, -1, -1,
                -1,  8, -1,
                -1, -1, -1
            };
            float[] convOutput = new float[size];
            exec.Convolve3x3(grayscale, convOutput, kernel, width, height);
            Console.WriteLine($"  Convolve3x3 (edge detect): center value = {convOutput[32 * width + 32]:F4}");

            Console.WriteLine();
        }

        private void TestComplexNumbers()
        {
            Console.WriteLine("--- Complex Number Operations ---");

            int n = 100;
            ComplexF[] a = new ComplexF[n];
            ComplexF[] b = new ComplexF[n];
            ComplexF[] c = new ComplexF[n];
            float[] magnitudes = new float[n];
            float[] phases = new float[n];

            // Initialize with test values
            for (int i = 0; i < n; i++)
            {
                a[i] = new ComplexF { Real = (float)Math.Cos(i * 0.1f), Imag = (float)Math.Sin(i * 0.1f) };
                b[i] = new ComplexF { Real = 2.0f, Imag = 0.0f };
            }

            // Complex multiplication
            exec.ComplexMul(a, b, c);
            Console.WriteLine($"  ComplexMul: ({a[10].Real:F4} + {a[10].Imag:F4}i) * 2 = ({c[10].Real:F4} + {c[10].Imag:F4}i)");

            // Complex magnitude
            exec.ComplexMagnitude(a, magnitudes);
            Console.WriteLine($"  ComplexMagnitude: |a[10]| = {magnitudes[10]:F4} (should be ~1.0 for unit circle)");

            // Complex phase
            exec.ComplexPhase(a, phases);
            Console.WriteLine($"  ComplexPhase: arg(a[10]) = {phases[10]:F4} radians");

            // Polar to complex
            ComplexF[] fromPolar = new ComplexF[n];
            exec.PolarToComplex(magnitudes, phases, fromPolar);
            Console.WriteLine($"  PolarToComplex: back to ({fromPolar[10].Real:F4} + {fromPolar[10].Imag:F4}i)");

            Console.WriteLine();
        }

        private void TestNeuralNetworkOps()
        {
            Console.WriteLine("--- Neural Network Operations ---");

            int inputSize = 128;
            int outputSize = 64;

            float[] input = new float[inputSize];
            float[] weights = new float[outputSize * inputSize];
            float[] biases = new float[outputSize];
            float[] output = new float[outputSize];

            // Initialize with random values
            Random rng = new Random(123);
            for (int i = 0; i < inputSize; i++)
                input[i] = (float)(rng.NextDouble() * 2 - 1);

            for (int i = 0; i < weights.Length; i++)
                weights[i] = (float)(rng.NextDouble() * 0.1 - 0.05);

            for (int i = 0; i < outputSize; i++)
                biases[i] = 0.01f;

            // Dense forward pass
            exec.DenseForward(input, weights, biases, output, inputSize, outputSize);
            Console.WriteLine($"  DenseForward: output[0]={output[0]:F6}, output[31]={output[31]:F6}");

            // ReLU activation
            float[] reluData = new float[100];
            for (int i = 0; i < 100; i++) reluData[i] = i - 50;
            exec.ReLU(reluData);
            Console.WriteLine($"  ReLU: input=-10 -> {reluData[40]:F1}, input=10 -> {reluData[60]:F1}");

            // Leaky ReLU
            float[] leakyData = new float[100];
            for (int i = 0; i < 100; i++) leakyData[i] = i - 50;
            exec.LeakyReLU(leakyData, 0.1f);
            Console.WriteLine($"  LeakyReLU (0.1): input=-10 -> {leakyData[40]:F1}, input=10 -> {leakyData[60]:F1}");

            // Sigmoid
            float[] sigmoidData = new float[100];
            for (int i = 0; i < 100; i++) sigmoidData[i] = (i - 50) * 0.1f;
            exec.Sigmoid(sigmoidData);
            Console.WriteLine($"  Sigmoid: input=-2 -> {sigmoidData[30]:F4}, input=0 -> {sigmoidData[50]:F4}, input=2 -> {sigmoidData[70]:F4}");

            // Tanh
            float[] tanhData = new float[100];
            for (int i = 0; i < 100; i++) tanhData[i] = (i - 50) * 0.1f;
            exec.Tanh(tanhData);
            Console.WriteLine($"  Tanh: input=-2 -> {tanhData[30]:F4}, input=0 -> {tanhData[50]:F4}, input=2 -> {tanhData[70]:F4}");

            // Batch normalization
            float[] bnInput = new float[100];
            float[] bnOutput = new float[100];
            float[] gamma = new float[] { 1.0f };
            float[] beta = new float[] { 0.0f };
            for (int i = 0; i < 100; i++) bnInput[i] = i * 0.1f;

            float mean = 5.0f;
            float variance = 8.333f;
            exec.BatchNormForward(bnInput, bnOutput, gamma, beta, mean, variance, 1e-5f);
            Console.WriteLine($"  BatchNorm: normalized[50] = {bnOutput[50]:F4}");

            Console.WriteLine();
        }

        private void TestReductions()
        {
            Console.WriteLine("--- Reduction Operations ---");

            int n = 10000;
            float[] data = new float[n];
            int workItems = 256;
            float[] partialResults = new float[workItems];

            // Initialize with known values for verification
            float expectedSum = 0;
            for (int i = 0; i < n; i++)
            {
                data[i] = i * 0.001f;
                expectedSum += data[i];
            }

            // Sum reduction
            exec.ReduceSum(data, partialResults, n);
            float gpuSum = 0;
            for (int i = 0; i < workItems; i++) gpuSum += partialResults[i];
            Console.WriteLine($"  ReduceSum: GPU={gpuSum:F4}, Expected={expectedSum:F4}");

            // Max reduction
            exec.ReduceMax(data, partialResults, n);
            float gpuMax = float.MinValue;
            for (int i = 0; i < workItems; i++) gpuMax = Math.Max(gpuMax, partialResults[i]);
            Console.WriteLine($"  ReduceMax: GPU={gpuMax:F4}, Expected={(n - 1) * 0.001f:F4}");

            // Min reduction
            exec.ReduceMin(data, partialResults, n);
            float gpuMin = float.MaxValue;
            for (int i = 0; i < workItems; i++) gpuMin = Math.Min(gpuMin, partialResults[i]);
            Console.WriteLine($"  ReduceMin: GPU={gpuMin:F4}, Expected=0.0000");

            // Sum of squares (for variance)
            float meanVal = expectedSum / n;
            exec.ReduceSumSquares(data, partialResults, meanVal, n);
            float sumSq = 0;
            for (int i = 0; i < workItems; i++) sumSq += partialResults[i];
            float variance = sumSq / n;
            Console.WriteLine($"  ReduceSumSquares: Variance={variance:F6}");

            Console.WriteLine();
        }

        private void TestUtilityKernels()
        {
            Console.WriteLine("--- Utility Kernels ---");

            int n = 1000;

            // FillFloat
            float[] floatData = new float[n];
            exec.FillFloat(floatData, 3.14159f);
            Console.WriteLine($"  FillFloat: data[500]={floatData[500]:F5}");

            // FillInt
            int[] intData = new int[n];
            exec.FillInt(intData, 42);
            Console.WriteLine($"  FillInt: data[500]={intData[500]}");

            // Scale
            float[] scaleData = new float[n];
            for (int i = 0; i < n; i++) scaleData[i] = i;
            exec.Scale(scaleData, 2.5f);
            Console.WriteLine($"  Scale: 100 * 2.5 = {scaleData[100]:F1}");

            // Abs
            float[] absData = new float[n];
            for (int i = 0; i < n; i++) absData[i] = i - 500;
            exec.Abs(absData);
            Console.WriteLine($"  Abs: |-250| = {absData[250]:F1}");

            // Clamp
            float[] clampData = new float[n];
            for (int i = 0; i < n; i++) clampData[i] = i;
            exec.Clamp(clampData, 100.0f, 200.0f);
            Console.WriteLine($"  Clamp [100,200]: clamp(50)={clampData[50]:F1}, clamp(150)={clampData[150]:F1}, clamp(300)={clampData[300]:F1}");

            // Copy
            float[] source = new float[n];
            float[] dest = new float[n];
            for (int i = 0; i < n; i++) source[i] = i * 0.5f;
            exec.Copy(source, dest);
            Console.WriteLine($"  Copy: source[100]={source[100]:F1} -> dest[100]={dest[100]:F1}");

            // Iota
            int[] iotaData = new int[n];
            exec.Iota(iotaData, 1000);
            Console.WriteLine($"  Iota: starting at 1000, data[50]={iotaData[50]}");

            Console.WriteLine();
        }
    }
}
