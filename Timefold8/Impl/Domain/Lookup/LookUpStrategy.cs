namespace TimefoldSharp.Core.Impl.Domain.Lookup
{
    public interface LookUpStrategy
    {
        void AddWorkingObject(Dictionary<object, object> idToWorkingObjectMap, Object workingObject);
    }
}
