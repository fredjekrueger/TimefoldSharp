using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Config.ExhaustiveSearch
{
    internal class ExhaustiveSearchPhaseConfig : AbstractPhaseConfig
    {
        public override AbstractPhaseConfig CopyConfig()
        {
            return new ExhaustiveSearchPhaseConfig().Inherit(this);
        }

        public override void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }
    }
}
