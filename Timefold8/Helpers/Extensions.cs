namespace TimefoldSharp.Core.Helpers
{


    public static class DictionaryExtensions
    {
        public static TKey HigherKey<TKey, TValue>(this SortedDictionary<TKey, TValue> dictionary, TKey key)
        {
            List<TKey> list;
            var indices = GetPossibleIndices(dictionary, key, true, out list);
            if (indices.Item2 == list.Count)
                return default(TKey);

            return list[indices.Item2];
        }
        private static Tuple<int, int> GetPossibleIndices<TKey, TValue>(SortedDictionary<TKey, TValue> dictionary, TKey key, bool strictlyDifferent, out List<TKey> list)
        {
            list = dictionary.Keys.ToList();
            int index = list.BinarySearch(key, dictionary.Comparer);
            if (index >= 0)
            {
                // exists
                if (strictlyDifferent)
                    return Tuple.Create(index - 1, index + 1);
                else
                    return Tuple.Create(index, index);
            }
            else
            {
                // doesn't exist
                int indexOfBiggerNeighbour = ~index; //bitwise complement of the return value

                if (indexOfBiggerNeighbour == list.Count)
                {
                    // bigger than all elements
                    return Tuple.Create(list.Count - 1, list.Count);
                }
                else if (indexOfBiggerNeighbour == 0)
                {
                    // smaller than all elements
                    return Tuple.Create(-1, 0);
                }
                else
                {
                    // Between 2 elements
                    int indexOfSmallerNeighbour = indexOfBiggerNeighbour - 1;
                    return Tuple.Create(indexOfSmallerNeighbour, indexOfBiggerNeighbour);
                }
            }
        }


        // Extension method to mimic Java's computeIfAbsent
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
        {
            if (dictionary.TryGetValue(key, out TValue existingValue))
            {
                return existingValue;
            }

            TValue newValue = valueFactory(key);
            dictionary.Add(key, newValue);
            return newValue;
        }

        public static TValue GetOrAdd<TKey, TValue>(this SortedDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
        {
            if (dictionary.TryGetValue(key, out TValue existingValue))
            {
                return existingValue;
            }

            TValue newValue = valueFactory(key);
            dictionary.Add(key, newValue);
            return newValue;
        }
    }
}
