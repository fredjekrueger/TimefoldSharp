using TimefoldSharp.Core.Config.LocalSearch.Decider.Acceptor;
using TimefoldSharp.Core.Config.LocalSearch.Decider.Forager;
using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Config.LocalSearch
{
    public class LocalSearchPhaseConfig : AbstractPhaseConfig
    {

        private AbstractMoveSelectorConfig moveSelectorConfig = null;

        private LocalSearchForagerConfig foragerConfig = null;
        protected LocalSearchType? localSearchType = null;
        private LocalSearchAcceptorConfig acceptorConfig = null;

        public override AbstractPhaseConfig CopyConfig()
        {
            throw new NotImplementedException();
        }

        public override void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }

        public LocalSearchForagerConfig GetForagerConfig()
        {
            return foragerConfig;
        }

        public LocalSearchType? GetLocalSearchType()
        {
            return localSearchType;
        }

        public LocalSearchAcceptorConfig GetAcceptorConfig()
        {
            return acceptorConfig;
        }


        public AbstractMoveSelectorConfig GetMoveSelectorConfig()
        {
            return moveSelectorConfig;
        }
    }
}
