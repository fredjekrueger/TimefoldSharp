using System.Collections;
using System.Text;

namespace TimefoldSharp.Core.Impl.Util
{
    public sealed class ElementAwareList<T> : IEnumerable<T>
    {
        private ElementAwareListEntry<T> first = null;
        private ElementAwareListEntry<T> last = null;
        private int size = 0;

        public ElementAwareList()
        {

        }



        public int Size()
        {
            return size;
        }


        public void Remove(ElementAwareListEntry<T> entry)
        {
            if (first == entry)
            {
                first = entry.Next;
            }
            else
            {
                entry.Previous.Next = entry.Next;
            }
            if (last == entry)
            {
                last = entry.Previous;
            }
            else
            {
                entry.Next.Previous = entry.Previous;
            }
            entry.Previous = null;
            entry.Next = null;
            size--;
        }

        public ElementAwareListEntry<T> Add(T tuple)
        {
            ElementAwareListEntry<T> entry = new ElementAwareListEntry<T>(this, tuple, last);
            if (first == null)
            {
                first = entry;
            }
            else
            {
                last.Next = entry;
            }
            last = entry;
            size++;
            return entry;
        }


        public override string ToString()
        {
            switch (size)
            {
                case 0:
                    {
                        return "[]";
                    }
                case 1:
                    {
                        return "[" + first.GetElement() + "]";
                    }
                default:
                    {
                        StringBuilder builder = new StringBuilder("[");
                        foreach (var entry in this)
                        {
                            builder.Append(entry).Append(", ");
                        }
                        //builder.Replace(builder.Length - 2, builder.Length, "");
                        return builder.Append("]").ToString();
                    }
            }
        }

        public void ForEach(Action<T> tupleConsumer)
        {
            ElementAwareListEntry<T> entry = first;
            while (entry != null)
            {
                // Extract next before processing it, in case the entry is removed and entry.next becomes null
                ElementAwareListEntry<T> next = entry.Next;
                tupleConsumer.Invoke((T)entry.GetElement());
                entry = next;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new ElementAwareListIterator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new ElementAwareListIterator(this);
        }

        private class ElementAwareListIterator : IEnumerator<T>
        {
            private ElementAwareListEntry<T> nextEntry;
            private ElementAwareList<T> parent;

            public ElementAwareListIterator(ElementAwareList<T> parent)
            {
                this.parent = parent;
                nextEntry = parent.first;
            }

            public object Current
            {
                get
                {
                    object element = nextEntry.GetElement();
                    nextEntry = nextEntry.Next;
                    return element;
                }
            }

            object IEnumerator.Current => Current;

            T IEnumerator<T>.Current => throw new NotImplementedException();

            public bool MoveNext()
            {
                if (parent.size == 0)
                    return false;

                return nextEntry != null;
            }

            public void Reset()
            {
                nextEntry = parent.first;
            }

            public void Dispose()
            {
                // Dispose if necessary
            }
        }

    }
}
