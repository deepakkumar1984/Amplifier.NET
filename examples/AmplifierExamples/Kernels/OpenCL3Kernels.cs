using Amplifier.OpenCL;

namespace AmplifierExamples.Kernels
{
    /// <summary>
    /// Comprehensive OpenCL 3.0 test kernels using float and int types only.
    /// Tests various compute patterns: vector math, matrix operations,
    /// particle physics, image processing, reductions, and neural network ops.
    /// </summary>
    public class OpenCL3Kernels : OpenCLFunctions
    {
        #region Vector Operations

        /// <summary>
        /// Vector addition: c = a + b
        /// </summary>
        [OpenCLKernel]
        public void VectorAdd([Global] float[] a, [Global] float[] b, [Global] float[] c)
        {
            int i = get_global_id(0);
            c[i] = a[i] + b[i];
        }

        /// <summary>
        /// Vector multiply-add: result = a * b + c (FMA pattern)
        /// </summary>
        [OpenCLKernel]
        public void VectorFMA([Global] float[] a, [Global] float[] b, [Global] float[] c, [Global] float[] result)
        {
            int i = get_global_id(0);
            result[i] = fma(a[i], b[i], c[i]);
        }

        /// <summary>
        /// Compute magnitude of 3D vectors
        /// </summary>
        [OpenCLKernel]
        public void VectorMagnitude([Global][Struct] Float3[] vectors, [Global] float[] magnitudes)
        {
            int i = get_global_id(0);
            float x = vectors[i].x;
            float y = vectors[i].y;
            float z = vectors[i].z;
            magnitudes[i] = sqrt(x * x + y * y + z * z);
        }

        /// <summary>
        /// Normalize 3D vectors
        /// </summary>
        [OpenCLKernel]
        public void VectorNormalize([Global][Struct] Float3[] vectors, [Global][Struct] Float3[] normalized)
        {
            int i = get_global_id(0);
            float x = vectors[i].x;
            float y = vectors[i].y;
            float z = vectors[i].z;
            float mag = sqrt(x * x + y * y + z * z);

            if (mag > 0.0001f)
            {
                float invMag = 1.0f / mag;
                normalized[i].x = x * invMag;
                normalized[i].y = y * invMag;
                normalized[i].z = z * invMag;
            }
            else
            {
                normalized[i].x = 0.0f;
                normalized[i].y = 0.0f;
                normalized[i].z = 0.0f;
            }
        }

        /// <summary>
        /// Dot product of 3D vectors
        /// </summary>
        [OpenCLKernel]
        public void VectorDot([Global][Struct] Float3[] a, [Global][Struct] Float3[] b, [Global] float[] result)
        {
            int i = get_global_id(0);
            result[i] = a[i].x * b[i].x + a[i].y * b[i].y + a[i].z * b[i].z;
        }

        /// <summary>
        /// Cross product of 3D vectors
        /// </summary>
        [OpenCLKernel]
        public void VectorCross([Global][Struct] Float3[] a, [Global][Struct] Float3[] b, [Global][Struct] Float3[] result)
        {
            int i = get_global_id(0);
            result[i].x = a[i].y * b[i].z - a[i].z * b[i].y;
            result[i].y = a[i].z * b[i].x - a[i].x * b[i].z;
            result[i].z = a[i].x * b[i].y - a[i].y * b[i].x;
        }

        #endregion

        #region Matrix Operations

        /// <summary>
        /// Matrix-vector multiplication (4x4 matrix * 4-element vector)
        /// </summary>
        [OpenCLKernel]
        public void MatrixVectorMul(
            [Global][Struct] Matrix4x4F[] matrices,
            [Global] float[] vectors,  // 4 floats per vector (x, y, z, w)
            [Global] float[] results,
            int vectorStride)
        {
            int i = get_global_id(0);
            int vIdx = i * vectorStride;

            float x = vectors[vIdx];
            float y = vectors[vIdx + 1];
            float z = vectors[vIdx + 2];
            float w = vectors[vIdx + 3];

            results[vIdx]     = matrices[i].M00 * x + matrices[i].M01 * y + matrices[i].M02 * z + matrices[i].M03 * w;
            results[vIdx + 1] = matrices[i].M10 * x + matrices[i].M11 * y + matrices[i].M12 * z + matrices[i].M13 * w;
            results[vIdx + 2] = matrices[i].M20 * x + matrices[i].M21 * y + matrices[i].M22 * z + matrices[i].M23 * w;
            results[vIdx + 3] = matrices[i].M30 * x + matrices[i].M31 * y + matrices[i].M32 * z + matrices[i].M33 * w;
        }

