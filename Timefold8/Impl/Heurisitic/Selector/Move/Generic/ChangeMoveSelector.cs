using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.InverseRelation;
using TimefoldSharp.Core.Impl.Domain.Variable.Supply;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Value;
using TimefoldSharp.Core.Impl.Solver.Scope;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Generic
{
    public class ChangeMoveSelector : GenericMoveSelector
    {

        protected readonly EntitySelector entitySelector;
        protected readonly ValueSelector valueSelector;
        protected readonly bool randomSelection;
        protected readonly bool chained;
        protected SingletonInverseVariableSupply inverseVariableSupply = null;

        public ChangeMoveSelector(EntitySelector entitySelector, ValueSelector valueSelector,
           bool randomSelection)
        {
            this.entitySelector = entitySelector;
            this.valueSelector = valueSelector;
            this.randomSelection = randomSelection;
            GenuineVariableDescriptor variableDescriptor = valueSelector.GetVariableDescriptor();
            chained = variableDescriptor.IsChained();
            phaseLifecycleSupport.AddEventListener(entitySelector);
            phaseLifecycleSupport.AddEventListener(valueSelector);
        }

        public override void SolvingEnded(SolverScope solverScope)
        {
            base.SolvingEnded(solverScope);
            if (chained)
            {
                inverseVariableSupply = null;
            }
        }

        public override void SolvingStarted(SolverScope solverScope)
        {
            base.SolvingStarted(solverScope);
            if (chained)
            {
                SupplyManager supplyManager = solverScope.ScoreDirector.GetSupplyManager();
                inverseVariableSupply = supplyManager.Demand(new SingletonInverseVariableDemand(valueSelector.GetVariableDescriptor()))as SingletonInverseVariableSupply;
            }
        }

        public override IEnumerator<Heurisitic.Move.Move> GetEnumerator()
        {
            GenuineVariableDescriptor variableDescriptor = valueSelector.GetVariableDescriptor();
            if (!randomSelection)
            {
                if (chained)
                {
                    return new AbstractOriginalChangeIterator<Heurisitic.Move.Move>(entitySelector, valueSelector)
                    {
                        NewChangeSelection = (entity, toValue) => new ChainedChangeMove(variableDescriptor, entity, toValue, inverseVariableSupply)
                    };
                }
                else
                {
                    return new AbstractOriginalChangeIterator<Heurisitic.Move.Move>(entitySelector, valueSelector)
                    {
                        NewChangeSelection = (entity, toValue) => new ChangeMove(variableDescriptor, entity, toValue)
                    };
                }
            }
            else
            {
                if (chained)
                {
                    return new AbstractRandomChangeIterator<Heurisitic.Move.Move>(entitySelector, valueSelector)
                    {
                        NewChangeSelection = (entity, toValue) => new ChainedChangeMove(variableDescriptor, entity, toValue, inverseVariableSupply)
                    };
                }
                else
                {
                    return new AbstractRandomChangeIterator<Heurisitic.Move.Move>(entitySelector, valueSelector)
                    {
                        NewChangeSelection = (entity, toValue) => new ChangeMove(variableDescriptor, entity, toValue)
                    };
                }
            }
        }

        public override bool IsCountable()
        {
            return entitySelector.IsCountable() && valueSelector.IsCountable();
        }

        public override bool IsNeverEnding()
        {
            return randomSelection || entitySelector.IsNeverEnding() || valueSelector.IsNeverEnding();
        }

        public override long GetSize()
        {
            if (valueSelector is IterableSelector<object>)
            {
                return entitySelector.GetSize() * ((IterableSelector<object>)valueSelector).GetSize();
            }
            else
            {
                long size = 0;
                for (var it = entitySelector.EndingIterator(); it.MoveNext();)
                {
                    Object entity = it.Current;
                    size += valueSelector.GetSize(entity);
                }
                return size;
            }
        }

        public override string ToString()
        {
            return GetType().Name + "(" + entitySelector + ", " + valueSelector + ")";
        }
    }
}

