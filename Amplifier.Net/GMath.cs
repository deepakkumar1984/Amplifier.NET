/*
Amplifier.NET - LGPL 2.1 License
Please consider purchasing a commerical license - it helps development, frees you from LGPL restrictions
and provides you with support.  Thank you!
Copyright (C) 2011 Hybrid DSP Systems
http://www.hybriddsp.com

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplifier
{
#warning TODO complete float versions where necessary    
    
    /// <summary>
    /// Many of the .NET math methods are double only.  When single point (float) is used 
    /// this results in an unwanted cast to double. 
    /// </summary>
    public static class GMath
    {

        /// <summary>
        /// Returns the absolute value of a single precision floating point number. For OpenCL compatibility, first cast
        /// value to an integer.
        /// </summary>
        /// <param name="value">The value to find absolute value of.</param>
        /// <returns>Absolute of specified value.</returns>
        public static float Abs(float value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Returns the absolute value of a single precision floating point number.  For OpenCL compatibility, first cast
        /// value to an integer.
        /// </summary>
        /// <param name="value">The value to find absolute value of.</param>
        /// <returns>Absolute of specified value.</returns>
        public static float Abs(int value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Returns the absolute value of a single precision floating point number.  For OpenCL compatibility, first cast
        /// value to an integer.
        /// </summary>
        /// <param name="value">The value to find absolute value of.</param>
        /// <returns>Absolute of specified value.</returns>
        public static float Abs(long value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Returns the square root of a specified number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float Sqrt(float value)
        {
            return (float)Math.Sqrt(value);
        }

        /// <summary>
        /// Returns the cosine of the specified angle. 
        /// </summary>
        /// <param name="value">An angle, measured in radians.</param>
        /// <returns>The cosine of value. If value is equal to NaN, NegativeInfinity, or PositiveInfinity, this method returns NaN.</returns>
        public static float Cos(float value)
        {
            return (float)Math.Cos(value);
        }

        /// <summary>
        /// Acoses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static float Acos(float value)
        {
            return (float)Math.Acos(value);
        }

        /// <summary>
        /// Returns the hyperbolic cosine of the specified angle.
        /// </summary>
        /// <param name="value">An angle, measured in radians.</param>
        /// <returns>The hyperbolic cosine of value. If value is equal to NegativeInfinity or PositiveInfinity, PositiveInfinity is returned. If value is equal to NaN, NaN is returned.</returns>
        public static float Cosh(float value)
        {
            return (float)Math.Cosh(value);
        }

        /// <summary>
        /// Returns the sine of the specified angle. 
        /// </summary>
        /// <param name="value">An angle, measured in radians.</param>
        /// <returns>The sine of value. If value is equal to NaN, NegativeInfinity, or PositiveInfinity, this method returns NaN.</returns>
        public static float Sin(float value)
        {
            return (float)Math.Sin(value);
        }

        /// <summary>
        /// Returns the sine of the specified angle. 
        /// </summary>
        /// <param name="value">An angle, measured in radians.</param>
        /// <returns>The sine of value. If value is equal to NaN, NegativeInfinity, or PositiveInfinity, this method returns NaN.</returns>
        public static float Asin(float value)
        {
            return (float)Math.Asin(value);
        }

        /// <summary>
        /// Returns the hyperbolic sine of the specified angle. 
        /// </summary>
        /// <param name="value">An angle, measured in radians.</param>
        /// <returns>The hyperbolic sine of value. If value is equal to NaN, NegativeInfinity, or PositiveInfinity, this method returns NaN.</returns> 
        public static float Sinh(float value)
        {
            return (float)Math.Sinh(value);
        }

        /// <summary>
        /// Returns the tan of the specified angle. 
        /// </summary>
        /// <param name="value">An angle, measured in radians.</param>
        /// <returns>The tan of value.</returns>
        public static float Tan(float value)
        {
            return (float)Math.Tan(value);
        }

        /// <summary>
        /// Returns the tan of the specified angle. 
        /// </summary>
        /// <param name="value">An angle, measured in radians.</param>
        /// <returns>The tan of value.</returns>
        public static float Atan(float value)
        {
            return (float)Math.Atan(value);
        }

        /// <summary>
        /// Returns the angle whose tangent is the quotient of two specified numbers.
        /// </summary>
        /// <param name="y">The y coordinate of a point.</param>
        /// <param name="x">The x coordinate of a point.</param>
        /// <returns>Type: System.Double</returns>
        public static float Atan2(float y, float x)
        {
            return (float)Math.Atan2(y, x);
        }

        /// <summary>
        /// Returns hyperbolic the tangent of the specified angle. 
        /// </summary>
        /// <param name="value">An angle, measured in radians.</param>
        /// <returns>The hyperbolic the tangent of value.</returns>
        public static float Tanh(float value)
        {
            return (float)Math.Tanh(value);
        }

        /// <summary>
        /// Rounds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Rounded value.</returns>
        public static float Round(float value)
        {
            return (float)Math.Round(value);
        }

        /// <summary>
        /// Ceilings the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static float Ceiling(float value)
        {
            return (float)Math.Ceiling(value);
        }

        /// <summary>
        /// Floors the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static float Floor(float value)
        {
            return (float)Math.Floor(value);
        }

        /// <summary>
        /// Represents the ratio of the circumference of a circle to its diameter, specified by the constant, π.
        /// </summary>
        public const float PI = (float)Math.PI;

        /// <summary>
        /// Represents the natural logarithmic base, specified by the constant, e.
        /// </summary>
        public const float E = (float)Math.E;

        /// <summary>
        /// Returns the specified number raised to the specified power.
        /// </summary>
        /// <param name="x">Number to be raised to a power.</param>
        /// <param name="y">Number that specifies the power.</param>
        /// <returns>X to the power of y.</returns>
        public static float Pow(float x, float y)
        {
            return (float)Math.Pow(x, y);
        }

        /// <summary>
        /// Returns the base 10 log of the specified number.
        /// </summary>
        /// <param name="value">A number whose logarithm is to be found.</param>
        /// <returns>Result.</returns>
        public static float Log10(float value)
        {
            return (float)Math.Log10(value);
        }
#warning TODO Ensure that two args are not passed to Math.Log
        /// <summary>
        /// Returns the natural (base e) logarithm of a specified number.
        /// </summary>
        /// <param name="value">A number whose logarithm is to be found.</param>
        /// <returns>Result.</returns>
        public static float Log(float value)
        {
            return (float)Math.Log(value);
        }

        /// <summary>
        /// Returns e raised to the specified power.
        /// </summary>
        /// <param name="value">A number specifying a power. </param>
        /// <returns>The number e raised to the power d. If d equals NaN or PositiveInfinity, that value is returned. If d equals NegativeInfinity, 0 is returned.</returns>
        public static float Exp(float value)
        {
            return (float)Math.Exp(value);
        }


        /// <summary>
        /// Truncates the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static float Truncate(float value)
        {
            return (float)Math.Truncate(value);
        }

        /// <summary>
        /// Returns the larger of two single float precision numbers.
        /// </summary>
        /// <param name="x">The first number to compare.</param>
        /// <param name="y">The second number to compare.</param>
        /// <returns>The larger of the two numbers.</returns>
        public static float Max(float x, float y)
        {
            return (float)Math.Max(x, y);
        }

        /// <summary>
        /// Returns the smaller of two single float precision numbers.
        /// </summary>
        /// <param name="x">The first number to compare.</param>
        /// <param name="y">The second number to compare.</param>
        /// <returns>The smaller of the two numbers.</returns>
        public static float Min(float x, float y)
        {
            return (float)Math.Min(x, y);
        }

        ///// <summary>
        ///// Exp10s the specified value.
        ///// </summary>
        ///// <param name="value">The value.</param>
        ///// <returns></returns>
        //public static float Exp10(float value)
        //{
        //    return (float)Math.Pow(Math.E, value);
        //}

        ///// <summary>
        ///// Exp10s the specified d.
        ///// </summary>
        ///// <param name="d">The d.</param>
        ///// <returns></returns>
        //public static double Exp10(double d)
        //{
        //    return Math.Pow(Math.E, d);
        //}

//        __device__ ​ float acosf ( float  x )
//Calculate the arc cosine of the input argument.
//__device__ ​ float acoshf ( float  x )
//Calculate the nonnegative arc hyperbolic cosine of the input argument.
//__device__ ​ float asinf ( float  x )
//Calculate the arc sine of the input argument.
//__device__ ​ float asinhf ( float  x )
//Calculate the arc hyperbolic sine of the input argument.
//__device__ ​ float atan2f ( float  x, float  y )
//Calculate the arc tangent of the ratio of first and second input arguments.
//__device__ ​ float atanf ( float  x )
//Calculate the arc tangent of the input argument.
//__device__ ​ float atanhf ( float  x )
//Calculate the arc hyperbolic tangent of the input argument.
//__device__ ​ float cbrtf ( float  x )
//Calculate the cube root of the input argument.
//__device__ ​ float ceilf ( float  x )
//Calculate ceiling of the input argument.
//__device__ ​ float copysignf ( float  x, float  y )
//Create value with given magnitude, copying sign of second value.
//__device__ ​ float cosf ( float  x )
//Calculate the cosine of the input argument.
//__device__ ​ float coshf ( float  x )
//Calculate the hyperbolic cosine of the input argument.
//__device__ ​ float cospif ( float  x )
//Calculate the cosine of the input argument $\times \pi$ ×π.
//__device__ ​ float erfcf ( float  x )
//Calculate the complementary error function of the input argument.
//__device__ ​ float erfcinvf ( float  y )
//Calculate the inverse complementary error function of the input argument.
//__device__ ​ float erfcxf ( float  x )
//Calculate the scaled complementary error function of the input argument.
//__device__ ​ float erff ( float  x )
//Calculate the error function of the input argument.
//__device__ ​ float erfinvf ( float  y )
//Calculate the inverse error function of the input argument.
//__device__ ​ float exp10f ( float  x )
//Calculate the base 10 exponential of the input argument.
//__device__ ​ float exp2f ( float  x )
//Calculate the base 2 exponential of the input argument.
//__device__ ​ float expf ( float  x )
//Calculate the base $e$ e exponential of the input argument.
//__device__ ​ float expm1f ( float  x )
//Calculate the base $e$ e exponential of the input argument, minus 1.
//__device__ ​ float fabsf ( float  x )
//Calculate the absolute value of its argument.
//__device__ ​ float fdimf ( float  x, float  y )
//Compute the positive difference between x and y.
//__device__ ​ float fdividef ( float  x, float  y )
//Divide two floating point values.
//__device__ ​ float floorf ( float  x )
//Calculate the largest integer less than or equal to x.
//__device__ ​ float fmaf ( float  x, float  y, float  z )
//Compute $x \times y + z$ x×y+z as a single operation.
//__device__ ​ float fmaxf ( float  x, float  y )
//Determine the maximum numeric value of the arguments.
//__device__ ​ float fminf ( float  x, float  y )
//Determine the minimum numeric value of the arguments.
//__device__ ​ float fmodf ( float  x, float  y )
//Calculate the floating-point remainder of x / y.
//__device__ ​ float frexpf ( float  x, int* nptr )
//Extract mantissa and exponent of a floating-point value.
//__device__ ​ float hypotf ( float  x, float  y )
//Calculate the square root of the sum of squares of two arguments.
//__device__ ​ int ilogbf ( float  x )
//Compute the unbiased integer exponent of the argument.
//__device__ ​ int isfinite ( float  a )
//Determine whether argument is finite.
//__device__ ​ int isinf ( float  a )
//Determine whether argument is infinite.
//__device__ ​ int isnan ( float  a )
//Determine whether argument is a NaN.
//__device__ ​ float j0f ( float  x )
//Calculate the value of the Bessel function of the first kind of order 0 for the input argument.
//__device__ ​ float j1f ( float  x )
//Calculate the value of the Bessel function of the first kind of order 1 for the input argument.
//__device__ ​ float jnf ( int  n, float  x )
//Calculate the value of the Bessel function of the first kind of order n for the input argument.
//__device__ ​ float ldexpf ( float  x, int  exp )
//Calculate the value of $x\cdot 2^{exp}$ x⋅2exp.
//__device__ ​ float lgammaf ( float  x )
//Calculate the natural logarithm of the absolute value of the gamma function of the input argument.
//__device__ ​ long long int 	llrintf ( float  x )
//Round input to nearest integer value.
//__device__ ​ long long int 	llroundf ( float  x )
//Round to nearest integer value.
//__device__ ​ float log10f ( float  x )
//Calculate the base 10 logarithm of the input argument.
//__device__ ​ float log1pf ( float  x )
//Calculate the value of $log_{e}(1+x)$ $\lfloor x \rfloor$ loge(1+x).
//__device__ ​ float log2f ( float  x )
//Calculate the base 2 logarithm of the input argument.
//__device__ ​ float logbf ( float  x )
//Calculate the floating point representation of the exponent of the input argument.
//__device__ ​ float logf ( float  x )
//Calculate the natural logarithm of the input argument.
//__device__ ​ long int lrintf ( float  x )
//Round input to nearest integer value.
//__device__ ​ long int lroundf ( float  x )
//Round to nearest integer value.
//__device__ ​ float modff ( float  x, float* iptr )
//Break down the input argument into fractional and integral parts.
//__device__ ​ float nanf ( const char* tagp )
//Returns "Not a Number" value.
//__device__ ​ float nearbyintf ( float  x )
//Round the input argument to the nearest integer.
//__device__ ​ float nextafterf ( float  x, float  y )
//Return next representable single-precision floating-point value afer argument.
//__device__ ​ float normcdff ( float  y )
//Calculate the standard normal cumulative distribution function.
//__device__ ​ float normcdfinvf ( float  y )
//Calculate the inverse of the standard normal cumulative distribution function.
//__device__ ​ float powf ( float  x, float  y )
//Calculate the value of first argument to the power of second argument.
//__device__ ​ float rcbrtf ( float  x )
//Calculate reciprocal cube root function.
//__device__ ​ float remainderf ( float  x, float  y )
//Compute single-precision floating-point remainder.
//__device__ ​ float remquof ( float  x, float  y, int* quo )
//Compute single-precision floating-point remainder and part of quotient.
//__device__ ​ float rintf ( float  x )
//Round input to nearest integer value in floating-point.
//__device__ ​ float roundf ( float  x )
//Round to nearest integer value in floating-point.
//__device__ ​ float rsqrtf ( float  x )
//Calculate the reciprocal of the square root of the input argument.
//__device__ ​ float scalblnf ( float  x, long int  n )
//Scale floating-point input by integer power of two.
//__device__ ​ float scalbnf ( float  x, int  n )
//Scale floating-point input by integer power of two.
//__device__ ​ int signbit ( float  a )
//Return the sign bit of the input.
//__device__ ​ void sincosf ( float  x, float* sptr, float* cptr )
//Calculate the sine and cosine of the first input argument.
//__device__ ​ void sincospif ( float  x, float* sptr, float* cptr )
//Calculate the sine and cosine of the first input argument $\times \pi$ ×π.
//__device__ ​ float sinf ( float  x )
//Calculate the sine of the input argument.
//__device__ ​ float sinhf ( float  x )
//Calculate the hyperbolic sine of the input argument.
//__device__ ​ float sinpif ( float  x )
//Calculate the sine of the input argument $\times \pi$ ×π.
//__device__ ​ float sqrtf ( float  x )
//Calculate the square root of the input argument.
//__device__ ​ float tanf ( float  x )
//Calculate the tangent of the input argument.
//__device__ ​ float tanhf ( float  x )
//Calculate the hyperbolic tangent of the input argument.
//__device__ ​ float tgammaf ( float  x )
//Calculate the gamma function of the input argument.
//__device__ ​ float truncf ( float  x )
//Truncate input argument to the integral part.
//__device__ ​ float y0f ( float  x )
//Calculate the value of the Bessel function of the second kind of order 0 for the input argument.
//__device__ ​ float y1f ( float  x )
//Calculate the value of the Bessel function of the second kind of order 1 for the input argument.
//__device__ ​ float ynf ( int  n, float  x )
//Calculate the value of the Bessel function of the second kind of order n for the input argument.
    }
}
