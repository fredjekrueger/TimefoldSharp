using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimefoldSharp.Examples.Orderpicking.Orderpicking.Domain
{
    public class OrderItem
    {
        public String ID { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }

        public OrderItem()
        {
            //marshalling constructor
        }

        public OrderItem(String id, Order order, Product product)
        {
            this.ID = id;
            this.Order = order;
            this.Product = product;
        }

        public string GetOrderId()
        {
            return Order?.ID;
        }

        public override string ToString()
        {
            return "OrderItem{" +
                    "id='" + ID + '\'' +
                    ", order=" + Order +
                    ", product=" + Product +
                    '}';
        }
    }
}
