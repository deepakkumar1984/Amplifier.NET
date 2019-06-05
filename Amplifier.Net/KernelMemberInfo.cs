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
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Reflection;
//using GASS.CUDA.Types;

namespace Amplifier
{
    /// <summary>
    /// Base class for kernel constants, methods and types.
    /// </summary>
    public abstract class KernelMemberInfo
    {
        /// <summary>
        /// Name
        /// </summary>
        protected const string csNAME = "Name";
        /// <summary>
        /// Type
        /// </summary>
        protected const string csTYPE = "Type";
        /// <summary>
        /// Checksum
        /// </summary>
        protected const string csCHECKSUM = "Checksum";
        /// <summary>
        /// Assembly
        /// </summary>
        protected const string csASSEMBLY = "Assembly";
        /// <summary>
        /// AssemblyName
        /// </summary>
        protected const string csASSEMBLYNAME = "AssemblyName";
        /// <summary>
        /// AssemblyPath
        /// </summary>
        protected const string csASSEMBLYPATH = "AssemblyPath";
        /// <summary>
        /// IsDummy
        /// </summary>
        protected const string csISDUMMY = "IsDummy";
        /// <summary>
        /// DummyBehaviour
        /// </summary>
        protected const string csDUMMYBEHAVIOUR = "DummyBehaviour";

        public AmplifierModule ParentModule { get; internal set; }

        /// <summary>
        /// Gets or sets the deserialized checksum.
        /// </summary>
        /// <value>
        /// The deserialized checksum.
        /// </value>
        public long DeserializedChecksum { get; protected set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public virtual Type Type { get; protected set; }
        /// <summary>
        /// Gets a value indicating whether this instance is dummy.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is dummy; otherwise, <c>false</c>.
        /// </value>
        public bool IsDummy { get; internal set; }


        /// <summary>
        /// Gets a value indicating whether to include header file for dummy or not.
        /// </summary>
        public eAmplifierDummyBehaviour Behaviour { get; internal set; }

        /// <summary>
        /// Gets the checksum of the assembly on which this member was based.
        /// </summary>
        /// <returns>Crc32 check sum.</returns>
        public long GetAssemblyChecksum()
        {
            long checksum = 0;
            if (this.Type != null)
            {
                checksum = Crc32.ComputeChecksum(this.Type.Assembly.Location);
            }
            return checksum;
        }

        /// <summary>
        /// Checks if the assembly checksum and deserialized checksum are the same.
        /// </summary>
        /// <returns>True if the same, else false.</returns>
        public bool TryVerifyChecksums()
        {
            if (DeserializedChecksum == 0)
                return true;
            long currentChecksum = GetAssemblyChecksum();
            return DeserializedChecksum == currentChecksum;
        }

        /// <summary>
        /// Checks if the assembly checksum and deserialized checksum are the same.
        /// </summary>
        /// <exception cref="AmplifierException">Checksums do not match.</exception>
        public void VerifyChecksums()
        {
            if (!TryVerifyChecksums())
                throw new AmplifierException(AmplifierException.csCHECKSUM_FOR_ASSEMBLY_X_DOES_NOT_MATCH_DESERIALIZED_VALUE, Type.Assembly.FullName);
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public abstract string Name { get; internal set; }

        internal abstract XElement GetXElement();

        /// <summary>
        /// Returns the Name.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
