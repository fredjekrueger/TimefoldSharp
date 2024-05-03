using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;
using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Composite
{
    public class UnionMoveSelector : CompositeMoveSelector
    {
        protected ScoreDirector scoreDirector;
        protected readonly SelectionProbabilityWeightFactory<MoveSelector> selectorProbabilityWeightFactory;

        public UnionMoveSelector(List<MoveSelector> childMoveSelectorList, bool randomSelection)
            : this(childMoveSelectorList, randomSelection, null)
        {

        }

        public UnionMoveSelector(List<MoveSelector> childMoveSelectorList, bool randomSelection,
                SelectionProbabilityWeightFactory<MoveSelector> selectorProbabilityWeightFactory)
            : base(childMoveSelectorList, randomSelection)
        {
            this.selectorProbabilityWeightFactory = selectorProbabilityWeightFactory;
            if (!randomSelection)
            {
                if (selectorProbabilityWeightFactory != null)
                {
                    throw new Exception("The selector (" + this
                            + ") without randomSelection (" + randomSelection
                            + ") cannot have a selectorProbabilityWeightFactory (" + selectorProbabilityWeightFactory
                            + ").");
                }
            }
        }

        private static IEnumerable<Heurisitic.Move.Move> ToStream(MoveSelector moveSelector)
        {
            return moveSelector; // StreamSupport.stream(moveSelector.Spliterator(), false);
        }

        public override IEnumerator<Heurisitic.Move.Move> GetEnumerator()
        {
            if (!randomSelection)
            {
                IEnumerable<Heurisitic.Move.Move> stream = Enumerable.Empty<Heurisitic.Move.Move>();
                foreach (MoveSelector moveSelector in childMoveSelectorList)
                {
                    stream = stream.Concat(ToStream(moveSelector));
                }
                return stream.GetEnumerator();
            }
            else if (selectorProbabilityWeightFactory == null)
            {
                return new UniformRandomUnionMoveIterator(childMoveSelectorList, workingRandom);
            }
            else
            {
                /*return new BiasedRandomUnionMoveIterator<>(childMoveSelectorList,
                moveSelector=> {
                        double weight = selectorProbabilityWeightFactory.createProbabilityWeight(scoreDirector, moveSelector);
                if (weight < 0.0)
                {
                    throw new Exception(
                            "The selectorProbabilityWeightFactory (" + selectorProbabilityWeightFactory
                                    + ") returned a negative probabilityWeight (" + weight + ").");
                }
                return weight;
            }, workingRandom);*/
                throw new NotImplementedException();
            }
        }

        public override long GetSize()
        {
            long size = 0L;
            foreach (var moveSelector in childMoveSelectorList)
            {
                size += moveSelector.GetSize();
            }
            return size;
        }

        public override bool IsNeverEnding()
        {
            if (randomSelection)
            {
                foreach (var moveSelector in childMoveSelectorList)
                {
                    if (moveSelector.IsNeverEnding())
                    {
                        return true;
                    }
                }
                // The UnionMoveSelector is special: it can be randomSelection true and still neverEnding false
                return false;
            }
            else
            {
                // Only the last childMoveSelector can be neverEnding
                return childMoveSelectorList.Count > 0 && childMoveSelectorList[childMoveSelectorList.Count - 1].IsNeverEnding();
            }
        }

        public override void StepStarted(AbstractStepScope stepScope)
        {
            scoreDirector = stepScope.GetScoreDirector();
            base.StepStarted(stepScope);
        }

        public override string ToString()
        {
            return "Union(" + string.Join("-", childMoveSelectorList) + ")";
        }
    }
}
