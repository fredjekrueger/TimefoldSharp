using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Mimic
{
    public interface ValueMimicRecorder
    {
        void AddMimicReplayingValueSelector(MimicReplayingValueSelector replayingValueSelector);
        GenuineVariableDescriptor GetVariableDescriptor();
    }
}
