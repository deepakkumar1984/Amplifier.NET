using System;
using System.Collections.Generic;
using System.Text;

namespace Amplifier.OpenCL
{
    public struct size_t
    {
        public static implicit operator size_t(uint d)
        {
            return new size_t();
        }
    }

    public struct ptrdiff_t
    {
        public static implicit operator ptrdiff_t(uint d)
        {
            return new ptrdiff_t();
        }
    }

    public struct intptr_t
    {
        public static implicit operator intptr_t(uint d)
        {
            return new intptr_t();
        }
    }

    public struct uintptr_t
    {
        public static implicit operator uintptr_t(uint d)
        {
            return new uintptr_t();
        }
    }
}
