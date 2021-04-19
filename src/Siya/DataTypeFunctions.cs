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
    }
}
