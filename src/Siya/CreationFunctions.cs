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
        public static NDArray arange(float start, float stop, float step, DType dtype = DType.Float32, Device device = null) => throw new NotImplementedException();

        public static NDArray asarray(Array obj, DType dtype= DType.Float32, Device device= null, bool? copy= null) => throw new NotImplementedException();

        public static NDArray empty(Shape shape, DType dtype = DType.Float32, Device device = null) => throw new NotImplementedException();

        public static NDArray empty_like(NDArray obj, DType dtype = DType.Float32, Device device = null) => throw new NotImplementedException();

        public static NDArray eye(int N, int? M = null, int k = 0, DType dtype = DType.Float32, Device device = null) => throw new NotImplementedException();

        public static NDArray full(Shape shape, double fill_value, DType dtype = DType.Float32, Device device = null) => throw new NotImplementedException();

        public static NDArray full_like(NDArray obj, double fill_value, DType dtype = DType.Float32, Device device = null) => throw new NotImplementedException();

        public static NDArray linspace(float start, float stop, int num, DType dtype= DType.Float32, Device device= null, bool endpoint= true) => throw new NotImplementedException();

        public static NDArray ones(Shape shape, DType dtype = DType.Float32, Device device = null) => throw new NotImplementedException();

        public static NDArray ones_like(NDArray obj, DType dtype = DType.Float32, Device device = null) => throw new NotImplementedException();

        public static NDArray zeros(Shape shape, DType dtype = DType.Float32, Device device = null) => throw new NotImplementedException();

        public static NDArray zeros_like(NDArray obj, DType dtype = DType.Float32, Device device = null) => throw new NotImplementedException();
    }
}
