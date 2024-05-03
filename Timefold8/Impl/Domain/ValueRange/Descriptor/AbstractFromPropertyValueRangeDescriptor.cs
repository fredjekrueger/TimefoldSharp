using TimefoldSharp.Core.API.Domain.ValueRange;
using TimefoldSharp.Core.Config.Util;
using TimefoldSharp.Core.Impl.Domain.Common;
using TimefoldSharp.Core.Impl.Domain.Common.Accessor;
using TimefoldSharp.Core.Impl.Domain.Entity.Descriptor;
using TimefoldSharp.Core.Impl.Domain.ValueRange.Buildin.Collection;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Domain.ValueRange.Descriptor
{
    public abstract class AbstractFromPropertyValueRangeDescriptor : AbstractValueRangeDescriptor
    {
        protected readonly MemberAccessor memberAccessor;
        protected bool collectionWrapping;
        protected bool arrayWrapping;
        protected bool countable;

        public AbstractFromPropertyValueRangeDescriptor(GenuineVariableDescriptor variableDescriptor, bool addNullInValueRange, MemberAccessor memberAccessor)
            : base(variableDescriptor, addNullInValueRange)
        {

            this.memberAccessor = memberAccessor;
            ValueRangeProviderAttribute valueRangeProviderAnnotation = memberAccessor.GetAnnotation<ValueRangeProviderAttribute>(typeof(ValueRangeProviderAttribute));
            if (valueRangeProviderAnnotation == null)
            {
                throw new Exception("The member (" + memberAccessor
                        + ") must have a valueRangeProviderAnnotation (" + valueRangeProviderAnnotation + ").");
            }
            ProcessValueRangeProviderAnnotation(valueRangeProviderAnnotation);
            if (addNullInValueRange && !countable)
            {
                throw new Exception("The valueRangeDescriptor (.");
            }
        }

        private void ProcessValueRangeProviderAnnotation(ValueRangeProviderAttribute valueRangeProviderAnnotation)
        {
            EntityDescriptor entityDescriptor = variableDescriptor.EntityDescriptor;
            Type type = memberAccessor.GetClass();
            collectionWrapping = type.GetGenericTypeDefinition() == typeof(List<>);
            arrayWrapping = type.IsArray;
            if (!collectionWrapping && !arrayWrapping && !typeof(ValueRange<>).IsAssignableFrom(type))
            {
                throw new Exception("The entityClass.");
            }
            if (collectionWrapping)
            {
                Type collectionElementClass = ConfigUtils.ExtractCollectionGenericTypeParameterStrictly(
                       "solutionClass or entityClass", memberAccessor.GetDeclaringClass(),
                       memberAccessor.GetClass(), memberAccessor.GetGenericType(),
                       typeof(ValueRangeProviderAttribute), memberAccessor.GetName());
                if (!variableDescriptor.AcceptsValueType(collectionElementClass))
                {
                    throw new Exception("The entityCla");
                }

            }
            else if (arrayWrapping)
            {
                /*   Class <?> arrayElementClass = type.getComponentType();
                   if (!variableDescriptor.acceptsValueType(arrayElementClass))
                   {
                       throw new Exception("The entityClass (" + entityDescriptor.getEntityClass()
                               + ") has a @" + PlanningVariable.class.getSimpleName()
                               + " annotated property (" + variableDescriptor.getVariableName()
                               + ") that refers to a @" + ValueRangeProvider.class.getSimpleName()
                               + " annotated member (" + memberAccessor
                               + ") that returns an array with elements of type (" + arrayElementClass
                               + ") which cannot be assigned to the @" + PlanningVariable.class.getSimpleName()
                               + "'s type (" + variableDescriptor.getVariablePropertyType() + ").");
                           }
                       */
            }
            countable = collectionWrapping || arrayWrapping || typeof(CountableValueRange<>).IsAssignableFrom(type);
        }

        public override bool IsCountable()
        {
            {
                return countable;
            }
        }

        protected ValueRange<object> ReadValueRange(Object bean)
        {
            object valueRangeObject = memberAccessor.ExecuteGetter(bean);
            if (valueRangeObject == null)
            {
                throw new Exception("The @ annotated member (" + memberAccessor
                    + ") called on bean (" + bean
                    + ") must not return a null valueRangeObject (" + valueRangeObject + ").");
            }
            ValueRange<object> valueRange;
            if (collectionWrapping || arrayWrapping)
            {
                IEnumerable<object> list = collectionWrapping ? TransformCollectionToList((IEnumerable<object>)valueRangeObject)
                        : ReflectionHelper.TransformArrayToList(valueRangeObject);
                // Don't check the entire list for performance reasons, but do check common pitfalls
                if (list.Any() && (list.First() == null || list.Last() == null))
                {
                    throw new Exception("The @  annotated member (" + memberAccessor
                            + ") called on bean (" + bean
                            + ") must not return a  array"
                            + "(" + list + ") with an element that is null.\n"
                            + "Maybe remove that null element from the dataset.\n"
                            + "Maybe use @ (nullable = true) instead.");
                }
                valueRange = new ListValueRange<object>(list);
            }
            else
            {
                valueRange = (ValueRange<Object>)valueRangeObject;
            }
            valueRange = DoNullInValueRangeWrapping(valueRange);
            if (valueRange.IsEmpty())
            {
                throw new Exception("The @ annotated member (" + memberAccessor
                        + ") called on bean (" + bean
                        + ") must not return an empty valueRange (" + valueRangeObject + ").\n"
                        + "Maybe apply overconstrained planning as described in the documentation.");
            }
            return valueRange;
        }

        private IEnumerable<T> TransformCollectionToList<T>(IEnumerable<T> collection)
        {
            if (collection is List<T> list)
            {
                if (collection is LinkedList<T> linkedList)
                {
                    // ValueRange.createRandomIterator(Random) and ValueRange.get(int) wouldn't be efficient.
                    return new LinkedList<T>(linkedList);
                }
                else
                {
                    return list;
                }
            }
            else
            {
                // TODO If only ValueRange.createOriginalIterator() is used, cloning a Set to a List is a waste of time.
                return new List<T>(collection);
            }
        }

    }
}
