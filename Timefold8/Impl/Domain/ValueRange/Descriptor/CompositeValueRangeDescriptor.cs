using System.Collections.Generic;
using TimefoldSharp.Core.API.Domain.ValueRange;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.ValueRange.Buildin.Composite;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Domain.ValueRange.Descriptor
{
    public class CompositeValueRangeDescriptor : AbstractValueRangeDescriptor, EntityIndependentValueRangeDescriptor

    {
        protected readonly List<ValueRangeDescriptor> childValueRangeDescriptorList;
        protected bool entityIndependent;

        public CompositeValueRangeDescriptor(GenuineVariableDescriptor variableDescriptor, bool addNullInValueRange, List<ValueRangeDescriptor> childValueRangeDescriptorList)
            : base(variableDescriptor, addNullInValueRange)
        {

            this.childValueRangeDescriptorList = childValueRangeDescriptorList;
            entityIndependent = true;
            foreach (var valueRangeDescriptor in childValueRangeDescriptorList)
            {
                if (!valueRangeDescriptor.IsCountable())
                {
                    throw new Exception("The valueRangeDescriptor (" + this
                            + ") has a childValueRangeDescriptor (" + valueRangeDescriptor
                            + ") with countable (" + valueRangeDescriptor.IsCountable() + ").");
                }
                if (!valueRangeDescriptor.IsEntityIndependent())
                {
                    entityIndependent = false;
                }
            }
        }

        public ValueRange<object> ExtractValueRange(ISolution solution)
        {
            List < CountableValueRange <object>> childValueRangeList = new List<CountableValueRange<object>>(childValueRangeDescriptorList.Count);
            foreach (var valueRangeDescriptor in childValueRangeDescriptorList)
            {
                EntityIndependentValueRangeDescriptor entityIndependentValueRangeDescriptor = (EntityIndependentValueRangeDescriptor)valueRangeDescriptor;
                childValueRangeList.Add((CountableValueRange<object>)entityIndependentValueRangeDescriptor.ExtractValueRange(solution));
            }
            return doNullInValueRangeWrapping(new CompositeCountableValueRange<object>(childValueRangeList));
        }

        public override bool IsCountable()
        {
            return true;
        }

        public override bool IsEntityIndependent()
        {
            return entityIndependent;
        }
    }
}
