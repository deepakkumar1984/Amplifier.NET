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

        public NDArray T => throw new NotImplementedException();

        public NDArray(Array data, Device device = null) : base(data, Direction.Input)
        {
        }

        public NDArray(Shape shape, DType dtype = DType.Float32, Device device = null) : base(shape.Data.ToArray(), dtype, Direction.Input)
        {
        }

        public static NDArray operator +(NDArray lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator +(NDArray lhs, double rhs) => throw new NotImplementedException();

        public static NDArray operator +(double lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator -(NDArray lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator -(NDArray lhs, double rhs) => throw new NotImplementedException();

        public static NDArray operator -(double lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator *(NDArray lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator *(NDArray lhs, double rhs) => throw new NotImplementedException();

        public static NDArray operator *(double lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator /(NDArray lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator /(NDArray lhs, double rhs) => throw new NotImplementedException();

        public static NDArray operator /(double lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator >(NDArray lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator >(NDArray lhs, double rhs) => throw new NotImplementedException();

        public static NDArray operator <(NDArray lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator <(NDArray lhs, double rhs) => throw new NotImplementedException();

        public static NDArray operator >=(NDArray lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator >=(NDArray lhs, double rhs) => throw new NotImplementedException();

        public static NDArray operator <=(NDArray lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator <=(NDArray lhs, double rhs) => throw new NotImplementedException();

        public static NDArray operator ==(NDArray lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator ==(NDArray lhs, double rhs) => throw new NotImplementedException();

        public static NDArray operator ==(double lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator !=(NDArray lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator !=(NDArray lhs, double rhs) => throw new NotImplementedException();

        public static NDArray operator !=(double lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator -(NDArray x) => throw new NotImplementedException();

        public static NDArray operator ~(NDArray x) => throw new NotImplementedException();

        public static NDArray operator %(NDArray lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator %(NDArray lhs, double rhs) => throw new NotImplementedException();

        public static NDArray operator %(double lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator ^(NDArray lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator ^(NDArray lhs, double rhs) => throw new NotImplementedException();

        public static NDArray operator ^(double lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator &(NDArray lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator &(NDArray lhs, double rhs) => throw new NotImplementedException();

        public static NDArray operator &(double lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator |(NDArray lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator |(NDArray lhs, double rhs) => throw new NotImplementedException();

        public static NDArray operator |(double lhs, NDArray rhs) => throw new NotImplementedException();

        public static NDArray operator >>(NDArray lhs, int rhs) => throw new NotImplementedException();

        public static NDArray operator <<(NDArray lhs, int rhs) => throw new NotImplementedException();

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
            throw new NotImplementedException();
        }
    }
}
