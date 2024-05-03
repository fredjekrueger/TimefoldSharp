namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Index
{
    public class SingleIndexProperties : IndexProperties
    {
        private readonly object property;

        public SingleIndexProperties(object property)
        {
            this.property = property;
        }

        public object ToKey(int index)
        {
            if (index != 0)
            {
                throw new Exception("Impossible state: index (" + index + ") != 0");
            }
            return property;
        }

        public object ToKey(int from, int to)
        {
            if (to != 1)
            {
                throw new Exception("Impossible state: key from (" + from + ") to (" + to + ").");
            }
            return ToKey(from);
        }

        public override string ToString()
        {
            return "[" + property + "]";
        }

        public override int GetHashCode()
        {
            return property.GetHashCode();
        }

        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            if (!(o is SingleIndexProperties))
            {
                return false;
            }
            SingleIndexProperties other = (SingleIndexProperties)o;
            return Object.Equals(property, other.property);
        }
    }
}
