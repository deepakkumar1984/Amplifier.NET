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

namespace Amplifier
{
    /// <summary>
    /// General Error Strings (GES).
    /// </summary>
    public static class GES
    {
        /// <summary>
        /// Element '{0}' not found.
        /// </summary>
        public const string csELEMENT_X_NOT_FOUND = "Element '{0}' not found.";
        /// <summary>
        /// Attribute '{0}' not found.
        /// </summary>
        public const string csATTRIBUTE_X_NOT_FOUND = "Attribute '{0}' not found.";
        /// <summary>
        /// {0} is not yet implemented!
        /// </summary>
        public static string NOT_IMPLEMENTED = "{0} is not yet implemented!";

        /// <summary>
        /// Binary input string must be 32 characters long.
        /// </summary>
        public static string BINARY_STRING_LEN_ERR = "Binary input string must be 32 characters long.";

        /// <summary>
        /// Unexpected character '{0}' found in binary input string.
        /// </summary>
        public static string BINARY_STRING_FORMAT_ERR = "Unexpected character '{0}' found in binary input string.";

        /// <summary>
        /// Could not find required parameter '{0}'.
        /// </summary>
        public static string COULD_NOT_FIND_REQ_PARAM = "Could not find required parameter '{0}'.";

        /// <summary>
        /// Illegal value '{0}' for parameter '{1}'.
        /// </summary>
        public static string ILLEGAL_VALUE_FOR_PARAM = "Illegal value '{0}' for parameter '{1}'.";

        /// <summary>
        /// Exception '{0}' caught by '{1}'.
        /// </summary>
        public static string EXCEP_CAUGHT_BY = "Exception '{0}' caught by '{1}'.";

        /// <summary>
        /// Basestream not set for '{0}'.
        /// </summary>
        public static string BASESTREAM_NOT_SET_FOR_X = "Basestream not set for '{0}'.";

        /// <summary>
        /// File '{0}' not found for '{1}'.
        /// </summary>
        public static string FILE_X_NOT_FOUND_FOR_X = "File '{0}' not found for '{1}'.";

        /// <summary>
        /// Attribute '{0}' not found for node '{1}'.
        /// </summary>
        public static string csATTRIBUTE_X_NOT_FOUND_FOR_NODE_X = "Attribute '{0}' not found for node '{1}'.";

        /// <summary>
        /// Failed to convert attribute '{0}' to Int32. Error: '{1}'.
        /// </summary>
        public static string csFAILED_TO_CONVERT_ATTRIBUTE_X_TO_INT32_ERROR_X = "Failed to convert attribute '{0}' to Int32. Error: '{1}'.";
    }

    /// <summary>
    /// AmplifierCompileException.
    /// </summary>
    [global::System.Serializable]
    public class AmplifierCompileException : AmplifierException
    {
#pragma warning disable 1591
        public AmplifierCompileException(string message) : base(message) { }

        public AmplifierCompileException(Exception inner, string message) : base(message, inner) { }

        public AmplifierCompileException(string errMsg, params object[] args) : base(string.Format(errMsg, args)) { CheckParamsAreNoExceptions(args); }

        public AmplifierCompileException(Exception inner, string errMsg, params object[] args) : base(string.Format(errMsg, args)) { CheckParamsAreNoExceptions(args); }
        /// <summary>
        /// No source code files specified.
        /// </summary>
        public const string csNO_SOURCES = "No source code files specified.";
        /// <summary>
        /// Compilation error: {0}.
        /// </summary>
        public const string csCOMPILATION_ERROR_X = "Compilation error: {0}.";
        /// <summary>
        /// CUDA directory not found.
        /// </summary>
        public const string csCUDA_DIR_NOT_FOUND = "CUDA directory not found.";
        /// <summary>
        /// No {0} source code present in Amplifier module.
        /// </summary>
        public const string csNO_X_SOURCE_CODE_PRESENT_IN_Amplifier_MODULE = "No {0} source code present in Amplifier module.";
        /// <summary>
        /// No {0} source code present in Amplifier module for {1}.
        /// </summary>
        public const string csNO_X_SOURCE_CODE_PRESENT_IN_Amplifier_MODULE_FOR_X = "No {0} source code present in Amplifier module for {1}.";

