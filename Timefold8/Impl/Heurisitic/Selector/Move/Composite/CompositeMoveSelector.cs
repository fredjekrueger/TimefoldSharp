using TimefoldSharp.Core.Impl.Heurisitic.Selector.AbstractSelectors;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Composite
{
    public abstract class CompositeMoveSelector : AbstractMoveSelector
    {

        public readonly List<MoveSelector> childMoveSelectorList;
        protected readonly bool randomSelection;

        protected CompositeMoveSelector(List<MoveSelector> childMoveSelectorList, bool randomSelection)
        {
            this.childMoveSelectorList = childMoveSelectorList;
            this.randomSelection = randomSelection;
            foreach (var childMoveSelector in childMoveSelectorList)
            {
                phaseLifecycleSupport.AddEventListener(childMoveSelector);
            }
            if (!randomSelection)
            {
                // Only the last childMoveSelector can be neverEnding
                if (childMoveSelectorList.Count > 0)
                {
                    foreach (var childMoveSelector in childMoveSelectorList.GetRange(0, childMoveSelectorList.Count - 1))
                    {
                        if (childMoveSelector.IsNeverEnding())
                        {
                            throw new Exception("The selector .");
                        }
                    }
                }
            }
        }

        public override bool IsCountable()
        {
            foreach (var moveSelector in childMoveSelectorList)
            {
                if (!moveSelector.IsCountable())
                {
                    return false;
                }
            }
            return true;
        }
    }
}



