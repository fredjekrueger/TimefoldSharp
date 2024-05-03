using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Uni;
using TimefoldSharp.Core.Constraints.Streams.Common.Inliner;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet
{
    public class BavetConstraintSession
    {
        public BavetConstraintSession(AbstractScoreInliner scoreInliner)
            : this(scoreInliner, new Dictionary<Type, List<IAbstractForEachUniNode>>(), new Propagator[0][])
        {

        }

        public void Update(Object fact)
        {
            var factClass = fact.GetType();
            foreach (var node in FindNodes(factClass))
            {
                node.Update(fact);
            }
        }

        public BavetConstraintSession(AbstractScoreInliner scoreInliner, Dictionary<Type, List<IAbstractForEachUniNode>> declaredClassToNodeMap, Propagator[][] layeredNodes)
        {
            this.scoreInliner = scoreInliner;
            this.declaredClassToNodeMap = declaredClassToNodeMap;
            this.layeredNodes = layeredNodes;
            this.effectiveClassToNodeArrayMap = new Dictionary<Type, IAbstractForEachUniNode[]>();
        }

        private readonly Propagator[][] layeredNodes; // First level is the layer, second determines iteration order.
        private readonly Dictionary<Type, IAbstractForEachUniNode[]> effectiveClassToNodeArrayMap;
        private readonly Dictionary<Type, List<IAbstractForEachUniNode>> declaredClassToNodeMap;
        private readonly AbstractScoreInliner scoreInliner;

        public Score CalculateScore(int initScore)
        {
            var layerCount = layeredNodes.Length;


            for (var layerIndex = 0; layerIndex < layerCount; layerIndex++)
            {
                CalculateScoreInLayer(layerIndex);
            }

            return scoreInliner.ExtractScore(initScore);
        }

        public void Insert(object fact)
        {
            var factClass = fact.GetType();
            foreach (var node in FindNodes(factClass))
            {
                node.Insert(fact);
            }
        }

        private IAbstractForEachUniNode[] FindNodes(Type factClass)
        {
            // Map.computeIfAbsent() would have created lambdas on the hot path, this will not.

            IAbstractForEachUniNode[] nodeArray;

            var found = effectiveClassToNodeArrayMap.TryGetValue(factClass, out nodeArray);
            if (!found)
            {
                nodeArray = declaredClassToNodeMap
                 .Where(entry => entry.Key.IsAssignableFrom(factClass))
                 .SelectMany(entry => entry.Value)
                 .ToArray();
                effectiveClassToNodeArrayMap.Add(factClass, nodeArray);
            }
            return nodeArray;
        }

        private void CalculateScoreInLayer(int layerIndex)
        {
            var nodesInLayer = layeredNodes[layerIndex];
            var nodeCount = nodesInLayer.Length;
            if (nodeCount == 1)
            {
                nodesInLayer[0].PropagateEverything();
            }
            else
            {
                foreach (var node in nodesInLayer)
                {
                    node.PropagateRetracts();
                }
                foreach (var node in nodesInLayer)
                {
                    node.PropagateUpdates();
                }
                foreach (var node in nodesInLayer)
                {
                    node.PropagateInserts();
                }
            }
        }
    }
}
