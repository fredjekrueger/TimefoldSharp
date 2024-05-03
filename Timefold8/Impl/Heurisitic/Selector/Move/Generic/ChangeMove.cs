using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Move;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Move.Generic
{
    public class ChangeMove : AbstractMove
    {
        protected readonly GenuineVariableDescriptor variableDescriptor;
        protected readonly Object entity;
        protected readonly Object toPlanningValue;

        public ChangeMove(GenuineVariableDescriptor variableDescriptor, Object entity, Object toPlanningValue)
        {
            this.variableDescriptor = variableDescriptor;
            this.entity = entity;
            this.toPlanningValue = toPlanningValue;
        }

        public override bool IsMoveDoable(ScoreDirector scoreDirector)
        {
            Object oldValue = variableDescriptor.GetValue(entity);
            return !object.Equals(oldValue, toPlanningValue);
        }

        protected override AbstractMove CreateUndoMove(ScoreDirector scoreDirector)
        {
            Object oldValue = variableDescriptor.GetValue(entity);
            return new ChangeMove(variableDescriptor, entity, oldValue);
        }

        protected override void DoMoveOnGenuineVariables(ScoreDirector scoreDirector)
        {
            InnerScoreDirector innerScoreDirector = (InnerScoreDirector)scoreDirector;
            innerScoreDirector.ChangeVariableFacade(variableDescriptor, entity, toPlanningValue);
        }

        public override string ToString()
        {
            Object oldValue = variableDescriptor.GetValue(entity);
            return entity + " {" + oldValue + " -> " + toPlanningValue + "}";
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
