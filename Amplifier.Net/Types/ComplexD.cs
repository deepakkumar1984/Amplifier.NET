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

namespace Amplifier.Types
{
    
    /// <summary>
    /// Represents a complex single floating point number that is mapped to the native GPU equivalent.
    /// </summary>
    public struct ComplexD
    {
        /// <summary>
        /// Real part.
        /// </summary>
        public double x;

        /// <summary>
        /// Imaginary part.
        /// </summary>
        public double y;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexD"/> struct.
        /// </summary>
        /// <param name="real">The real part.</param>
        /// <param name="imaginary">The imaginary part.</param>
        public ComplexD(double real, double imaginary)
        {
            this.x = (double)real;
            this.y = (double)imaginary;
        }

        /// <summary>
        /// Conjugates the specified value.
        /// </summary>
        /// <param name="x">The value.</param>
        /// <returns>Conjugated value.</returns>
        public static ComplexD Conj(ComplexD x)
        {
            return new ComplexD(x.x, -1 * x.y);
        }

        /// <summary>
        /// Adds value y to value x.
        /// </summary>
        /// <param name="x">Value one.</param>
        /// <param name="y">Value to be added.</param>
        /// <returns>New value.</returns>
        public static ComplexD Add(ComplexD x, ComplexD y)
        {
            x.x = x.x + y.x;
            x.y = x.y + y.y;
            return x;
        }

        /// <summary>
        /// Subtracts value y from value x.
        /// </summary>
        /// <param name="x">Value one.</param>
        /// <param name="y">Value to be subtracted.</param>
        /// <returns>New value.</returns>
        public static ComplexD Subtract(ComplexD x, ComplexD y)
        {
            x.x = x.x - y.x;
            x.y = x.y - y.y;
            return x;
        }

        /// <summary>
        /// Multiplies value x and y.
        /// </summary>
        /// <param name="x">Value one.</param>
        /// <param name="y">Value two.</param>
        /// <returns>New value.</returns>
        public static ComplexD Multiply(ComplexD x, ComplexD y)
        {
            ComplexD c = new ComplexD();
            c.x = x.x * y.x - x.y * y.y;
            c.y = x.x * y.y + x.y * y.x;
            return c;
        }

        /// <summary>
        /// Divides value x by y.
        /// </summary>
        /// <param name="x">Value one.</param>
        /// <param name="y">Value two.</param>
        /// <returns>New value.</returns>
        public static ComplexD Divide(ComplexD x, ComplexD y)
        {
            double s = Math.Abs(y.x) + Math.Abs(y.y);
            double oos = 1.0f / s;
            double ars = x.x * oos;
            double ais = x.y * oos;
            double brs = y.x * oos;
            double bis = y.y * oos;
            s = (brs * brs) + (bis * bis);
            oos = 1.0f / s;
            ComplexD quot = new ComplexD(((ars * brs) + (ais * bis)) * oos,
                                        ((ais * brs) - (ars * bis)) * oos);
            return quot;
        }

        /// <summary>
        /// Gets the absolute of the specified value.
        /// </summary>
        /// <param name="x">The value.</param>
        /// <returns>Absolute.</returns>
        public static double Abs(ComplexD x)
        {
            double a = x.x;
            double b = x.y;
            double v, w, t;
            a = Math.Abs(a);
            b = Math.Abs(b);
            if (a > b)
            {
                v = a;
                w = b;
            }
            else
            {
                v = b;
                w = a;
            }
            t = w / v;
            t = 1.0f + t * t;
            t = v * Math.Sqrt(t);
            if ((v == 0.0) || (v > 1.79769313486231570e+308) || (w > 1.79769313486231570e+308))
            {
                t = v + w;
            }
            return t;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("( {0}, {1}i )", x, y);
        }
    }
}
