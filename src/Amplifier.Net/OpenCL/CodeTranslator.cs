using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Amplifier.OpenCL
{
    public class CodeTranslator
    {
        /// <summary>
        /// The replace empty list
        /// </summary>
        private static string[] replaceEmptyList = new string[] { "public", "private", "protected", "internal", "<float>", "<double>", "<complex>",
                                        "<int>", "<uint>", "<long>", "<byte>", "<sbyte>", "this.", "base.", "unsafe fixed", "unsafe" };


        public static string Translate(string code)
        {
            code = GlobalCleanup(code);
            code = LineCleanup(code);
            code = RemoveNamespace(code);
            return code;
        }

        private static string GlobalCleanup(string code)
        {
            code = code
                    .Replace("[OpenCLKernel]", "__kernel")
                    .Replace("[Global]", "global")
                    .Replace("[Struct]", "struct")
                    .Replace("[Input]", "")
                    .Replace("[Output]", "")
                    .Replace("@", "v_");

            foreach (var item in replaceEmptyList)
            {
                code = code.Replace(item, "");
            }

            Regex floatRegEx = new Regex(@"(\d+)(\.\d+)*f]?");
            var matches = floatRegEx.Matches(code);
            foreach (Match match in matches)
            {
                code = code.Replace(match.Value, match.Value.Replace("f", ""));
            }

            floatRegEx = new Regex(@"(\d+)(\.\d+)*u]?");
            matches = floatRegEx.Matches(code);
            foreach (Match match in matches)
            {
                code = code.Replace(match.Value, match.Value.Replace("u", ""));
            }

            return code;
        }

        private static string LineCleanup(string code)
        {
            string[] splitCode = code.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            int length = splitCode.Length;
            int start = 0;
           
            StringBuilder result = new StringBuilder();

            for (int i = start; i < length; i++)
            {
                string line = splitCode[i];
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.Contains("using "))
                    continue;

                if (line.Contains("global") && line.Contains("[]"))
                {
                    line = line.Replace("[]", "*");
                }

                if (line.Contains("for") && line.Contains(";"))
                {
                    string[] loopSplit = line.Split(';');
                    if (loopSplit[1].Contains("Length"))
                    {

                    }
                }

                result.AppendLine(line);
            }

            return result.ToString();
        }

        private static string RemoveNamespace(string code)
        {
            string[] splitCode = code.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            int length = splitCode.Length;
            int start = 0;
            if (code.Contains("namespace"))
            {
                start = 2;
                length = splitCode.ToList().LastIndexOf("}");
            }

            StringBuilder result = new StringBuilder();

            for (int i = start; i < length; i++)
            {
                result.AppendLine(splitCode[i]);
            }

            return result.ToString();
        }
    }
}
