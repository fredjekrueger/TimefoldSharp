using System.Collections;
using TimefoldSharp.Core.Impl.Heurisitic.Move;

namespace TimefoldSharp.Core.Impl.ConstructionHeuristic.Placer
{
    public class Placement : IEnumerable<Move>
    {
        public IEnumerator<Move> GetEnumerator()
        {
            return moveIterator;
        }

        private IEnumerator<Move> moveIterator;

        public Placement(IEnumerator<Move> moveIterator)
        {
            this.moveIterator = moveIterator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Placement (" + moveIterator + ")";
        }
    }
}
