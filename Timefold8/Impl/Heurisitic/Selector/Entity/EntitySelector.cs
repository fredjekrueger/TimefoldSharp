using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator.ListIteratble;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity
{
    public interface EntitySelector : ListIterableSelector<object>
    {
        EntityDescriptor GetEntityDescriptor();
        IEnumerator<Object> EndingIterator();
    }
}
