using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Domain.Solution;
using TimefoldSharp.Core.API.Domain.ValueRange;
using TimefoldSharp.Core.API.Score;
using TimefoldSharp.Core.API.Score.Buildin.HardSoftLong;

namespace TimefoldSharp.Examples.Orderpicking.Orderpicking.Domain
{
    [PlanningSolution]
    public class OrderPickingSolution : ISolution
    {
        [ValueRangeProvider]
        [ProblemFactCollectionProperty]
    public List<Trolley> TrolleyList { get; set; }

        [ValueRangeProvider]
        [PlanningEntityCollectionProperty]
        public List<TrolleyStep> TrolleyStepList { get; set; }

        [PlanningScore]
        public HardSoftLongScore Score { get; set; }

        public OrderPickingSolution()
        {
            // Marshalling constructor
        }

        public OrderPickingSolution(List<Trolley> trolleyList, List<TrolleyStep> trolleyStepList)
        {
            this.TrolleyList = trolleyList;
            this.TrolleyStepList = trolleyStepList;
        }


    }
}
