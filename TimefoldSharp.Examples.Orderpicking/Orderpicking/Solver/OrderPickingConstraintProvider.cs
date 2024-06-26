using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TimefoldSharp.Core.API.Score.Buildin.HardSoftLong;
using TimefoldSharp.Core.API.Score.Stream;
using TimefoldSharp.Examples.Orderpicking.Orderpicking.Domain;

namespace TimefoldSharp.Examples.Orderpicking.Orderpicking.Solver
{
    public class OrderPickingConstraintProvider : ConstraintProvider
    {
        public List<Constraint> DefineConstraints(ConstraintFactory constraintFactory)
        {
            return new List<Constraint> {
                RequiredNumberOfBuckets(constraintFactory),
                MinimizeDistanceFromPreviousTrolleyStep(constraintFactory),
                MinimizeDistanceFromLastTrolleyStepToPathOrigin(constraintFactory),
                MinimizeOrderSplitByTrolley(constraintFactory)
        };
        }

        /*
     * An Order should ideally be prepared on the same trolley, penalize the order splitting into different trolleys.
     */
        Constraint MinimizeOrderSplitByTrolley(ConstraintFactory constraintFactory)
        {
            return constraintFactory.ForEach<TrolleyStep>(typeof(TrolleyStep))
                .GroupBy(trolleyStep => trolleyStep.OrderItem.Order, ConstraintCollectors.CountDistinctLong<TrolleyStep>(t => t.Trolley))
                .PenalizeLong(HardSoftLongScore.ONE_SOFT, (order, trolleySpreadCount) => trolleySpreadCount * 1000)
                .AsConstraint("Minimize order split by trolley");
        }

        /*
     * Minimize the distance travelled by the trolley by ensuring that the distance with the previous element in the
     * chain is as short as possible.
     * 
     * @see TrolleyStep for more information about the model constructed by the Solver.
     */
        Constraint MinimizeDistanceFromPreviousTrolleyStep(ConstraintFactory constraintFactory)
        {
            return constraintFactory.ForEach<TrolleyStep>(typeof(TrolleyStep))
                .PenalizeLong(HardSoftLongScore.ONE_SOFT,
                        trolleyStep => Warehouse.CalculateDistance(trolleyStep.PreviousElement.GetLocation(), trolleyStep.GetLocation()))
                .AsConstraint("Minimize the distance from the previous trolley step");
        }

        /**
     * Minimize the distance travelled by the trolley by ensuring that the distance of the last element in the chain
     * with the return point (the Trolley location) is as short as possible.
     *
     * @see TrolleyStep for more information about the model constructed by the Solver.
     */
        Constraint MinimizeDistanceFromLastTrolleyStepToPathOrigin(ConstraintFactory constraintFactory)
        {
            return constraintFactory.ForEach<TrolleyStep>(typeof(TrolleyStep))
                .Filter(t => t.IsLast())
                .PenalizeLong(HardSoftLongScore.ONE_SOFT, trolleyStep => Warehouse.CalculateDistance(trolleyStep.GetLocation(), trolleyStep.Trolley.GetLocation()))
                .AsConstraint("Minimize the distance from last trolley step to the path origin");
        }

        /*
     * Ensure that a Trolley has a sufficient number of buckets for holding all elements picked along the path and
     * consider that buckets are not shared between orders.
     */
        Constraint RequiredNumberOfBuckets(ConstraintFactory constraintFactory)
        {
            return constraintFactory.ForEach<TrolleyStep>(typeof(TrolleyStep))
                //raw total volume per order
                .GroupBy(trolleyStep => trolleyStep.Trolley, trolleyStep => trolleyStep.OrderItem.Order, ConstraintCollectors.Sum<TrolleyStep>(trolleyStep => trolleyStep.OrderItem.Product.Volume))
                //required buckets per order
                .GroupBy((trolley, order, orderTotalVolume) => trolley, (trolley, order, orderTotalVolume) => order,
                ConstraintCollectors.Sum<Trolley, Order, int>((trolley, order, orderTotalVolume) => CalculateOrderRequiredBuckets(orderTotalVolume, trolley.BucketCapacity)))
                //required buckets per trolley
                .GroupBy((trolley, order, orderTotalBuckets) => trolley, ConstraintCollectors.Sum<Trolley, Order, int>((trolley, order, orderTotalBuckets) => orderTotalBuckets))
                //penalization if the trolley don't have enough buckets to hold the orders
                .Filter((trolley, trolleyTotalBuckets) => trolley.BucketCount < trolleyTotalBuckets)
                .Penalize(HardSoftLongScore.ONE_HARD, (trolley, trolleyTotalBuckets) => trolleyTotalBuckets - trolley.BucketCount)
                .AsConstraint("Required number of buckets");
        }

        private int CalculateOrderRequiredBuckets(int orderVolume, int bucketVolume)
        {
            return (orderVolume + (bucketVolume - 1)) / bucketVolume;
        }
    }
}
