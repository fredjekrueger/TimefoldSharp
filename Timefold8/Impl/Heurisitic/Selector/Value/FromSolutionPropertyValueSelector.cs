using System.Collections;
using TimefoldSharp.Core.API.Domain.ValueRange;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Impl.Domain.ValueRange.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value
{
    public sealed class FromSolutionPropertyValueSelector : AbstractDemandEnabledSelector, EntityIndependentValueSelector
    {

        private readonly EntityIndependentValueRangeDescriptor valueRangeDescriptor;
        private readonly SelectionCacheType minimumCacheType;
        private readonly bool randomSelection;
        private readonly bool valueRangeMightContainEntity;

        private ValueRange<Object> cachedValueRange = null;
        private long? cachedEntityListRevision = null;
        private bool cachedEntityListIsDirty = false;

        public FromSolutionPropertyValueSelector(EntityIndependentValueRangeDescriptor valueRangeDescriptor,
                SelectionCacheType minimumCacheType, bool randomSelection)
        {
            this.valueRangeDescriptor = valueRangeDescriptor;
            this.minimumCacheType = minimumCacheType;
            this.randomSelection = randomSelection;
            valueRangeMightContainEntity = valueRangeDescriptor.MightContainEntity();
        }

        private void CheckCachedEntityListIsDirty()
        {
            if (cachedEntityListIsDirty)
            {
                throw new Exception("The selector (" + this + ") with minimumCacheType (" + minimumCacheType
                        + ")'s workingEntityList became dirty between steps but is still used afterwards.");
            }
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<object> GetEnumerator()
        {
            CheckCachedEntityListIsDirty();
            if (randomSelection)
            {
                return cachedValueRange.CreateRandomIterator(workingRandom);
            }
            if (cachedValueRange is CountableValueRange<object> range)
            {
                return range.CreateOriginalIterator();
            }
            throw new Exception("Value range's class (" + cachedValueRange.GetType() + ") " +
                    "does not implement " +
                "yet selectionOrder is not " + SelectionOrder.RANDOM + ".\n" +
                "Maybe switch selectors' selectionOrder to " + SelectionOrder.RANDOM + "?\n" +
                "Maybe switch selectors' cacheType to " + SelectionCacheType.JUST_IN_TIME + "?");
        }

        public GenuineVariableDescriptor GetVariableDescriptor()
        {
            return valueRangeDescriptor.GetVariableDescriptor();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override bool IsCountable()
        {
            return valueRangeDescriptor.IsCountable();
        }

        public override bool IsNeverEnding()
        {
            return randomSelection || !IsCountable();
        }

        public IEnumerator<object> Iterator(object entity)
        {
            return GetEnumerator();
        }

        public override void PhaseStarted(AbstractPhaseScope phaseScope)
        {
            base.PhaseStarted(phaseScope);
            InnerScoreDirector scoreDirector = phaseScope.GetScoreDirector();
            cachedValueRange = (ValueRange<Object>)valueRangeDescriptor.ExtractValueRange(scoreDirector.GetWorkingSolution());
            if (valueRangeMightContainEntity)
            {
                cachedEntityListRevision = scoreDirector.GetWorkingEntityListRevision();
                cachedEntityListIsDirty = false;
            }
        }

        public override void PhaseEnded(AbstractPhaseScope phaseScope)
        {
            base.PhaseEnded(phaseScope);
            cachedValueRange = null;
            if (valueRangeMightContainEntity)
            {
                cachedEntityListRevision = null;
                cachedEntityListIsDirty = false;
            }
        }

        public override void StepStarted(AbstractStepScope stepScope)
        {
            base.StepStarted(stepScope);
            if (valueRangeMightContainEntity)
            {
                InnerScoreDirector scoreDirector = stepScope.GetScoreDirector();
                if (scoreDirector.IsWorkingEntityListDirty(cachedEntityListRevision.Value))
                {
                    if (minimumCacheType.CompareTo(SelectionCacheType.STEP) > 0)
                    {
                        cachedEntityListIsDirty = true;
                    }
                    else
                    {
                        cachedValueRange = (ValueRange<Object>)valueRangeDescriptor
                                .ExtractValueRange(scoreDirector.GetWorkingSolution());
                        cachedEntityListRevision = scoreDirector.GetWorkingEntityListRevision();
                    }
                }
            }
        }

        public long GetSize(object entity)
        {
            return GetSize();
        }

        public long GetSize()
        {
            return ((CountableValueRange<object>)cachedValueRange).GetSize();
        }
    }
}
