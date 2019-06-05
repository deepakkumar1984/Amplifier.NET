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
    /// Describes a .NET type (structure) that was translated to Cuda function.
    /// </summary>
    public class KernelTypeInfo : KernelMemberInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KernelTypeInfo"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="isDummy">if set to <c>true</c> is dummy.</param>
        /// <param name="noDummyInclude"></param>
        public KernelTypeInfo(Type type, bool isDummy = false, eAmplifierDummyBehaviour behaviour = eAmplifierDummyBehaviour.Default)
        {
            Type = type;
            Name = type != null ? type.Name : csAmplifierTYPE;
            IsDummy = isDummy;
            Behaviour = behaviour;
        }
        
        internal const string csAmplifierTYPE = "AmplifierType";

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name { get; internal set; }

        internal string GetDummyInclude()
        {
            if (!IsDummy || Behaviour == eAmplifierDummyBehaviour.SuppressInclude)
                return string.Empty;
            string ts = string.Format(@"#include ""{0}.cu""", Name);
            return ts;
        }

        internal override XElement GetXElement()
        {
            XElement xe = new XElement(csAmplifierTYPE);
            xe.SetAttributeValue(csNAME, Type.Name);
            xe.SetAttributeValue(csISDUMMY, IsDummy);
            xe.SetAttributeValue(csDUMMYBEHAVIOUR, Behaviour);
            xe.Add(new XElement(csTYPE, this.Type != null ? this.Type.FullName : csAmplifierTYPE));
            xe.Add(new XElement(csASSEMBLY, this.Type != null ? this.Type.Assembly.FullName : string.Empty));
            xe.Add(new XElement(csASSEMBLYNAME, this.Type != null ? this.Type.Assembly.GetName().Name : string.Empty));
            xe.Add(new XElement(csASSEMBLYPATH, this.Type != null ? this.Type.Assembly.Location : string.Empty));
            xe.Add(new XElement(csCHECKSUM, XmlConvert.ToString(GetAssemblyChecksum())));
            return xe;
        }

        internal static KernelTypeInfo Deserialize(XElement xe, string directory = null)
        {
            string name = xe.GetAttributeValue(csNAME);
            bool? isDummy = xe.TryGetAttributeBoolValue(csISDUMMY);
            string behaviourStr = xe.TryGetAttributeValue(csDUMMYBEHAVIOUR);            
            string typeName = xe.Element(csTYPE).Value;
            string assemblyFullName = xe.Element(csASSEMBLY).Value;
            string assemblyName = xe.Element(csASSEMBLYNAME).Value;
            string assemblyPath = xe.TryGetElementValue(csASSEMBLYPATH);
            long checksum = XmlConvert.ToInt64(xe.Element(csCHECKSUM).Value);
            eAmplifierDummyBehaviour behaviour = string.IsNullOrEmpty(behaviourStr) ? eAmplifierDummyBehaviour.Default : (eAmplifierDummyBehaviour)Enum.Parse(typeof(eAmplifierDummyBehaviour), behaviourStr);
            Type type = null;
            KernelTypeInfo kti = new KernelTypeInfo(null);

            if (!string.IsNullOrEmpty(typeName) && !string.IsNullOrEmpty(assemblyFullName))
            {
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.Load(assemblyFullName);
                }
                catch (FileNotFoundException)
                {
                    directory = directory != null ? directory : string.Empty;
                    assemblyName = directory + Path.DirectorySeparatorChar + assemblyName;
                    if (File.Exists(assemblyName + ".dll"))
                    {
                        assembly = Assembly.LoadFrom(assemblyName + ".dll");
                    }
                    else if (File.Exists(assemblyName + ".exe"))
                    {
                        assembly = Assembly.LoadFrom(assemblyName + ".exe");
                    }
                    else if (!string.IsNullOrEmpty(assemblyPath))
                    {
                        assembly = Assembly.LoadFrom(assemblyPath);
                    }
                    else
                        throw;
                }
                if (assembly == null)
                    throw new AmplifierException(AmplifierException.csCOULD_NOT_LOAD_ASSEMBLY_X, assemblyFullName);
                type = assembly.GetType(typeName);
                kti = new KernelTypeInfo(type, isDummy == true ? true : false, behaviour);
            }
            kti.DeserializedChecksum = checksum;
            return kti;
        }
    }
}
