namespace TimefoldSharp.Core.Impl.Util
{
    public class ElementAwareListEntry<T>
    {
        private ElementAwareList<T> list;
        private T element;
        public ElementAwareListEntry<T> Previous;
        public ElementAwareListEntry<T> Next;

        public ElementAwareListEntry(ElementAwareList<T> list, T element, ElementAwareListEntry<T> previous)
        {
            this.list = list;
            this.element = element;
            this.Previous = previous;
            this.Next = null;
        }

        public void Remove()
        {
            if (list == null)
            {
                throw new Exception("The element (" + element + ") was already removed.");
            }
            list.Remove(this);
            list = null;
        }


        public ElementAwareList<T> GetList()
        {
            return list;
        }

        public object GetElement()
        {
            return element;
        }

        public override string ToString()
        {
            return element.ToString();
        }
    }
}
