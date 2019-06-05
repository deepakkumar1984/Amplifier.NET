using System;
using System.Collections.Generic;
using System.Reflection;

namespace Amplifier.SIMDFunctions
{
    public static class SIMDFuncs
    {
        // TODO How can we do this in OpenCL?  Add our own custom header file?

        private const string ERRORMSG = "Emulation of SIMDFuncs";

        private static UInt16 minu2 = UInt16.MinValue;
        private static UInt16 maxu2 = UInt16.MaxValue;
        private static Int16 mins2 = Int16.MinValue;
        private static Int16 maxs2 = Int16.MaxValue;

        private static byte minu4 = byte.MinValue;
        private static byte maxu4 = byte.MaxValue;
        private static sbyte mins4 = sbyte.MinValue;
        private static sbyte maxs4 = sbyte.MaxValue;


        // The meat of the SIMD-in-a-word emulated functions:

        private static UInt16 ProcessHalfWord(string fnName, UInt16 a)
        {
            switch (fnName)
            {
                case "vabs2":
                    return (UInt16)Math.Abs(Convert.ToInt32((Int16)a));
                case "vabsss2":
                    return (UInt16)(Math.Min(maxs2, Math.Abs(Convert.ToInt32((Int16)a))));
                case "vneg2":
                    return (UInt16)(-(Int16)a);
                case "vnegss2":
                    return (UInt16)(Math.Min(maxs2, -(Int16)a));
                default:
                    throw new Exception(fnName + " not handled");
            }
        }

        private static byte ProcessByte(string fnName, byte a)
        {
            switch (fnName)
            {
                case "vabs4":
                    return (byte)Math.Abs(Convert.ToInt32((sbyte)a));
                case "vabsss4":
                    return (byte)(Math.Min(maxs4, Math.Abs(Convert.ToInt32((sbyte)a))));
                case "vneg4":
                    return (byte)(-(sbyte)a);
                case "vnegss4":
                    return (byte)(Math.Min(maxs4, -(sbyte)a));
                default:
                    throw new Exception(fnName + " not handled");
            }
        }

        private static UInt16 ProcessHalfWord(string fnName, UInt16 a, UInt16 b)
        {
            switch (fnName)
            {
                case "vabsdiffs2":
                case "vsads2":
                    return (UInt16)Math.Abs((Int16)a - (Int16)b);
                case "vabsdiffu2":
                case "vsadu2":
                    return (UInt16)Math.Abs(a - b);
                case "vadd2":
                    return (UInt16)((a + b) & maxu2);
                case "vaddss2":
                    return (UInt16)Math.Min(maxs2, Math.Max(mins2, (Int16)a + (Int16)b));
                case "vaddus2":
                    return (UInt16)Math.Min(maxu2, Math.Max(minu2, a + b));
                case "vavgs2":
                    return (UInt16)(((Int16)a + (Int16)b + (((Int16)a + (Int16)b) >= 0 ? 1 : 0)) >> 1);
                case "vavgu2":
                    return (UInt16)((a + b + 1) / 2);
                case "vcmpeq2":
                    return (UInt16)(a == b ? maxu2 : minu2);
                case "vcmpges2":
                    return (UInt16)((Int16)a >= (Int16)b ? maxu2 : minu2);
                case "vcmpgeu2":
                    return (UInt16)(a >= b ? maxu2 : minu2);
                case "vcmpgts2":
                    return (UInt16)((Int16)a > (Int16)b ? maxu2 : minu2);
                case "vcmpgtu2":
                    return (UInt16)(a > b ? maxu2 : minu2);
                case "vcmples2":
                    return (UInt16)((Int16)a <= (Int16)b ? maxu2 : minu2);
                case "vcmpleu2":
                    return (UInt16)(a <= b ? maxu2 : minu2);
                case "vcmplts2":
                    return (UInt16)((Int16)a < (Int16)b ? maxu2 : minu2);
                case "vcmpltu2":
                    return (UInt16)(a < b ? maxu2 : minu2);
                case "vcmpne2":
                    return (UInt16)(a != b ? maxu2 : minu2);
                case "vhaddu2":
                    return (UInt16)((a + b) / 2);
                case "vmaxs2":
                    return (Int16)a > (Int16)b ? a : b;
                case "vmaxu2":
                    return a > b ? a : b;
                case "vmins2":
                    return (Int16)a < (Int16)b ? a : b;
                case "vminu2":
                    return a < b ? a : b;
                case "vseteq2":
                    return (UInt16)(a == b ? 1 : 0);
                case "vsetges2":
                    return (UInt16)((Int16)a >= (Int16)b ? 1 : 0);
                case "vsetgeu2":
                    return (UInt16)(a >= b ? 1 : 0);
                case "vsetgts2":
                    return (UInt16)((Int16)a > (Int16)b ? 1 : 0);
                case "vsetgtu2":
                    return (UInt16)(a > b ? 1 : 0);
                case "vsetles2":
                    return (UInt16)((Int16)a <= (Int16)b ? 1 : 0);
                case "vsetleu2":
                    return (UInt16)(a <= b ? 1 : 0);
                case "vsetlts2":
                    return (UInt16)((Int16)a < (Int16)b ? 1 : 0);
                case "vsetltu2":
                    return (UInt16)(a < b ? 1 : 0);
                case "vsetne2":
                    return (UInt16)(a != b ? 1 : 0);
                case "vsub2":
                    return (UInt16)((a - b) & maxu2);
                case "vsubss2":
                    return (UInt16)Math.Min(maxs2, Math.Max(mins2, (Int16)a - (Int16)b));
                case "vsubus2":
                    return (UInt16)Math.Min(maxu2, Math.Max(minu2, a - b));
                default:
                    throw new Exception(fnName + " not handled");
            }
        }