        /// <summary>
        /// Matrix multiplication C = A * B (for square matrices stored as 1D arrays)
        /// </summary>
        [OpenCLKernel]
        public void MatrixMulNxN([Global] float[] A, [Global] float[] B, [Global] float[] C, int N)
        {
            int row = get_global_id(0);
            int col = get_global_id(1);

            float sum = 0.0f;
            for (int k = 0; k < N; k++)
            {
                sum += A[row * N + k] * B[k * N + col];
            }
            C[row * N + col] = sum;
        }

        /// <summary>
        /// Matrix transpose
        /// </summary>
        [OpenCLKernel]
        public void MatrixTranspose([Global] float[] input, [Global] float[] output, int rows, int cols)
        {
            int row = get_global_id(0);
            int col = get_global_id(1);

            output[col * rows + row] = input[row * cols + col];
        }

        #endregion

        #region Particle Physics Simulation

        /// <summary>
        /// Update particle positions based on velocity
        /// </summary>
        [OpenCLKernel]
        public void ParticleIntegrate([Global][Struct] Particle[] particles, float deltaTime)
        {
            int i = get_global_id(0);

            if (particles[i].Active == 1)
            {
                particles[i].PosX += particles[i].VelX * deltaTime;
                particles[i].PosY += particles[i].VelY * deltaTime;
                particles[i].PosZ += particles[i].VelZ * deltaTime;
            }
        }

        /// <summary>
        /// Apply gravity force to particles
        /// </summary>
        [OpenCLKernel]
        public void ParticleApplyGravity([Global][Struct] Particle[] particles, float gravityY, float deltaTime)
        {
            int i = get_global_id(0);

            if (particles[i].Active == 1)
            {
                particles[i].VelY += gravityY * deltaTime;
            }
        }

        /// <summary>
        /// Compute N-body gravitational forces (simplified)
        /// </summary>
        [OpenCLKernel]
        public void NBodyForces(
            [Global][Struct] Particle[] particles,
            [Global] float[] forceX,
            [Global] float[] forceY,
            [Global] float[] forceZ,
            int numParticles,
            float G,
            float softening)
        {
            int i = get_global_id(0);

            float fx = 0.0f;
            float fy = 0.0f;
            float fz = 0.0f;

            float xi = particles[i].PosX;
            float yi = particles[i].PosY;
            float zi = particles[i].PosZ;
            float mi = particles[i].Mass;

            for (int j = 0; j < numParticles; j++)
            {
                if (i != j && particles[j].Active == 1)
                {
                    float dx = particles[j].PosX - xi;
                    float dy = particles[j].PosY - yi;
                    float dz = particles[j].PosZ - zi;

                    float distSq = dx * dx + dy * dy + dz * dz + softening;
                    float dist = sqrt(distSq);
                    float invDist3 = 1.0f / (distSq * dist);

                    float force = G * mi * particles[j].Mass * invDist3;

                    fx += force * dx;
                    fy += force * dy;
                    fz += force * dz;
                }
            }

            forceX[i] = fx;
            forceY[i] = fy;
            forceZ[i] = fz;
        }