        protected AmplifierCompileException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
#pragma warning restore 1591
    }
    
    
    /// <summary>
    /// Base exception for all exceptions except for AmplifierFatalException.
    /// </summary>
    [global::System.Serializable]
    public class AmplifierException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplifierException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AmplifierException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplifierException"/> class.
        /// </summary>
        /// <param name="inner">The inner.</param>
        /// <param name="message">The message.</param>
        public AmplifierException(Exception inner, string message) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplifierException"/> class.
        /// </summary>
        /// <param name="errMsg">The err MSG.</param>
        /// <param name="args">The args.</param>
        public AmplifierException(string errMsg, params object[] args) : base(string.Format(errMsg, args)) { CheckParamsAreNoExceptions(args); }
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplifierException"/> class.
        /// </summary>
        /// <param name="inner">The inner exception.</param>
        /// <param name="errMsg">The err message.</param>
        /// <param name="args">The parameters.</param>
        public AmplifierException(Exception inner, string errMsg, params object[] args) : base(string.Format(errMsg, args)) { CheckParamsAreNoExceptions(args); }
     
#pragma warning disable 1591
        //public const string csCONSTANT_MEMORY_NOT_FOUND = "Constant memory not found.";
        //public const string csHOST_AND_DEVICE_ARRAYS_ARE_OF_DIFFERENT_SIZES = "Host and device arrays are of different sizes.";
        public const string csDEVICE_X_NOT_FOUND = "Device '{0}' not found.";
        public const string csSETTING_X_NOT_FOUND = "Setting '{0}' not found.";
        public const string csMETHOD_X_ALREADY_ADDED_TO_THIS_MODULE = "Method with name '{0}' already added to this module.";
        public const string csCONSTANT_X_ALREADY_ADDED_TO_THIS_MODULE = "Constant with name '{0}' already added to this module.";
        public const string csTYPE_X_ALREADY_ADDED_TO_THIS_MODULE = "Type with name '{0}' already added to this module.";
        public const string csNO_METHODS_FOUND = "No methods found marked to Amplifier.";
        public const string csNO_MEMBERS_FOUND = "No members found.";
        public const string csREFERENCED_Amplifier_ASSEMBLIES_DO_NOT_MATCH = "Referenced Amplifier.dll assemblies do not match.";
        public const string csCOULD_NOT_LOAD_ASSEMBLY_X = "Could not load assembly '{0}'.";
        public const string csCOULD_NOT_FIND_FUNCTION_X = "Could not find function '{0}' in module.";
        public const string csRESOURCE_NOT_FOUND = "Resource not found.";
        public const string csCOULD_NOT_FIND_TYPE_X_IN_ASSEMBLY_X = "Could not find type '{0}' in assembly '{1}'.";
        public const string csCOULD_NOT_FIND_METHOD_X_IN_TYPE_X_IN_ASSEMBLY_X = "Could not find method '{0}' in type '{1}' in assembly '{1}'.";
        public const string csCHECKSUM_FOR_ASSEMBLY_X_DOES_NOT_MATCH_DESERIALIZED_VALUE = "Checksum for assembly '{0}' does not match deserialized value.";
        public const string csX_NOT_SUPPORTED = "{0} is not supported.";
        public const string csX_NOT_SET = "{0} is not set.";
        public const string csNO_SUITABLE_X_PRESENT_IN_Amplifier_MODULE = "No suitable {0} present in Amplifier module.";
        //public const string csPARAMETER_PASSED_BY_REFERENCE_X_NOT_CURRENTLY_SUPPORTED = "Parameter passed by reference ({0}) is currently not supported.";
        //"Parameter passed by reference (out)"
        public const string csX_NOT_CURRENTLY_SUPPORTED = "{0} is not currently supported.";
        //public const string csRATIO_OF_INPUT_AND_OUTPUT_ARRAY_05_10_20 = "Ratio of input array size to output array size must be 0.5, 1.0 or 2.0.";

        public const string csFIELD_X_MUST_BE_MARKED_AS_CONSTANT = "Field {0} must be marked as a constant.";
        public const string csFIELD_X_IS_NOT_MARKED_AS_STATIC = "Field '{0}' is not marked as static.";
        public const string csFIELD_X_CANNOT_BE_MARKED_AS_CONSTANT = "Field '{0}' cannot be marked as constant.";

        //public const string csCAN_ONLY_LAUNCH_GLOBAL_METHODS = "Can only launch global methods.";
        public const string csAmplifier_ATTRIBUTE_IS_MISSING_ON_X = "Amplifier attribute is missing on {0}.";
        public const string csAmplifier_ATTRIBUTE_MUST_BE_X_ON_X = "Amplifier attribute must be {0} on {1}.";
        //public const string csCANNOT_EMULATE_DUMMY_FUNCTION_X = "Cannot launch dummy function '{0}'.";
        //public const string csNO_X_PRESENT_IN_Amplifier_MODULE = "No {0} present in Amplifier module.";

        public const string csVERSION_MISMATCH_EXPECTED_X_GOT_X = "Version mismatch. Expected {0} got {1}.";

        //public const string csFAILED_TO_GET_PROPERIES_X = "Failed to get properties: {0}";
        public const string csCONTEXT_IS_NOT_CURRENT = "Context is not current to the device. Use SetCurrentContext().";
        

        protected virtual void CheckParamsAreNoExceptions(object[] args)
        {
            foreach (object o in args)
                if ((o is Exception))
                    throw new AmplifierFatalException(AmplifierFatalException.csEXCEPTION_PARAMS_LIST);
        }

