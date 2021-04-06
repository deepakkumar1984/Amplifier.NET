using Amplifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarknetOpencl
{
    public class Activation : Layer
    {
        public Activation(Activations act_type)
        {
            Set("ActType", act_type);
        }

        public override XArray Backward(XArray x, XArray delta, params XArray[] args)
        {
            amp.exec.activate_array_kernel(x, 0, x.Count, Get<Activations>("ActType"));
            return x;
        }

        public override XArray Forward(XArray x, params XArray[] args)
        {
            amp.exec.activate_array_kernel(x, 0, x.Count, Get<Activations>("ActType"));
            return x;
        }
    }
}
