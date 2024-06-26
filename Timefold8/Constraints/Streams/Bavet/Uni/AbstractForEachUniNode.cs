using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Uni
{
    public interface IAbstractForEachUniNode
    {
        Type GetForEachClass();
        void Update(object a);
        void Insert(object a);
    }

    public abstract class AbstractForEachUniNode<A> : AbstractNode, IAbstractForEachUniNode
    {
        private readonly int outputStoreSize;
        protected readonly Dictionary<object, UniTuple<A>> tupleMap = new Dictionary<object, UniTuple<A>>();
        private readonly StaticPropagationQueue propagationQueue;
        private readonly Type forEachClass;

        public AbstractForEachUniNode(Type forEachClass, TupleLifecycle nextNodesTupleLifecycle, int outputStoreSize)
        {
            this.forEachClass = forEachClass;
            this.outputStoreSize = outputStoreSize;
            this.propagationQueue = new StaticPropagationQueue(nextNodesTupleLifecycle);
        }

        public virtual void Retract(object a)
        {
            tupleMap.TryGetValue(a, out UniTuple<A> tuple);
            tupleMap.Remove(a);
            if (tuple == null)
            {
                throw new Exception("The fact (" + a + ") was never inserted, so it cannot retract.");
            }
            TupleState state = tuple.State;
            if (TupleStateHelper.IsDirty(state))
            {
                if (state == TupleState.DYING || state == TupleState.ABORTING)
                {
                    throw new Exception("The fact (" + a + ") was already retracted, so it cannot retract.");
                }
                propagationQueue.Retract(tuple, state == TupleState.CREATING ? TupleState.ABORTING : TupleState.DYING);
            }
            else
            {
                propagationQueue.Retract(tuple, TupleState.DYING);
            }
        }


        protected void InnerUpdate(object a, UniTuple<A> tuple)
        {
            TupleState state = tuple.State;
            if (TupleStateHelper.IsDirty(state))
            {
                if (state == TupleState.DYING || state == TupleState.ABORTING)
                {
                    throw new Exception("The fact (" + a + ") was retracted, so it cannot update.");
                }
                // CREATING or UPDATING is ignored; it's already in the queue.
            }
            else
            {
                propagationQueue.Update(tuple);
            }
        }

        public abstract void Update(object a);

        public virtual void Insert(object a)
        {
            UniTuple<A> tuple = new UniTuple<A>((A)a, outputStoreSize);
            tupleMap.Add(a, tuple);
            propagationQueue.Insert(tuple);
        }

        public Type GetForEachClass()
        {
            return forEachClass;
        }

        public override Propagator GetPropagator()
        {
            return propagationQueue;
        }
    }
}
