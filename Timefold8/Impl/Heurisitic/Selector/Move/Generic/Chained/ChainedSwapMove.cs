using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.InverseRelation;
using TimefoldSharp.Core.Impl.Heurisitic.Move;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Generic.Chained
{
    public class ChainedSwapMove : SwapMove
    {

        protected List<object> oldLeftTrailingEntityList;
        protected List<object> oldRightTrailingEntityList;

        public ChainedSwapMove(List<GenuineVariableDescriptor> variableDescriptorList,
        List<SingletonInverseVariableSupply> inverseVariableSupplyList, object leftEntity, object rightEntity)
            : base(variableDescriptorList, leftEntity, rightEntity)
        {
            oldLeftTrailingEntityList = new List<object>(inverseVariableSupplyList.Count);
            oldRightTrailingEntityList = new List<object>(inverseVariableSupplyList.Count);
            foreach (var inverseVariableSupply in inverseVariableSupplyList)
            {
                bool hasSupply = inverseVariableSupply != null;
                oldLeftTrailingEntityList.Add(hasSupply ? inverseVariableSupply.GetInverseSingleton(leftEntity) : null);
                oldRightTrailingEntityList.Add(hasSupply ? inverseVariableSupply.GetInverseSingleton(rightEntity) : null);
            }
        }

        public ChainedSwapMove(List<GenuineVariableDescriptor> genuineVariableDescriptors, object leftEntity, object rightEntity, List<object> oldLeftTrailingEntityList, List<object> oldRightTrailingEntityList)
            : base(genuineVariableDescriptors, leftEntity, rightEntity)
        {
            this.oldLeftTrailingEntityList = oldLeftTrailingEntityList;
            this.oldRightTrailingEntityList = oldRightTrailingEntityList;
        }

        protected override void DoMoveOnGenuineVariables(ScoreDirector scoreDirector)
        {
            for (int i = 0; i < variableDescriptorList.Count; i++)
            {
                GenuineVariableDescriptor variableDescriptor = variableDescriptorList[i];
                Object oldLeftValue = variableDescriptor.GetValue(leftEntity);
                Object oldRightValue = variableDescriptor.GetValue(rightEntity);
                if (!oldLeftValue.Equals(oldRightValue))
                {
                    InnerScoreDirector innerScoreDirector = (InnerScoreDirector) scoreDirector;
                    if (!variableDescriptor.IsChained())
                    {
                        innerScoreDirector.ChangeVariableFacade(variableDescriptor, leftEntity, oldRightValue);
                        innerScoreDirector.ChangeVariableFacade(variableDescriptor, rightEntity, oldLeftValue);
                    }
                    else
                    {
                        Object oldLeftTrailingEntity = oldLeftTrailingEntityList[i];
                        Object oldRightTrailingEntity = oldRightTrailingEntityList[i];
                        if (oldRightValue == leftEntity)
                        {
                            // Change the right entity
                            innerScoreDirector.ChangeVariableFacade(variableDescriptor, rightEntity, oldLeftValue);
                            // Change the left entity
                            innerScoreDirector.ChangeVariableFacade(variableDescriptor, leftEntity, rightEntity);
                            // Reroute the new left chain
                            if (oldRightTrailingEntity != null)
                            {
                                innerScoreDirector.ChangeVariableFacade(variableDescriptor, oldRightTrailingEntity, leftEntity);
                            }
                        }
                        else if (oldLeftValue == rightEntity)
                        {
                            // Change the right entity
                            innerScoreDirector.ChangeVariableFacade(variableDescriptor, leftEntity, oldRightValue);
                            // Change the left entity
                            innerScoreDirector.ChangeVariableFacade(variableDescriptor, rightEntity, leftEntity);
                            // Reroute the new left chain
                            if (oldLeftTrailingEntity != null)
                            {
                                innerScoreDirector.ChangeVariableFacade(variableDescriptor, oldLeftTrailingEntity, rightEntity);
                            }
                        }
                        else
                        {
                            // Change the left entity
                            innerScoreDirector.ChangeVariableFacade(variableDescriptor, leftEntity, oldRightValue);
                            // Change the right entity
                            innerScoreDirector.ChangeVariableFacade(variableDescriptor, rightEntity, oldLeftValue);
                            // Reroute the new left chain
                            if (oldRightTrailingEntity != null)
                            {
                                innerScoreDirector.ChangeVariableFacade(variableDescriptor, oldRightTrailingEntity, leftEntity);
                            }
                            // Reroute the new right chain
                            if (oldLeftTrailingEntity != null)
                            {
                                innerScoreDirector.ChangeVariableFacade(variableDescriptor, oldLeftTrailingEntity, rightEntity);
                            }
                        }
                    }
                }
            }
        }

        protected override AbstractMove CreateUndoMove(ScoreDirector scoreDirector)
        {
            return new ChainedSwapMove(variableDescriptorList, rightEntity, leftEntity, oldLeftTrailingEntityList, oldRightTrailingEntityList);
        }
    }
}
