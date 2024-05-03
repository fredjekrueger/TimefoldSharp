using TimefoldSharp.Core.API.Domain.ValueRange;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;
using TimefoldSharp.Core.Impl.Domain.ValueRange.Buildin.Composite;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Domain.ValueRange.Descriptor
{
    public abstract class AbstractValueRangeDescriptor : ValueRangeDescriptor
    {

        protected readonly GenuineVariableDescriptor variableDescriptor;
        protected readonly bool addNullInValueRange;

        public AbstractValueRangeDescriptor(GenuineVariableDescriptor variableDescriptor, bool addNullInValueRange)
        {
            this.variableDescriptor = variableDescriptor;
            this.addNullInValueRange = addNullInValueRange;
        }

        protected ValueRange<T> DoNullInValueRangeWrapping<T>(ValueRange<T> valueRange)
        {
            if (addNullInValueRange)
            {
                valueRange = new NullableCountableValueRange<T>((CountableValueRange<T>)valueRange);
            }
            return valueRange;
        }

        protected ValueRange<T> doNullInValueRangeWrapping<T>(ValueRange<T> valueRange)
        {
            if (addNullInValueRange)
            {
                valueRange = new NullableCountableValueRange<T>((CountableValueRange<T>)valueRange);
            }
            return valueRange;
        }

        public long ExtractValueRangeSize(ISolution solution, object entity)
        {
            throw new NotImplementedException();
        }

        public GenuineVariableDescriptor GetVariableDescriptor()
        {
            return variableDescriptor;
        }

        public abstract bool IsCountable();

        public abstract bool IsEntityIndependent();

        public bool MightContainEntity()
        {
            SolutionDescriptor solutionDescriptor = variableDescriptor.EntityDescriptor.GetSolutionDescriptor();
            Type variablePropertyType = variableDescriptor.GetVariablePropertyType();
            foreach (var entityClass in solutionDescriptor.GetEntityClassSet())
            {
                if (variablePropertyType.IsAssignableFrom(entityClass))
                {
                    return true;
                }
            }
            return false;
        }

        public ValueRange<object> ExtractValueRange(ISolution solution, object entity)
        {
            throw new NotImplementedException();
        }
    }
}
