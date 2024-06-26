using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Domain.Variable;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Listener
{
    public interface SourcedVariableListener : AbstractVariableListener<object>, Core.Impl.Domain.Variable.Supply.Supply
    {
        VariableDescriptor GetSourceVariableDescriptor();
    }
}
