using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public interface GroupNodeConstructor<Tuple_> where Tuple_ : AbstractTuple
    {
        void Build(NodeBuildHelper buildHelper, BavetAbstractConstraintStream parentTupleSource, BavetAbstractConstraintStream aftStream,
            List<ConstraintStream> aftStreamChildList, BavetAbstractConstraintStream thisStream, List<ConstraintStream> thisStreamChildList, EnvironmentMode environmentMode);
    }


    public interface NodeConstructorWithAccumulate<Tuple_> where Tuple_ : AbstractTuple
    {
        AbstractNode Apply(int groupStoreIndex, int undoStoreIndex, TupleLifecycle nextNodesTupleLifecycle, int outputStoreSize, EnvironmentMode environmentMode);
    }

    public static class GroupNodeConstructorHelper
    {
        public static GroupNodeConstructor<Tuple_> Of<Tuple_>(Func<int, int, TupleLifecycle, int, EnvironmentMode, AbstractNode> applyAction) where Tuple_ : AbstractTuple
        {
            return new GroupNodeConstructorWithAccumulate<Tuple_>(applyAction);

        }
        public static GroupNodeConstructor<Tuple_> Of<Tuple_>(Func<int, TupleLifecycle, int, EnvironmentMode, AbstractNode> applyAction) where Tuple_ : AbstractTuple
        {
            return new GroupNodeConstructorWithoutAccumulate<Tuple_>(applyAction);
        }
    }
}