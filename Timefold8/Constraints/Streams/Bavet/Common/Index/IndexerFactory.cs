using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.Constraints.Streams.Common;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Index
{
    public class IndexerFactory
    {
        private readonly JoinerType[] joinerTypes;

        public bool HasJoiners()
        {
            return joinerTypes.Length > 0;
        }

        public Indexer<T> BuildIndexer<T>(bool isLeftBridge)
        {
            if (joinerTypes.Length == 0)
            { // NoneJoiner results in NoneIndexer.
                return new NoneIndexer<T>();
            }
            else if (joinerTypes.Length == 1)
            { // Single joiner maps directly to EqualsIndexer or ComparisonIndexer.
                JoinerType joinerType = joinerTypes[0];
                if (joinerType == JoinerType.EQUAL)
                {
                    return new EqualsIndexer<T>(() => new NoneIndexer<T>());
                }
                else
                {
                    return new ComparisonIndexer<T>(isLeftBridge ? joinerType : JoinerTypeEnumHelper.Flip(joinerType), () => new NoneIndexer<T>());
                }
            }
            SortedDictionary<int?, JoinerType> joinerTypeMap = new SortedDictionary<int?, JoinerType>();
            for (int i = 1; i <= joinerTypes.Length; i++)
            {
                JoinerType? joinerType = null;
                if (i < joinerTypes.Length)
                    joinerType = joinerTypes[i];
                JoinerType previousJoinerType = joinerTypes[i - 1];
                if (joinerType != JoinerType.EQUAL || previousJoinerType != joinerType)
                {
                    joinerTypeMap.Add(i, previousJoinerType);
                }
            }
            var descendingJoinerTypeMap = new SortedDictionary<int?, JoinerType>(joinerTypeMap, Comparer<int?>.Create((x, y) => y.Value.CompareTo(x)));
            Func<Indexer<T>> downstreamIndexerSupplier = () => new NoneIndexer<T>();
            foreach (var entry in descendingJoinerTypeMap)
            {
                int? endingPropertyExclusive = entry.Key;
                int? previousEndingPropertyExclusiveOrNull = descendingJoinerTypeMap.HigherKey(endingPropertyExclusive);
                int previousEndingPropertyExclusive = previousEndingPropertyExclusiveOrNull == null ? 0 : previousEndingPropertyExclusiveOrNull.Value;
                JoinerType joinerType = entry.Value;
                Func<Indexer<T>> actualDownstreamIndexerSupplier = downstreamIndexerSupplier;
                if (joinerType == JoinerType.EQUAL)
                {
                    if (endingPropertyExclusive <= previousEndingPropertyExclusive)
                    {
                        throw new Exception("Impossible state: index key ending position <= starting position ("
                                + endingPropertyExclusive + " <= " + previousEndingPropertyExclusive + ")");
                    }
                    downstreamIndexerSupplier = () => new EqualsIndexer<T>(previousEndingPropertyExclusive, endingPropertyExclusive.Value, actualDownstreamIndexerSupplier);
                }
                else
                {
                    JoinerType actualJoinerType = isLeftBridge ? joinerType : JoinerTypeEnumHelper.Flip(joinerType);

                    downstreamIndexerSupplier = () => new ComparisonIndexer<T>(actualJoinerType, previousEndingPropertyExclusive, actualDownstreamIndexerSupplier);
                }
            }
            return downstreamIndexerSupplier.Invoke();
        }

        public IndexerFactory(IJoiner joiner)
        {
            int joinerCount = joiner.GetJoinerCount();
            joinerTypes = new JoinerType[joinerCount];
            for (int i = 0; i < joinerCount; i++)
            {
                JoinerType joinerType = joiner.GetJoinerType(i);
                switch (joinerType)
                {
                    case JoinerType.EQUAL:
                    case JoinerType.LESS_THAN:
                    case JoinerType.LESS_THAN_OR_EQUAL:
                    case JoinerType.GREATER_THAN:
                    case JoinerType.GREATER_THAN_OR_EQUAL:
                        break;
                    default:
                        throw new Exception("Unsupported joiner type (" + joinerType + ").");
                }
                joinerTypes[i] = joiner.GetJoinerType(i);
            }
        }
    }
}
