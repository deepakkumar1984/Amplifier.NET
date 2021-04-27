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
        public static NDArray arange(float start, float stop, float step, DType dtype = DType.Float32) => throw new NotImplementedException();

        public static NDArray asarray(Array obj, DType dtype = DType.Float32, bool? copy = null)
        {
            if(copy.HasValue && copy.Value)
            {
                Array newArray = (Array)obj.Clone();
                return sx.astype(new NDArray(newArray), dtype);
            }

            return sx.astype(new NDArray(obj), dtype);
        }

        public static NDArray empty(Shape shape, DType dtype = DType.Float32) => new NDArray(shape, dtype);

        public static NDArray empty_like(NDArray obj, DType? dtype = null) => new NDArray(obj.shape, dtype != null ? dtype.Value : obj.dtype);

        public static NDArray eye(int N, int? M = null, int k = 0, DType dtype = DType.Float32)
        {
            if (M == null)
                M = N;

            var ret = zeros(new Shape(N, M.Value), dtype);
            ret = sx.diagonal(ret, k);
            ret[":"] = 1;
            return ret;
        }

        public static NDArray identity(int N, DType dtype = DType.Float32) => eye(N, dtype: dtype);

        public static NDArray full(Shape shape, double fill_value, DType dtype = DType.Float32)
        {
            var x = new NDArray(shape, dtype);
            full_exec(fill_value, x);
            return x;
        }

        public static NDArray full_like(NDArray obj, double fill_value, DType? dtype = null)
                    => full(obj.shape, fill_value, dtype.HasValue ? dtype.Value : obj.dtype);

        public static NDArray linspace(float start, float stop, int num, DType dtype= DType.Float32, bool endpoint= true) => throw new NotImplementedException();

        public static NDArray ones(Shape shape, DType dtype = DType.Float32) => full(shape, 1, dtype);

        public static NDArray ones_like(NDArray obj, DType dtype = DType.Float32) => full_like(obj, 1, dtype);

        public static NDArray zeros(Shape shape, DType dtype = DType.Float32) => full(shape, 0, dtype);

        public static NDArray zeros_like(NDArray obj, DType dtype = DType.Float32) => full_like(obj, 0, dtype);

        public static NDArray diagonal(NDArray x, int k = 0)
        {
            throw new NotImplementedException();
        }
    }
}
