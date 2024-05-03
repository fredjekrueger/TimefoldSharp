using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Composite
{
    public sealed class UniformRandomUnionMoveIterator : SelectionIterator<Heurisitic.Move.Move>
    {

        private IEnumerable<IEnumerator<Heurisitic.Move.Move>> moveIteratorList;
        private Random workingRandom;

        public UniformRandomUnionMoveIterator(List<MoveSelector> childMoveSelectorList, Random workingRandom)
        {
            var enumerators = childMoveSelectorList.Select(selector => selector.GetEnumerator());
            moveIteratorList = enumerators.Where(iterator => iterator.MoveNext()).ToList();
            this.workingRandom = workingRandom;
        }

        public override Heurisitic.Move.Move Current
        {
            get
            {
                int index = workingRandom.Next(moveIteratorList.Count());
                var moveIterator = moveIteratorList.ElementAt(index);
                var next = moveIterator.Current;
                if (!moveIterator.MoveNext())
                {
                    //moveIteratorList..RemoveAt(index);
                }
                return next;
            }
        }

        public override bool MoveNext()
        {
            return moveIteratorList.Any();
        }
    }
}
