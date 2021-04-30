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
        public static LinearAlgebraFunctions linalg = new LinearAlgebraFunctions();

        public static NDArray matmul(NDArray x1, NDArray x2)
        {
            return linalg.matmul(x1, x2);
        }

        public static NDArray cross(NDArray x1, NDArray x2)
        {
            return linalg.cross(x1, x2);
        }

        public static NDArray dot(NDArray a, NDArray b)
        {
            return linalg.dot(a, b);
        }
    }

    public class LinearAlgebraFunctions
    {
        public NDArray matrix_rank(NDArray M, NDArray tol = null, bool hermitian = false)
        {
            throw new NotImplementedException();
        }

        public NDArray matrix_power(NDArray a, int n)
        {
            throw new NotImplementedException();
        }

        public (NDArray, NDArray, NDArray, NDArray) lstsq(NDArray a, NDArray b, string rcond = "warn")
        {
            throw new NotImplementedException();
        }

        public NDArray pinv(NDArray a, float rcond = 1e-15f, bool hermitian = false)
        {
            throw new NotImplementedException();
        }

        public NDArray norm(NDArray x, string ord = null, Shape axis = null, bool keepdims = false)
        {
            throw new NotImplementedException();
        }

        public NDArray svd(NDArray a)
        {
            throw new NotImplementedException();
        }

        public NDArray cholesky(NDArray a)
        {
            throw new NotImplementedException();
        }

        public NDArray qr(NDArray a, string mode = "reduced")
        {
            throw new NotImplementedException();
        }

        public NDArray inv(NDArray a)
        {
            throw new NotImplementedException();
        }

        public NDArray matmul(NDArray a, NDArray b)
        {
            var (dtype, dtype_str) = nd.check_and_get_dtype(new NDArray[] { a, b });
            if (a.ndim != 2 && b.ndim != 2)
            {
                throw new ArgumentException("Dot products works with 2 dimensional array");
            }

            long M = a.shape[0];
            long N = b.shape[1];
            long K = a.shape[1];

            var r = nd.zeros(new Shape(M, N), dtype);

            using (new ExecuteOptions(null, (M, N), null))
            {
                nd.compiler.Execute($"{dtype_str}_matmul", M, N, K, a, b, r);
            }

            return r;
        }

        public NDArray cross(NDArray x1, NDArray x2)
        {
            throw new NotImplementedException();
        }

        public NDArray dot(NDArray a, NDArray b)
        {
            var (dtype, dtype_str) = nd.check_and_get_dtype(new NDArray[] { a, b });
            if (a.ndim != 2 && b.ndim != 2)
            {
                throw new ArgumentException("Dot products works with 2 dimensional array");
            }

            long M = a.shape[0];
            long N = b.shape[1];
            long K = a.shape[1];

            var r = nd.zeros(new Shape(M, N), dtype);

            using (new ExecuteOptions(null, (M, N), null))
            {
                nd.compiler.Execute($"{dtype_str}_matmul", M, N, K, a, b, r);
            }

            return r;
        }

        public NDArray vdot(NDArray a, NDArray b)
        {
            throw new NotImplementedException();
        }

        public NDArray inner(NDArray a, NDArray b)
        {
            throw new NotImplementedException();
        }

        public NDArray outer(NDArray a, NDArray b)
        {
            throw new NotImplementedException();
        }

        public NDArray kron(NDArray a, NDArray b)
        {
            throw new NotImplementedException();
        }

        public NDArray det(NDArray a)
        {
            throw new NotImplementedException();
        }

        public NDArray slogdet(NDArray a)
        {
            throw new NotImplementedException();
        }

        public NDArray solve(NDArray a, NDArray b)
        {
            throw new NotImplementedException();
        }

        public NDArray tensorinv(NDArray a, int ind = 2)
        {
            throw new NotImplementedException();
        }

        public NDArray tensorsolve(NDArray a, NDArray b, params int[] axes)
        {
            throw new NotImplementedException();
        }

        public NDArray eigvals(NDArray a)
        {
            throw new NotImplementedException();
        }

        public NDArray eigvalsh(NDArray a, string UPLO = "L")
        {
            throw new NotImplementedException();
        }

        public (NDArray, NDArray) eig(NDArray a)
        {
            throw new NotImplementedException();
        }

        public (NDArray, NDArray) eigh(NDArray a, string UPLO = "L")
        {
            throw new NotImplementedException();
        }

        public NDArray einsum(string subscripts, NDArray operands, DType dtype= DType.Float32, string order= "K", string casting= "safe", bool optimize= false)
        {
            throw new NotImplementedException();
        }
    }
}
