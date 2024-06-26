using TimefoldSharp.Core.Helpers;

namespace TimefoldSharp.Core.Impl.Domain.Variable.Supply
{
    public interface SupplyManager
    {
        Supply Demand(Demand demand);
    }
}
