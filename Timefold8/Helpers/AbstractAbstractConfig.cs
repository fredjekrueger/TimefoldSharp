using TimefoldSharp.Core.Config.Solver;

namespace TimefoldSharp.Core.Helpers
{
    public class AbstractAbstractConfig : AbstractConfig<AbstractAbstractConfig>
    {
        public AbstractAbstractConfig CopyConfig()
        {
            throw new NotImplementedException();
        }

        public AbstractAbstractConfig Inherit(AbstractAbstractConfig inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }
    }
}
