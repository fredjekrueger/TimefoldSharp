using System.Collections;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity.Decorator
{
    public sealed class SelectedCountLimitEntitySelector : AbstractDemandEnabledSelector, EntitySelector
    {

        public SelectedCountLimitEntitySelector(EntitySelector childEntitySelector, bool randomSelection, long? selectedCountLimit)
        {

        }

        public IEnumerator<object> EndingIterator()
        {
            throw new NotImplementedException();
        }

        public EntityDescriptor GetEntityDescriptor()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<object> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public long GetSize()
        {
            throw new NotImplementedException();
        }

        public override bool IsCountable()
        {
            throw new NotImplementedException();
        }

        public override bool IsNeverEnding()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<object> ListIterator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
