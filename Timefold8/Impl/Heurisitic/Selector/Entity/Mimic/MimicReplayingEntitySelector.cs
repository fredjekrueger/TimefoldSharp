using System.Collections;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator;
using TimefoldSharp.Core.Impl.Phase.Scope;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity.Mimic
{
    public class MimicReplayingEntitySelector : AbstractDemandEnabledSelector, EntitySelector
    {

        private readonly EntityMimicRecorder entityMimicRecorder;

        public bool hasRecordingCreated;
        public bool hasRecording;
        public bool recordingCreated;
        public object recording;
        public bool recordingAlreadyReturned;

        public MimicReplayingEntitySelector(EntityMimicRecorder entityMimicRecorder)
        {
            this.entityMimicRecorder = entityMimicRecorder;
            // No PhaseLifecycleSupport because the MimicRecordingEntitySelector is hooked up elsewhere too
            entityMimicRecorder.AddMimicReplayingEntitySelector(this);
        }

        public override void PhaseEnded(AbstractPhaseScope phaseScope)
        {
            base.PhaseEnded(phaseScope);
            // Doing this in phaseEnded instead of stepEnded due to QueuedEntityPlacer compatibility
            hasRecordingCreated = false;
            hasRecording = false;
            recordingCreated = false;
            recording = null;
        }

        public override void PhaseStarted(AbstractPhaseScope phaseScope)
        {
            base.PhaseStarted(phaseScope);
            hasRecordingCreated = false;
            recordingCreated = false;
        }

        public void RecordedHasNext(bool hasNext)
        {
            hasRecordingCreated = true;
            hasRecording = hasNext;
            recordingCreated = false;
            recording = null;
            recordingAlreadyReturned = false;
        }

        public void RecordedNext(Object next)
        {
            hasRecordingCreated = true;
            hasRecording = true;
            recordingCreated = true;
            recording = next;
            recordingAlreadyReturned = false;
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public EntityDescriptor GetEntityDescriptor()
        {
            return entityMimicRecorder.GetEntityDescriptor();
        }

        public IEnumerator<object> GetEnumerator()
        {
            return new ReplayingEntityIterator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override bool IsNeverEnding()
        {
            return entityMimicRecorder.IsNeverEnding();
        }

        public override bool IsCountable()
        {
            return entityMimicRecorder.IsCountable();
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

        private class ReplayingEntityIterator : SelectionIterator<Object>
        {
            private MimicReplayingEntitySelector mimicReplayingEntitySelector;

            public ReplayingEntityIterator(MimicReplayingEntitySelector mimicReplayingEntitySelector)
            {
                this.mimicReplayingEntitySelector = mimicReplayingEntitySelector;
                mimicReplayingEntitySelector.recordingAlreadyReturned = false;
            }

            public override object Current
            {
                get
                {
                    if (!mimicReplayingEntitySelector.recordingCreated)
                    {
                        throw new Exception("Replay must occur after record." + " The recordingEntitySelector (" + mimicReplayingEntitySelector.entityMimicRecorder + ")'s next() has not been called yet. ");
                    }
                    if (mimicReplayingEntitySelector.recordingAlreadyReturned)
                    {
                        throw new Exception("The recordingAlreadyReturned (" + mimicReplayingEntitySelector.recordingAlreadyReturned + ") is impossible. Check if hasNext() returns true before this call.");
                    }
                    // Until the recorder records something, this iterator has no next.
                    mimicReplayingEntitySelector.recordingAlreadyReturned = true;
                    return mimicReplayingEntitySelector.recording;
                }
            }

            public override bool MoveNext()
            {
                if (!mimicReplayingEntitySelector.hasRecordingCreated)
                {
                    throw new Exception("Replay must occur after record."
                            + " The recordingEntitySelector (" + mimicReplayingEntitySelector.entityMimicRecorder
                            + ")'s hasNext() has not been called yet. ");
                }
                return mimicReplayingEntitySelector.hasRecording && !mimicReplayingEntitySelector.recordingAlreadyReturned;
            }
        }
    }
}
