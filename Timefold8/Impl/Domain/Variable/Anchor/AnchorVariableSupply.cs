using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.Impl.Domain.Variable.Supply;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Anchor
{
    public interface AnchorVariableSupply : Core.Impl.Domain.Variable.Supply.Supply
    {
        object GetAnchor(object entity);
    }
}
