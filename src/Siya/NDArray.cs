using Amplifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siya
{
    public class NDArray : XArray
    {
        public string device { get; internal set; }

        public DType dtype => base.DataType;

        public int ndim => Sizes.Length;

        public Shape shape => new Shape(base.Sizes);

        public long size => Count;

        public long len => Sizes[0];

        public Array data => ToArray();

        public NDArray T => throw new NotImplementedException();

        public NDArray(Array data) : base(data, Direction.Input)
        {
        }

        public NDArray(Shape shape, DType dtype = DType.Float32) : base(shape.Data.ToArray(), dtype, Direction.Input)
        {
        }

        internal NDArray(IntPtr ptr, Shape shape, DType dtype = DType.Float32) : base(ptr, shape.Data.ToArray(), dtype)
        {
        }

        public NDArray reshape(Shape shape)
        {
            var xarray = Reshape(shape.Data.ToArray());
            return new NDArray(xarray.NativePtr, new Shape(xarray.Sizes), xarray.DataType);
        }

        public object this[string key]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public static NDArray operator +(NDArray lhs, NDArray rhs) => nd.add(lhs, rhs);

        public static NDArray operator +(NDArray lhs, double rhs) => nd.add(lhs, nd.full_like(lhs, rhs));

        public static NDArray operator +(double lhs, NDArray rhs) => nd.add(nd.full_like(rhs, lhs), rhs);

        public static NDArray operator -(NDArray lhs, NDArray rhs) => nd.subtract(lhs, rhs);

        public static NDArray operator -(NDArray lhs, double rhs) => nd.subtract(lhs, nd.full_like(lhs, rhs));

        public static NDArray operator -(double lhs, NDArray rhs) => nd.subtract(nd.full_like(rhs, lhs), rhs);

        public static NDArray operator *(NDArray lhs, NDArray rhs) => nd.multiply(lhs, rhs);

        public static NDArray operator *(NDArray lhs, double rhs) => nd.multiply(lhs, nd.full_like(lhs, rhs));

        public static NDArray operator *(double lhs, NDArray rhs) => nd.multiply(nd.full_like(rhs, lhs), rhs);

        public static NDArray operator /(NDArray lhs, NDArray rhs) => nd.divide(lhs, rhs);

        public static NDArray operator /(NDArray lhs, double rhs) => nd.divide(lhs, nd.full_like(lhs, rhs));

        public static NDArray operator /(double lhs, NDArray rhs) => nd.divide(nd.full_like(rhs, lhs), rhs);

        public static NDArray operator >(NDArray lhs, NDArray rhs) => nd.greater(lhs, rhs);

        public static NDArray operator >(NDArray lhs, double rhs) => nd.greater(lhs, nd.full_like(lhs, rhs));

        public static NDArray operator >(double lhs, NDArray rhs) => nd.greater(nd.full_like(rhs, lhs), rhs);

        public static NDArray operator <(NDArray lhs, NDArray rhs) => nd.less(lhs, rhs);

        public static NDArray operator <(NDArray lhs, double rhs) => nd.less(lhs, nd.full_like(lhs, rhs));

        public static NDArray operator <(double lhs, NDArray rhs) => nd.less(nd.full_like(rhs, lhs), rhs);

        public static NDArray operator >=(NDArray lhs, NDArray rhs) => nd.greater_equal(lhs, rhs);

        public static NDArray operator >=(NDArray lhs, double rhs) => nd.greater_equal(lhs, nd.full_like(lhs, rhs));

        public static NDArray operator >=(double lhs, NDArray rhs) => nd.greater_equal(nd.full_like(rhs, lhs), rhs);

        public static NDArray operator <=(NDArray lhs, NDArray rhs) => nd.less_equal(lhs, rhs);

        public static NDArray operator <=(NDArray lhs, double rhs) => nd.less_equal(lhs, nd.full_like(lhs, rhs));

        public static NDArray operator <=(double lhs, NDArray rhs) => nd.less_equal(nd.full_like(rhs, lhs), rhs);

        public static NDArray operator ==(NDArray lhs, NDArray rhs) => nd.equal(lhs, rhs);

        public static NDArray operator ==(NDArray lhs, double rhs) => nd.equal(lhs, nd.full_like(lhs, rhs));

        public static NDArray operator ==(double lhs, NDArray rhs) => nd.equal(nd.full_like(rhs, lhs), rhs);

        public static NDArray operator !=(NDArray lhs, NDArray rhs) => nd.not_equal(lhs, rhs);

        public static NDArray operator !=(NDArray lhs, double rhs) => nd.not_equal(lhs, nd.full_like(lhs, rhs));

        public static NDArray operator !=(double lhs, NDArray rhs) => nd.not_equal(nd.full_like(rhs, lhs), rhs);

        public static NDArray operator -(NDArray x) => nd.negative(x);

        public static NDArray operator ~(NDArray x) => nd.abs(x);

        public static NDArray operator %(NDArray lhs, NDArray rhs) => nd.mod(lhs, rhs);

        public static NDArray operator %(NDArray lhs, double rhs) => nd.mod(lhs, nd.full_like(lhs, rhs));

        public static NDArray operator %(double lhs, NDArray rhs) => nd.mod(nd.full_like(rhs, lhs), rhs);

        public static NDArray operator ^(NDArray lhs, NDArray rhs) => nd.logical_xor(lhs, rhs);

        public static NDArray operator ^(NDArray lhs, double rhs) => nd.logical_xor(lhs, nd.full_like(lhs, rhs));

        public static NDArray operator ^(double lhs, NDArray rhs) => nd.logical_xor(nd.full_like(rhs, lhs), rhs);

        public static NDArray operator &(NDArray lhs, NDArray rhs) => nd.bitwise_and(lhs, rhs);

        public static NDArray operator &(NDArray lhs, double rhs) => nd.bitwise_and(lhs, nd.full_like(lhs, rhs));

        public static NDArray operator &(double lhs, NDArray rhs) => nd.bitwise_and(nd.full_like(rhs, lhs), rhs);

        public static NDArray operator |(NDArray lhs, NDArray rhs) => nd.bitwise_or(lhs, rhs);

        public static NDArray operator |(NDArray lhs, double rhs) => nd.bitwise_or(lhs, nd.full_like(lhs, rhs));

        public static NDArray operator |(double lhs, NDArray rhs) => nd.bitwise_or(nd.full_like(rhs, lhs), rhs);

        public static NDArray operator >>(NDArray lhs, int rhs) => nd.bitwise_right_shift(lhs, nd.full_like(lhs, rhs));

        public static NDArray operator <<(NDArray lhs, int rhs) => nd.bitwise_left_shift(lhs, nd.full_like(lhs, rhs));

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return NativePtr.ToInt32();
        }
    }
}
