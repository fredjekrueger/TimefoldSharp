using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimefoldSharp.Core.Impl.Util
{
    public sealed class MutableLong : IComparable<MutableLong>
    {

        public long Value { get; set; }

        public MutableLong() : this(0)
        {

        }

        public long Increment()
        {
            return Add(1);
        }

        public long Decrement()
        {
            return Subtract(1);
        }

        public long Add(long addend)
        {
            Value += addend;
            return Value;
        }

        public long Subtract(long subtrahend)
        {
            Value -= subtrahend;
            return Value;
        }

        public MutableLong(long value)
        {
            this.Value = value;
        }

        public int CompareTo(MutableLong other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
