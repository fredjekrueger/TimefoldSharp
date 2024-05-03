using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.InverseRelation;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Generic.Chained
{
    public class ChainedSwapMove : SwapMove
    {

        protected List<object> oldLeftTrailingEntityList;
        protected List<object> oldRightTrailingEntityList;

        public ChainedSwapMove(List<GenuineVariableDescriptor> variableDescriptorList,
        List<SingletonInverseVariableSupply> inverseVariableSupplyList, Object leftEntity, Object rightEntity)
            : base(variableDescriptorList, leftEntity, rightEntity)
        {
            oldLeftTrailingEntityList = new List<object>(inverseVariableSupplyList.Count());
            oldRightTrailingEntityList = new List<object>(inverseVariableSupplyList.Count());
            foreach (var inverseVariableSupply in inverseVariableSupplyList)
            {
                bool hasSupply = inverseVariableSupply != null;
                oldLeftTrailingEntityList.Add(hasSupply ? inverseVariableSupply.GetInverseSingleton(leftEntity) : null);
                oldRightTrailingEntityList.Add(hasSupply ? inverseVariableSupply.GetInverseSingleton(rightEntity) : null);
            }
        }

        public ChainedSwapMove(List<GenuineVariableDescriptor> genuineVariableDescriptors, Object leftEntity, Object rightEntity, List<Object> oldLeftTrailingEntityList, List<Object> oldRightTrailingEntityList)
            : base(genuineVariableDescriptors, leftEntity, rightEntity)
        {
            this.oldLeftTrailingEntityList = oldLeftTrailingEntityList;
            this.oldRightTrailingEntityList = oldRightTrailingEntityList;
        }
    }
}
