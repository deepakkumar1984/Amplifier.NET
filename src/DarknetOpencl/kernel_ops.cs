using Amplifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarknetOpencl
{
    public class kernel_ops
    {
        public void activate_array_kernel(XArray x, int offset, int n, Activations a)
        {
            amp.Ops.activate_array_kernel(x, offset, n, (int)a);
        }

        public void gradient_array_kernel(XArray x, int offset, int n, Activations a, XArray delta)
        {
            amp.Ops.gradient_array_kernel(x, offset, n, (int)a, delta);
        }

        public void forward_avgpool_layer_kernel(int n, int w, int h, int c, XArray input, XArray output)
        {
            amp.Ops.forward_avgpool_layer_kernel(n, w, h, c, input, output);
        }

        public void backward_avgpool_layer_kernel(int n, int w, int h, int c, XArray in_delta, XArray out_delta)
        {
            amp.Ops.backward_avgpool_layer_kernel(n, w, h, c, in_delta, out_delta);
        }

        public void scale_bias_kernel(int N, XArray output, XArray biases, int batch, int n, int size)
        {
            amp.Ops.scale_bias_kernel(N, output, biases, batch, n, size);
        }

        public void backward_scale_kernel(int tuning, XArray sums, int batch, int n, int size, XArray x_norm, XArray delta, XArray scale_updates)
        {
            amp.Ops.backward_scale_kernel(tuning, sums, batch, n, size, x_norm, delta, scale_updates);
        }

        public void add_bias_kernel(int N, XArray output, XArray biases, int batch, int n, int size)
        {
            amp.Ops.add_bias_kernel(N, output, biases, batch, n, size);
        }

        public void backward_bias_kernel(int tuning, XArray sums, int batch, int n, int size, XArray bias_updates, XArray delta)
        {
            amp.Ops.backward_bias_kernel(tuning, sums, batch, n, size, bias_updates, delta);
        }

        public void mean_kernel(int N, XArray x, int batch, int filters, int spatial, XArray mean)
        {
            amp.Ops.mean_kernel(N, x, batch, filters, spatial, mean);
        }

        public void variance_kernel(int N, XArray x, XArray mean, int batch, int filters, int spatial, XArray variance)
        {
            amp.Ops.variance_kernel(N, x, mean, batch, filters, spatial, variance);
        }

        public void mean_delta_kernel(int N, XArray delta, XArray variance, int batch, int filters, int spatial, XArray mean_delta)
        {
            amp.Ops.mean_delta_kernel(N, delta, variance, batch, filters, spatial, mean_delta);
        }

        public void variance_delta_kernel(int N, XArray x, XArray delta, XArray mean, XArray variance, int batch, int filters, int spatial, XArray variance_delta)
        {
            amp.Ops.variance_delta_kernel(N, x, delta, mean, variance, batch, filters, spatial, variance_delta);
        }

        public void accumulate_kernel(XArray x, int n, int groups, XArray sum)
        {
            amp.Ops.accumulate_kernel(x, n, groups, sum);
        }

        public void fast_mean_kernel(int tuning, XArray sums, int filters, int batch, int spatial, XArray x, XArray mean)
        {
            amp.Ops.fast_mean_kernel(tuning, sums, filters, batch, spatial, x, mean);
        }

        public void fast_variance_kernel(int tuning, XArray sums, int filters, int batch, int spatial, XArray x, XArray mean, XArray variance)
        {
            amp.Ops.fast_variance_kernel(tuning, sums, filters, batch, spatial, x, mean, variance);
        }

        public void fast_mean_delta_kernel(int tuning, XArray sums, int filters, int batch, int spatial, XArray variance, XArray delta, XArray mean_delta)
        {
            amp.Ops.fast_mean_delta_kernel(tuning, sums, filters, batch, spatial, variance, delta, mean_delta);
        }

        public void fast_variance_delta_kernel(int tuning, XArray sums, int filters, int batch, int spatial, XArray x, XArray variance, XArray delta, XArray mean, XArray variance_delta)
        {
            amp.Ops.fast_variance_delta_kernel(tuning, sums, filters, batch, spatial, x, variance, delta, mean, variance_delta);
        }


        public void adam_kernel(int N, XArray x, XArray m, XArray v, float B1, float B2, float rate, float eps, int t)
        {
            amp.Ops.adam_kernel(N, x, m, v, B1, B2, rate, eps, t);
        }


        public void normalize_kernel(int N, XArray x, XArray mean, XArray variance, int batch, int filters, int spatial)
        {
            amp.Ops.normalize_kernel(N, x, mean, variance, batch, filters, spatial);
        }

        public void normalize_delta_kernel(int N, XArray x, XArray mean, XArray variance, XArray mean_delta, XArray variance_delta, int batch, int filters, int spatial, XArray delta)
        {
            amp.Ops.normalize_delta_kernel(N, x, mean, variance, mean_delta, variance_delta, batch, filters, spatial, delta);
        }


        public void l2norm_kernel(int N, XArray x, XArray dx, int batch, int filters, int spatial)
        {
            amp.Ops.l2norm_kernel(N, x, dx, batch, filters, spatial);
        }

        public void reorg_kernel(int N, XArray x, int w, int h, int c, int batch, int stride, int forward, XArray @out)
        {
            amp.Ops.reorg_kernel(N, x, w, h, c, batch, stride, forward, @out);
        }


        public void axpy_kernel(int N, float ALPHA, XArray X, int OFFX, int INCX, XArray Y, int OFFY, int INCY)
        {
            amp.Ops.axpy_kernel(N, ALPHA, X, OFFX, INCX, Y, OFFY, INCY);
        }


        public void pow_kernel(int N, float ALPHA, XArray X, int OFFX, int INCX, XArray Y, int OFFY, int INCY)
        {
            amp.Ops.pow_kernel(N, ALPHA, X, OFFX, INCX, Y, OFFY, INCY);
        }


        public void const_kernel(int N, float ALPHA, XArray X, int OFFX, int INCX)
        {
            amp.Ops.const_kernel(N, ALPHA, X, OFFX, INCX);
        }


        public void constrain_kernel(int N, float ALPHA, XArray X, int INCX)
        {
            amp.Ops.constrain_kernel(N, ALPHA, X, INCX);
        }


        public void supp_kernel(int N, float ALPHA, XArray X, int INCX)
        {
            amp.Ops.supp_kernel(N, ALPHA, X, INCX);
        }


        public void add_kernel(int N, float ALPHA, XArray X, int INCX)
        {
            amp.Ops.add_kernel(N, ALPHA, X, INCX);
        }


        public void scal_kernel(int N, float ALPHA, XArray X, int INCX)
        {
            amp.Ops.scal_kernel(N, ALPHA, X, INCX);
        }


        public void fill_kernel(int N, float ALPHA, XArray X, int OFFX, int INCX)
        {
            amp.Ops.fill_kernel(N, ALPHA, X, OFFX, INCX);
        }


        public void mask_kernel(int n, XArray x, float mask_num, XArray mask, float val)
        {
            amp.Ops.mask_kernel(n, x, mask_num, mask, val);
        }


        public void copy_kernel(int N, XArray X, int OFFX, int INCX, XArray Y, int OFFY, int INCY)
        {
            amp.Ops.copy_kernel(N, X, OFFX, INCX, Y, OFFY, INCY);
        }


        public void mul_kernel(int N, XArray X, int INCX, XArray Y, int INCY)
        {
            amp.Ops.mul_kernel(N, X, INCX, Y, INCY);
        }


        public void flatten_kernel(int N, XArray x, int spatial, int layers, int batch, int forward, XArray @out)
        {
            amp.Ops.flatten_kernel(N, x, spatial, layers, batch, forward, @out);
        }


        public void shortcut_kernel(int size, int minw, int minh, int minc, int stride, int sample, int batch, int w1, int h1, int c1, XArray add, int w2, int h2, int c2, float s1, float s2, XArray @out)
        {
            
        }


        public void smooth_l1_kernel(int n, XArray pred, XArray truth, XArray delta, XArray error)
        {
            
        }


        public void softmax_x_ent_kernel(int n, XArray pred, XArray truth, XArray delta, XArray error)
        {
            
        }


        public void logistic_x_ent_kernel(int n, XArray pred, XArray truth, XArray delta, XArray error)
        {
           
        }


        public void l2_kernel(int n, XArray pred, XArray truth, XArray delta, XArray error)
        {
            
        }


        public void l1_kernel(int n, XArray pred, XArray truth, XArray delta, XArray error)
        {
            
        }


        public void wgan_kernel(int n, XArray pred, XArray truth, XArray delta, XArray error)
        {
            
        }


        public void weighted_sum_kernel(int n, XArray a, XArray b, XArray s, XArray c)
        {
            
        }


        public void weighted_delta_kernel(int n, XArray a, XArray b, XArray s, XArray da, XArray db, XArray ds, XArray dc)
        {
            
        }


        public void mult_add_into_kernel(int n, XArray a, XArray b, XArray c)
        {
            
        }


        public void deinter_kernel(int NX, XArray X, int NY, XArray Y, int B, XArray OUT)
        {
            
        }


        public void inter_kernel(int NX, XArray X, int NY, XArray Y, int B, XArray OUT)
        {
            
        }


        public void softmax_device(XArray input, int n, float temp, int stride, XArray output)
        {
            
        }


        public void softmax_kernel(XArray input, int offset, int n, int batch, int batch_offset, int groups, int group_offset, int stride, float temp, XArray output)
        {
            
        }


        public void softmax_tree_kernel(XArray input, int offset, int index, int spatial, int batch, int stride, float temp, XArray output, int groups, XArray group_size, XArray group_offset)
        {
            
        }


        public void scale_mask_kernel(int n, XArray x, float mask_num, XArray mask, float scale)
        {
            
        }


        public void dot_kernel(XArray output, float scale, int batch, int n, int size, XArray delta)
        {
            
        }


        public void upsample_kernel(int N, XArray x, int w, int h, int c, int batch, int stride, int forward, float scale, XArray @out)
        {
            
        }


        public void gemm_kernel(
            int tuning, XArray sums,
            int TA, int TB,
            int M, int N, int K,
            float ALPHA,
            XArray A, int offset_A, int lda,
            XArray B, int offset_B, int ldb,
            float BETA,
            XArray C, int offset_C, int ldc)
        {

            
        }


        public void scal_add_kernel(int N, float ALPHA, float BETA, XArray X, int OFFX, int INCX)
        {
            
        }

        public void mean_array_kernel(int N, float alpha, XArray s, XArray a)
        {
           
        }
    }
}
