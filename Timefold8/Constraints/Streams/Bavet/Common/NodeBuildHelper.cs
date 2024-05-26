using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Bi;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;
using TimefoldSharp.Core.Constraints.Streams.Common.Inliner;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public sealed class NodeBuildHelper
    {
        private HashSet<BavetAbstractConstraintStream> constraintStreamSet;
        private AbstractScoreInliner scoreInliner;
        private List<AbstractNode> reversedNodeList;
        private readonly Dictionary<ConstraintStream, int?> storeIndexMap;

        private readonly Dictionary<ConstraintStream, TupleLifecycle> tupleLifecycleMap;


        private readonly HashSet<ConstraintStream> activeStreamSet;
        private readonly Dictionary<AbstractNode, BavetAbstractConstraintStream> nodeCreatorMap;

        public NodeBuildHelper(HashSet<ConstraintStream> activeStreamSet, AbstractScoreInliner scoreInliner)
        {
            this.activeStreamSet = activeStreamSet;
            this.scoreInliner = scoreInliner;
            int activeStreamSetSize = activeStreamSet.Count;
            this.nodeCreatorMap = new Dictionary<AbstractNode, BavetAbstractConstraintStream>(Math.Max(16, activeStreamSetSize));
            this.tupleLifecycleMap = new Dictionary<ConstraintStream, TupleLifecycle>(Math.Max(16, activeStreamSetSize));
            this.storeIndexMap = new Dictionary<ConstraintStream, int?>(Math.Max(16, activeStreamSetSize / 2));
            this.reversedNodeList = new List<AbstractNode>(activeStreamSetSize);
        }

        public AbstractNode FindParentNode(BavetAbstractConstraintStream childNodeCreator)
        {
            if (childNodeCreator == null)
            { // We've recursed to the bottom without finding a parent node.
                throw new Exception(
                        "Impossible state: node-creating stream (" + childNodeCreator + ") has no parent node.");
            }
            // Look the stream up among node creators and if found, the node is the parent node.
            foreach (var entry in nodeCreatorMap)
            {
                if (entry.Value == childNodeCreator)
                {
                    return entry.Key;
                }
            }
            // Otherwise recurse to the parent node creator;
            // this happens for bridges, filters and other streams that do not create nodes.
            return FindParentNode(childNodeCreator.GetParent());
        }

        public void AddNode(AbstractNode node, BavetAbstractConstraintStream creator, BavetAbstractConstraintStream leftParent, BavetAbstractConstraintStream rightParent)
        {
            reversedNodeList.Add(node);
            nodeCreatorMap.Add(node, creator);
            PutInsertUpdateRetract(leftParent, TupleLifecycleHelper.OfLeft((LeftTupleLifecycle)node));
            PutInsertUpdateRetract(rightParent, TupleLifecycleHelper.OfRight((RightTupleLifecycle)node));
        }

        public BavetAbstractConstraintStream GetNodeCreatingStream(AbstractNode node)
        {
            BavetAbstractConstraintStream item;
            nodeCreatorMap.TryGetValue(node, out item);
            return item;
        }

        public void AddNode(AbstractNode node, BavetAbstractConstraintStream creator, BavetAbstractConstraintStream parent)
        {
            reversedNodeList.Add(node);
            nodeCreatorMap.Add(node, creator);
            /*if (!(node is AbstractForEachUniNode<object>)) 
            {
                if (parent == null)
                {
                    throw new Exception("Impossible state: The node (" + node + ") has no parent (" + parent + ").");
                }
                PutInsertUpdateRetract(parent, node as TupleLifecycle);
            }*/
        }

        public int ExtractTupleStoreSize(ConstraintStream tupleSourceStream)
        {
            int? lastIndex;
            if (!storeIndexMap.TryGetValue(tupleSourceStream, out lastIndex))
                storeIndexMap.Add(tupleSourceStream, int.MinValue);

            return (lastIndex == null) ? 0 : lastIndex.Value + 1;
        }

        public void PutInsertUpdateRetract(ConstraintStream stream, TupleLifecycle tupleLifecycle)
        {
            tupleLifecycleMap.Add(stream, tupleLifecycle);
        }



        public AbstractScoreInliner GetScoreInliner()
        {
            return scoreInliner;
        }

        public int ReserveTupleStoreIndex(ConstraintStream tupleSourceStream)
        {
            if (!storeIndexMap.TryGetValue(tupleSourceStream, out var index))
            {
                index = 0;
                storeIndexMap.Add(tupleSourceStream, index);
            }
            else if (index < 0)
            {
                throw new Exception($"Impossible state: the tupleSourceStream ({tupleSourceStream}) is reserving a store after it has been extracted.");
            }
            else
            {
                storeIndexMap[tupleSourceStream] = index + 1;
            }

            return storeIndexMap[tupleSourceStream].Value;
        }

        public bool IsStreamActive(ConstraintStream stream)
        {
            return activeStreamSet.Contains(stream);
        }

        private static TupleLifecycle GetTupleLifecycle(ConstraintStream stream,
           Dictionary<ConstraintStream, TupleLifecycle> tupleLifecycleMap)
        {
            TupleLifecycle tupleLifecycle = (TupleLifecycle)tupleLifecycleMap[stream];
            if (tupleLifecycle == null)
            {
                throw new Exception("Impossible state: the stream (" + stream + ") hasn't built a node yet.");
            }
            return tupleLifecycle;
        }

        public TupleLifecycle GetAggregatedTupleLifecycle(IEnumerable<ConstraintStream> streamList)
        {
            TupleLifecycle[] tupleLifecycles = streamList.Where(i => IsStreamActive(i))
                    .Select(s => GetTupleLifecycle(s, tupleLifecycleMap))
                    .ToArray();
            switch (tupleLifecycles.Length)
            {
                case 0:
                    throw new Exception("Impossible state: None of the streamList (" + streamList
                            + ") are active.");
                case 1:
                    return tupleLifecycles[0];
                default:
                    return new AggregatedTupleLifecycle(tupleLifecycles);
            }
        }

        public List<AbstractNode> DestroyAndGetNodeList()
        {
            List<AbstractNode> nodeList = this.reversedNodeList;
            nodeList.Reverse();
            this.reversedNodeList = null;
            return nodeList;
        }

        public void PutInsertUpdateRetract(ConstraintStream stream, List<BavetAbstractConstraintStream> childStreamList, Func<TupleLifecycle,
            TupleLifecycle> tupleLifecycleFunction)
        {
            TupleLifecycle tupleLifecycle = GetAggregatedTupleLifecycle(childStreamList);
            PutInsertUpdateRetract(stream, tupleLifecycleFunction.Invoke(tupleLifecycle));
        }
    }
}
