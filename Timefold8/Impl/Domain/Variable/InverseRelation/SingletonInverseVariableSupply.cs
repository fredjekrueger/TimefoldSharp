namespace TimefoldSharp.Core.Impl.Domain.Variable.InverseRelation
{
    public interface SingletonInverseVariableSupply : Supply.Supply
    {
        Object GetInverseSingleton(Object planningValue);
    }
}
