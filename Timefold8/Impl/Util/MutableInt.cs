namespace TimefoldSharp.Core.Impl.Util
{
    public sealed class MutableInt : IComparable<MutableInt>
    {

        public int Value { get; set; }

        public MutableInt() : this(0)
        {

        }

        public int Increment()
        {
            return Add(1);
        }

        public int Decrement()
        {
            return Subtract(1);
        }

        public int Add(int addend)
        {
            Value += addend;
            return Value;
        }

        public int Subtract(int subtrahend)
        {
            Value -= subtrahend;
            return Value;
        }

        public MutableInt(int value)
        {
            this.Value = value;
        }

        public int CompareTo(MutableInt other)
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
