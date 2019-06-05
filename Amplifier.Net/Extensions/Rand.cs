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

namespace Amplifier.Rand
{
    public static class RandomExt
    {
        #region curand

        //        __device__ unsigned long long curand (curandStateScrambledSobol64_t
        // state)
        public static ulong curand(this GThread thread, RandStateScrambledSobol64[] state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand");
        }

        // __device__ unsigned long long curand (curandStateSobol64_t  state)
        public static ulong curand(this GThread thread, RandStateSobol64[] state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand");
        }

        //__device__ unsigned int curand (curandStateScrambledSobol32_t 
        //state)
        public static uint curand(this GThread thread, RandStateScrambledSobol32[] state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand");
        }

        //__device__ unsigned int curand (curandStateSobol32_t  state)
        public static uint curand(this GThread thread, RandStateSobol32[] state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand");
        }

        // __device__ unsigned int curand (curandStateXORWOW_t  state)
        public static uint curand(this GThread thread, ref RandStateXORWOW state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand");
        }

        #endregion

        #region curand_init

//        __device__ void curand_init (curandDirectionVectors64_t
//direction_vectors, unsigned long long scramble_c, unsigned long long offset,
//curandStateScrambledSobol64_t  state)
        public static void curand_int(this GThread thread, RandDirectionVectors64 direction_vectors, ulong scramble_c, ulong offset, ref RandStateScrambledSobol64 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_init");
        }

//__device__ void curand_init (curandDirectionVectors64_t
//direction_vectors, unsigned long long offset, curandStateSobol64_t  state
        public static void curand_int(this GThread thread, RandDirectionVectors64 direction_vectors, ulong offset, ref RandStateScrambledSobol64 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_init");
        }

//        __device__ void curand_init (curandDirectionVectors32_t
//direction_vectors, unsigned int scramble_c, unsigned int offset,
//curandStateScrambledSobol32_t  state)
        public static void curand_int(this GThread thread, RandDirectionVectors32 direction_vectors, uint scramble_c, uint offset, ref RandStateScrambledSobol32 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_init");
        }

//        __device__ void curand_init (curandDirectionVectors32_t
//direction_vectors, unsigned int offset, curandStateSobol32_t  state)
        public static void curand_init(this GThread thread, RandDirectionVectors32 direction_vectors, uint offset, ref RandStateScrambledSobol32 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_init");
        }

//__device__ void curand_init (unsigned long long seed, unsigned long long
//subsequence, unsigned long long offset, curandStateXORWOW_t  state)
        public static void curand_init(this GThread thread, ulong seed, ulong subsequence, ulong offset, ref RandStateXORWOW state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_init");
        }

        #endregion

        #region curand_log_normal

//__device__ oat curand_log_normal (curandStateScrambledSobol64_t 
//state, oat mean, oat stddev)
        public static float curand_log_normal(this GThread thread, ref RandStateScrambledSobol64 state, float mean, float stddev)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_log_normal");
        }

//__device__ oat curand_log_normal (curandStateSobol64_t  state, oat
//mean, oat stddev)
        public static float curand_log_normal(this GThread thread, ref RandStateSobol64 state, float mean, float stddev)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_log_normal");
        }

//__device__ oat curand_log_normal (curandStateScrambledSobol32_t 
//state, oat mean, oat stddev)
        public static float curand_log_normal(this GThread thread, ref RandStateScrambledSobol32 state, float mean, float stddev)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_log_normal");
        }

//        __device__ oat curand_log_normal (curandStateSobol32_t  state, oat
//mean, oat stddev)
        public static float curand_log_normal(this GThread thread, ref RandStateSobol32 state, float mean, float stddev)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_log_normal");
        }

//__device__ oat curand_log_normal (curandStateXORWOW_t  state,
//oat mean, oat stddev)
        public static float curand_log_normal(this GThread thread, ref RandStateXORWOW state, float mean, float stddev)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_log_normal");
        }

#warning TODO curand_log_normal2 and curand_log_normal2_double

//__device__ double curand_log_normal_double (curandStateScrambled-
//Sobol64_t  state, double mean, double stddev)
        public static float curand_log_normal_double(this GThread thread, ref RandStateScrambledSobol64 state, double mean, double stddev)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_log_normal");
        }

//__device__ double curand_log_normal_double (curandStateSobol64_t 
//state, double mean, double stddev)
        public static float curand_log_normal_double(this GThread thread, ref RandStateSobol64 state, double mean, double stddev)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_log_normal");
        }

//        __device__ double curand_log_normal_double (curandStateScrambled-
//Sobol32_t  state, double mean, double stddev)
        public static float curand_log_normal_double(this GThread thread, ref RandStateScrambledSobol32 state, double mean, double stddev)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_log_normal");
        }

