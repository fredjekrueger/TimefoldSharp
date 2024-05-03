using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Uni;
using TimefoldSharp.Core.Constraints.Streams.Common.Inliner;
using TimefoldSharp.Core.Helpers;
using TimefoldSharp.Core.Impl.Domain.Score.Definition;
using TimefoldSharp.Core.Impl.Domain.Solution.Descriptor;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet
{
    public sealed class BavetConstraintSessionFactory
    {
        private SolutionDescriptor solutionDescriptor;
        private List<BavetConstraint> constraintList;

        public BavetConstraintSessionFactory(SolutionDescriptor solutionDescriptor, List<BavetConstraint> constraintList)
        {
            this.solutionDescriptor = solutionDescriptor;
            this.constraintList = constraintList;
        }

        public BavetConstraintSession BuildSession(bool constraintMatchEnabled, ISolution workingSolution)
        {
            ScoreDefinition scoreDefinition = solutionDescriptor.GetScoreDefinition();
            Score zeroScore = scoreDefinition.GetZeroScore();
            HashSet<BavetAbstractConstraintStream> constraintStreamSet = new HashSet<BavetAbstractConstraintStream>();
            Dictionary<Constraint, Score> constraintWeightMap = new Dictionary<Constraint, Score>();
            foreach (var constraint in constraintList)
            {
                Score constraintWeight = constraint.ExtractConstraintWeight(workingSolution);
                /*
                 * Filter out nodes that only lead to constraints with zero weight.
                 * Note: Node sharing happens earlier, in BavetConstraintFactory#share(Stream_).
                 */
                if (!constraintWeight.Equals(zeroScore))
                {
                    /*
                     * Relies on BavetConstraintFactory#share(Stream_) occurring for all constraint stream instances
                     * to ensure there are no 2 equal ConstraintStream instances (with different child stream lists).
                     */
                    constraint.CollectActiveConstraintStreams(constraintStreamSet);
                    constraintWeightMap.Add(constraint, constraintWeight);
                }
            }
            AbstractScoreInliner scoreInliner = AbstractScoreInliner.BuildScoreInliner<AbstractScoreInliner>(scoreDefinition, constraintWeightMap, constraintMatchEnabled);
            if (constraintStreamSet.Count == 0)
            { // All constraints were disabled.
                return new BavetConstraintSession(scoreInliner);
            }
            /*
             * Build constraintStreamSet in reverse order to create downstream nodes first
             * so every node only has final variables (some of which have downstream node method references).
             */
            NodeBuildHelper buildHelper = new NodeBuildHelper(new HashSet<ConstraintStream>(constraintStreamSet), scoreInliner);
            List<BavetAbstractConstraintStream> reversedConstraintStreamList = new List<BavetAbstractConstraintStream>(constraintStreamSet);
            reversedConstraintStreamList.Reverse();
            foreach (var constraintStream in reversedConstraintStreamList)
            {
                constraintStream.BuildNode(buildHelper);
            }
            List<AbstractNode> nodeList = buildHelper.DestroyAndGetNodeList();
            Dictionary<Type, List<IAbstractForEachUniNode>> declaredClassToNodeMap = new Dictionary<Type, List<IAbstractForEachUniNode>>();
            long nextNodeId = 0;
            foreach (var node in nodeList)
            {
                /*
                 * Nodes are iterated first to last, starting with forEach(), the ultimate parent.
                 * Parents are guaranteed to come before children.
                 */
                node.SetId(nextNodeId++);
                node.SetLayerIndex(DetermineLayerIndex(node, buildHelper));
                if (Utils.IsGenericTypeOfType(typeof(AbstractForEachUniNode<>), node.GetType()))
                {
                    var forEachUniNode = (IAbstractForEachUniNode)node;
                    Type forEachClass = forEachUniNode.GetForEachClass();
                    List<IAbstractForEachUniNode> forEachUniNodeList = declaredClassToNodeMap.GetOrAdd(forEachClass, k => new List<IAbstractForEachUniNode>());
                    if (forEachUniNodeList.Count == 2)
                    {
                        // Each class can have at most two forEach nodes: one including null vars, the other excluding them.
                        throw new Exception("Impossible state: For class (" + forEachClass
                                + ") there are already 2 nodes (" + forEachUniNodeList + "), not adding another ("
                                + forEachUniNode + ").");
                    }
                    forEachUniNodeList.Add(forEachUniNode);
                }
            }
            SortedDictionary<long?, List<Propagator>> layerMap = new SortedDictionary<long?, List<Propagator>>();
            foreach (var node in nodeList)
            {
                layerMap.GetOrAdd(node.GetLayerIndex(), k => new List<Propagator>())
                        .Add(node.GetPropagator());
            }
            int layerCount = layerMap.Count;
            Propagator[][] layeredNodes = new Propagator[layerCount][];
            for (int i = 0; i < layerCount; i++)
            {
                layerMap.TryGetValue(i, out List<Propagator> layer);
                layeredNodes[i] = layer.ToArray(); //hier stond nog  new Propagator[0]
            }
            return new BavetConstraintSession(scoreInliner, declaredClassToNodeMap, layeredNodes);
        }



        private long DetermineLayerIndex(AbstractNode node, NodeBuildHelper buildHelper)
        {
            if (Utils.IsGenericTypeOfType(typeof(AbstractForEachUniNode<>), node.GetType()))
            { // ForEach nodes, and only they, are in layer 0.
                return 0;
            }
            else if (Utils.IsGenericTypeOfType(typeof(AbstractJoinNode<,,>), node.GetType()))
            {
                var nodeCreator = (BavetJoinConstraintStream)buildHelper.GetNodeCreatingStream(node);
                var leftParent = nodeCreator.GetLeftParent();
                var rightParent = nodeCreator.GetRightParent();
                var leftParentNode = buildHelper.FindParentNode(leftParent);
                var rightParentNode = buildHelper.FindParentNode(rightParent);
                return Math.Max(leftParentNode.GetLayerIndex(), rightParentNode.GetLayerIndex()) + 1;
            }
            else if (Utils.IsGenericTypeOfType(typeof(AbstractIfExistsNode<,>), node.GetType()))
            {
                var nodeCreator = (BavetIfExistsConstraintStream)buildHelper.GetNodeCreatingStream(node);
                var leftParent = nodeCreator.GetLeftParent();
                var rightParent = nodeCreator.GetRightParent();
                var leftParentNode = buildHelper.FindParentNode(leftParent);
                var rightParentNode = buildHelper.FindParentNode(rightParent);
                return Math.Max(leftParentNode.GetLayerIndex(), rightParentNode.GetLayerIndex()) + 1;
            }
            else
            {
                var nodeCreator = buildHelper.GetNodeCreatingStream(node);
                var parentNode = buildHelper.FindParentNode(nodeCreator.GetParent());
                return parentNode.GetLayerIndex() + 1;
            }
        }
    }
}
