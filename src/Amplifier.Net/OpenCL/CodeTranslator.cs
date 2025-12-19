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


        public static string Translate(string code)
        {
            code = GlobalCleanup(code);
            code = LineCleanup(code);
            code = RemoveNamespace(code);
            code = FixSpacing(code);
            return code;
        }

        private static string FixSpacing(string code)
        {
            // Fix "globalstruct" -> "global struct" (and similar for local/constant)
            code = Regex.Replace(code, @"\bglobal\s*struct\b", "global struct");
            code = Regex.Replace(code, @"\blocal\s*struct\b", "local struct");
            code = Regex.Replace(code, @"\bconstant\s*struct\b", "constant struct");

            // Clean up multiple spaces into single space
            code = Regex.Replace(code, @"  +", " ");

            // Clean up space before opening parenthesis in function declarations
            // e.g., "void Fill (" -> "void Fill("
            code = Regex.Replace(code, @"(\w)\s+\(", "$1(");

            // Clean up double semicolons (final pass)
            code = code.Replace(";;", ";");

            return code;
        }

        private static string GlobalCleanup(string code)
        {
            // Remove C# attributes that don't translate to OpenCL
            code = Regex.Replace(code, @"\[StructLayout\([^\]]*\)\]\s*", "");
            code = Regex.Replace(code, @"\[MarshalAs\([^\]]*\)\]\s*", "");

            code = code
                    .Replace("[OpenCLKernel]", "__kernel")
                    .Replace("[Global]", "global ")
                    .Replace("[Local]", "local ")
                    .Replace("[Constant]", "constant ")
                    .Replace("[Struct]", "struct ")
                    .Replace("[Input]", "")
                    .Replace("[Output]", "")
                    .Replace("@", "v_");

            // Replace access modifiers with word boundaries to avoid partial matches
            // and preserve the struct/class definitions
            code = Regex.Replace(code, @"\bpublic\s+", "");
            code = Regex.Replace(code, @"\bprivate\s+", "");
            code = Regex.Replace(code, @"\bprotected\s+", "");
            code = Regex.Replace(code, @"\binternal\s+", "");

            // Replace other items in the list (excluding access modifiers already handled)
            string[] otherReplacements = new string[] { "<float>", "<double>", "<complex>",
                                        "<int>", "<uint>", "<long>", "<byte>", "<sbyte>", "this.", "base.", "unsafe fixed", "unsafe" };
            foreach (var item in otherReplacements)
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

                // Handle fixed arrays: "fixed float Data[10]" -> "float Data[10]"
                if (line.Contains("fixed "))
                {
                    line = line.Replace("fixed ", "");
                }

                if (line.Contains("for") && line.Contains(";"))
                {
                    string[] loopSplit = line.Split(';');
                    if (loopSplit[1].Contains("Length"))
                    {

                    }
                }

                // Clean up double semicolons
                line = line.Replace(";;", ";");

                result.AppendLine(line);
            }

            return result.ToString();
        }

        private static string RemoveNamespace(string code)
        {
            string[] splitCode = code.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < splitCode.Length; i++)
            {
                string line = splitCode[i];
                string trimmedLine = line.Trim();

                // Skip empty lines
                if (string.IsNullOrWhiteSpace(trimmedLine))
                    continue;

                // Skip using statements
                if (trimmedLine.StartsWith("using "))
                    continue;

                // Skip file-scoped namespace (C# 10): "namespace Foo.Bar;"
                if (trimmedLine.StartsWith("namespace ") && trimmedLine.EndsWith(";"))
                    continue;

                // Skip traditional namespace declaration: "namespace Foo.Bar" or "namespace Foo.Bar {"
                if (trimmedLine.StartsWith("namespace "))
                    continue;

                // Output the content
                result.AppendLine(line);
            }

            // For traditional namespaces with braces, we need to remove the outer braces
            string output = result.ToString();

            // Check if the output starts with just "{" and ends with just "}"
            string[] outputLines = output.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            if (outputLines.Length >= 2)
            {
                string firstLine = outputLines[0].Trim();
                string lastLine = outputLines[outputLines.Length - 1].Trim();

                // If we have namespace braces wrapping everything, remove them
                if (firstLine == "{" && lastLine == "}")
                {
                    result.Clear();
                    for (int i = 1; i < outputLines.Length - 1; i++)
                    {
                        result.AppendLine(outputLines[i]);
                    }
                    return result.ToString();
                }
            }

            return output;
        }
    }
}
