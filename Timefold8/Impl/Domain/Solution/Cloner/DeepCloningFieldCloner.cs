using System.Reflection;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;

namespace TimefoldSharp.Core.Impl.Domain.Solution.Cloner
{
    public sealed class DeepCloningFieldCloner
    {
        private readonly PropertyInfo field;

        private Metadata valueDeepCloneDecision = null; //Atomic REFERENCE !
        private int fieldDeepCloneDecision = -1;

        internal object Clone<C>(SolutionDescriptor solutionDescriptor, C original, C clone)
        {
            object originalValue = FieldCloningUtils.GetFieldValue<object>(original, field);
            if (DeepClone(solutionDescriptor, original.GetType(), originalValue))
            { // Defer filling in the field.
                return originalValue;
            }
            else
            { // Shallow copy.
                FieldCloningUtils.SetObjectFieldValue(clone, field, originalValue);
                return null;
            }
        }

        object lockerValueDeepCloneDecision = new object();
        object lockerFieldDeepCloneDecision = new object();
        private bool DeepClone(SolutionDescriptor solutionDescriptor, Type fieldTypeClass, object originalValue)
        {
            if (originalValue == null)
            {
                return false;
            }
            /*
             * This caching mechanism takes advantage of the fact that, for a particular field on a particular class,
             * the types of values contained are unlikely to change and therefore it is safe to cache the calculation.
             * In the unlikely event of a cache miss, we recompute.
             */

            bool isValueDeepCloned = false;
            lock (lockerValueDeepCloneDecision)
            {
                Type originalClass = originalValue.GetType();
                if (valueDeepCloneDecision == null || valueDeepCloneDecision.Clz != originalClass)
                {
                    valueDeepCloneDecision = new Metadata(originalClass, DeepCloningUtils.IsClassDeepCloned(solutionDescriptor, originalClass));
                }
                isValueDeepCloned = valueDeepCloneDecision.Decision;
            }

            if (isValueDeepCloned)
            { // The value has to be deep-cloned. Does not matter what the field says.
                return true;
            }
            /*
             * The decision to clone a field is constant once it has been made.
             * The fieldTypeClass is guaranteed to not change for the particular field.
             */
            lock (lockerFieldDeepCloneDecision)
            {
                if (fieldDeepCloneDecision < 0)
                {
                    fieldDeepCloneDecision = (DeepCloningUtils.IsFieldDeepCloned(solutionDescriptor, field, fieldTypeClass) ? 1 : 0);
                }
                return fieldDeepCloneDecision == 1;
            }
        }

        internal PropertyInfo GetField()
        {
            return field;
        }

        public DeepCloningFieldCloner(PropertyInfo field)
        {
            this.field = field;
        }

        private class Metadata
        {
            public Type Clz { get; set; }
            public bool Decision { get; set; }

            public Metadata(Type clz, bool decision)
            {
                this.Clz = clz;
                this.Decision = decision;
            }
        }
    }
}
