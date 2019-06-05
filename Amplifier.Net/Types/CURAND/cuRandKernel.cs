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
namespace Amplifier.Rand
{
    using System;
    using System.Runtime.InteropServices;


    public struct RandDirectionVectors32
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public uint direction_vectors;
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct RandDirectionVectors64
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public ulong[] direction_vectors;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct RandStateXORWOW 
    {
        public uint d;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public uint[] v;
        public int boxmuller_flag;
        public float boxmuller_extra;
        public double boxmuller_extra_double;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct RandStateSobol32
    {
        public uint i;
        public uint x;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public int[] direction_vectors;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RandStateScrambledSobol32 
    {
        public uint i;
        public uint x; 
        public uint c;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public uint[] direction_vectors;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct RandStateSobol64
    {
        public ulong i;
        public ulong x;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public ulong[] direction_vectors;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RandStateScrambledSobol64
    {
        public ulong i;
        public ulong x;
        public ulong c;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public ulong[] direction_vectors;
    };


}
