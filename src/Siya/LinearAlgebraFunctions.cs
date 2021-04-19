using Amplifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siya
{
    public partial class sx
    {
        public static LinearAlgebraFunctions linalg = new LinearAlgebraFunctions();

        public NDArray matmul(NDArray x1, NDArray x2)
        {
            return linalg.matmul(x1, x2);
        }

        public NDArray cross(NDArray x1, NDArray x2)
        {
            return linalg.cross(x1, x2);
        }

        public NDArray dot(NDArray a, NDArray b)
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

        public NDArray matmul(NDArray x1, NDArray x2)
        {
            throw new NotImplementedException();
        }

        public NDArray cross(NDArray x1, NDArray x2)
        {
            throw new NotImplementedException();
        }

        public NDArray dot(NDArray a, NDArray b)
        {
            throw new NotImplementedException();
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