        private static byte ProcessByte(string fnName, byte a, byte b)
        {
            switch (fnName)
            {
                case "vabsdiffs4":
                case "vsads4":
                    return (byte)Math.Abs((sbyte)a - (sbyte)b);
                case "vabsdiffu4":
                case "vsadu4":
                    return (byte)Math.Abs(a - b);
                case "vadd4":
                    return (byte)((a + b) & maxu4);
                case "vaddss4":
                    return (byte)Math.Min(maxs4, Math.Max(mins4, (sbyte)a + (sbyte)b));
                case "vaddus4":
                    return (byte)Math.Min(maxu4, Math.Max(minu4, a + b));
                case "vavgs4":
                    return (byte)(((sbyte)a + (sbyte)b + (((sbyte)a + (sbyte)b) >= 0 ? 1 : 0)) >> 1);
                case "vavgu4":
                    return (byte)((a + b + 1) / 2);
                case "vcmpeq4":
                    return (byte)(a == b ? maxu4 : minu4);
                case "vcmpges4":
                    return (byte)((sbyte)a >= (sbyte)b ? maxu4 : minu4);
                case "vcmpgeu4":
                    return (byte)(a >= b ? maxu4 : minu4);
                case "vcmpgts4":
                    return (byte)((sbyte)a > (sbyte)b ? maxu4 : minu4);
                case "vcmpgtu4":
                    return (byte)(a > b ? maxu4 : minu4);
                case "vcmples4":
                    return (byte)((sbyte)a <= (sbyte)b ? maxu4 : minu4);
                case "vcmpleu4":
                    return (byte)(a <= b ? maxu4 : minu4);
                case "vcmplts4":
                    return (byte)((sbyte)a < (sbyte)b ? maxu4 : minu4);
                case "vcmpltu4":
                    return (byte)(a < b ? maxu4 : minu4);
                case "vcmpne4":
                    return (byte)(a != b ? maxu4 : minu4);
                case "vhaddu4":
                    return (byte)((a + b) / 2);
                case "vmaxs4":
                    return (sbyte)a > (sbyte)b ? a : b;
                case "vmaxu4":
                    return a > b ? a : b;
                case "vmins4":
                    return (sbyte)a < (sbyte)b ? a : b;
                case "vminu4":
                    return a < b ? a : b;
                case "vseteq4":
                    return (byte)(a == b ? 1 : 0);
                case "vsetges4":
                    return (byte)((sbyte)a >= (sbyte)b ? 1 : 0);
                case "vsetgeu4":
                    return (byte)(a >= b ? 1 : 0);
                case "vsetgts4":
                    return (byte)((sbyte)a > (sbyte)b ? 1 : 0);
                case "vsetgtu4":
                    return (byte)(a > b ? 1 : 0);
                case "vsetles4":
                    return (byte)((sbyte)a <= (sbyte)b ? 1 : 0);
                case "vsetleu4":
                    return (byte)(a <= b ? 1 : 0);
                case "vsetlts4":
                    return (byte)((sbyte)a < (sbyte)b ? 1 : 0);
                case "vsetltu4":
                    return (byte)(a < b ? 1 : 0);
                case "vsetne4":
                    return (byte)(a != b ? 1 : 0);
                case "vsub4":
                    return (byte)((a - b) & maxu4);
                case "vsubss4":
                    return (byte)Math.Min(maxs4, Math.Max(mins4, (sbyte)a - (sbyte)b));
                case "vsubus4":
                    return (byte)Math.Min(maxu4, Math.Max(minu4, a - b));
                default:
                    throw new Exception(fnName + " not handled");
            }
        }

