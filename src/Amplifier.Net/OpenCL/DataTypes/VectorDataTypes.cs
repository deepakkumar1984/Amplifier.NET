using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Amplifier.OpenCL
{
    public interface IVecDType
    {

    }

    public struct uint2 : IVecDType
    {
        public static implicit operator uint2((uint, uint) d)
        {
            return new uint2();
        }
    }

    public struct uint3 : IVecDType
    {
        public static implicit operator uint3((uint, uint, uint) d)
        {
            return new uint3();
        }
    }

    public struct uint4
    {
        public static implicit operator uint4((uint, uint, uint, uint) d)
        {
            return new uint4();
        }
    }

    public struct uint8
    {
        public static implicit operator uint8((uint, uint, uint, uint, uint, uint, uint, uint) d)
        {
            return new uint8();
        }
    }

    public struct uint16
    {
        public static implicit operator uint16((uint, uint, uint, uint, uint, uint, uint, uint, uint, uint, uint, uint, uint, uint, uint, uint) d)
        {
            return new uint16();
        }
    }

    public struct int2
    {
        public static implicit operator int2((int, int) d)
        {
            return new int2();
        }
    }

    public struct int3
    {
        public static implicit operator int3((int, int, int) d)
        {
            return new int3();
        }
    }

    public struct int4
    {
        public static implicit operator int4((int, int, int, int) d)
        {
            return new int4();
        }
    }

    public struct int8
    {
        public static implicit operator int8((int, int, int, int, int, int, int, int) d)
        {
            return new int8();
        }
    }

    public struct int16
    {
        public static implicit operator int16((int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int) d)
        {
            return new int16();
        }
    }

    public struct long2
    {
        public static implicit operator long2((long, long) d)
        {
            return new long2();
        }
    }

    public struct long3
    {
        public static implicit operator long3((long, long, long) d)
        {
            return new long3();
        }
    }

    public struct long4
    {
        public static implicit operator long4((long, long, long, long) d)
        {
            return new long4();
        }
    }

    public struct long8
    {
        public static implicit operator long8((long, long, long, long, long, long, long, long) d)
        {
            return new long8();
        }
    }

    public struct long16
    {
        public static implicit operator long16((long, long, long, long, long, long, long, long, long, long, long, long, long, long, long, long) d)
        {
            return new long16();
        }
    }

    public struct ulong2
    {
        public static implicit operator ulong2((ulong, ulong) d)
        {
            return new ulong2();
        }
    }

    public struct ulong3
    {
        public static implicit operator ulong3((ulong, ulong, ulong) d)
        {
            return new ulong3();
        }
    }

    public struct ulong4
    {
        public static implicit operator ulong4((ulong, ulong, ulong, ulong) d)
        {
            return new ulong4();
        }
    }

    public struct ulong8
    {
        public static implicit operator ulong8((ulong, ulong, ulong, ulong, ulong, ulong, ulong, ulong) d)
        {
            return new ulong8();
        }
    }

    public struct ulong16
    {
        public static implicit operator ulong16((ulong, ulong, ulong, ulong, ulong, ulong, ulong, ulong, ulong, ulong, ulong, ulong, ulong, ulong, ulong, ulong) d)
        {
            return new ulong16();
        }
    }

    public struct float2
    {
        public static implicit operator float2((float, float) d)
        {
            return new float2();
        }
    }

    public struct float3
    {
        public static implicit operator float3((float, float, float) d)
        {
            return new float3();
        }
    }

    public struct float4
    {
        public static implicit operator float4((float, float, float, float) d)
        {
            return new float4();
        }
    }

    public struct float8
    {
        public static implicit operator float8((float, float, float, float, float, float, float, float) d)
        {
            return new float8();
        }
    }

    public struct float16
    {
        public static implicit operator float16((float, float, float, float, float, float, float, float, float, float, float, float, float, float, float, float) d)
        {
            return new float16();
        }
    }

    public struct double2
    {
        public static implicit operator double2((double, double) d)
        {
            return new double2();
        }
    }

    public struct double3
    {
        public static implicit operator double3((double, double, double) d)
        {
            return new double3();
        }
    }

    public struct double4
    {
        public static implicit operator double4((double, double, double, double) d)
        {
            return new double4();
        }
    }

    public struct double8
    {
        public static implicit operator double8((double, double, double, double, double, double, double, double) d)
        {
            return new double8();
        }
    }

    public struct double16
    {
        public static implicit operator double16((double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double) d)
        {
            return new double16();
        }
    }

    public struct short2
    {
        public static implicit operator short2((short, short) d)
        {
            return new short2();
        }
    }

    public struct short3
    {
        public static implicit operator short3((short, short, short) d)
        {
            return new short3();
        }
    }

    public struct short4
    {
        public static implicit operator short4((short, short, short, short) d)
        {
            return new short4();
        }
    }

    public struct short8
    {
        public static implicit operator short8((short, short, short, short, short, short, short, short) d)
        {
            return new short8();
        }
    }

    public struct short16
    {
        public static implicit operator short16((short, short, short, short, short, short, short, short, short, short, short, short, short, short, short, short) d)
        {
            return new short16();
        }
    }
}
