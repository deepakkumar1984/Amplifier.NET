using System;
using System.Collections.Generic;
using System.Text;

namespace Amplifier.OpenCL
{
    public struct size_t
    {
        public static implicit operator size_t(uint d)
        {
            return new size_t();
        }
    }

    public struct ptrdiff_t
    {
        public static implicit operator ptrdiff_t(uint d)
        {
            return new ptrdiff_t();
        }
    }

    public struct intptr_t
    {
        public static implicit operator intptr_t(uint d)
        {
            return new intptr_t();
        }
    }

    public struct uintptr_t
    {
        public static implicit operator uintptr_t(uint d)
        {
            return new uintptr_t();
        }
    }

    public struct half
    {
        private ushort Value;

        public override string ToString()
        {
            return ((float)this).ToString();
        }

        public static explicit operator half(float d)
        {
            return new half(d);
        }

        public unsafe static explicit operator float(half d)
        {
            bool isPos          = (d.Value & Float16Params.SignMask) == 0;
            uint biasedExponent = (d.Value & Float16Params.ExpMask) >> Float16Params.ExpOffset;
            uint frac           = (d.Value & Float16Params.FracMask);
            bool isInf          = biasedExponent == Float16Params.BiasedExpMax && (frac == 0);

            if (isInf)
            {
                return isPos ? float.PositiveInfinity : float.NegativeInfinity;
            }

            bool isNan = biasedExponent == Float16Params.BiasedExpMax && (frac != 0);
            if (isNan)
            {
                return float.NaN;
            }

            bool isSubnormal = biasedExponent == 0;
            if (isSubnormal)
            {
                return frac * Float16Params.SmallestSubnormalAsFloat * (isPos ? 1.0f : -1.0f);
            }

            int unbiasedExp         = (int)biasedExponent - Float16Params.ExpBias;
            uint biasedF32Exponent  = (uint)(unbiasedExp + Float32Params.ExpBias);

            uint bits;

            bits = (isPos ? 0u : 1u << Float32Params.SignOffset)
                    | (biasedF32Exponent << Float32Params.ExpOffset)
                    | (frac << (Float32Params.ExpOffset - Float16Params.ExpOffset));

            return *(float*)&bits;
        }

        public unsafe half(float d)
        {
            uint bits = *(uint*)&d;

            uint fAbsBits = bits & Float32Params.AbsValueMask;
            bool isNeg    = (bits & Float32Params.SignBitMask) != 0;
            uint sign     = (bits & Float32Params.SignBitMask) >> (Float16Params.NumFracBits + Float16Params.NumExpBits + 1);
            uint half;

            if (float.IsNaN(d))
            {
                half = (Float16Params.ExpMask | Float16Params.FracMask);
            }
            else if (float.IsInfinity(d))
            {
                half = isNeg ? Float16Params.SignMask | Float16Params.ExpMask : Float16Params.ExpMask;
            }
            else if (fAbsBits > Float16Params.MaxNormal)
            {
                // Clamp to max float 16 value
                half = sign | (((1 << Float16Params.NumExpBits) - 1) << Float16Params.NumFracBits) | Float16Params.FracMask;
            }
            else if (fAbsBits < Float16Params.MinNormal)
            {
                uint fracBits    = (fAbsBits & Float32Params.MantissaMask) | (1 << Float32Params.NumMantissaBits);
                int nshift       = Float16Params.Emin + Float32Params.Emax - (int)(fAbsBits >> Float32Params.NumMantissaBits);
                uint shiftedBits = nshift < 24 ? fracBits >> nshift : 0;
                half             = sign | (shiftedBits >> Float16Params.FracBitsDiff);
            }
            else
            {
                half = sign | ((fAbsBits + Float16Params.BiasDiff) >> Float16Params.FracBitsDiff);
            }
            this.Value = (ushort)half;
        }

        private static class Float16Params
        {
            public const uint BitSize = 16;                                                   // total number of bits in the representation
            public const int NumFracBits = 10;                                                // number of fractional (mantissa) bits
            public const int NumExpBits = 5;                                                  // number of (biased) exponent bits
            public const uint SignBit = 15;                                                   // position of the sign bit
            public const uint SignMask = 1 << 15;                                             // mask to extract sign bit
            public const uint FracMask = (1 << 10) - 1;                                       // mask to extract the fractional (mantissa) bits
            public const uint ExpMask = ((1 << 5) - 1) << 10;                                 // mask to extract the exponent bits
            public const uint Emax = (1 << (5 - 1)) - 1;                                      // max value for the exponent
            public const int Emin = -((1 << (5 - 1)) - 1) + 1;                                // min value for the exponent
            public const uint MaxNormal = ((((1 << (5 - 1)) - 1) + 127) << 23) | 0x7FE000;    // max value that can be represented by the 16 bit float
            public const uint MinNormal = ((-((1 << (5 - 1)) - 1) + 1) + 127) << 23;          // min value that can be represented by the 16 bit float
            public const uint BiasDiff = unchecked((uint)(((1 << (5 - 1)) - 1) - 127) << 23); // difference in bias between the float16 and float32 exponent
            public const int FracBitsDiff = 23 - 10;                                          // difference in number of fractional bits between float16/float32

            public const int ExpBias = 15;
            public const int ExpOffset = 10;
            public const ushort BiasedExpMax = (1 << 5) - 1;
            public const float SmallestSubnormalAsFloat = 5.96046448e-8f;
        }

        private static class Float32Params
        {
            public const uint AbsValueMask = 0x7FFFFFFF; // ANDing with this value gives the abs value
            public const uint SignBitMask = 0x80000000;  // ANDing with this value gives the sign
            public const int Emax = 127;                 // max value for the exponent
            public const int NumMantissaBits = 23;       // 23 bit mantissa on single precision floats
            public const uint MantissaMask = 0x007FFFFF; // 23 bit mantissa on single precision floats

            public const int SignOffset = 31;
            public const int ExpBias = 127;
            public const int ExpOffset = 23;
        }
    }
}
