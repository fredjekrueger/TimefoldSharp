using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Tuple;

namespace TimefoldSharp.Core.Constraints.Streams.Bavet.Common
{
    public class Group<OutTuple_, ResultContainer_> : AbstractPropagationMetadataCarrier<OutTuple_> where OutTuple_ : AbstractTuple
    {
        private GroupData groupData;
        private OutTuple_ outTuple;
        public int parentCount = 1;

        private Group(GroupData groupData, OutTuple_ outTuple)
        {
            this.groupData = groupData;
            this.outTuple = outTuple;
        }

        public static Group<OutTuple_, ResultContainer_> CreateWithoutGroupKey(ResultContainer_ resultContainer, OutTuple_ outTuple)
        {
            return new Group<OutTuple_, ResultContainer_>(new GroupDataWithAccumulate(resultContainer), outTuple);
        }

        public static Group<OutTuple_, ResultContainer_> Create(Object groupKey, ResultContainer_ resultContainer, OutTuple_ outTuple)
        {
            return new Group<OutTuple_, ResultContainer_>(new GroupDataWithKeyAndAccumulate(groupKey, resultContainer), outTuple);
        }

        public static Group<OutTuple_, ResultContainer_>CreateWithoutAccumulate(Object groupKey, OutTuple_ outTuple)
        {
            return new Group<OutTuple_, ResultContainer_> (new GroupDataWithKey(groupKey), outTuple);
        }

        public ResultContainer_ GetResultContainer()
        {
            return groupData.ResultContainer();
        }

        public override OutTuple_ GetTuple()
        {
            return outTuple;
        }

        public override TupleState GetState()
        {
            return outTuple.State;
        }

        public object GetGroupKey()
        {
            return groupData.GroupKey();
        }

        public override void SetState(TupleState state)
        {
            outTuple.State = state;
        }

        private interface GroupData
        {
            object GroupKey();
            ResultContainer_ ResultContainer();
        }

        private record GroupDataWithAccumulate(ResultContainer_ resultContainer) : GroupData
        {
            public object GroupKey()
            {
                throw new Exception("Impossible state: no group key.");
            }

            public ResultContainer_ ResultContainer()
            {
                return resultContainer;
            }
        }

        private record GroupDataWithKey(object groupKey) : GroupData
        {
            public object GroupKey()
            {
                return groupKey;
            }

            public ResultContainer_ ResultContainer()
            {
                throw new Exception("Impossible state: no result container for group (" + this.groupKey + ").");
            }
        }

        private record GroupDataWithKeyAndAccumulate(Object groupKey, ResultContainer_ resultContainer) : GroupData
        {
            public object GroupKey()
            {
                return groupKey;
            }

            public ResultContainer_ ResultContainer()
            {
                return resultContainer;
            }
        }

    }
}

