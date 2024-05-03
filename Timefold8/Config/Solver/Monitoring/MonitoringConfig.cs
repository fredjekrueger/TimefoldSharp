using TimefoldSharp.Core.Config.Util;

namespace TimefoldSharp.Core.Config.Solver.Monitoring
{
    public class MonitoringConfig : AbstractConfig<MonitoringConfig>
    {
        public List<SolverMetric> SolverMetricList { get; set; } = null;

        public MonitoringConfig CopyConfig()
        {
            return new MonitoringConfig().Inherit(this);
        }

        public MonitoringConfig Inherit(MonitoringConfig inheritedConfig)
        {
            SolverMetricList = ConfigUtils.InheritMergeableListProperty(SolverMetricList, inheritedConfig.SolverMetricList);
            return this;
        }

        public MonitoringConfig WithSolverMetricList(List<SolverMetric> solverMetricList)
        {
            this.SolverMetricList = solverMetricList;
            return this;
        }

        public void VisitReferencedClasses(Action<Type> classVisitor)
        {
        }
    }
}