        /// <summary>
        /// Check particle collision with bounding box
        /// </summary>
        [OpenCLKernel]
        public void ParticleBoundsCheck(
            [Global][Struct] Particle[] particles,
            [Global][Struct] BoundingBox[] bounds,
            float damping)
        {
            int i = get_global_id(0);

            if (particles[i].Active == 0) return;

            // Check X bounds
            if (particles[i].PosX < bounds[0].MinX)
            {
                particles[i].PosX = bounds[0].MinX;
                particles[i].VelX = -particles[i].VelX * damping;
            }
            else if (particles[i].PosX > bounds[0].MaxX)
            {
                particles[i].PosX = bounds[0].MaxX;
                particles[i].VelX = -particles[i].VelX * damping;
            }

            // Check Y bounds
            if (particles[i].PosY < bounds[0].MinY)
            {
                particles[i].PosY = bounds[0].MinY;
                particles[i].VelY = -particles[i].VelY * damping;
            }
            else if (particles[i].PosY > bounds[0].MaxY)
            {
                particles[i].PosY = bounds[0].MaxY;
                particles[i].VelY = -particles[i].VelY * damping;
            }

            // Check Z bounds
            if (particles[i].PosZ < bounds[0].MinZ)
            {
                particles[i].PosZ = bounds[0].MinZ;
                particles[i].VelZ = -particles[i].VelZ * damping;
            }
            else if (particles[i].PosZ > bounds[0].MaxZ)
            {
                particles[i].PosZ = bounds[0].MaxZ;
                particles[i].VelZ = -particles[i].VelZ * damping;
            }
        }

        #endregion

        #region Image Processing

        /// <summary>
        /// Convert RGB to grayscale
        /// </summary>
        [OpenCLKernel]
        public void RgbToGrayscale([Global][Struct] ColorRGBA[] input, [Global] float[] output)
        {
            int i = get_global_id(0);
            // Standard luminance weights
            output[i] = 0.299f * input[i].R + 0.587f * input[i].G + 0.114f * input[i].B;
        }

        /// <summary>
        /// Apply brightness and contrast adjustment
        /// </summary>
        [OpenCLKernel]
        public void BrightnessContrast([Global][Struct] ColorRGBA[] pixels, float brightness, float contrast)
        {
            int i = get_global_id(0);

            float factor = (259.0f * (contrast + 255.0f)) / (255.0f * (259.0f - contrast));

            float valR = factor * (pixels[i].R * 255.0f - 128.0f + brightness) + 128.0f;
            float valG = factor * (pixels[i].G * 255.0f - 128.0f + brightness) + 128.0f;
            float valB = factor * (pixels[i].B * 255.0f - 128.0f + brightness) + 128.0f;

            pixels[i].R = fmin(fmax(valR, 0.0f), 255.0f) / 255.0f;
            pixels[i].G = fmin(fmax(valG, 0.0f), 255.0f) / 255.0f;
            pixels[i].B = fmin(fmax(valB, 0.0f), 255.0f) / 255.0f;
        }

        /// <summary>
        /// Apply gamma correction
        /// </summary>
        [OpenCLKernel]
        public void GammaCorrection([Global][Struct] ColorRGBA[] pixels, float gamma)
        {
            int i = get_global_id(0);
            float invGamma = 1.0f / gamma;

            pixels[i].R = pow(pixels[i].R, invGamma);
            pixels[i].G = pow(pixels[i].G, invGamma);
            pixels[i].B = pow(pixels[i].B, invGamma);
        }

        /// <summary>
        /// 3x3 Convolution filter for image filtering
        /// </summary>
        [OpenCLKernel]
        public void Convolve3x3(
            [Global] float[] input,
            [Global] float[] output,
            [Global] float[] filter,
            int width,
            int height)
        {
            int x = get_global_id(0);
            int y = get_global_id(1);

            if (x < 1 || x >= width - 1 || y < 1 || y >= height - 1)
            {
                output[y * width + x] = input[y * width + x];
                return;
            }

            float sum = 0.0f;
            for (int ky = -1; ky <= 1; ky++)
            {
                for (int kx = -1; kx <= 1; kx++)
                {
                    int idx = (y + ky) * width + (x + kx);
                    int kidx = (ky + 1) * 3 + (kx + 1);
                    sum += input[idx] * filter[kidx];
                }
            }
            output[y * width + x] = sum;
        }

        /// <summary>
        /// Box blur filter (average of neighbors)
        /// </summary>
        [OpenCLKernel]
        public void BoxBlur([Global] float[] input, [Global] float[] output, int width, int height, int radius)
        {
            int x = get_global_id(0);
            int y = get_global_id(1);

            float sum = 0.0f;
            int count = 0;

            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    int nx = x + dx;
                    int ny = y + dy;

                    if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                    {
                        sum += input[ny * width + nx];
                        count++;
                    }
                }
            }

