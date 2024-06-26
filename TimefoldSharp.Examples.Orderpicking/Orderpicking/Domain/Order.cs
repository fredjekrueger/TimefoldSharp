using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimefoldSharp.Examples.Orderpicking.Orderpicking.Domain
{
    public class Order
    {
        public string ID {  get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public Order(String id, List<OrderItem> items)
        {
            this.ID = id;
            this.Items = items;
        }

        public override string ToString()
        {
            return "Order: " + ID;
        }
    }
}
