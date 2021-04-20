using Amplifier;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siya
{
    public partial class sx
    {
        internal static dynamic exec = null;
        public static void use_device(DeviceType type, int id)
        {
            compiler.UseDevice(id);
            compiler.CompileKernel(get_source());
            exec = compiler.Exec;
        }

        private static string get_source()
        {
            StringBuilder sb = new StringBuilder();
            DirectoryInfo dir = new DirectoryInfo("./kernels/");
            var files = dir.GetFiles("*.cu");
            foreach (var file in files)
            {
                if(file.Name.StartsWith("template_"))
                {
                    string template = file.OpenText().ReadToEnd();
                    sb.AppendLine(template.Replace("<DTYPE_NAME>", "float"));
                    sb.AppendLine(template.Replace("<DTYPE_NAME>", "double"));
                    //template = template.Replace("fabs(", "abs(");
                    //sb.AppendLine(template.Replace("<DTYPE_NAME>", "char"));
                    //sb.AppendLine(template.Replace("<DTYPE_NAME>", "uchar"));
                    //sb.AppendLine(template.Replace("<DTYPE_NAME>", "short"));
                    //sb.AppendLine(template.Replace("<DTYPE_NAME>", "int"));
                    //sb.AppendLine(template.Replace("<DTYPE_NAME>", "long"));
                    //sb.AppendLine(template.Replace("<DTYPE_NAME>", "ushort"));
                    //sb.AppendLine(template.Replace("<DTYPE_NAME>", "uint"));
                    //sb.AppendLine(template.Replace("<DTYPE_NAME>", "ulong"));
                    //sb.AppendLine(template.Replace("<DTYPE_NAME>", "bool"));
                }
                
                sb.AppendLine();
            }

            return sb.ToString();
        }

        internal static NDArray unary_exec(NDArray x, string function)
        {
            var (dtype, dtype_str) = check_and_get_dtype(new NDArray[] { x });
            NDArray r = new NDArray(x.shape, dtype);
            compiler.Execute($"{dtype_str}_{function}", x, r);
            return r;
        }

        internal static NDArray binary_exec(NDArray x1, NDArray x2, string function)
        {
            var (dtype, dtype_str) = check_and_get_dtype(new NDArray[] { x1, x2 });
            NDArray r = new NDArray(x1.shape, dtype);
            compiler.Execute($"{dtype_str}_{function}", x1, x2, r);
            return r;
        }

        private static (DType, string) check_and_get_dtype(NDArray[] arrays)
        {
            int size = arrays[0].dtype.Size();
            DType finalDtype = arrays[0].dtype;
            foreach (var item in arrays)
            {
                if (item.dtype.Size() > size)
                {
                    size = item.dtype.Size();
                    finalDtype = item.dtype;
                }
            }

            return (finalDtype, get_dtype_str(finalDtype));
        }

        private static string get_dtype_str(DType dtype)
        {
            switch (dtype)
            {
                case DType.Float32:
                    return "float";
                case DType.Float64:
                    return "double";
                case DType.Int8:
                    return "float";
                case DType.Int16:
                    return "float";
                case DType.Int32:
                    return "float";
                case DType.Int64:
                    return "double";
                case DType.UInt8:
                    return "float";
                case DType.UInt16:
                    return "float";
                case DType.UInt32:
                    return "float";
                case DType.UInt64:
                    return "double";
                case DType.Bool:
                    return "float";
                default:
                    return "float";
            }
        }
    }
}
