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
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;

namespace Amplifier
{
    /// <summary>
    /// Xml extension class.
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        /// Converts an XmlNode to XElement. 
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>XElement</returns>
        public static XElement GetXElement(this XmlNode node)
        {
            XDocument xDoc = new XDocument();
            using (XmlWriter xmlWriter = xDoc.CreateWriter())
                node.WriteTo(xmlWriter);
            return xDoc.Root;
        }

        /// <summary>
        /// Converts an XElement to XmlNode. 
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>XmlNode</returns>
        public static XmlNode GetXmlNode(this XElement element)
        {
            using (XmlReader xmlReader = element.CreateReader())
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                return xmlDoc;
            }
        }

        /// <summary>
        /// Tries to get element value.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <returns>Value of element or null if element does not exist.</returns>
        public static string TryGetElementValue(this XElement element, string elementName)
        {
            XElement elem = element.Element(elementName);
            if (elem == null)
                return null;
            return elem.Value;
        }

        /// <summary>
        /// Gets the attribute value.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>String value.</returns>
        /// <exception cref="XmlException">Attribute not found.</exception>
        public static string GetAttributeValue(this XElement element, string attributeName)
        {
            XAttribute attr = element.Attribute(attributeName);
            if (attr == null)
                throw new XmlException(string.Format(GES.csATTRIBUTE_X_NOT_FOUND_FOR_NODE_X, attributeName, element.Name));
            return attr.Value;
        }

        /// <summary>
        /// Tries to get attribute value.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>String value, or null if not found.</returns>
        public static string TryGetAttributeValue(this XElement element, string attributeName)
        {
            XAttribute attr = element.Attribute(attributeName);
            if (attr == null)
                return null;
            return attr.Value;
        }

        public static TEnum TryGetAttributeEnum<TEnum>(this XElement element, string attributeName) where TEnum : struct
        {
            string enumValue = element.TryGetAttributeValue(attributeName);
            if (enumValue == null)
                return default(TEnum);
            TEnum result = default(TEnum);
            bool rc = true;
#if NET35
            rc = Utility.TryEnumParse<TEnum>(enumValue, out result);
#else
            rc = Enum.TryParse<TEnum>(enumValue, out result);
#endif  
            if (!rc)
                return default(TEnum);
            return result;
        }

        public static TEnum GetAttributeEnum<TEnum>(this XElement element, string attributeName) where TEnum : struct
        {
            string enumValue = element.TryGetAttributeValue(attributeName);
            if (enumValue == null)
                throw new XmlException(string.Format(GES.csATTRIBUTE_X_NOT_FOUND_FOR_NODE_X, attributeName, element.Name));
            TEnum result;
#if NET35
            bool rc = Utility.TryEnumParse<TEnum>(enumValue, out result);
#else
            bool rc = Enum.TryParse<TEnum>(enumValue, out result);
#endif
            if (!rc)
                throw new XmlException(string.Format(GES.csATTRIBUTE_X_NOT_FOUND_FOR_NODE_X, attributeName, element.Name));
            return result;
        }

        public static string TryGetElementBase64(this XElement element, string elementName)
        {
            var value = element.TryGetElementValue(elementName);
            byte[] ba = Convert.FromBase64String(value);
            string result = UnicodeEncoding.ASCII.GetString(ba);
            return result;
        }

        public static string GetElementBase64(this XElement element, string elementName)
        {
            string value = element.TryGetElementBase64(elementName);
            if(value == null)
                throw new XmlException(string.Format(GES.csELEMENT_X_NOT_FOUND, elementName));
            return value;
        }
        /// <summary>
        /// Gets the attribute as Int32 value.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>Int32 value.</returns>
        /// <exception cref="XmlException">Attribute not found.</exception>
        public static int GetAttributeInt32Value(this XElement element, string attributeName)
        {
            XAttribute attr = element.Attribute(attributeName);
            if (attr == null)
                throw new XmlException(string.Format(GES.csATTRIBUTE_X_NOT_FOUND_FOR_NODE_X, attributeName, element.Name));
            int i = 0;
            try
            {
                i = XmlConvert.ToInt32(attr.Value);
            }
            catch (FormatException fe)
            {
                throw new XmlException(string.Format(GES.csFAILED_TO_CONVERT_ATTRIBUTE_X_TO_INT32_ERROR_X, attributeName, fe.Message));
            }
            catch (OverflowException oe)
            {
                throw new XmlException(string.Format(GES.csFAILED_TO_CONVERT_ATTRIBUTE_X_TO_INT32_ERROR_X, attributeName, oe.Message));
            }
            return i;
        }


        /// <summary>
        /// Tries to get attribute as Int32 value.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>Int32 value, or null if not found.</returns>
        public static int? TryGetAttributeInt32Value(this XElement element, string attributeName)
        {
            XAttribute attr = element.Attribute(attributeName);
            if (attr == null)
                return null;
            int i = 0;
            try
            {
                i = XmlConvert.ToInt32(attr.Value);
                return i;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format(GES.csFAILED_TO_CONVERT_ATTRIBUTE_X_TO_INT32_ERROR_X, attributeName, ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Tries the get attribute as bool value.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>Boolean value, or null if not found.</returns>
        public static bool? TryGetAttributeBoolValue(this XElement element, string attributeName)
        {
            XAttribute attr = element.Attribute(attributeName);
            if (attr == null)
                return null;
            bool? b = null;
            try
            {
                b = XmlConvert.ToBoolean(attr.Value);
                return b;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format(GES.csFAILED_TO_CONVERT_ATTRIBUTE_X_TO_INT32_ERROR_X, attributeName, ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Loads an XDocument from the specified stream.
        /// </summary>
        /// <param name="inStream">The input stream.</param>
        /// <returns>XDocument</returns>
        public static XDocument LoadStream(Stream inStream)
        {
            using (XmlReader xr = XmlReader.Create(inStream))
            {
                return XDocument.Load(xr);
            }
        }

        /// <summary>
        /// Saves the stream to XDocument supplied.
        /// </summary>
        /// <param name="xmlDoc">The XML doc.</param>
        /// <param name="outStream">The out stream.</param>
        public static void SaveStream(this XDocument xmlDoc, Stream outStream)
        {
            using (XmlWriter xw = XmlWriter.Create(outStream, new XmlWriterSettings() { Indent = true }))
            {
                xmlDoc.Save(xw);
            }
        }
    }
}
