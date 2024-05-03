namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Index
{
    internal class TwoIndexProperties : IndexProperties
    {
        private readonly object propertyA;
        private readonly object propertyB;

        public TwoIndexProperties(object propertyA, object propertyB)
        {
            this.propertyA = propertyA;
            this.propertyB = propertyB;
        }

        public object ToKey(int index)
        {
            switch (index)
            {
                case 0:
                    return propertyA;
                case 1:
                    return propertyB;
                default:
                    throw new Exception("Impossible state: index (" + index + ") != 0");
            }
        }

        public object ToKey(int from, int to)
        {
            switch (to - from)
            {
                case 1:
                    return ToKey(from);
                case 2:
                    if (from != 0 || to != 2)
                    {
                        throw new Exception("Impossible state: key from (" + from + ") to (" + to + ").");
                    }
                    return this;
                default:
                    throw new Exception("Impossible state: key from (" + from + ") to (" + to + ").");
            }
        }

        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            if (!(o is TwoIndexProperties))
            {
                return false;
            }
            TwoIndexProperties other = (TwoIndexProperties)o;
            return object.Equals(propertyA, other.propertyA)
                && object.Equals(propertyB, other.propertyB);
        }

        public override int GetHashCode()
        {
            int result = propertyA.GetHashCode();
            result = 31 * result + propertyB.GetHashCode();
            return result;
        }

        public override string ToString()
        {
            return "[" + propertyA + ", " + propertyB + "]";
        }
    }
}
