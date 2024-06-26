using Serilog.Core;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Iterator;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Decorator
{
    public class InitializedValueSelector : AbstractDemandEnabledSelector, ValueSelector
    {
        readonly GenuineVariableDescriptor variableDescriptor;
        readonly ValueSelector childValueSelector;
        readonly bool bailOutEnabled;


        public static ValueSelector Create(ValueSelector valueSelector)
        {
            if (valueSelector is EntityIndependentValueSelector)
            {
                return new EntityIndependentInitializedValueSelector((EntityIndependentValueSelector)valueSelector);
            }
            else
            {
                return new InitializedValueSelector(valueSelector);
            }
        }

        public override bool IsCountable()
        {
            return childValueSelector.IsCountable();
        }

        public override bool IsNeverEnding()
        {
            return childValueSelector.IsNeverEnding();
        }

        public GenuineVariableDescriptor GetVariableDescriptor()
        {
            return childValueSelector.GetVariableDescriptor();
        }

        public IEnumerator<object> Iterator(object entity)
        {
            return new JustInTimeInitializedValueIterator(entity, childValueSelector.Iterator(entity), this);
        }

        public long GetSize(object entity)
        {
            return childValueSelector.GetSize(entity);
        }

        protected InitializedValueSelector(ValueSelector childValueSelector)
        {
            this.variableDescriptor = childValueSelector.GetVariableDescriptor();
            this.childValueSelector = childValueSelector;
            bailOutEnabled = childValueSelector.IsNeverEnding();
            phaseLifecycleSupport.AddEventListener(childValueSelector);
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public class JustInTimeInitializedValueIterator : UpcomingSelectionIterator<object>
        {
            private IEnumerator<object> childValueIterator;
            private long bailOutSize;
            private InitializedValueSelector parent;

            public JustInTimeInitializedValueIterator(Object entity, IEnumerator<object> childValueIterator, InitializedValueSelector parent)
            {

                this.childValueIterator = childValueIterator;
                this.parent = parent;
                this.bailOutSize = DetermineBailOutSize(entity);
            }

            protected long DetermineBailOutSize(object entity)
            {
                if (!parent.bailOutEnabled)
                {
                    return -1L;
                }
                return parent.childValueSelector.GetSize(entity) * 10L;
            }

            public JustInTimeInitializedValueIterator(IEnumerator<object> childValueIterator, long bailOutSize, InitializedValueSelector parent)
            {
                this.childValueIterator = childValueIterator;
                this.bailOutSize = bailOutSize;
                this.parent = parent;
            }

            protected override object CreateUpcomingSelection()
            {
                object next;
                long attemptsBeforeBailOut = bailOutSize;
                do
                {
                    if (!childValueIterator.MoveNext())
                    {
                        return NoUpcomingSelection();
                    }
                    if (parent.bailOutEnabled)
                    {
                        // if childValueIterator is neverEnding and nothing is accepted, bail out of the infinite loop
                        if (attemptsBeforeBailOut <= 0L)
                        {
                            return NoUpcomingSelection();
                        }
                        attemptsBeforeBailOut--;
                    }
                    next = childValueIterator.Current;
                } while (!Accept(next));
                return next;
            }

            public override bool Equals(object other)
            {
                if (this == other)
                    return true;
                if (other == null || GetType() != other.GetType())
                    return false;
                InitializedValueSelector that = (InitializedValueSelector) other;
                return parent.variableDescriptor.Equals(that.variableDescriptor) && parent.childValueSelector.Equals(that.childValueSelector);
            }

            protected bool Accept(object value)
            {
                return value == null
                        || !parent.variableDescriptor.EntityDescriptor.EntityClass.IsAssignableFrom(value.GetType())
                        || parent.variableDescriptor.IsInitialized(value);
            }

            public override int GetHashCode()
            {
                return Utils.CombineHashCodes(parent.variableDescriptor, parent.childValueSelector);
            }

            public override string ToString()
            {
                return "Initialized(" + parent.childValueSelector + ")";
            }
        }
    }
}
