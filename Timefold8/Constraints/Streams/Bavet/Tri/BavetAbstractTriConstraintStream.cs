using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Score.Stream.Bi;
using TimefoldSharp.Core.API.Score.Stream.Tri;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Bridge;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Constraints.Streams.Common;
using TimefoldSharp.Core.Constraints.Streams.Common.Tri;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Tri
{
    public class BavetAbstractTriConstraintStream<A, B, C> : BavetAbstractConstraintStream, InnerTriConstraintStream<A, B, C>
    {

        protected BavetAbstractTriConstraintStream(BavetConstraintFactory constraintFactory, BavetAbstractConstraintStream parent) : base(constraintFactory, parent)
        {
        }

        protected BavetAbstractTriConstraintStream(BavetConstraintFactory constraintFactory, RetrievalSemantics retrievalSemantics) : base(constraintFactory, retrievalSemantics) 
        {
        }

        public override void BuildNode(NodeBuildHelper buildHelper)
        {
            throw new NotImplementedException();
        }

        public API.Score.Stream.Tri.TriConstraintStream<GroupKeyA_, GroupKeyB_, Result_> GroupBy<GroupKeyA_, GroupKeyB_, ResultContainer_, Result_>(Func<A, B, C, GroupKeyA_> groupKeyAMapping, Func<A, B, C, GroupKeyB_> groupKeyBMapping, API.Score.Stream.Tri.TriConstraintCollector<A, B, C, ResultContainer_, Result_> collector)
        {
            GroupNodeConstructor<TriTuple<GroupKeyA_, GroupKeyB_, Result_>> nodeConstructor =
                GroupNodeConstructorHelper.Of<TriTuple<GroupKeyA_, GroupKeyB_, Result_>>((groupStoreIndex, undoStoreIndex, tupleLifecycle, outputStoreSize, environmentMode)
                => new Group2Mapping1CollectorTriNode<A,B,C, GroupKeyA_, GroupKeyB_, Result_, ResultContainer_>(groupKeyAMapping, groupKeyBMapping, groupStoreIndex, undoStoreIndex, collector, tupleLifecycle, outputStoreSize, environmentMode));
            return BuildTriGroupBy(nodeConstructor);
        }

        private  TriConstraintStream<NewA, NewB, NewC> BuildTriGroupBy<NewA, NewB, NewC>(GroupNodeConstructor<TriTuple<NewA, NewB, NewC>> nodeConstructor)
        {
            var stream = ShareAndAddChild(new BavetTriGroupTriConstraintStream<A, B,C, NewA,NewB,NewC>(constraintFactory, this, nodeConstructor));
            return constraintFactory.Share(new BavetAftBridgeTriConstraintStream<NewA, NewB, NewC > (constraintFactory, stream), stream.SetAftBridge);
        }

        public API.Score.Stream.Bi.BiConstraintStream<GroupKey_, Result_> GroupBy<GroupKey_, ResultContainer_, Result_>(Func<A, B, C, GroupKey_> groupKeyMapping, API.Score.Stream.Tri.TriConstraintCollector<A, B, C, ResultContainer_, Result_> collector)
        {
            GroupNodeConstructor<BiTuple<GroupKey_, Result_>> nodeConstructor = GroupNodeConstructorHelper.Of<BiTuple<GroupKey_, Result_>>(
                (groupStoreIndex, undoStoreIndex, tupleLifecycle, outputStoreSize, environmentMode)=> 
                new Group1Mapping1CollectorTriNode<A,B,C, GroupKey_, Result_,ResultContainer_>(groupKeyMapping, groupStoreIndex, undoStoreIndex, collector, tupleLifecycle, 
                outputStoreSize, environmentMode));
            return BuildBiGroupBy(nodeConstructor);
        }

        private BiConstraintStream<NewA, NewB> BuildBiGroupBy<NewA, NewB>(GroupNodeConstructor<BiTuple<NewA, NewB>> nodeConstructor)
        {
            var stream = ShareAndAddChild(new BavetBiGroupTriConstraintStream<A,B,C,NewA,NewB>(constraintFactory, this, nodeConstructor));
            return constraintFactory.Share(new BavetAftBridgeBiConstraintStream<NewA, NewB>(constraintFactory, stream), stream.SetAftBridge);
        }

        protected override IndictedObjectsMapping_ GetDefaultIndictedObjectsMapping<IndictedObjectsMapping_>()
        {
            throw new NotImplementedException();
        }

        protected override JustificationMapping_ GetDefaultJustificationMapping<JustificationMapping_>()
        {
            throw new NotImplementedException();
        }
    }
}