        protected AmplifierException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
#pragma warning restore 1591
    }

    /// <summary>
    /// An error mostly likely resulting from a programming error within the Amplifier library.
    /// </summary>
    [global::System.Serializable]
    public class AmplifierFatalException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplifierFatalException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AmplifierFatalException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplifierFatalException"/> class.
        /// </summary>
        /// <param name="inner">The inner.</param>
        /// <param name="message">The message.</param>
        public AmplifierFatalException(Exception inner, string message) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplifierFatalException"/> class.
        /// </summary>
        /// <param name="errMsg">The err MSG.</param>
        /// <param name="args">The args.</param>
        public AmplifierFatalException(string errMsg, params object[] args) : base(string.Format(errMsg, args)) { }
#pragma warning disable 1591

        public const string csEXCEPTION_PARAMS_LIST = "Exception params list cannot contain Exception types.";
        public const string csUNEXPECTED_STATE_X = "Unexpected state: {0}";
        protected AmplifierFatalException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
#pragma warning restore 1591
    }



    /// <summary>
    /// Base exception for all dataflow exceptions except for DataflowFatalException.
    /// </summary>
    [global::System.Serializable]
    public class AmplifierLanguageException : AmplifierException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplifierLanguageException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AmplifierLanguageException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplifierLanguageException"/> class.
        /// </summary>
        /// <param name="inner">The inner.</param>
        /// <param name="message">The message.</param>
        public AmplifierLanguageException(Exception inner, string message) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplifierLanguageException"/> class.
        /// </summary>
        /// <param name="errMsg">The err MSG.</param>
        /// <param name="args">The args.</param>
        public AmplifierLanguageException(string errMsg, params object[] args) : base(string.Format(errMsg, args)) { CheckParamsAreNoExceptions(args); }
        /// <summary>
        /// Initializes a new instance of the <see cref="AmplifierLanguageException"/> class.
        /// </summary>
        /// <param name="inner">The inner.</param>
        /// <param name="errMsg">The err MSG.</param>
        /// <param name="args">The args.</param>
        public AmplifierLanguageException(Exception inner, string errMsg, params object[] args) : base(string.Format(errMsg, args)) { CheckParamsAreNoExceptions(args); }

#pragma warning disable 1591
        public const string csMETHOD_X_ON_TYPE_X_IS_NOT_SUPPORTED = "Method '{0}' on type '{1}' is not supported.";

        public const string csMETHOD_X_X_IS_NOT_SUPPORTED = "Method '{0}.{1}' is not supported.";

        public const string csX_IS_NOT_SUPPORTED = "{0} is not supported.";

        public const string csX_IS_NOT_SUPPORTED_IN_X = "{0} is not supported in {1}.";

        public const string csX_IS_NOT_SUPPORTED_FOR_COMPUTE_X = "{0} is not supported for compute capability {1}.";

        public const string csX_ARE_NOT_SUPPORTED = "{0} are not supported.";

        public const string csCONSTANTS_MUST_BE_INITIALIZED = "Constant memory variables must be initialized in-line.";

        public const string csMULTIPLE_VARIABLE_DECLARATIONS_ARE_NOT_SUPPORTED = "Multiple variable declarations in one line are not supported.";

        public const string csMETHOD_X_X_ONLY_SUPPORTS_X = "Method '{0}.{1}' only supports {2}.";

        public const string csEXCEPTION_PARAMS_LIST = "Exception params list cannot contain Exception types.";

        public const string csDIMENSION_OUT_OF_BOUNDS = "Dimension out of bounds.";

        public const string csDIMENSION_OUT_OF_BOUNDS_IN_X = "Dimension out of bounds in {0}.";

        public const string csERROR_IN_X_X_X = "Error in method {0}.{1}: {2}";

        public const string csCOULD_NOT_FIND_CUDA_LANGUAGE = "Could not find Cuda Language!";

        public const string csARRAY_CANNOT_HAVE_MORE_THAN_3D = "Array cannot have more than 3 dimensions.";

        public const string csJAGGED_ARRAYS_NOT_SUPPORTED = "Jagged arrays are not supported.";

        public const string csERROR_TRANSLATING_MEMBER_X_X = "Error translating Member {0}: {1}";

        public const string csSHARED_MEMORY_MUST_BE_CONSTANT = "Shared memory size must be constant at compile time.";

        public const string csX_IS_A_RESERVED_KEYWORD = "'{0}' is a reserved keyword.";

        protected override void CheckParamsAreNoExceptions(object[] args)
        {
            foreach (object o in args)
                if ((o is Exception))
                    throw new AmplifierLanguageException(AmplifierLanguageException.csEXCEPTION_PARAMS_LIST);
        }

        protected AmplifierLanguageException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
#pragma warning restore 1591
    }


}
