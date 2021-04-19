using Amplifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siya
{
    public partial class sx
    {
        internal static OpenCLCompiler compiler = new OpenCLCompiler();

        public static Device cpu()
        {
            return compiler.Devices.Where(x => x.Type == DeviceType.CPU).FirstOrDefault();
        }

        public static Device gpu(int device_id = 0)
        {
            return compiler.Devices.Where(x => x.Type == DeviceType.GPU && x.ID == device_id).FirstOrDefault();
        }
    }
}
