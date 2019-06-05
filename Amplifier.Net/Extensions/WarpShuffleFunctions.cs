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

namespace Amplifier.WarpShuffleFunctions
{
    public static class WarpFuncs
    {
        const int WARP_SIZE = 32;

        public static int Shuffle(this GThread thread, int var, int srcLane, int width = WARP_SIZE)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "Shuffle");
        }

        public static int ShuffleUp(this GThread thread, int var, uint delta, int width = WARP_SIZE)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "ShuffleUp");
        }

        public static int ShuffleDown(this GThread thread, int var, uint delta, int width = WARP_SIZE)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "ShuffleDown");
        }

        public static int ShuffleXor(this GThread thread, int var, int laneMask, int width = WARP_SIZE)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "ShuffleXor");
        }

        public static float Shuffle(this GThread thread, float var, int srcLane, int width = WARP_SIZE)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "Shuffle");
        }

        public static float ShuffleUp(this GThread thread, float var, uint delta, int width = WARP_SIZE)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "ShuffleUp");
        }

        public static float ShuffleDown(this GThread thread, float var, uint delta, int width = WARP_SIZE)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "ShuffleDown");
        }

        public static float ShuffleXor(this GThread thread, float var, int laneMask, int width = WARP_SIZE)
        {
            throw new AmplifierException(AmplifierException.csX_NOT_SUPPORTED, "ShuffleXor");
        }

    }
}
