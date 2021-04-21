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
        public static DType int8 => DType.Int8;

        public static DType int16 => DType.Int16;

        public static DType int32 => DType.Int32;

        public static DType int64 => DType.Int64;

        public static DType uint8 => DType.UInt8;

        public static DType uint16 => DType.UInt16;

        public static DType uint32 => DType.UInt32;

        public static DType uint64 => DType.UInt64;

        public static DType floar32 => DType.Float32;

        public static DType float64 => DType.Float64;

        public static DType @bool => DType.Bool;

        public static (int, float, float, float) finfo(DType dtype) => throw new NotImplementedException();

        public static (int, float, float, float) finfo(NDArray obj) => throw new NotImplementedException();

        public static (int, int, int) iinfo(DType dtype) => throw new NotImplementedException();

        public static (int, int, int) iinfo(NDArray obj) => throw new NotImplementedException();

        public static DType result_type(DType[] dtypes) => throw new NotImplementedException();

        public static DType result_type(NDArray[] objects) => throw new NotImplementedException();

        public static NDArray astype(NDArray x, DType dtype)
        {
            if (x.dtype == dtype)
                return x;

            var array = (Array)x.data.Clone();
            switch (dtype)
            {
                case DType.Float32:
                    array = array.Cast<float>().ToArray();
                    break;
                case DType.Float64:
                    array = array.Cast<double>().ToArray();
                    break;
                case DType.Int8:
                    array = array.Cast<sbyte>().ToArray();
                    break;
                case DType.Int16:
                    array = array.Cast<short>().ToArray();
                    break;
                case DType.Int32:
                    array = array.Cast<int>().ToArray();
                    break;
                case DType.Int64:
                    array = array.Cast<long>().ToArray();
                    break;
                case DType.UInt8:
                    array = array.Cast<byte>().ToArray();
                    break;
                case DType.UInt16:
                    array = array.Cast<ushort>().ToArray();
                    break;
                case DType.UInt32:
                    array = array.Cast<uint>().ToArray();
                    break;
                case DType.UInt64:
                    array = array.Cast<ulong>().ToArray();
                    break;
                case DType.Bool:
                    array = array.Cast<bool>().ToArray();
                    break;
                default:
                    break;
            }

            return new NDArray(array).reshape(x.shape);
        }
    }
}
