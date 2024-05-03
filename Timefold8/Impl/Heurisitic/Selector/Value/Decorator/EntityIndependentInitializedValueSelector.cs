using System.Collections;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Decorator
{
    public sealed class EntityIndependentInitializedValueSelector : InitializedValueSelector, EntityIndependentValueSelector
    {

        public EntityIndependentInitializedValueSelector(EntityIndependentValueSelector childValueSelector)
            : base(childValueSelector)
        {
        }

        public IEnumerator<object> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public long GetSize()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
