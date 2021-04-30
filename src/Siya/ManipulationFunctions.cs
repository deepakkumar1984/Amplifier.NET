using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siya
{
    public partial class nd
    {
        public static NDArray concat(NDArray[] arrays, int axis = 0) => throw new NotImplementedException();

        public static NDArray expand_dims(NDArray x, int axis) => throw new NotImplementedException();

        public static NDArray flip(NDArray x, int? axis = null) => throw new NotImplementedException();

        public static NDArray reshape(NDArray x, Shape shape) => throw new NotImplementedException();

        public static NDArray roll(NDArray x, Shape shift, int? axis = null) => throw new NotImplementedException();

        public static NDArray squeeze(NDArray x, int? axis = null) => throw new NotImplementedException();

        public static NDArray stack(NDArray[] arrays, int axis = 0) => throw new NotImplementedException();
    }
}