        /// <summary>
        /// Break the 32-bit input word into two 16-bit halfwords and pass them off to be processed separately.
        /// </summary>
        /// <param name="fnName">function name to be passed along to method ProcessHalfWord</param>
        /// <param name="a">32-bit word to be broken into two halfwords</param>
        /// <returns>32-bit word assembled from two halfwords</returns>
        private static uint ProcessHalfWords(string fnName, uint a)
        {
            byte[] abytes = BitConverter.GetBytes(a);
            byte[] cbytes = new byte[4];
            UInt16[] ashorts = new UInt16[] { BitConverter.ToUInt16(abytes, 0), BitConverter.ToUInt16(abytes, 2) };
            UInt16[] cshorts = new UInt16[2];
            for (int i = 0; i < 2; i++)
                cshorts[i] = ProcessHalfWord(fnName, ashorts[i]);
            cbytes[0] = BitConverter.GetBytes(cshorts[0])[0];
            cbytes[1] = BitConverter.GetBytes(cshorts[0])[1];
            cbytes[2] = BitConverter.GetBytes(cshorts[1])[0];
            cbytes[3] = BitConverter.GetBytes(cshorts[1])[1];
            return (uint)BitConverter.ToUInt32(cbytes, 0);
        }

        /// <summary>
        /// Break each 32-bit input word into two 16-bit halfwords and pass them off to be processed separately.
        /// </summary>
        /// <param name="fnName">function name to be passed along to method ProcessHalfWord</param>
        /// <param name="a">32-bit word to be broken into two halfwords</param>
        /// <param name="b">32-bit word to be broken into two halfwords</param>
        /// <returns>32-bit word assembled from two halfwords</returns>
        private static uint ProcessHalfWords(string fnName, uint a, uint b)
        {
            byte[] abytes = BitConverter.GetBytes(a);
            byte[] bbytes = BitConverter.GetBytes(b);
            byte[] cbytes = new byte[4];
            UInt16[] ashorts = new UInt16[] { BitConverter.ToUInt16(abytes, 0), BitConverter.ToUInt16(abytes, 2) };
            UInt16[] bshorts = new UInt16[] { BitConverter.ToUInt16(bbytes, 0), BitConverter.ToUInt16(bbytes, 2) };
            UInt16[] cshorts = new UInt16[2];
            for (int i = 0; i < 2; i++)
                cshorts[i] = ProcessHalfWord(fnName, ashorts[i], bshorts[i]);
            cbytes[0] = BitConverter.GetBytes(cshorts[0])[0];
            cbytes[1] = BitConverter.GetBytes(cshorts[0])[1];
            cbytes[2] = BitConverter.GetBytes(cshorts[1])[0];
            cbytes[3] = BitConverter.GetBytes(cshorts[1])[1];
            return (uint)BitConverter.ToUInt32(cbytes, 0);
        }

        /// <summary>
        /// Break each 32-bit input word into two 16-bit halfwords and pass them off to be processed separately.
        /// Sum the results.
        /// </summary>
        /// <param name="fnName">function name to be passed along to method ProcessHalfWord</param>
        /// <param name="a">32-bit word to be broken into two halfwords</param>
        /// <param name="b">32-bit word to be broken into two halfwords</param>
        /// <returns>32-bit word -- sum of two halfwords</returns>
        private static uint SumHalfWords(string fnName, uint a, uint b)
        {
            byte[] abytes = BitConverter.GetBytes(a);
            byte[] bbytes = BitConverter.GetBytes(b);
            byte[] cbytes = new byte[4];
            UInt16[] ashorts = new UInt16[] { BitConverter.ToUInt16(abytes, 0), BitConverter.ToUInt16(abytes, 2) };
            UInt16[] bshorts = new UInt16[] { BitConverter.ToUInt16(bbytes, 0), BitConverter.ToUInt16(bbytes, 2) };
            UInt16[] cshorts = new UInt16[2];
            for (int i = 0; i < 2; i++)
                cshorts[i] = ProcessHalfWord(fnName, ashorts[i], bshorts[i]);
            return (uint)(cshorts[0] + cshorts[1]);
        }

