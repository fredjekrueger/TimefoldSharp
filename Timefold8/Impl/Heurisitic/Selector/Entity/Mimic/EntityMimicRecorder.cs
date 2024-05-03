using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity.Mimic
{
    public interface EntityMimicRecorder
    {
        void AddMimicReplayingEntitySelector(MimicReplayingEntitySelector replayingEntitySelector);
        EntityDescriptor GetEntityDescriptor();

        bool IsCountable();


        bool IsNeverEnding();
    }
}
