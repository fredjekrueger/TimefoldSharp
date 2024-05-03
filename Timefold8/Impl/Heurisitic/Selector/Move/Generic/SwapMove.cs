using TimefoldSharp.Core.Impl.Domain.ValueRange.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Move;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Generic
{
    public class SwapMove : AbstractMove
    {
        private object leftEntity;
        private object rightEntity;
        protected List<GenuineVariableDescriptor> variableDescriptorList;

        public SwapMove(List<GenuineVariableDescriptor> variableDescriptorList, Object leftEntity, Object rightEntity)
        {
            this.variableDescriptorList = variableDescriptorList;
            this.leftEntity = leftEntity;
            this.rightEntity = rightEntity;
        }

        public override bool IsMoveDoable(ScoreDirector scoreDirector)
        {
            bool movable = false;
            foreach (var variableDescriptor in variableDescriptorList)
            {
                Object leftValue = variableDescriptor.GetValue(leftEntity);
                Object rightValue = variableDescriptor.GetValue(rightEntity);
                if (leftValue.Equals(rightValue))
                {
                    movable = true;
                    if (!variableDescriptor.IsValueRangeEntityIndependent())
                    {
                        ValueRangeDescriptor valueRangeDescriptor = variableDescriptor.ValueRangeDescriptor;
                        var workingSolution = scoreDirector.GetWorkingSolution();
                        var rightValueRange = valueRangeDescriptor.ExtractValueRange(workingSolution, rightEntity);
                        if (!rightValueRange.Contains(leftValue))
                        {
                            return false;
                        }
                        var leftValueRange = valueRangeDescriptor.ExtractValueRange(workingSolution, leftEntity);
                        if (!leftValueRange.Contains(rightValue))
                        {
                            return false;
                        }
                    }
                }
            }
            return movable;
        }

        protected override AbstractMove CreateUndoMove(ScoreDirector scoreDirector)
        {
            return new SwapMove(variableDescriptorList, rightEntity, leftEntity);
        }

        protected override void DoMoveOnGenuineVariables(ScoreDirector scoreDirector)
        {
            InnerScoreDirector innerScoreDirector = (InnerScoreDirector)scoreDirector;
            foreach (var variableDescriptor in variableDescriptorList)
            {
                Object oldLeftValue = variableDescriptor.GetValue(leftEntity);
                Object oldRightValue = variableDescriptor.GetValue(rightEntity);
                if (!oldLeftValue.Equals(oldRightValue))
                {
                    innerScoreDirector.ChangeVariableFacade(variableDescriptor, leftEntity, oldRightValue);
                    innerScoreDirector.ChangeVariableFacade(variableDescriptor, rightEntity, oldLeftValue);
                }
            }
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
