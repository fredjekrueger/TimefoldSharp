namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple
{

    public interface ITuple
    {
        TupleState State { get; set; }

        object GetStore(int index);
        void SetStore(int index, object value);
        object RemoveStore(int index);
    }

    public class AbstractTuple : ITuple
    {
        private object[] arrayStore;
        private object store;
        private readonly bool storeIsArray;
        public TupleState State { get; set; } = TupleState.DEAD; // It's the node's job to mark a new tuple as CREATING.

        protected AbstractTuple(int storeSize)
        {
            if (storeSize >= 2)
            {
                arrayStore = new object[storeSize];
                storeIsArray = true;
            }
        }

        public object RemoveStore(int index)
        {
            object value;
            if (storeIsArray)
            {
                value = arrayStore[index];
                arrayStore[index] = null;
            }
            else
            {
                value = store;
                store = null;
            }
            return value;
        }

        public object GetStore(int index)
        {
            return storeIsArray ? arrayStore[index] : store;
        }

        public void SetStore(int index, object value)
        {
            if (storeIsArray)
            {
                arrayStore[index] = value;
            }
            else
            {
                store = value;
            }
        }
    }
}
