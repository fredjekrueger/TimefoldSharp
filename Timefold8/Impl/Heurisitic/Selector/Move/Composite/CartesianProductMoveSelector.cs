using TimefoldSharp.Core.Impl.Heurisitic.Move;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Composite
{
    public class CartesianProductMoveSelector : CompositeMoveSelector
    {

        public static readonly Heurisitic.Move.Move EMPTY_MARK = new NoChangeMove();
        public readonly bool ignoreEmptyChildIterators;

        public CartesianProductMoveSelector(List<MoveSelector> childMoveSelectorList, bool ignoreEmptyChildIterators, bool randomSelection)
            : base(childMoveSelectorList, randomSelection)
        {
            this.ignoreEmptyChildIterators = ignoreEmptyChildIterators;
        }


        public override IEnumerator<Heurisitic.Move.Move> GetEnumerator()
        {
            if (!randomSelection)
            {
                return new OriginalCartesianProductMoveIterator(this);
            }
            else
            {
                return new RandomCartesianProductMoveIterator(this);
            }
        }

        public override long GetSize()
        {
            throw new NotImplementedException();
        }

        public override bool IsCountable()
        {
            throw new NotImplementedException();
        }

        public override bool IsNeverEnding()
        {
            throw new NotImplementedException();
        }
    }

    public class OriginalCartesianProductMoveIterator : UpcomingSelectionIterator<Heurisitic.Move.Move>
    {

        private List<IEnumerator<Heurisitic.Move.Move>> moveIteratorList;

        private Heurisitic.Move.Move[] subSelections;

        CartesianProductMoveSelector cartesianProductMoveSelector;
        public OriginalCartesianProductMoveIterator(CartesianProductMoveSelector cartesianProductMoveSelector)
        {
            this.cartesianProductMoveSelector = cartesianProductMoveSelector;
            moveIteratorList = new List<IEnumerator<Heurisitic.Move.Move>>(cartesianProductMoveSelector.childMoveSelectorList.Count);
            for (int i = 0; i < cartesianProductMoveSelector.childMoveSelectorList.Count; i++)
            {
                moveIteratorList.Add(null);
            }
            subSelections = null;
        }

        protected override Heurisitic.Move.Move CreateUpcomingSelection()
        {
            int childSize = moveIteratorList.Count;
            int startingIndex;
            Heurisitic.Move.Move[] moveList = new Heurisitic.Move.Move[childSize];
            if (subSelections == null)
            {
                startingIndex = -1;
            }
            else
            {
                startingIndex = childSize - 1;
                while (startingIndex >= 0)
                {
                    IEnumerator<Heurisitic.Move.Move> moveIterator = moveIteratorList[startingIndex];
                    if (moveIterator.MoveNext())
                    {
                        break;
                    }
                    startingIndex--;
                }
                if (startingIndex < 0)
                {
                    return NoUpcomingSelection();
                }
                // Clone to avoid CompositeMove corruption
                Array.Copy(subSelections, 0, moveList, 0, startingIndex);
                moveList[startingIndex] = moveIteratorList[startingIndex].Current; // Increment the 4 in 004999
            }
            for (int i = startingIndex + 1; i < childSize; i++)
            { // Increment the 9s in 004999
                IEnumerator<Heurisitic.Move.Move> moveIterator = cartesianProductMoveSelector.childMoveSelectorList[i].GetEnumerator();
                moveIteratorList[i] = moveIterator;
                Heurisitic.Move.Move next;
                if (!moveIterator.MoveNext())
                { // in case a moveIterator is empty
                    if (cartesianProductMoveSelector.ignoreEmptyChildIterators)
                    {
                        next = (Heurisitic.Move.Move)CartesianProductMoveSelector.EMPTY_MARK;
                    }
                    else
                    {
                        return NoUpcomingSelection();
                    }
                }
                else
                {
                    next = moveIterator.Current;
                }
                moveList[i] = next;
            }
            // No need to clone to avoid CompositeMove corruption because subSelections's elements never change
            subSelections = moveList;
            if (cartesianProductMoveSelector.ignoreEmptyChildIterators)
            {
                // Clone because EMPTY_MARK should survive in subSelections
                Heurisitic.Move.Move[] newMoveList = new Heurisitic.Move.Move[childSize];
                int newSize = 0;
                for (int i = 0; i < childSize; i++)
                {
                    if (moveList[i] != CartesianProductMoveSelector.EMPTY_MARK)
                    {
                        newMoveList[newSize] = moveList[i];
                        newSize++;
                    }
                }
                if (newSize == 0)
                {
                    return NoUpcomingSelection();
                }
                else if (newSize == 1)
                {
                    return newMoveList[0];
                }
                moveList = newMoveList.Take(newSize).ToArray();
            }
            return CompositeMove.BuildMove(moveList);
        }
    }

    public class RandomCartesianProductMoveIterator : SelectionIterator<Heurisitic.Move.Move>
    {

        private List<IEnumerator<Heurisitic.Move.Move>> moveIteratorList;
        private bool? empty;
        CartesianProductMoveSelector cartesianProductMoveSelector;

        public RandomCartesianProductMoveIterator(CartesianProductMoveSelector cartesianProductMoveSelector)
        {
            this.cartesianProductMoveSelector = cartesianProductMoveSelector;
            moveIteratorList = new List<IEnumerator<Heurisitic.Move.Move>>(cartesianProductMoveSelector.childMoveSelectorList.Count);
            empty = null;
            foreach (var moveSelector in cartesianProductMoveSelector.childMoveSelectorList)
            {
                moveIteratorList.Add(moveSelector.GetEnumerator());
            }
        }

        public override bool MoveNext()
        {
            if (empty == null)
            { // Only done in the first call
                int emptyCount = 0;
                foreach (var moveIterator in moveIteratorList)
                {
                    if (!moveIterator.MoveNext())
                    {
                        emptyCount++;
                        if (!cartesianProductMoveSelector.ignoreEmptyChildIterators)
                        {
                            break;
                        }
                    }
                }
                empty = cartesianProductMoveSelector.ignoreEmptyChildIterators ? emptyCount == moveIteratorList.Count : emptyCount > 0;
            }
            return !empty.Value;
        }

        public override Heurisitic.Move.Move Current
        {
            get
            {
                var moveList = new List<Heurisitic.Move.Move>(moveIteratorList.Count);
                for (int i = 0; i < moveIteratorList.Count; i++)
                {
                    var moveIterator = moveIteratorList[i];
                    bool skip = false;
                    if (!moveIterator.MoveNext())
                    {
                        MoveSelector moveSelector = cartesianProductMoveSelector.childMoveSelectorList[i];
                        moveIterator = moveSelector.GetEnumerator();
                        moveIteratorList[i] = moveIterator;
                        if (!moveIterator.MoveNext())
                        {
                            if (cartesianProductMoveSelector.ignoreEmptyChildIterators)
                            {
                                skip = true;
                            }
                            else
                            {
                                throw new Exception("The iterator of childMoveSelector (" + moveSelector + ") is empty.");
                            }
                        }
                    }
                    if (!skip)
                    {
                        moveList.Add(moveIterator.Current);
                    }
                }
                if (cartesianProductMoveSelector.ignoreEmptyChildIterators)
                {
                    if (moveList.Count == 0)
                    {
                        throw new Exception("All iterators of childMoveSelectorList (" + cartesianProductMoveSelector.childMoveSelectorList + ") are empty.");
                    }
                    else if (moveList.Count == 1)
                    {
                        return moveList[0];
                    }
                }
                return CompositeMove.BuildMove(moveList.ToArray());
            }
        }
    }
}
