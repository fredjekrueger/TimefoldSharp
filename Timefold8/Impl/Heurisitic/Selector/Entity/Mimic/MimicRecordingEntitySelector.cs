using System.Collections;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity.Mimic
{
    public class MimicRecordingEntitySelector : AbstractDemandEnabledSelector, EntityMimicRecorder, EntitySelector
    {

        private readonly EntitySelector childEntitySelector;
        private readonly List<MimicReplayingEntitySelector> replayingEntitySelectorList;

        public MimicRecordingEntitySelector(EntitySelector childEntitySelector)
        {
            this.childEntitySelector = childEntitySelector;
            phaseLifecycleSupport.AddEventListener(childEntitySelector);
            replayingEntitySelectorList = new List<MimicReplayingEntitySelector>();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public void AddMimicReplayingEntitySelector(MimicReplayingEntitySelector replayingEntitySelector)
        {
            replayingEntitySelectorList.Add(replayingEntitySelector);
        }

        public EntityDescriptor GetEntityDescriptor()
        {
            return childEntitySelector.GetEntityDescriptor();
        }

        public IEnumerator<object> GetEnumerator()
        {
            return new RecordingEntityIterator(this, childEntitySelector.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new RecordingEntityIterator(this, childEntitySelector.GetEnumerator());
        }

        public override bool IsNeverEnding()
        {
            return childEntitySelector.IsNeverEnding();
        }

        public override bool IsCountable()
        {
            return childEntitySelector.IsCountable();
        }

        public long GetSize()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<object> ListIterator()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<object> EndingIterator()
        {
            throw new NotImplementedException();
        }

        protected class RecordingEntityIterator : SelectionIterator<Object>
        {
            private readonly IEnumerator<object> childEntityIterator;
            MimicRecordingEntitySelector mimicRecordingEntitySelector;

            public RecordingEntityIterator(MimicRecordingEntitySelector mimicRecordingEntitySelector, IEnumerator<object> childEntityIterator)
            {
                this.childEntityIterator = childEntityIterator;
                this.mimicRecordingEntitySelector = mimicRecordingEntitySelector;
            }

            public override object Current
            {
                get
                {
                    Object next = childEntityIterator.Current;
                    foreach (var replayingEntitySelector in mimicRecordingEntitySelector.replayingEntitySelectorList)
                    {
                        replayingEntitySelector.RecordedNext(next);
                    }
                    return next;

                }
            }

            public override bool MoveNext()
            {
                bool hasNext = childEntityIterator.MoveNext();
                foreach (var replayingEntitySelector in mimicRecordingEntitySelector.replayingEntitySelectorList)
                {
                    replayingEntitySelector.RecordedHasNext(hasNext);
                }
                return hasNext;
            }
        }
    }
}
