using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Supply
{
    public abstract class AbstractVariableDescriptorBasedDemand : Demand
    {
        protected VariableDescriptor variableDescriptor;

        protected AbstractVariableDescriptorBasedDemand(VariableDescriptor variableDescriptor)
        {
            this.variableDescriptor = variableDescriptor;
        }


        public override bool Equals(object other)
        {
            if (this == other)
                return true;
            if (other == null || GetType() != other.GetType())
                return false;
            AbstractVariableDescriptorBasedDemand that = (AbstractVariableDescriptorBasedDemand) other;
            return variableDescriptor.Equals(that.variableDescriptor);
        }

        public override int GetHashCode()
        {
            int result = this.GetType().Name.GetHashCode();
            result = 31 * result + variableDescriptor.GetHashCode();
            return result;
        }

        public abstract Supply CreateExternalizedSupply(SupplyManager supplyManager);
    }
}
