using TimefoldSharp.Core.Impl.Domain.Common.Accessor;
using TimefoldSharp.Core.Impl.Heurisitic.Selector.Common.Decorator;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Selector.Entity.Decorator
{
    public class PinEntityFilter : SelectionFilter<object>
    {
        private readonly MemberAccessor memberAccessor;

        public PinEntityFilter(MemberAccessor memberAccessor)
        {
            this.memberAccessor = memberAccessor;
            Accept = AcceptInt;
        }

        public override Func<ScoreDirector, object, bool> Accept { get; set; }

        bool AcceptInt(ScoreDirector scoreDirector, object entity)
        {
            bool pinned = (bool)memberAccessor.ExecuteGetter(entity);

            return !pinned;
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
