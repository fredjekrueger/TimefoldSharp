namespace TimefoldSharp.Core.Impl.Domain.Variable.Supply
{
    public interface Demand 
    {
        Supply CreateExternalizedSupply(SupplyManager supplyManager);
    }
}
