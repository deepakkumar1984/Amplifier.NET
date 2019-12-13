using System;
using System.Collections.Generic;
using System.Text;

namespace Amplifier.OpenCL
{
    /// <summary>
    /// Please download the full method specification from here: https://www.khronos.org/registry/OpenCL/specs/2.2/pdf/OpenCL_C.pdf
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class OpenCLFunctions
    {
        public T shuffle<T>(T x, dynamic mask) where T : struct
        {
            return x;
        }

        public T shuffle2<T>(T x, dynamic mask) where T : struct
        {
            return x;
        }

        public int vec_step<T>(T a) where T : struct
        {
            return 0;
        }

        public T vload2<T>(int offset, T[] p) where T : struct
        {
            return default(T);
        }

        public T vload3<T>(int offset, T[] p) where T : struct
        {
            return default(T);
        }

        public T vload4<T>(int offset, T[] p) where T : struct
        {
            return default(T);
        }

        public T vload8<T>(int offset, T[] p) where T : struct
        {
            return default(T);
        }

        public T vload16<T>(int offset, T[] p) where T : struct
        {
            return default(T);
        }

        public void vstore2<T>(T data, int offset, T[] p) where T : struct
        {
        }

        public void vstore3<T>(T data, int offset, T[] p) where T : struct
        {
        }

        public void vstore4<T>(T data, int offset, T[] p) where T : struct
        {
        }

        public void vstore8<T>(T data, int offset, T[] p) where T : struct
        {
        }

        public void vstore16<T>(T data, int offset, T[] p) where T : struct
        {
        }

        public float vload_half(int offset, half[] p)
        {
            return 0f;
        }

        public float2 vload_half2(int offset, half[] p)
        {
            return new float2();
        }

        public float3 vload_half3(int offset, half[] p)
        {
            return new float3();
        }

        public float4 vload_half4(int offset, half[] p)
        {
            return new float4();
        }

        public float8 vload_half8(int offset, half[] p)
        {
            return new float8();
        }

        public float16 vload_half16(int offset, half[] p)
        {
            return new float16();
        }

        public float2 vloada_half2(int offset, half[] p)
        {
            return new float2();
        }

        public float3 vloada_half3(int offset, half[] p)
        {
            return new float3();
        }

        public float4 vloada_half4(int offset, half[] p)
        {
            return new float4();
        }

        public float8 vloada_half8(int offset, half[] p)
        {
            return new float8();
        }

        public float16 vloada_half16(int offset, half[] p)
        {
            return new float16();
        }

        public void vstore_half(float data, int offset, half[] p)
        {
        }

        public void vstore_half2(float2 data, int offset, half[] p)
        {
        }

        public void vstore_half3(float3 data, int offset, half[] p)
        {
        }

        public void vstore_half4(float4 data, int offset, half[] p)
        {
        }

        public void vstore_half8(float8 data, int offset, half[] p)
        {
        }

        public void vstore_half16(float16 data, int offset, half[] p)
        {
        }

        public void vstore_half2_rte(float2 data, int offset, half[] p)
        {
        }

        public void vstore_half3_rte(float3 data, int offset, half[] p)
        {
        }

        public void vstore_half4_rte(float4 data, int offset, half[] p)
        {
        }

        public void vstore_half8_rte(float8 data, int offset, half[] p)
        {
        }

        public void vstore_half16_rte(float16 data, int offset, half[] p)
        {
        }

        public void vstore_half2_rtz(float2 data, int offset, half[] p)
        {
        }

        public void vstore_half3_rtz(float3 data, int offset, half[] p)
        {
        }

        public void vstore_half4_rtz(float4 data, int offset, half[] p)
        {
        }

        public void vstore_half8_rtz(float8 data, int offset, half[] p)
        {
        }

        public void vstore_half16_rtz(float16 data, int offset, half[] p)
        {
        }

        public void vstore_half2_rtp(float2 data, int offset, half[] p)
        {
        }

        public void vstore_half3_rtp(float3 data, int offset, half[] p)
        {
        }

        public void vstore_half4_rtp(float4 data, int offset, half[] p)
        {
        }

        public void vstore_half8_rtp(float8 data, int offset, half[] p)
        {
        }

        public void vstore_half16_rtp(float16 data, int offset, half[] p)
        {
        }

        public void vstore_half2_rtn(float2 data, int offset, half[] p)
        {
        }

        public void vstore_half3_rtn(float3 data, int offset, half[] p)
        {
        }

        public void vstore_half4_rtn(float4 data, int offset, half[] p)
        {
        }

        public void vstore_half8_rtn(float8 data, int offset, half[] p)
        {
        }

        public void vstore_half16_rtn(float16 data, int offset, half[] p)
        {
        }

        public void vstorea_half2(float2 data, int offset, half[] p)
        {
        }

        public void vstorea_half3(float3 data, int offset, half[] p)
        {
        }

        public void vstorea_half4(float4 data, int offset, half[] p)
        {
        }

        public void vstorea_half8(float8 data, int offset, half[] p)
        {
        }

        public void vstorea_half16(float16 data, int offset, half[] p)
        {
        }

        public void vstorea_half2_rte(float2 data, int offset, half[] p)
        {
        }

        public void vstorea_half3_rte(float3 data, int offset, half[] p)
        {
        }

        public void vstorea_half4_rte(float4 data, int offset, half[] p)
        {
        }

        public void vstorea_half8_rte(float8 data, int offset, half[] p)
        {
        }

        public void vstorea_half16_rte(float16 data, int offset, half[] p)
        {
        }

        public void vstorea_half2_rtz(float2 data, int offset, half[] p)
        {
        }

        public void vstorea_half3_rtz(float3 data, int offset, half[] p)
        {
        }

        public void vstorea_half4_rtz(float4 data, int offset, half[] p)
        {
        }

        public void vstorea_half8_rtz(float8 data, int offset, half[] p)
        {
        }

        public void vstorea_half16_rtz(float16 data, int offset, half[] p)
        {
        }

        public void vstorea_half2_rtp(float2 data, int offset, half[] p)
        {
        }

        public void vstorea_half3_rtp(float3 data, int offset, half[] p)
        {
        }

        public void vstorea_half4_rtp(float4 data, int offset, half[] p)
        {
        }

        public void vstorea_half8_rtp(float8 data, int offset, half[] p)
        {
        }

        public void vstorea_half16_rtp(float16 data, int offset, half[] p)
        {
        }

        public void vstorea_half2_rtn(float2 data, int offset, half[] p)
        {
        }

        public void vstorea_half3_rtn(float3 data, int offset, half[] p)
        {
        }

        public void vstorea_half4_rtn(float4 data, int offset, half[] p)
        {
        }

        public void vstorea_half8_rtn(float8 data, int offset, half[] p)
        {
        }

        public void vstorea_half16_rtn(float16 data, int offset, half[] p)
        {
        }

        public void vstore_half(double data, int offset, half[] p)
        {
        }

        public void vstore_half2(double2 data, int offset, half[] p)
        {
        }

        public void vstore_half3(double3 data, int offset, half[] p)
        {
        }

        public void vstore_half4(double4 data, int offset, half[] p)
        {
        }

        public void vstore_half8(double8 data, int offset, half[] p)
        {
        }

        public void vstore_half16(double16 data, int offset, half[] p)
        {
        }

        public void vstore_half2_rte(double2 data, int offset, half[] p)
        {
        }

        public void vstore_half3_rte(double3 data, int offset, half[] p)
        {
        }

        public void vstore_half4_rte(double4 data, int offset, half[] p)
        {
        }

        public void vstore_half8_rte(double8 data, int offset, half[] p)
        {
        }

        public void vstore_half16_rte(double16 data, int offset, half[] p)
        {
        }

        public void vstore_half2_rtz(double2 data, int offset, half[] p)
        {
        }

        public void vstore_half3_rtz(double3 data, int offset, half[] p)
        {
        }

        public void vstore_half4_rtz(double4 data, int offset, half[] p)
        {
        }

        public void vstore_half8_rtz(double8 data, int offset, half[] p)
        {
        }

        public void vstore_half16_rtz(double16 data, int offset, half[] p)
        {
        }

        public void vstore_half2_rtp(double2 data, int offset, half[] p)
        {
        }

        public void vstore_half3_rtp(double3 data, int offset, half[] p)
        {
        }

        public void vstore_half4_rtp(double4 data, int offset, half[] p)
        {
        }

        public void vstore_half8_rtp(double8 data, int offset, half[] p)
        {
        }

        public void vstore_half16_rtp(double16 data, int offset, half[] p)
        {
        }

        public void vstore_half2_rtn(double2 data, int offset, half[] p)
        {
        }

        public void vstore_half3_rtn(double3 data, int offset, half[] p)
        {
        }

        public void vstore_half4_rtn(double4 data, int offset, half[] p)
        {
        }

        public void vstore_half8_rtn(double8 data, int offset, half[] p)
        {
        }

        public void vstore_half16_rtn(double16 data, int offset, half[] p)
        {
        }

        public void vstorea_half2(double2 data, int offset, half[] p)
        {
        }

        public void vstorea_half3(double3 data, int offset, half[] p)
        {
        }

        public void vstorea_half4(double4 data, int offset, half[] p)
        {
        }

        public void vstorea_half8(double8 data, int offset, half[] p)
        {
        }

        public void vstorea_half16(double16 data, int offset, half[] p)
        {
        }

        public void vstorea_half2_rte(double2 data, int offset, half[] p)
        {
        }

        public void vstorea_half3_rte(double3 data, int offset, half[] p)
        {
        }

        public void vstorea_half4_rte(double4 data, int offset, half[] p)
        {
        }

        public void vstorea_half8_rte(double8 data, int offset, half[] p)
        {
        }

        public void vstorea_half16_rte(double16 data, int offset, half[] p)
        {
        }

        public void vstorea_half2_rtz(double2 data, int offset, half[] p)
        {
        }

        public void vstorea_half3_rtz(double3 data, int offset, half[] p)
        {
        }

        public void vstorea_half4_rtz(double4 data, int offset, half[] p)
        {
        }

        public void vstorea_half8_rtz(double8 data, int offset, half[] p)
        {
        }

        public void vstorea_half16_rtz(double16 data, int offset, half[] p)
        {
        }

        public void vstorea_half2_rtp(double2 data, int offset, half[] p)
        {
        }

        public void vstorea_half3_rtp(double3 data, int offset, half[] p)
        {
        }

        public void vstorea_half4_rtp(double4 data, int offset, half[] p)
        {
        }

        public void vstorea_half8_rtp(double8 data, int offset, half[] p)
        {
        }

        public void vstorea_half16_rtp(double16 data, int offset, half[] p)
        {
        }

        public void vstorea_half2_rtn(double2 data, int offset, half[] p)
        {
        }

        public void vstorea_half3_rtn(double3 data, int offset, half[] p)
        {
        }

        public void vstorea_half4_rtn(double4 data, int offset, half[] p)
        {
        }

        public void vstorea_half8_rtn(double8 data, int offset, half[] p)
        {
        }

        public void vstorea_half16_rtn(double16 data, int offset, half[] p)
        {
        }
    }
}
