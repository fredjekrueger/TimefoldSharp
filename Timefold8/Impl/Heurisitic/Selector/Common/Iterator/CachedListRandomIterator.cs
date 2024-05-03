namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator
{
    public class CachedListRandomIterator<S> : SelectionIterator<S>
    {
        protected readonly List<S> cachedList;
        protected readonly Random workingRandom;
        protected readonly bool empty;

        public CachedListRandomIterator(List<S> cachedList, Random workingRandom)
        {
            this.cachedList = cachedList;
            this.workingRandom = workingRandom;
            empty = cachedList.Count == 0;
        }

        public override S Current
        {
            get
            {
                if (empty)
                {
                    throw new Exception();
                }
                int index = workingRandom.Next(cachedList.Count());
                return cachedList[index];
            }
        }

        public override bool MoveNext()
        {
            return !empty;
        }
    }
}