//__device__ double curand_log_normal_double (curandStateSobol32_t 
//state, double mean, double stddev)
        public static float curand_log_normal_double(this GThread thread, ref RandStateSobol32 state, double mean, double stddev)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_log_normal");
        }

//__device__ double curand_log_normal_double (curandStateXORWOW_t 
//state, double mean, double stddev)
        public static float curand_log_normal_double(this GThread thread, ref RandStateXORWOW state, double mean, double stddev)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_log_normal");
        }

//__device__ oat curand_normal (curandStateScrambledSobol64_t 
//state)
        public static float curand_normal(this GThread thread, ref RandStateScrambledSobol64 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_normal");
        }

        //__device__ oat curand_normal (curandStateSobol64_t  state)
        public static float curand_normal(this GThread thread, ref RandStateSobol64 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_normal");
        }

//__device__ oat curand_normal (curandStateScrambledSobol32_t 
//state)
        public static float curand_normal(this GThread thread, ref RandStateScrambledSobol32 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_normal");
        }

//__device__ oat curand_normal (curandStateSobol32_t  state)
        public static float curand_normal(this GThread thread, ref RandStateSobol32 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_normal");
        }

//__device__ oat curand_normal (curandStateXORWOW_t  state)
        public static float curand_normal(this GThread thread, ref RandStateXORWOW state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_normal");
        }

#warning TODO curand_normal2_double and curand_normal2

//__device__ double curand_normal_double (curandStateScrambled-
//Sobol64_t  state)
        public static double curand_normal_double(this GThread thread, ref RandStateScrambledSobol64 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_normal_double");
        }

        //__device__ double curand_normal_double (curandStateSobol64_t  state)
        public static double curand_normal_double(this GThread thread, ref RandStateSobol64 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_normal_double");
        }

        public static double curand_normal_double(this GThread thread, ref RandStateScrambledSobol32 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_normal_double");
        }

        public static double curand_normal_double(this GThread thread, ref RandStateSobol32 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_normal_double");
        }

        public static double curand_normal_double(this GThread thread, ref RandStateXORWOW state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_normal_double");
        }

        #endregion

        #region curand_uniform

        //__device__ oat curand_uniform (curandStateScrambledSobol64_t state)
        public static float curand_uniform(this GThread thread, ref RandStateScrambledSobol64 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_uniform");
        }

        //__device__ oat curand_uniform (curandStateSobol64_t  state)
        public static float curand_uniform(this GThread thread, ref RandStateSobol64 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_uniform");
        }

        //__device__ oat curand_uniform (curandStateScrambledSobol32_t state)
        public static float curand_uniform(this GThread thread, ref RandStateScrambledSobol32 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_uniform");
        }

        //__device__ oat curand_uniform (curandStateSobol32_t  state)
        public static float curand_uniform(this GThread thread, ref RandStateSobol32 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_uniform");
        }

        //__device__ oat curand_uniform (curandStateXORWOW_t  state)
        public static float curand_uniform(this GThread thread, ref RandStateXORWOW state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_uniform");
        }

        // __device__ double curand_uniform_double (curandStateScrambledSobol64_t  state)
        public static double curand_uniform_double(this GThread thread, ref RandStateScrambledSobol64 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_uniform_double");
        }

        //__device__ oat curand_uniform (curandStateSobol64_t  state)
        public static double curand_uniform_double(this GThread thread, ref RandStateSobol64 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_uniform_double");
        }

        //__device__ oat curand_uniform (curandStateScrambledSobol32_t state)
        public static double curand_uniform_double(this GThread thread, ref RandStateScrambledSobol32 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_uniform_double");
        }

        //__device__ oat curand_uniform (curandStateSobol32_t  state)
        public static double curand_uniform_double(this GThread thread, ref RandStateSobol32 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_uniform_double");
        }

        //__device__ oat curand_uniform (curandStateXORWOW_t  state)
        public static double curand_uniform_double(this GThread thread, ref RandStateXORWOW state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "curand_uniform_double");
        }
        #endregion

        public static void skipahead(this GThread thread, ulong n, ref RandStateSobol64 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "skipahead");
        }

        public static void skipahead(this GThread thread, uint n, ref RandStateSobol32 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "skipahead");
        }

        public static void skipahead(this GThread thread, ulong n, ref RandStateScrambledSobol64 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "skipahead");
        }

        public static void skipahead(this GThread thread, uint n, ref RandStateScrambledSobol32 state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "skipahead");
        }

        public static void skipahead(this GThread thread, ulong n, ref RandStateXORWOW state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "skipahead");
        }

        public static void skipahead_sequence(this GThread thread, ulong n, ref RandStateXORWOW state)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "skipahead_sequence");
        }
    }

}
