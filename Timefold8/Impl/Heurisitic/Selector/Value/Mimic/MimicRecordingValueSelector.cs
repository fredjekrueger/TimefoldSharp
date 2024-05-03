using System.Collections;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Mimic
{
    public class MimicRecordingValueSelector : AbstractDemandEnabledSelector, ValueMimicRecorder, EntityIndependentValueSelector
    {

        protected readonly EntityIndependentValueSelector childValueSelector;
        protected readonly List<MimicReplayingValueSelector> replayingValueSelectorList;

        public MimicRecordingValueSelector(EntityIndependentValueSelector childValueSelector)
        {
            this.childValueSelector = childValueSelector;
            phaseLifecycleSupport.AddEventListener(childValueSelector);
            replayingValueSelectorList = new List<MimicReplayingValueSelector>();
        }

        public void AddMimicReplayingValueSelector(MimicReplayingValueSelector replayingValueSelector)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<object> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public GenuineVariableDescriptor GetVariableDescriptor()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override bool IsCountable()
        {
            return childValueSelector.IsCountable();
        }

        public override bool IsNeverEnding()
        {
            return childValueSelector.IsNeverEnding();
        }

        public IEnumerator<object> Iterator(object entity)
        {
            throw new NotImplementedException();
        }

        public long GetSize(object entity)
        {
            throw new NotImplementedException();
        }

        public long GetSize()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
