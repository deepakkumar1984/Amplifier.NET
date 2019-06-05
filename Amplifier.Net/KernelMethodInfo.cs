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
    /// The method type is either Global or Device.
    /// </summary>
    public enum eKernelMethodType 
    {
        /// <summary>
        /// Global function can be launched.
        /// </summary>
        Global,
        /// <summary>
        /// Device function can be called from global functions or other device function.
        /// </summary>
        Device 
    };

    /// <summary>
    /// Describes a .NET method that was translated to Cuda function.
    /// </summary>
    public class KernelMethodInfo : KernelMemberInfo
    {
        ///// <summary>
        ///// Initializes a new instance of the <see cref="KernelMethodInfo"/> class.
        ///// </summary>
        ///// <param name="type">The type.</param>
        ///// <param name="method">The method.</param>
        ///// <param name="gpuMethodType">Type of the gpu method.</param>
        ///// <param name="noDummyInclude"></param>
        ///// <param name="parentModule"></param>
        //public KernelMethodInfo(Type type, MethodInfo method, eKernelMethodType gpuMethodType, bool noDummyInclude, AmplifierModule parentModule)
        //    : this(type, method, gpuMethodType, false, false, parentModule)
        //{
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="KernelMethodInfo"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <param name="gpuMethodType">Type of the gpu method.</param>
        /// <param name="isDummy">if set to <c>true</c> is dummy.</param>
        /// <param name="behaviour"></param>
        /// <param name="parentModule">Module of which this is a part.</param>
        public KernelMethodInfo(Type type, MethodInfo method, eKernelMethodType gpuMethodType, bool isDummy, eAmplifierDummyBehaviour behaviour, AmplifierModule parentModule)
        {
            Type = type;
            Method = method;
            MethodType = gpuMethodType;
            DeserializedChecksum = 0;
            IsDummy = isDummy;
            Behaviour = behaviour;
            ParentModule = parentModule;
        }

        /// <summary>
        /// Gets the method.
        /// </summary>
        public MethodInfo Method { get; private set; }
        /// <summary>
        /// Gets the type of the method.
        /// </summary>
        /// <value>
        /// The type of the method.
        /// </value>
        public eKernelMethodType MethodType { get; private set; }
        /// <summary>
        /// Gets the kernel function.
        /// </summary>
        public object KernelFunction { get; internal set; }
        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name { get { return Method != null ? Method.Name : csAmplifierKERNELMETHOD; } internal set{ } }

        internal const string csAmplifierKERNELMETHOD = "AmplifierKernelMethod";
        //private const string csDOTNETSOURCE = "DotNetSource";

        private const string csPARAMETERS = "Parameters";

        private const string csRETURNTYPE = "ReturnType";
        private const string csPARAMETER = "Parameter";
        private const string csPOSITION = "Position";

        /// <summary>
        /// Gets the parameters as a comma seperated string.
        /// </summary>
        /// <returns>Paramter string.</returns>
        public string GetParametersString()
        {
            string ts = string.Empty;
            if(Method != null)
            {
                ParameterInfo[] prms = Method.GetParameters();
                int pIndex = 0;
                foreach(var pi in prms)
                {
                    ts += string.Format("{0} {1}{2}", pi.ParameterType.Name, pi.Name, ++pIndex < prms.Length ? ", " : string.Empty);
                }
            }
            return ts;
        }

        internal string GetDummyInclude()
        {
            if (!IsDummy || Behaviour == eAmplifierDummyBehaviour.SuppressInclude)
                return string.Empty;
            string ts = string.Format(@"#include ""{0}.cu""", Name);
            return ts;
        }

        internal override XElement GetXElement()
        {
            XElement xe = new XElement(csAmplifierKERNELMETHOD);
            xe.SetAttributeValue(csNAME, Method.Name);
            xe.SetAttributeValue(csTYPE, MethodType);
            xe.SetAttributeValue(csISDUMMY, IsDummy);
            xe.SetAttributeValue(csDUMMYBEHAVIOUR, Behaviour);
            xe.Add(new XElement(csTYPE, this.Type != null ? this.Type.FullName : string.Empty));
            xe.Add(new XElement(csASSEMBLY, this.Type != null ? this.Type.Assembly.FullName : string.Empty));
            xe.Add(new XElement(csASSEMBLYNAME, this.Type != null ? this.Type.Assembly.GetName().Name : string.Empty));
            xe.Add(new XElement(csASSEMBLYPATH, this.Type != null ? this.Type.Assembly.Location : string.Empty));
            xe.Add(new XElement(csCHECKSUM, XmlConvert.ToString(GetAssemblyChecksum())));

            XElement mi = new XElement(csPARAMETERS,
                new XElement(csRETURNTYPE, Method.ReturnParameter.ParameterType.FullName));

            foreach (ParameterInfo pi in Method.GetParameters())
            {
                XElement pxe = new XElement(csPARAMETER);
                pxe.SetAttributeValue(csTYPE, pi.ParameterType);
                pxe.SetAttributeValue(csNAME, pi.Name);
                pxe.SetAttributeValue(csPOSITION, pi.Position);
                mi.Add(pxe);
            }

            xe.Add(mi);
            return xe;
        }

        internal static KernelMethodInfo Deserialize(XElement xe, AmplifierModule parentModule, string directory = null)
        {
            string methodName = xe.GetAttributeValue(csNAME);
            string methodTypeName = xe.GetAttributeValue(csTYPE);
            eKernelMethodType methodType = (eKernelMethodType)Enum.Parse(typeof(eKernelMethodType), methodTypeName);
            bool? isDummy = xe.TryGetAttributeBoolValue(csISDUMMY);
            string behaviourStr = xe.TryGetAttributeValue(csDUMMYBEHAVIOUR);  
            string typeName = xe.Element(csTYPE).Value;
            string assemblyFullName = xe.Element(csASSEMBLY).Value;
            string assemblyName = xe.Element(csASSEMBLYNAME).Value;
            string assemblyPath = xe.TryGetElementValue(csASSEMBLYPATH);
            long checksum = XmlConvert.ToInt64(xe.Element(csCHECKSUM).Value);
            eAmplifierDummyBehaviour behaviour = string.IsNullOrEmpty(behaviourStr) ? eAmplifierDummyBehaviour.Default : (eAmplifierDummyBehaviour)Enum.Parse(typeof(eAmplifierDummyBehaviour), behaviourStr);
            MethodInfo mi = null;
            KernelMethodInfo kmi = null;

            if(!string.IsNullOrEmpty(typeName) && !string.IsNullOrEmpty(assemblyFullName))
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
                Type type = assembly.GetType(typeName);
                if (type == null)
                    throw new AmplifierException(AmplifierException.csCOULD_NOT_FIND_TYPE_X_IN_ASSEMBLY_X, typeName, assemblyFullName);
                mi = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (mi == null)
                    throw new AmplifierException(AmplifierException.csCOULD_NOT_FIND_METHOD_X_IN_TYPE_X_IN_ASSEMBLY_X, methodName, typeName, assemblyFullName);
                kmi = new KernelMethodInfo(type, mi, methodType, isDummy == true ? true : false, behaviour, parentModule);                
            }
            kmi.DeserializedChecksum = checksum;
            return kmi;
        }
    } 
}
