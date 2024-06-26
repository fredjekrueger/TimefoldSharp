using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score.Stream.Tri;
using TimefoldSharp.Core.API.Score.Stream.Uni;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Index;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Util;

namespace TimefoldSharp.Core.API.Score.Stream
{
    public class ConstraintCollectors
    {
        public static TriConstraintCollector<A, B, C, object, int> Sum<A, B, C>(Func<A, B, C, int> groupValueMapping)
        {
            return new DefaultTriConstraintCollector<A, B, C, object, int>(() => new MutableInt(),
                (resultContainer, a, b, c) =>
                {
                    int value = groupValueMapping(a, b, c);
                    return InnerSum((MutableInt)resultContainer, value);
                },
                (mi) => ((MutableInt)mi).Value);
        }

        public static UniConstraintCollector<A, Dictionary<object, MutableLong>, long> CountDistinctLong<A>(Func<A, object> groupValueMapping)
        {
            return new DefaultUniConstraintCollector<A, Dictionary<object, MutableLong>, long>(() => new Dictionary<object, MutableLong>(),
                    (resultContainer, a) => {
                        object value = groupValueMapping.Invoke(a);
                        return InnerCountDistinctLong(resultContainer, value);
                    },
                resultContainer => (long)resultContainer.Count());
        }

        private static Action InnerCountDistinctLong<Value_>(Dictionary<Value_, MutableLong> resultContainer, Value_ value)
        {
            MutableLong valueCountContainer = resultContainer.GetOrAdd(value, k=> new MutableLong());
            valueCountContainer.Increment();
            return ()=> {
                long valueCount = valueCountContainer.Value;
                if (valueCount < 1L)
                {
                    throw new Exception("Impossible state: the value (" + value +
                            ") is removed more times than it was added.");
                }
                if (valueCount == 1L)
                {
                    resultContainer.Remove(value);
                }
                else
                {
                    valueCountContainer.Value = valueCount - 1L;
                }
            };
        }

        public static UniConstraintCollector<A, object, int> Sum<A>(Func<A, int> groupValueMapping)
        {
            return new DefaultUniConstraintCollector<A, object, int>(
                    () => new MutableInt(),
                    (resultContainer, a) =>
                    {
                        int value = groupValueMapping(a);
                        return InnerSum((MutableInt)resultContainer, value);
                    },
                (mi) => ((MutableInt)mi).Value);
        }

        private static Action InnerSum(MutableInt resultContainer, int value)
        {
            resultContainer.Add(value);
            return () => resultContainer.Subtract(value);
        }
    }
}
