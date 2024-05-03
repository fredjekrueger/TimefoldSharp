using System.Collections;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Mimic
{
    public class MimicReplayingValueSelector : AbstractDemandEnabledSelector, EntityIndependentValueSelector
    {

        protected readonly ValueMimicRecorder valueMimicRecorder;

        protected bool hasRecordingCreated;
        protected bool hasRecording;
        protected bool recordingCreated;
        protected object recording;
        protected bool recordingAlreadyReturned;

        public MimicReplayingValueSelector(ValueMimicRecorder valueMimicRecorder)
        {
            this.valueMimicRecorder = valueMimicRecorder;
            // No PhaseLifecycleSupport because the MimicRecordingValueSelector is hooked up elsewhere too
            valueMimicRecorder.AddMimicReplayingValueSelector(this);
            // Precondition for iterator(Object)'s current implementation
            if (!valueMimicRecorder.GetVariableDescriptor().IsValueRangeEntityIndependent())
            {
                throw new Exception(
                        "The current implementation support only an entityIndependent variable ("
                                + valueMimicRecorder.GetVariableDescriptor() + ").");
            }
        }


        public IEnumerator<object> GetEnumerator()
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

        public GenuineVariableDescriptor GetVariableDescriptor()
        {
            throw new NotImplementedException();
        }

        public override bool IsCountable()
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override bool IsNeverEnding()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<object> Iterator(object entity)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
