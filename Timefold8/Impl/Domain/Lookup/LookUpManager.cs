namespace TimefoldSharp.Core.Impl.Domain.Lookup
{
    public class LookUpManager
    {
        private readonly LookUpStrategyResolver lookUpStrategyResolver;
        private Dictionary<object, object> idToWorkingObjectMap;

        public LookUpManager(LookUpStrategyResolver lookUpStrategyResolver)
        {
            this.lookUpStrategyResolver = lookUpStrategyResolver;
            Reset();
        }

        public void Reset()
        {
            idToWorkingObjectMap = new Dictionary<object, object>();
        }

        public void AddWorkingObject(object workingObject)
        {
            LookUpStrategy lookUpStrategy = lookUpStrategyResolver.DetermineLookUpStrategy(workingObject);
            lookUpStrategy.AddWorkingObject(idToWorkingObjectMap, workingObject);
        }
    }
}
