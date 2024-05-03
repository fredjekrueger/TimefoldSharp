using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Score.Director;

namespace TimefoldSharp.Core.Impl.Heurisitic.Move
{
    public sealed class CompositeMove : Move
    {

        Move[] moves;

        public void DoMoveOnly(ScoreDirector scoreDirector)
        {
            DoMove(scoreDirector);
        }

        public override string ToString()
        {
            return string.Join(" ", moves.ToList());
        }


        public CompositeMove(Move[] moves)
        {
            this.moves = moves;
        }

        public static Move BuildMove(Move[] moveList)
        {
            int size = moveList.Length;
            if (size > 1)
            {
                return new CompositeMove(moveList.ToArray());
            }
            else if (size == 1)
            {
                return moveList[0];
            }
            else
            {
                return new NoChangeMove();
            }
        }

        public Move DoMove(ScoreDirector scoreDirector)
        {
            Move[] undoMoves = new Move[moves.Length];
            int doableCount = 0;
            foreach (var move in moves)
            {
                if (!move.IsMoveDoable(scoreDirector))
                {
                    continue;
                }
                // Calls scoreDirector.triggerVariableListeners() between moves
                // because a later move can depend on the shadow variables changed by an earlier move
                Move undoMove = move.DoMove(scoreDirector);
                // Undo in reverse order and each undoMove is created after previous moves have been done
                undoMoves[moves.Length - 1 - doableCount] = undoMove;
                doableCount++;
            }
            if (doableCount < undoMoves.Length)
            {
                undoMoves = undoMoves.Skip(undoMoves.Length - doableCount).ToArray();
            }
            // No need to call scoreDirector.triggerVariableListeners() because Move.doMove() already does it for every move.
            return new CompositeMove(undoMoves);
        }

        public bool IsMoveDoable(ScoreDirector scoreDirector)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object other)
        {
            if (this == other)
            {
                return true;
            }
            if (other == null || GetType() != other.GetType())
            {
                return false;
            }
            CompositeMove that = (CompositeMove)other;
            return Array.Equals(moves, that.moves);
        }

        public override int GetHashCode()
        {
            return Utils.CombineHashCodes(moves);
        }
    }
}
