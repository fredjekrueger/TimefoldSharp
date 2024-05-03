using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Config.Phase.Custom
{
    public class CustomPhaseConfig : AbstractPhaseConfig
    {
        public override AbstractPhaseConfig CopyConfig()
        {
            return new CustomPhaseConfig().Inherit(this);
        }

        public override void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }
    }
}
