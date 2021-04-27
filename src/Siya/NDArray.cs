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

        public static NDArray operator +(NDArray lhs, NDArray rhs) => sx.add(lhs, rhs);

        public static NDArray operator +(NDArray lhs, double rhs) => sx.add(lhs, sx.full_like(lhs, rhs));

        public static NDArray operator +(double lhs, NDArray rhs) => sx.add(sx.full_like(rhs, lhs), rhs);

        public static NDArray operator -(NDArray lhs, NDArray rhs) => sx.subtract(lhs, rhs);

        public static NDArray operator -(NDArray lhs, double rhs) => sx.subtract(lhs, sx.full_like(lhs, rhs));

        public static NDArray operator -(double lhs, NDArray rhs) => sx.subtract(sx.full_like(rhs, lhs), rhs);

        public static NDArray operator *(NDArray lhs, NDArray rhs) => sx.multiply(lhs, rhs);

        public static NDArray operator *(NDArray lhs, double rhs) => sx.multiply(lhs, sx.full_like(lhs, rhs));

        public static NDArray operator *(double lhs, NDArray rhs) => sx.multiply(sx.full_like(rhs, lhs), rhs);

        public static NDArray operator /(NDArray lhs, NDArray rhs) => sx.divide(lhs, rhs);

        public static NDArray operator /(NDArray lhs, double rhs) => sx.divide(lhs, sx.full_like(lhs, rhs));

        public static NDArray operator /(double lhs, NDArray rhs) => sx.divide(sx.full_like(rhs, lhs), rhs);

        public static NDArray operator >(NDArray lhs, NDArray rhs) => sx.greater(lhs, rhs);

        public static NDArray operator >(NDArray lhs, double rhs) => sx.greater(lhs, sx.full_like(lhs, rhs));

        public static NDArray operator >(double lhs, NDArray rhs) => sx.greater(sx.full_like(rhs, lhs), rhs);

        public static NDArray operator <(NDArray lhs, NDArray rhs) => sx.less(lhs, rhs);

        public static NDArray operator <(NDArray lhs, double rhs) => sx.less(lhs, sx.full_like(lhs, rhs));

        public static NDArray operator <(double lhs, NDArray rhs) => sx.less(sx.full_like(rhs, lhs), rhs);

        public static NDArray operator >=(NDArray lhs, NDArray rhs) => sx.greater_equal(lhs, rhs);

        public static NDArray operator >=(NDArray lhs, double rhs) => sx.greater_equal(lhs, sx.full_like(lhs, rhs));

        public static NDArray operator >=(double lhs, NDArray rhs) => sx.greater_equal(sx.full_like(rhs, lhs), rhs);

        public static NDArray operator <=(NDArray lhs, NDArray rhs) => sx.less_equal(lhs, rhs);

        public static NDArray operator <=(NDArray lhs, double rhs) => sx.less_equal(lhs, sx.full_like(lhs, rhs));

        public static NDArray operator <=(double lhs, NDArray rhs) => sx.less_equal(sx.full_like(rhs, lhs), rhs);

        public static NDArray operator ==(NDArray lhs, NDArray rhs) => sx.equal(lhs, rhs);

        public static NDArray operator ==(NDArray lhs, double rhs) => sx.equal(lhs, sx.full_like(lhs, rhs));

        public static NDArray operator ==(double lhs, NDArray rhs) => sx.equal(sx.full_like(rhs, lhs), rhs);

        public static NDArray operator !=(NDArray lhs, NDArray rhs) => sx.not_equal(lhs, rhs);

        public static NDArray operator !=(NDArray lhs, double rhs) => sx.not_equal(lhs, sx.full_like(lhs, rhs));

        public static NDArray operator !=(double lhs, NDArray rhs) => sx.not_equal(sx.full_like(rhs, lhs), rhs);

        public static NDArray operator -(NDArray x) => sx.negative(x);

        public static NDArray operator ~(NDArray x) => sx.abs(x);

        public static NDArray operator %(NDArray lhs, NDArray rhs) => sx.mod(lhs, rhs);

        public static NDArray operator %(NDArray lhs, double rhs) => sx.mod(lhs, sx.full_like(lhs, rhs));

        public static NDArray operator %(double lhs, NDArray rhs) => sx.mod(sx.full_like(rhs, lhs), rhs);

        public static NDArray operator ^(NDArray lhs, NDArray rhs) => sx.logical_xor(lhs, rhs);

        public static NDArray operator ^(NDArray lhs, double rhs) => sx.logical_xor(lhs, sx.full_like(lhs, rhs));

        public static NDArray operator ^(double lhs, NDArray rhs) => sx.logical_xor(sx.full_like(rhs, lhs), rhs);

        public static NDArray operator &(NDArray lhs, NDArray rhs) => sx.bitwise_and(lhs, rhs);

        public static NDArray operator &(NDArray lhs, double rhs) => sx.bitwise_and(lhs, sx.full_like(lhs, rhs));

        public static NDArray operator &(double lhs, NDArray rhs) => sx.bitwise_and(sx.full_like(rhs, lhs), rhs);

        public static NDArray operator |(NDArray lhs, NDArray rhs) => sx.bitwise_or(lhs, rhs);

        public static NDArray operator |(NDArray lhs, double rhs) => sx.bitwise_or(lhs, sx.full_like(lhs, rhs));

        public static NDArray operator |(double lhs, NDArray rhs) => sx.bitwise_or(sx.full_like(rhs, lhs), rhs);

        public static NDArray operator >>(NDArray lhs, int rhs) => sx.bitwise_right_shift(lhs, sx.full_like(lhs, rhs));

        public static NDArray operator <<(NDArray lhs, int rhs) => sx.bitwise_left_shift(lhs, sx.full_like(lhs, rhs));

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
