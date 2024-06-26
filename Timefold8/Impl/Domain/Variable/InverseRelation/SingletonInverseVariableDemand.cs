using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Supply;

namespace TimefoldSharp.Core.Impl.Domain.Variable.InverseRelation
{
    public class SingletonInverseVariableDemand : AbstractVariableDescriptorBasedDemand
    {
        public SingletonInverseVariableDemand(VariableDescriptor sourceVariableDescriptor) : base(sourceVariableDescriptor)
        {
        }

        public override SingletonInverseVariableSupply CreateExternalizedSupply(SupplyManager supplyManager)
        {
            return new ExternalizedSingletonInverseVariableSupply(variableDescriptor);
        }
    }
}
