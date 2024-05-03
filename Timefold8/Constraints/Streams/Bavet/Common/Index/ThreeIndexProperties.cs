using TimefoldSharp.Core.Impl.Util;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Index
{
    public class ThreeIndexProperties : IndexProperties
    {

        private readonly object propertyA;
        private readonly object propertyB;
        private readonly object propertyC;

        public ThreeIndexProperties(object propertyA, object propertyB, object propertyC)
        {
            this.propertyA = propertyA;
            this.propertyB = propertyB;
            this.propertyC = propertyC;
        }

        public object ToKey(int index)
        {
            switch (index)
            {
                case 0:
                    return propertyA;
                case 1:
                    return propertyB;
                case 2:
                    return propertyC;
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
                    return PairHelper<object, object>.Of(ToKey(from), ToKey(from + 1));
                case 3:
                    if (from != 0 || to != 3)
                    {
                        throw new Exception("Impossible state: key from (" + from + ") to (" + to + ").");
                    }
                    return this;
                default:
                    throw new Exception("Impossible state: key from (" + from + ") to (" + to + ").");
            }
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            else if (!(obj is ThreeIndexProperties))
            {
                return false;
            }
            else
            {
                ThreeIndexProperties other = (ThreeIndexProperties)obj;
                return Equals(this.propertyA, other.propertyA) &&
                       Equals(this.propertyB, other.propertyB) &&
                       Equals(this.propertyC, other.propertyC);
            }
        }

        public override int GetHashCode()
        {
            int result = propertyA.GetHashCode();
            result = 31 * result + (propertyB.GetHashCode());
            result = 31 * result + (propertyC.GetHashCode());
            return result;
        }

        public override string ToString()
        {
            return "[" + propertyA + ", " + propertyB + ", " + propertyC + "]";
        }

    }
}
