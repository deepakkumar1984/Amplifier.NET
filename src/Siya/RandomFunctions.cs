using Amplifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siya
{
    public partial class nd
    {
        public static RandomFunctions random = new RandomFunctions();
    }

    public class RandomFunctions
    {
        public NDArray randint(int low, int? high= null, Shape size= null, DType dtype= DType.Float32, NDArray @out= null)
        {
            throw new NotImplementedException();
        }

        public NDArray uniform(float low = 0, float high = 1, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }

        public NDArray normal(float loc = 0, float scale = 1, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }

        public NDArray lognormal(float mean = 0, float sigma = 1, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }

        public NDArray logistic(float loc = 0, float scale = 1, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }

        public NDArray gumbel(float loc = 0, float scale = 1, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }

        public NDArray multinomial(int n, float[] pvals, Shape size = null)
        {
            throw new NotImplementedException();
        }

        public NDArray multivariate_normal(NDArray mean, NDArray cov, Shape size= null, string check_valid= null, float? tol= null)
        {
            throw new NotImplementedException();
        }

        public NDArray choice(NDArray a, Shape size= null, bool replace= true, NDArray p= null, NDArray @out= null)
        {
            throw new NotImplementedException();
        }

        public NDArray rayleigh(float scale = 1, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }

        public NDArray rand(Shape size = null)
        {
            throw new NotImplementedException();
        }

        public NDArray exponential(float scale = 1, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }

        public NDArray weibull(float a, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }

        public NDArray weibull(NDArray a, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }

        public NDArray pareto(float a, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }

        public NDArray pareto(NDArray a, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }

        public NDArray power(float a, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }

        public NDArray power(NDArray a, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }

        public NDArray shuffle(NDArray x)
        {
            throw new NotImplementedException();
        }

        public NDArray gamma(float shape, float scale=1, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }

        public NDArray gamma(NDArray shape, NDArray scale, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }

        public NDArray beta(float a, float b, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }

        public NDArray beta(NDArray a, NDArray b, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }

        public NDArray f(float dfnum, float dfden, Shape size = null)
        {
            throw new NotImplementedException();
        }

        public NDArray f(NDArray dfnum, NDArray dfden, Shape size = null)
        {
            throw new NotImplementedException();
        }

        public NDArray chisquare(float df, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }

        public NDArray chisquare(NDArray df, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }

        public NDArray randn(Shape size)
        {
            throw new NotImplementedException();
        }

        public NDArray laplace(float loc = 0, float scale = 1, Shape size = null, DType dtype = DType.Float32)
        {
            throw new NotImplementedException();
        }
    }
}
