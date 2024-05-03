using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.InverseRelation;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Generic.Chained;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Generic
{
    public class SwapMoveSelector : GenericMoveSelector
    {

        protected readonly EntitySelector leftEntitySelector;
        protected readonly EntitySelector rightEntitySelector;
        protected readonly List<GenuineVariableDescriptor> variableDescriptorList;
        protected List<SingletonInverseVariableSupply> inverseVariableSupplyList = null;
        protected readonly bool randomSelection;
        protected readonly bool anyChained;

        public SwapMoveSelector(EntitySelector leftEntitySelector, EntitySelector rightEntitySelector,
            List<GenuineVariableDescriptor> variableDescriptorList, bool randomSelection)
        {
            this.leftEntitySelector = leftEntitySelector;
            this.rightEntitySelector = rightEntitySelector;
            this.variableDescriptorList = variableDescriptorList;
            this.randomSelection = randomSelection;
            EntityDescriptor leftEntityDescriptor = leftEntitySelector.GetEntityDescriptor();
            EntityDescriptor rightEntityDescriptor = rightEntitySelector.GetEntityDescriptor();
            if (!leftEntityDescriptor.EntityClass.Equals(rightEntityDescriptor.EntityClass))
            {
                throw new Exception("The selector (" + this
                        + ") has a leftEntitySelector's entityClass (" + leftEntityDescriptor.EntityClass
                        + ") which is not equal to the rightEntitySelector's entityClass ("
                        + rightEntityDescriptor.EntityClass + ").");
            }
            bool anyChained = false;
            if (variableDescriptorList.Count == 0)
            {
                throw new Exception("The selector (" + this
                        + ")'s variableDescriptors (" + variableDescriptorList + ") is empty.");
            }
            foreach (var variableDescriptor in variableDescriptorList)
            {
                if (!variableDescriptor.EntityDescriptor.EntityClass.IsAssignableFrom(leftEntityDescriptor.EntityClass))
                {
                    throw new Exception("The selector (" + this
                            + ") has a variableDescriptor with a entityClass ("
                            + variableDescriptor.EntityDescriptor.EntityClass
                            + ") which is not equal or a superclass to the leftEntitySelector's entityClass ("
                            + leftEntityDescriptor.EntityClass + ").");
                }
                if (variableDescriptor.IsChained())
                {
                    anyChained = true;
                }
            }
            this.anyChained = anyChained;
            phaseLifecycleSupport.AddEventListener(leftEntitySelector);
            if (leftEntitySelector != rightEntitySelector)
            {
                phaseLifecycleSupport.AddEventListener(rightEntitySelector);
            }
        }

        public override IEnumerator<Heurisitic.Move.Move> GetEnumerator()
        {
            if (!randomSelection)
            {
                return new AbstractOriginalSwapIterator(leftEntitySelector, rightEntitySelector)
                {
                    NewSwapSelection = (leftSubSelection, rightSubSelection) => anyChained
                            ? new ChainedSwapMove(variableDescriptorList, inverseVariableSupplyList, leftSubSelection,
                                    rightSubSelection)
                            : new SwapMove(variableDescriptorList, leftSubSelection, rightSubSelection)
                };
            }
            else
            {
                return new AbstractRandomSwapIterator(leftEntitySelector, rightEntitySelector)
                {
                    NewSwapSelection = (leftSubSelection, rightSubSelection) => anyChained
                        ? new ChainedSwapMove(variableDescriptorList, inverseVariableSupplyList, leftSubSelection,
                                rightSubSelection)
                        : new SwapMove(variableDescriptorList, leftSubSelection, rightSubSelection)
                };

            }
        }

        public override long GetSize()
        {
            return AbstractOriginalSwapIterator.GetSize(leftEntitySelector, rightEntitySelector);
        }

        public override bool IsCountable()
        {
            throw new NotImplementedException();
        }

        public override bool IsNeverEnding()
        {
            throw new NotImplementedException();
        }

        public override void SolvingEnded(SolverScope solverScope)
        {
            base.SolvingEnded(solverScope);
            if (anyChained)
            {
                inverseVariableSupplyList = null;
            }
        }

        public override string ToString()
        {
            return GetType().Name + "(" + leftEntitySelector + ", " + rightEntitySelector + ")";
        }
    }
}
