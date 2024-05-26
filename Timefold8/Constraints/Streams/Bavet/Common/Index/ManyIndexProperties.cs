using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Util;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Index
{
    public sealed class ManyIndexProperties : IndexProperties
    {

        public ManyIndexProperties(object[] properties)
        {
            this.properties = properties;
        }

        private object[] properties;

        public object ToKey(int from, int to)
        {
            switch (to - from)
            {
                case 1:
                    return ToKey(from);
                case 2:
                    return PairHelper<object, object>.Of(ToKey(from), ToKey(from + 1));
                case 3:
                    return TripleHelper<object,object,object>.Of(ToKey(from), ToKey(from + 1), ToKey(from + 2));
                case 4:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        public object ToKey(int index)
        {
            return properties[index];
        }

        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            if (!(o is ManyIndexProperties)) {
                return false;
            }
            ManyIndexProperties other = (ManyIndexProperties)o;
            return Enumerable.SequenceEqual(properties, other.properties);
        }
        public override int GetHashCode()
        {
            return Utils.CombineHashCodes(properties);
        }

        public override string ToString()
        {
            return string.Join(" - ",  properties);
        }
    }
}
