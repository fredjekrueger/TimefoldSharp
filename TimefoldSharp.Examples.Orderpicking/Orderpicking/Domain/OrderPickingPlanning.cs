using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Index;

namespace TimefoldSharp.Examples.Orderpicking.Orderpicking.Domain
{
    public class OrderPickingPlanning
    {
        public OrderPickingSolution solution { get; set; }
        public bool solverWasNeverStarted { get; set; }
        public Dictionary<string, int> distanceToTravelByTrolley { get; set; } = new Dictionary<string, int>();

        public OrderPickingPlanning()
        {
            //marshalling constructor
        }

        public OrderPickingPlanning(OrderPickingSolution solution, bool solverWasNeverStarted)
        {
            this.solution = solution;
            this.solverWasNeverStarted = solverWasNeverStarted;
            foreach (var trolley in solution.TrolleyList)
            {
                distanceToTravelByTrolley.Add(trolley.ID, Warehouse.CalculateDistanceToTravel(trolley));
            }
        }
    }
}
