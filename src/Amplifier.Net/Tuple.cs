using System;
using System.Collections.Generic;
using System.Text;

namespace Amplifier
{
    public class LongTuple
    {
        public long[] data;

        private LongTuple(params long[] _data)
        {
            data = _data;
        }

        public static implicit operator LongTuple(long s) => new LongTuple(s);

        public static implicit operator LongTuple((long, long) s) => new LongTuple(s.Item1, s.Item2);

        public static implicit operator LongTuple((long, long, long) s) => new LongTuple(s.Item1, s.Item2, s.Item3);

        public static implicit operator LongTuple((long, long, long, long) s) => new LongTuple(s.Item1, s.Item2, s.Item3, s.Item4);

        public static implicit operator LongTuple((long, long, long, long, long) s) => new LongTuple(s.Item1, s.Item2, s.Item3, s.Item4, s.Item5);
    }
}
