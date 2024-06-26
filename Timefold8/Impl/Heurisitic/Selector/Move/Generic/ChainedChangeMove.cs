using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Domain.Variable.InverseRelation;
using TimefoldSharp.Core.Impl.Heurisitic.Move;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Generic
{
    public class ChainedChangeMove : ChangeMove
    {
        protected readonly Object oldTrailingEntity;
        protected readonly Object newTrailingEntity;

        public ChainedChangeMove(GenuineVariableDescriptor variableDescriptor, object entity, object toPlanningValue,SingletonInverseVariableSupply inverseVariableSupply) 
            : base(variableDescriptor, entity, toPlanningValue)
        {
            oldTrailingEntity = inverseVariableSupply.GetInverseSingleton(entity);
            newTrailingEntity = toPlanningValue == null ? null : inverseVariableSupply.GetInverseSingleton(toPlanningValue);
        }

        public ChainedChangeMove(GenuineVariableDescriptor variableDescriptor, object entity, object toPlanningValue, object oldTrailingEntity, object newTrailingEntity)
            :base(variableDescriptor, entity, toPlanningValue)
        {
            this.oldTrailingEntity = oldTrailingEntity;
            this.newTrailingEntity = newTrailingEntity;
        }

        protected override void DoMoveOnGenuineVariables(ScoreDirector scoreDirector)
        {
            InnerScoreDirector innerScoreDirector = (InnerScoreDirector)scoreDirector;
            object oldValue = variableDescriptor.GetValue(entity);
            // Close the old chain
            if (oldTrailingEntity != null)
            {
                innerScoreDirector.ChangeVariableFacade(variableDescriptor, oldTrailingEntity, oldValue);
            }
            // Change the entity
            innerScoreDirector.ChangeVariableFacade(variableDescriptor, entity, toPlanningValue);
            // Reroute the new chain
            if (newTrailingEntity != null)
            {
                innerScoreDirector.ChangeVariableFacade(variableDescriptor, newTrailingEntity, entity);
            }
        }

        protected override AbstractMove CreateUndoMove(ScoreDirector scoreDirector)
        {
            object oldValue = variableDescriptor.GetValue(entity);
            return new ChainedChangeMove(variableDescriptor, entity, oldValue, newTrailingEntity, oldTrailingEntity);
        }

        public override bool IsMoveDoable(ScoreDirector scoreDirector)
        {
            return base.IsMoveDoable(scoreDirector) && !entity.Equals(toPlanningValue);
        }

    }
}