            output[y * width + x] = sum / (float)count;
        }

        #endregion

        #region Complex Number Operations (Signal Processing)

        /// <summary>
        /// Complex multiplication: c = a * b
        /// </summary>
        [OpenCLKernel]
        public void ComplexMul([Global][Struct] ComplexF[] a, [Global][Struct] ComplexF[] b, [Global][Struct] ComplexF[] c)
        {
            int i = get_global_id(0);
            // (ar + ai*i) * (br + bi*i) = (ar*br - ai*bi) + (ar*bi + ai*br)*i
            c[i].Real = a[i].Real * b[i].Real - a[i].Imag * b[i].Imag;
            c[i].Imag = a[i].Real * b[i].Imag + a[i].Imag * b[i].Real;
        }

        /// <summary>
        /// Complex magnitude (absolute value)
        /// </summary>
        [OpenCLKernel]
        public void ComplexMagnitude([Global][Struct] ComplexF[] input, [Global] float[] output)
        {
            int i = get_global_id(0);
            output[i] = sqrt(input[i].Real * input[i].Real + input[i].Imag * input[i].Imag);
        }

        /// <summary>
        /// Complex phase (angle)
        /// </summary>
        [OpenCLKernel]
        public void ComplexPhase([Global][Struct] ComplexF[] input, [Global] float[] output)
        {
            int i = get_global_id(0);
            output[i] = atan2(input[i].Imag, input[i].Real);
        }

        /// <summary>
        /// Convert polar to complex (magnitude, phase) -> (real, imag)
        /// </summary>
        [OpenCLKernel]
        public void PolarToComplex([Global] float[] magnitude, [Global] float[] phase, [Global][Struct] ComplexF[] output)
        {
            int i = get_global_id(0);
            output[i].Real = magnitude[i] * cos(phase[i]);
            output[i].Imag = magnitude[i] * sin(phase[i]);
        }

        #endregion

        #region Neural Network Operations

        /// <summary>
        /// Dense layer forward pass (fully connected)
        /// </summary>
        [OpenCLKernel]
        public void DenseForward(
            [Global] float[] input,
            [Global] float[] weights,
            [Global] float[] biases,
            [Global] float[] output,
            int inputSize,
            int outputSize)
        {
            int o = get_global_id(0);

            float sum = biases[o];
            for (int i = 0; i < inputSize; i++)
            {
                sum += input[i] * weights[o * inputSize + i];
            }
            output[o] = sum;
        }

        /// <summary>
        /// ReLU activation function
        /// </summary>
        [OpenCLKernel]
        public void ReLU([Global] float[] data)
        {
            int i = get_global_id(0);
            data[i] = fmax(0.0f, data[i]);
        }

        /// <summary>
        /// Leaky ReLU activation function
        /// </summary>
        [OpenCLKernel]
        public void LeakyReLU([Global] float[] data, float alpha)
        {
            int i = get_global_id(0);
            float val = data[i];
            data[i] = val > 0.0f ? val : alpha * val;
        }

        /// <summary>
        /// Sigmoid activation function
        /// </summary>
        [OpenCLKernel]
        public void Sigmoid([Global] float[] data)
        {
            int i = get_global_id(0);
            data[i] = 1.0f / (1.0f + exp(-data[i]));
        }

        /// <summary>
        /// Tanh activation function
        /// </summary>
        [OpenCLKernel]
        public void Tanh([Global] float[] data)
        {
            int i = get_global_id(0);
            data[i] = tanh(data[i]);
        }

        /// <summary>
        /// Softmax normalization (requires pre-computed max and sum)
        /// </summary>
        [OpenCLKernel]
        public void SoftmaxNormalize([Global] float[] data, float maxVal, float sumExp)
        {
            int i = get_global_id(0);
            data[i] = exp(data[i] - maxVal) / sumExp;
        }

        /// <summary>
        /// Batch normalization forward pass
        /// </summary>
        [OpenCLKernel]
        public void BatchNormForward(
            [Global] float[] input,
            [Global] float[] output,
            [Global] float[] gamma,
            [Global] float[] beta,
            float mean,
            float variance,
            float epsilon)
        {
            int i = get_global_id(0);
            float normalized = (input[i] - mean) / sqrt(variance + epsilon);
            output[i] = gamma[0] * normalized + beta[0];
        }

        #endregion

        #region Reduction Operations

        /// <summary>
        /// Parallel sum reduction (partial sums for each work group)
        /// </summary>
        [OpenCLKernel]
        public void ReduceSum([Global] float[] input, [Global] float[] output, int inputSize)
        {
            int i = get_global_id(0);
            int stride = get_global_size(0);

            float sum = 0.0f;
            for (int idx = i; idx < inputSize; idx += stride)
            {
                sum += input[idx];
            }
            output[i] = sum;
        }

        /// <summary>
        /// Find maximum value (partial max for each work item)
        /// </summary>
        [OpenCLKernel]
        public void ReduceMax([Global] float[] input, [Global] float[] output, int inputSize)
        {
            int i = get_global_id(0);
            int stride = get_global_size(0);

            float maxVal = -3.4028234e+38f;
            for (int idx = i; idx < inputSize; idx += stride)
            {
                maxVal = fmax(maxVal, input[idx]);
            }
            output[i] = maxVal;
        }

        /// <summary>
        /// Find minimum value (partial min for each work item)
        /// </summary>
        [OpenCLKernel]
        public void ReduceMin([Global] float[] input, [Global] float[] output, int inputSize)
        {
            int i = get_global_id(0);
            int stride = get_global_size(0);

            float minVal = 3.4028234e+38f;
            for (int idx = i; idx < inputSize; idx += stride)
            {
                minVal = fmin(minVal, input[idx]);
            }
            output[i] = minVal;
        }

        /// <summary>
        /// Compute sum of squares (for variance calculation)
        /// </summary>
        [OpenCLKernel]
        public void ReduceSumSquares([Global] float[] input, [Global] float[] output, float mean, int inputSize)
        {
            int i = get_global_id(0);
            int stride = get_global_size(0);

            float sum = 0.0f;
            for (int idx = i; idx < inputSize; idx += stride)
            {
                float diff = input[idx] - mean;
                sum += diff * diff;
            }
            output[i] = sum;
        }

        #endregion

        #region Utility Kernels

        /// <summary>
        /// Fill array with a constant value
        /// </summary>
        [OpenCLKernel]
        public void FillFloat([Global] float[] data, float value)
        {
            int i = get_global_id(0);
            data[i] = value;
        }

        /// <summary>
        /// Fill int array with a constant value
        /// </summary>
        [OpenCLKernel]
        public void FillInt([Global] int[] data, int value)
        {
            int i = get_global_id(0);
            data[i] = value;
        }

        /// <summary>
        /// Scale array by a constant
        /// </summary>
        [OpenCLKernel]
        public void Scale([Global] float[] data, float factor)
        {
            int i = get_global_id(0);
            data[i] *= factor;
        }

        /// <summary>
        /// Element-wise absolute value
        /// </summary>
        [OpenCLKernel]
        public void Abs([Global] float[] data)
        {
            int i = get_global_id(0);
            data[i] = fabs(data[i]);
        }

        /// <summary>
        /// Clamp values to a range
        /// </summary>
        [OpenCLKernel]
        public void Clamp([Global] float[] data, float minVal, float maxVal)
        {
            int i = get_global_id(0);
            data[i] = clamp(data[i], minVal, maxVal);
        }

        /// <summary>
        /// Copy array
        /// </summary>
        [OpenCLKernel]
        public void Copy([Global] float[] source, [Global] float[] dest)
        {
            int i = get_global_id(0);
            dest[i] = source[i];
        }

        /// <summary>
        /// Generate sequence of integers
        /// </summary>
        [OpenCLKernel]
        public void Iota([Global] int[] data, int start)
        {
            int i = get_global_id(0);
            data[i] = start + i;
        }

        #endregion
    }
}
