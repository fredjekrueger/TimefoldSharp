using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public abstract class AbstractGroupNode<InTuple_, OutTuple_ , GroupKey_, ResultContainer_, Result_>
        : AbstractNode, TupleLifecycle where InTuple_ : AbstractTuple where OutTuple_ : AbstractTuple
    {

        private int groupStoreIndex;
        private int undoStoreIndex;
        private Func<InTuple_, GroupKey_> groupKeyFunction;
        private Func<ResultContainer_> supplier;
        private Func<ResultContainer_, Result_> finisher;
        private bool hasMultipleGroups;
        private bool hasCollector;
        private Dictionary<object, Group<OutTuple_, ResultContainer_>> groupMap;
        private bool useAssertingGroupKey;
        private DynamicPropagationQueue<OutTuple_, Group<OutTuple_, ResultContainer_>> propagationQueue;


        protected AbstractGroupNode(int groupStoreIndex, Func<InTuple_, GroupKey_> groupKeyFunction, TupleLifecycle nextNodesTupleLifecycle, EnvironmentMode environmentMode)
            : this(groupStoreIndex, -1, groupKeyFunction, null, null, nextNodesTupleLifecycle, environmentMode)
        {
        }

        protected AbstractGroupNode(int groupStoreIndex, int undoStoreIndex,
            Func<InTuple_, GroupKey_> groupKeyFunction, Func<ResultContainer_> supplier,
            Func<ResultContainer_, Result_> finisher,
            TupleLifecycle nextNodesTupleLifecycle, EnvironmentMode environmentMode)
        {
            this.groupStoreIndex = groupStoreIndex;
            this.undoStoreIndex = undoStoreIndex;
            this.groupKeyFunction = groupKeyFunction;
            this.supplier = supplier;
            this.finisher = finisher;
            this.hasMultipleGroups = groupKeyFunction != null;
            this.hasCollector = supplier != null;
            /*
             * Not using the default sizing to 1000.
             * The number of groups can be very small, and that situation is not unlikely.
             * Therefore, the size of these collections is kept default.
             */
            this.groupMap = hasMultipleGroups ? new Dictionary<object, Group<OutTuple_, ResultContainer_>>() : null;
            if (hasCollector)
            {
                this.propagationQueue = new DynamicPropagationQueue<OutTuple_, Group<OutTuple_, ResultContainer_>>(nextNodesTupleLifecycle,
                    group =>
                    {
                        var outTuple = group.GetTuple();
                        var state = outTuple.State;
                        if (state == TupleState.CREATING || state == TupleState.UPDATING)
                        {
                            UpdateOutTupleToFinisher(outTuple, group.GetResultContainer());
                        }
                    });
            }
            else
            {
                this.propagationQueue = new DynamicPropagationQueue<OutTuple_, Group<OutTuple_, ResultContainer_>>(nextNodesTupleLifecycle);
            }

            this.useAssertingGroupKey = EnvironmentModeEnumHelper.IsAsserted(environmentMode);
        }

        private void UpdateOutTupleToFinisher(OutTuple_ outTuple, ResultContainer_ resultContainer)
        {
            UpdateOutTupleToResult(outTuple, finisher.Invoke(resultContainer));
        }

        public override Propagator GetPropagator()
        {
            return propagationQueue;
        }

        protected abstract Action Accumulate(ResultContainer_ resultContainer, InTuple_ tuple);

        private OutTuple_ Accumulate(InTuple_ tuple, Group<OutTuple_, ResultContainer_> group)
        {
            if (hasCollector)
            {
                var undoAccumulator = Accumulate(group.GetResultContainer(), tuple);
                tuple.SetStore(undoStoreIndex, undoAccumulator);
            }
            tuple.SetStore(groupStoreIndex, group);
            return group.GetTuple();
        }

        protected abstract OutTuple_ CreateOutTuple(GroupKey_ groupKey);

        private Group<OutTuple_, ResultContainer_> CreateGroupWithoutGroupKey()
        {
            var outTuple = CreateOutTuple(default(GroupKey_)); //hier stond null
            if (!hasCollector)
            {
                throw new Exception("Impossible state: The node (" + this + ") has no collector, "
                        + "but it is still trying to create a group without a group key.");
            }
            var group = Group<OutTuple_, ResultContainer_>.CreateWithoutGroupKey(supplier.Invoke(), outTuple);
            propagationQueue.Insert(group);
            return group;
        }

        private GroupKey_ ExtractUserSuppliedKey(Object groupMapKey)
        {
            return useAssertingGroupKey ? ((AssertingGroupKey)groupMapKey).GetKey() : (GroupKey_)groupMapKey;
        }

        private Group<OutTuple_, ResultContainer_> CreateGroupWithGroupKey(Object groupMapKey)
        {
            var userSuppliedKey = ExtractUserSuppliedKey(groupMapKey);
            var outTuple = CreateOutTuple(userSuppliedKey);
            var group = hasCollector ? Group<OutTuple_, ResultContainer_>.Create(groupMapKey, supplier.Invoke(), outTuple)
                    : Group<OutTuple_, ResultContainer_>.CreateWithoutAccumulate(groupMapKey, outTuple);
            propagationQueue.Insert(group);
            return group;
        }

        private Group<OutTuple_, ResultContainer_> singletonGroup;

        private Group<OutTuple_, ResultContainer_> GetOrCreateGroup(GroupKey_ userSuppliedKey)
        {
            object groupMapKey;
            if (useAssertingGroupKey)
                groupMapKey = new AssertingGroupKey(userSuppliedKey);
            else
                groupMapKey = userSuppliedKey;
            if (hasMultipleGroups)
            {
                // Avoids computeIfAbsent in order to not create lambdas on the hot path.
                
                Group<OutTuple_, ResultContainer_> group;
                groupMap.TryGetValue(groupMapKey, out group);
                if (group == null)
                {
                    group = CreateGroupWithGroupKey(groupMapKey);
                    groupMap.Add(groupMapKey, group);
                }
                else
                {
                    group.parentCount++;
                }
                return group;
            }
            else
            {
                if (singletonGroup == null)
                {
                    singletonGroup = CreateGroupWithoutGroupKey();
                }
                else
                {
                    singletonGroup.parentCount++;
                }
                return singletonGroup;
            }
        }

        private void CreateTuple(InTuple_ tuple, GroupKey_ userSuppliedKey)
        {
            var newGroup = GetOrCreateGroup(userSuppliedKey);
            var outTuple = Accumulate(tuple, newGroup);
            switch (outTuple.State)
            {
                case TupleState.CREATING: case TupleState.UPDATING:
                        break;
                case TupleState.OK: case TupleState.DYING: 
                        propagationQueue.Update(newGroup); break;
                case TupleState.ABORTING: 
                    propagationQueue.Insert(newGroup);
                    break;
                default: throw new Exception("Impossible state: The group (" + newGroup + ") in node (" + this
                        + ") is in an unexpected state (" + outTuple.State + ").");
            }
        }

        protected abstract void UpdateOutTupleToResult(OutTuple_ outTuple, Result_ result);

        public void Insert(ITuple tuple)
        {
            GroupKey_ userSuppliedKey;
            if (hasMultipleGroups)
                userSuppliedKey = groupKeyFunction.Invoke((InTuple_)tuple);
            else
                userSuppliedKey = default(GroupKey_); //hier stond null
            CreateTuple((InTuple_)tuple, userSuppliedKey);
        }

        public void Update(ITuple tuple)
        {
            var oldGroup = tuple.GetStore<Group<OutTuple_, ResultContainer_>>(groupStoreIndex);
            if (oldGroup == null)
            {
                // No fail fast if null because we don't track which tuples made it through the filter predicate(s)
                Insert(tuple);
                return;
            }
            if (hasCollector)
            {
                var undoAccumulator = tuple.GetStore<Action>(undoStoreIndex);
                undoAccumulator.Invoke();
            }

            var oldUserSuppliedGroupKey = hasMultipleGroups ? ExtractUserSuppliedKey(oldGroup.GetGroupKey()) : default(GroupKey_);
            var newUserSuppliedGroupKey = hasMultipleGroups ? groupKeyFunction.Invoke((InTuple_)tuple) : default(GroupKey_);
            if (newUserSuppliedGroupKey.Equals(oldUserSuppliedGroupKey))
            {
                // No need to change parentCount because it is the same group
                var outTuple = Accumulate((InTuple_)tuple, oldGroup);
                switch (outTuple.State)
                {
                    case TupleState.CREATING: case TupleState.UPDATING: {
                            break;
                        }
                    case TupleState.OK: propagationQueue.Update(oldGroup); break;
                    default: throw new Exception("Impossible state: The group (" + oldGroup + ") in node (" + this
                            + ") is in an unexpected state (" + outTuple.State + ").");
                }
            }
            else
            {
                KillTuple(oldGroup);
                CreateTuple((InTuple_)tuple, newUserSuppliedGroupKey);
            }
        }

        public void Retract(ITuple tuple)
        {
            var group = (Group<OutTuple_, ResultContainer_>)tuple.RemoveStore(groupStoreIndex);
            if (group == null)
            {
                // No fail fast if null because we don't track which tuples made it through the filter predicate(s)
                return;
            }
            if (hasCollector)
            {
                var undoAccumulator = (Action)tuple.RemoveStore(undoStoreIndex);
                undoAccumulator.Invoke();
            }
            KillTuple(group);
        }

        private Group<OutTuple_, ResultContainer_> RemoveGroup(Object groupKey)
        {
            if (hasMultipleGroups)
            {
                Group<OutTuple_, ResultContainer_> returnValue;
                groupMap.TryGetValue(groupKey, out returnValue);
                groupMap.Remove(groupKey);
                return returnValue;
            }
            else
            {
                var oldGroup = singletonGroup;
                singletonGroup = null;
                return oldGroup;
            }
        }

        private void KillTuple(Group<OutTuple_, ResultContainer_> group)
        {
            var newParentCount = --group.parentCount;
            var killGroup = (newParentCount == 0);
            if (killGroup)
            {
                var groupKey = hasMultipleGroups ? group.GetGroupKey() : null;
                var oldGroup = RemoveGroup(groupKey);
                if (oldGroup == null)
                {
                    throw new Exception("Impossible state: the group for the groupKey ("
                            + groupKey + ") doesn't exist in the groupMap.\n" +
                            "Maybe groupKey hashcode changed while it shouldn't have?");
                }
            }
            var outTuple = group.GetTuple();
            switch (outTuple.State)
            {
                case TupleState.CREATING: {
                        if (killGroup)
                        {
                            propagationQueue.Retract(group, TupleState.ABORTING);
                        }
                        break;
                    }
                case TupleState.UPDATING: {
                        if (killGroup)
                        {
                            propagationQueue.Retract(group, TupleState.DYING);
                        }
                        break;
                    }
                case TupleState.OK: {
                        if (killGroup)
                        {
                            propagationQueue.Retract(group, TupleState.DYING);
                        }
                        else
                        {
                            propagationQueue.Update(group);
                        }
                        break;
                    }
                default: throw new Exception("Impossible state: The group (" + group + ") in node (" + this
                        + ") is in an unexpected state (" + outTuple.State + ").");
            }
        }

        public class AssertingGroupKey
        {
            private GroupKey_ key;
            private  int initialHashCode;

            public AssertingGroupKey(GroupKey_ key)
            {
                this.key = key;
                this.initialHashCode = key == null ? 0 : key.GetHashCode();
            }

            public GroupKey_ GetKey()
            {
                if (key != null && key.GetHashCode() != initialHashCode)
                {
                    throw new Exception("hashCode of object (" + key + ") of class (" + key.GetType()
                            + ") has changed while it was being used as a group key within groupBy ("
                            +  ").\n"
                            + "Group key hashCode must consistently return the same integer, "
                            + "as required by the general hashCode contract.");
                }
                return key;
            }
        }
    }
}

