using Amplifier.OpenCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarknetOpencl.kernels
{
    public class ActivationKernels : OpenCLFunctions
    {
        public enum ACTIVATION
        {
            LOGISTIC, RELU, RELIE, LINEAR, RAMP, TANH, PLSE, LEAKY, ELU, LOGGY, STAIR, HARDTAN, LHTAN, SELU, MISH
        }

        float lhtan_activate_kernel(float x)
        {
            if (x < 0) return .001f * x;
            if (x > 1) return .001f * (x - 1) + 1;
            return x;
        }
        float lhtan_gradient_kernel(float x)
        {
            if (x > 0 && x < 1) return 1;
            return .001f;
        }

        float hardtan_activate_kernel(float x)
        {
            if (x < -1) return -1;
            if (x > 1) return 1;
            return x;
        }

        float linear_activate_kernel(float x) { return x; }
        float logistic_activate_kernel(float x) { return 1 / (1 + exp(-x)); }
        float loggy_activate_kernel(float x) { return 2 / (1 + exp(-x)) - 1; }
        float relu_activate_kernel(float x) { return x * ((x > 0) ? 1 : 0); }
        float elu_activate_kernel(float x) { return ((x >= 0) ? 1 : 0) * x + ((x < 0) ? 1 : 0) * (exp(x) - 1); }
        float selu_activate_kernel(float x) { return ((x >= 0) ? 1 : 0) * 1.0507f * x + ((x < 0) ? 1 : 0) * 1.0507f * 1.6732f * (exp(x) - 1); }
        float relie_activate_kernel(float x) { return (x > 0) ? x : .01f * x; }
        float ramp_activate_kernel(float x) { return x * ((x > 0) ? 1 : 0) + .1f * x; }
        float leaky_activate_kernel(float x) { return (x > 0) ? x : .1f * x; }
        float tanh_activate_kernel(float x) { return (2 / (1 + exp(-2 * x)) - 1); }

        float plse_activate_kernel(float x)
        {
            if (x < -4) return .01f * (x + 4);
            if (x > 4) return .01f * (x - 4) + 1;
            return .125f * x + .5f;
        }

        float stair_activate_kernel(float x)
        {
            float n = floor(x);
            if (n % 2 == 0) return floor(x / 2);
            else return (x - n) + floor(x / 2);
        }

        float hardtan_gradient_kernel(float x)
        {
            if (x > -1 && x < 1) return 1;
            return 0;
        }

        float linear_gradient_kernel(float x) { return 1; }
        float logistic_gradient_kernel(float x) { return (1 - x) * x; }

        float loggy_gradient_kernel(float x)
        {
            float y = (x + 1) / 2;
            return 2 * (1 - y) * y;
        }

        float relu_gradient_kernel(float x) { return (x > 0) ? 1 : 0; }
        float elu_gradient_kernel(float x) { return ((x >= 0) ? 1 : 0) + ((x < 0) ? 1 : 0) * (x + 1); }
        float selu_gradient_kernel(float x) { return ((x >= 0) ? 1 : 0) * 1.0507f + ((x < 0) ? 1 : 0) * (x + 1.0507f * 1.6732f); }
        float relie_gradient_kernel(float x) { return (x > 0) ? 1 : .01f; }
        float ramp_gradient_kernel(float x) { return ((x > 0) ? 1 : 0) + .1f; }
        float leaky_gradient_kernel(float x) { return (x > 0) ? 1 : .1f; }
        float tanh_gradient_kernel(float x) { return 1 - x * x; }
        float plse_gradient_kernel(float x) { return (x < 0 || x > 1) ? .01f : .125f; }

        float stair_gradient_kernel(float x)
        {
            if (floor(x) == x) return 0;
            return 1;
        }

        float softplus(float x)
        {
            float t = 27;
            if (x > t) return x;
            if (x < -t) return exp(x);
            return log1p(exp(x));
        }

        float mish_activate_kernel(float x)
        {
            //https://arxiv.org/abs/1908.08681v1
            float c = softplus(x);
            float a = x * tanh(c);
            return a;
        }

        float mish_gradient_kernel(float x)
        {
            //https://arxiv.org/abs/1908.08681v1
            //float d = 2*exp(2*x) + exp(2*x) + 2;
            //float w = 4*(x+1) + 4*exp(2*x) + exp(3*x) + exp(x*(4*x+6));
            //float g = exp(x) * w / pow(d,2);
            //return g;
            float sp = softplus(x);
            float g_sp = -expm1(-sp);
            float tsp = tanh(sp);
            float g_tsp = (1 - tsp * tsp) * g_sp;
            float g = x * g_tsp / tsp;
            return g;
        }

        float activate_kernel(float x, ACTIVATION a)
        {
            switch (a)
            {
                case ACTIVATION.LINEAR:
                    return linear_activate_kernel(x);
                case ACTIVATION.LOGISTIC:
                    return logistic_activate_kernel(x);
                case ACTIVATION.LOGGY:
                    return loggy_activate_kernel(x);
                case ACTIVATION.RELU:
                    return relu_activate_kernel(x);
                case ACTIVATION.ELU:
                    return elu_activate_kernel(x);
                case ACTIVATION.SELU:
                    return selu_activate_kernel(x);
                case ACTIVATION.RELIE:
                    return relie_activate_kernel(x);
                case ACTIVATION.RAMP:
                    return ramp_activate_kernel(x);
                case ACTIVATION.LEAKY:
                    return leaky_activate_kernel(x);
                case ACTIVATION.TANH:
                    return tanh_activate_kernel(x);
                case ACTIVATION.PLSE:
                    return plse_activate_kernel(x);
                case ACTIVATION.STAIR:
                    return stair_activate_kernel(x);
                case ACTIVATION.HARDTAN:
                    return hardtan_activate_kernel(x);
                case ACTIVATION.LHTAN:
                    return lhtan_activate_kernel(x);
                case ACTIVATION.MISH:
                    return mish_activate_kernel(x);
                default: return relu_activate_kernel(x);
            }
            return 0;
        }

        float gradient_kernel(float x, ACTIVATION a)
        {
            switch (a)
            {
                case ACTIVATION.LINEAR:
                    return linear_gradient_kernel(x);
                case ACTIVATION.LOGISTIC:
                    return logistic_gradient_kernel(x);
                case ACTIVATION.LOGGY:
                    return loggy_gradient_kernel(x);
                case ACTIVATION.RELU:
                    return relu_gradient_kernel(x);
                case ACTIVATION.ELU:
                    return elu_gradient_kernel(x);
                case ACTIVATION.SELU:
                    return selu_gradient_kernel(x);
                case ACTIVATION.RELIE:
                    return relie_gradient_kernel(x);
                case ACTIVATION.RAMP:
                    return ramp_gradient_kernel(x);
                case ACTIVATION.LEAKY:
                    return leaky_gradient_kernel(x);
                case ACTIVATION.TANH:
                    return tanh_gradient_kernel(x);
                case ACTIVATION.PLSE:
                    return plse_gradient_kernel(x);
                case ACTIVATION.STAIR:
                    return stair_gradient_kernel(x);
                case ACTIVATION.HARDTAN:
                    return hardtan_gradient_kernel(x);
                case ACTIVATION.LHTAN:
                    return lhtan_gradient_kernel(x);
                case ACTIVATION.MISH:
                    return mish_gradient_kernel(x);
                default: return relu_gradient_kernel(x);
            }

            return 0;
        }

        [OpenCLKernel]
        unsafe void activate_array_kernel([Global, Input] float* x, int offset, int n, ACTIVATION a)
        {
            int i = (get_group_id(0) + get_group_id(1) * get_num_groups(0)) * get_local_size(0) + get_local_id(0);
            if (i < n) x[i + offset] = activate_kernel(x[i + offset], a);
        }

        [OpenCLKernel]
        unsafe  void gradient_array_kernel([Global, Input] float* x, int offset, int n, ACTIVATION a, [Global, Input] float* delta)
        {
            int i = (get_group_id(0) + get_group_id(1) * get_num_groups(0)) * get_local_size(0) + get_local_id(0);
            if (i < n) delta[i + offset] *= gradient_kernel(x[i + offset], a);
        }
    }
}
