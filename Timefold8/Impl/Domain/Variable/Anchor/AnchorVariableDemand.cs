using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Supply;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Anchor
{
    public class AnchorVariableDemand : AbstractVariableDescriptorBasedDemand
    {

        public AnchorVariableDemand(VariableDescriptor sourceVariableDescriptor) : base(sourceVariableDescriptor)
        {
        }

        public override Supply.Supply CreateExternalizedSupply(SupplyManager supplyManager)
        {
            throw new NotImplementedException();
        }
    }
}