        /// <summary>
        /// Break the 32-bit input word into four bytes and pass them off to be processed separately.
        /// </summary>
        /// <param name="fnName">function name to be passed along to method ProcessByte</param>
        /// <param name="a">32-bit word to be broken into four bytes</param>
        /// <returns>32-bit word assembled from four bytes</returns>
        private static uint ProcessBytes(string fnName, uint a)
        {
            byte[] abytes = BitConverter.GetBytes(a);
            byte[] cbytes = new byte[4];
            for (int i = 0; i < 4; i++)
                cbytes[i] = ProcessByte(fnName, abytes[i]);
            return (uint)BitConverter.ToUInt32(cbytes, 0);
        }

        /// <summary>
        /// Break each 32-bit input word into four bytes and pass them off to be processed separately.
        /// </summary>
        /// <param name="fnName">function name to be passed along to method ProcessByte</param>
        /// <param name="a">32-bit word to be broken into four bytes</param>
        /// <param name="b">32-bit word to be broken into four bytes</param>
        /// <returns>32-bit word assembled from four bytes</returns>
        private static uint ProcessBytes(string fnName, uint a, uint b)
        {
            byte[] abytes = BitConverter.GetBytes(a);
            byte[] bbytes = BitConverter.GetBytes(b);
            byte[] cbytes = new byte[4];
            for (int i = 0; i < 4; i++)
                cbytes[i] = ProcessByte(fnName, abytes[i], bbytes[i]);
            return (uint)BitConverter.ToUInt32(cbytes, 0);
        }

        /// <summary>
        /// Break each 32-bit input word into four bytes and pass them off to be processed separately.
        /// Sum the results.
        /// </summary>
        /// <param name="fnName">function name to be passed along to method ProcessByte</param>
        /// <param name="a">32-bit word to be broken into four bytes</param>
        /// <param name="b">32-bit word to be broken into four bytes</param>
        /// <returns>32-bit word -- sum of four bytes</returns>
        private static uint SumBytes(string fnName, uint a, uint b)
        {
            byte[] abytes = BitConverter.GetBytes(a);
            byte[] bbytes = BitConverter.GetBytes(b);
            byte[] cbytes = new byte[4];
            for (int i = 0; i < 4; i++)
                cbytes[i] = ProcessByte(fnName, abytes[i], bbytes[i]);
            return (uint)(cbytes[0] + cbytes[1] + cbytes[2] + cbytes[3]);
        }

        // wrappers for the 82 SIMD-in-a-word functions
        //
        // To simplify code maintenance, MethodBase.GetCurrentMethod().Name is used in place of the literal method name;
        // this approach would fail if the compiler were to inline any of these methods.
        
        public static uint vabs2(this GThread thread, uint a)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a);
        }

        public static uint vabsss2(this GThread thread, uint a)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a);
        }

        public static uint vadd2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vaddss2 (this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vaddus2 (this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vavgs2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vavgu2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vhaddu2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmpeq2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmpges2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmpgeu2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmpgts2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmpgtu2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmples2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmpleu2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmplts2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmpltu2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmpne2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vabsdiffu2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vmaxs2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vmaxu2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vmins2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vminu2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vseteq2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsetges2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsetgeu2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsetgts2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsetgtu2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsetles2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsetleu2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsetlts2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsetltu2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsetne2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsads2(this GThread thread, uint a, uint b)
        {
            return SumHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsadu2(this GThread thread, uint a, uint b)
        {
            return SumHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsub2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsubss2 (this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsubus2 (this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vneg2(this GThread thread, uint a)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a);
        }

        public static uint vnegss2(this GThread thread, uint a)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a);
        }

        public static uint vabsdiffs2(this GThread thread, uint a, uint b)
        {
            return ProcessHalfWords(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vabs4(this GThread thread, uint a)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a);
        }

        public static uint vabsss4(this GThread thread, uint a)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a);
        }

        public static uint vadd4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vaddss4 (this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vaddus4 (this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vavgs4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vavgu4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vhaddu4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmpeq4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmpges4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmpgeu4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmpgts4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmpgtu4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmples4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmpleu4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmplts4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmpltu4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vcmpne4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vabsdiffu4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vmaxs4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vmaxu4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vmins4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vminu4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vseteq4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsetles4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsetleu4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsetlts4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsetltu4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsetges4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsetgeu4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsetgts4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsetgtu4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsetne4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsads4(this GThread thread, uint a, uint b)
        {
            return SumBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsadu4(this GThread thread, uint a, uint b)
        {
            return SumBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsub4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsubss4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vsubus4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

        public static uint vneg4(this GThread thread, uint a)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a);
        }

        public static uint vnegss4(this GThread thread, uint a)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a);
        }

        public static uint vabsdiffs4(this GThread thread, uint a, uint b)
        {
            return ProcessBytes(MethodBase.GetCurrentMethod().Name, a, b);
        }

    }
}
