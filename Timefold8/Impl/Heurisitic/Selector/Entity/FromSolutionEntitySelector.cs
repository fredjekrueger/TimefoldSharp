using System.Collections;
using TimefoldSharp.Core.Config.Heuristics.Selector.Common;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator;
using TimefoldSharp.Core.Impl.Phase.Scope;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity
{
    public sealed class FromSolutionEntitySelector : AbstractDemandEnabledSelector, EntitySelector
    {
        private EntityDescriptor entityDescriptor;
        private SelectionCacheType minimumCacheType;
        private bool randomSelection;
        private List<object> cachedEntityList = null;
        private bool cachedEntityListIsDirty = false;
        private long? cachedEntityListRevision = null;

        public FromSolutionEntitySelector(EntityDescriptor entityDescriptor, SelectionCacheType minimumCacheType, bool randomSelection)
        {
            this.entityDescriptor = entityDescriptor;
            this.minimumCacheType = minimumCacheType;
            this.randomSelection = randomSelection;
        }

        public EntityDescriptor GetEntityDescriptor()
        {
            return entityDescriptor;
        }

        public override SelectionCacheType GetCacheType()
        {
            SelectionCacheType intrinsicCacheType = SelectionCacheType.STEP;
            return intrinsicCacheType.CompareTo(minimumCacheType) > 0
                    ? intrinsicCacheType
                    : minimumCacheType;
        }

        public override void PhaseEnded(AbstractPhaseScope phaseScope)
        {
            base.PhaseEnded(phaseScope);
            cachedEntityList = null;
            cachedEntityListRevision = null;
            cachedEntityListIsDirty = false;
        }

        public override void PhaseStarted(AbstractPhaseScope phaseScope)
        {
            base.PhaseStarted(phaseScope);

            InnerScoreDirector scoreDirector = phaseScope.GetScoreDirector();
            cachedEntityList = entityDescriptor.ExtractEntities(scoreDirector.GetWorkingSolution());
            cachedEntityListRevision = scoreDirector.GetWorkingEntityListRevision();
            cachedEntityListIsDirty = false;
        }

        public override void StepStarted(AbstractStepScope stepScope)
        {
            base.StepStarted(stepScope);
            InnerScoreDirector scoreDirector = stepScope.GetScoreDirector();
            if (scoreDirector.IsWorkingEntityListDirty(cachedEntityListRevision.Value))
            {
                if (minimumCacheType.CompareTo(SelectionCacheType.STEP) > 0)
                {
                    cachedEntityListIsDirty = true;
                }
                else
                {
                    cachedEntityList = entityDescriptor.ExtractEntities(scoreDirector.GetWorkingSolution());
                    cachedEntityListRevision = scoreDirector.GetWorkingEntityListRevision();
                }
            }
        }

        private void CheckCachedEntityListIsDirty()
        {
            if (cachedEntityListIsDirty)
            {
                throw new Exception("The selector (" + this + ") with minimumCacheType (" + minimumCacheType
                        + ")'s workingEntityList became dirty between steps but is still used afterwards.");
            }
        }

        public IEnumerator<object> GetEnumerator()
        {
            CheckCachedEntityListIsDirty();
            if (!randomSelection)
            {
                return cachedEntityList.GetEnumerator();
            }
            else
            {
                return new CachedListRandomIterator<object>(cachedEntityList, workingRandom);
            }
        }

        public override bool IsCountable()
        {
            return true;
        }

        public override bool IsNeverEnding()
        {
            return randomSelection;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public long GetSize()
        {
            return cachedEntityList.Count;
        }

        public override string ToString()
        {
            return GetType().Name + "(" + entityDescriptor.EntityClass.Name + ")";
        }

        public override bool Equals(object other)
        {
            if (this == other)
                return true;
            if (other == null || GetType() != other.GetType())
                return false;
            var that = (FromSolutionEntitySelector)other;
            return randomSelection == that.randomSelection && entityDescriptor.Equals(that.entityDescriptor)
                    && minimumCacheType == that.minimumCacheType;
        }

        public override int GetHashCode()
        {
            return Utils.CombineHashCodes(entityDescriptor, minimumCacheType, randomSelection);
        }

        public IEnumerator<object> ListIterator()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<object> EndingIterator()
        {
            throw new NotImplementedException();
        }
    }
}
