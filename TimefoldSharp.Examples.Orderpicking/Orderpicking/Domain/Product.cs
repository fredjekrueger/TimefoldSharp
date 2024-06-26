using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimefoldSharp.Examples.Orderpicking.Orderpicking.Domain
{
    public class Product
    {
        public string ID { get; set; }
        public string Name { get; set; }
        /**
         * The volume of a product is measured in cm3.
         */
        public int Volume { get; set; }
        public WarehouseLocation Location { get; set; }

        public Product()
        {
            //marshalling constructor
        }

        public Product(String id, String name, int volume, WarehouseLocation location)
        {
            this.ID = id;
            this.Name = name;
            this.Volume = volume;
            this.Location = location;
        }



        public override string ToString()
        {
            return "Product{" +
                    "id='" + ID + '\'' +
                    ", name='" + Name + '\'' +
                    ", volume=" + Volume +
                    ", location=" + Location +
                    '}';
        }
    }
}
