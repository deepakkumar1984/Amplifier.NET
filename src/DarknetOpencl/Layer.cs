using Amplifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarknetOpencl
{
    public abstract class Layer : kernel_ops
    {
        public Dictionary<string, object> Parameters { get; set; }

        public Layer()
        {
            Parameters = new Dictionary<string, object>();
        }

        public void Set<T>(string name, T value)
        {
            if(Parameters.ContainsKey(name))
            {
                Parameters[name] = value;
            }
            else
            {
                Parameters.Add(name, value);
            }
        }

        public T Get<T>(string name)
        {
            if (Parameters.ContainsKey(name))
            {
                return (T)Parameters[name];
            }

            return default(T);
        }

        public abstract XArray Forward(XArray x, params XArray[] args);

        public abstract XArray Backward(XArray x, XArray delta, params XArray[] args);
    }
}
