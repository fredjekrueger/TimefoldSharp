using TimefoldSharp.Core.Impl.Domain.Variable.Descriptor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Value.Decorator
{
    public sealed class MovableChainedTrailingValueFilter : SelectionFilter<Object>
    {
        private readonly GenuineVariableDescriptor variableDescriptor;

        public MovableChainedTrailingValueFilter(GenuineVariableDescriptor variableDescriptor)
        {
            this.variableDescriptor = variableDescriptor;
            Accept = AcceptInt;
        }

        public override Func<ScoreDirector, object, bool> Accept { get; set; }

        public bool AcceptInt(ScoreDirector scoreDirector, object selection)
        {
            throw new NotImplementedException();
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
