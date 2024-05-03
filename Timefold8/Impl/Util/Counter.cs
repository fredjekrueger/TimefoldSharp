namespace TimefoldSharp.Core.Impl.Util
{
    public class Counter
    {
        private int count = 0;

        public void Increment()
        {
            Interlocked.Increment(ref count);
        }

        public void Increment(double value)
        {
            Interlocked.Add(ref count, (int)value);
        }

        public void Decrement()
        {
            Interlocked.Decrement(ref count);
        }

        public int Count => count;
    }
}
