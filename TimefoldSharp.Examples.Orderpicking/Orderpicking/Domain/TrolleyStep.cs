using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimefoldSharp.Core.API.Domain.Entity;
using TimefoldSharp.Core.API.Domain.Variable;

namespace TimefoldSharp.Examples.Orderpicking.Orderpicking.Domain
{
    [PlanningEntity]
    public class TrolleyStep : TrolleyOrTrolleyStep
    {
        public OrderItem OrderItem { get; set; }

        [PlanningVariable(GraphType = PlanningVariableGraphType.CHAINED)]
        public TrolleyOrTrolleyStep PreviousElement { get; set; }

        [AnchorShadowVariable(SourceVariableName = PREVIOUS_ELEMENT)]
        public Trolley Trolley { get; set; }

        public TrolleyStep()
        {
            //marshaling constructor.
        }

        public TrolleyStep(OrderItem orderItem)
        {
            this.OrderItem = orderItem;
        }

        public override WarehouseLocation GetLocation()
        {
            return OrderItem.Product.Location;
        }

        public bool IsLast()
        {
            return NextElement == null;
        }

        public string GetTrolleyId()
        {
            return Trolley?.ID;
        }

        public override string ToString()
        {
            return "TS " + OrderItem + " " + PreviousElement +" " + Trolley;
        }
    }
}
