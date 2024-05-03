using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.InverseRelation;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Generic
{
    public class ChainedChangeMove : ChangeMove
    {
        protected readonly Object oldTrailingEntity;
        protected readonly Object newTrailingEntity;

        public ChainedChangeMove(GenuineVariableDescriptor variableDescriptor, Object entity, Object toPlanningValue,
           SingletonInverseVariableSupply inverseVariableSupply) : base(variableDescriptor, entity, toPlanningValue)
        {
            oldTrailingEntity = inverseVariableSupply.GetInverseSingleton(entity);
            newTrailingEntity = toPlanningValue == null ? null : inverseVariableSupply.GetInverseSingleton(toPlanningValue);
        }
    }
}
