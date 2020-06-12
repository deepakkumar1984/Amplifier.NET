using System;
using System.Collections.Generic;
using System.Text;

namespace Amplifier
{
    public enum DType
    {
        Float32 = 0,
        Float64 = 1,
        //Int8 = 2,
        //Int16 = 3,
        Int32 = 4,
        //Int64 = 5,
        UInt8 = 6,
        //UInt16 = 7,
        //UInt32 = 8,
        //UInt64 = 8
    }

    /// <summary>
    /// Class DTypeExtensions.
    /// </summary>
    public static class DTypeExtensions
    {
        /// <summary>
        /// Sizes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="NotSupportedException">Element type " + value + " not supported.</exception>
        public static int Size(this DType value)
        {
            switch (value)
            {
                case DType.Float32: return 4;
                case DType.Float64: return 8;
                case DType.Int32: return 4;
                case DType.UInt8: return 1;
                //case DType.Int8: return 1;
                //case DType.UInt32: return 4;
                //case DType.Int16: return 2;
                //case DType.UInt16: return 2;
                //case DType.Int64: return 8;
                //case DType.UInt64: return 8;
                default:
                    throw new NotSupportedException("Element type " + value + " not supported.");
            }
        }

        /// <summary>
        /// Converts to clrtype.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Type.</returns>
        /// <exception cref="NotSupportedException">Element type " + value + " not supported.</exception>
        public static Type ToCLRType(this DType value)
        {
            switch (value)
            {
                case DType.Float32: return typeof(float);
                case DType.Float64: return typeof(double);
                case DType.Int32: return typeof(int);
                case DType.UInt8: return typeof(byte);
                //case DType.Int8: return typeof(sbyte);
                //case DType.UInt32: return typeof(uint);
                //case DType.Int16: return typeof(short);
                //case DType.UInt16: return typeof(ushort);
                //case DType.Int64: return typeof(long);
                //case DType.UInt64: return typeof(ulong);
                default:
                    throw new NotSupportedException("Element type " + value + " not supported.");
            }
        }
    }

    /// <summary>
    /// Class DTypeBuilder.
    /// </summary>
    public static class DTypeBuilder
    {
        /// <summary>
        /// Froms the type of the color.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>DType.</returns>
        /// <exception cref="NotSupportedException">No corresponding DType value for CLR type " + type</exception>
        public static DType FromCLRType(Type type)
        {
            if (type.Name.Contains("Single")) return DType.Float32;
            else if (type.Name.Contains("Double")) return DType.Float64;
            else if (type.Name.Contains("Int32")) return DType.Int32;
            else if (type.Name.Contains("Byte")) return DType.UInt8;
            else
                throw new NotSupportedException("No corresponding DType value for CLR type " + type);
        }
    }
}
