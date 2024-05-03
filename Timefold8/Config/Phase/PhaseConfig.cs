using System.Xml.Serialization;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Config.Solver.Termination;

namespace TimefoldSharp.Core.Config.Phase
{
    public abstract class PhaseConfig<Config> : AbstractConfig<Config> where Config : PhaseConfig<Config>
    {
        [XmlElement("termination")]
        private TerminationConfig terminationConfig = null;

        public TerminationConfig GetTerminationConfig()
        {
            return terminationConfig;
        }

        public void SetTerminationConfig(TerminationConfig terminationConfig)
        {
            this.terminationConfig = terminationConfig;
        }

        public Config WithTerminationConfig(TerminationConfig terminationConfig)
        {
            this.SetTerminationConfig(terminationConfig);
            return (Config)this;
        }

        public virtual Config Inherit(Config inheritedConfig)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return GetType().Name;
        }

        public virtual Config CopyConfig()
        {
            throw new NotImplementedException();
        }

        public virtual void VisitReferencedClasses(Action<Type> classVisitor)
        {
            throw new NotImplementedException();
        }
    }
}