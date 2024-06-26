using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimefoldSharp.Examples.Orderpicking.Orderpicking.Domain
{
    public class Trolley : TrolleyOrTrolleyStep
    {
        public string ID { get; set; }
        public int BucketCount { get; set; }
        public int BucketCapacity { get; set; }
        public WarehouseLocation Location { get; set; }

        public Trolley()
        {
            //marshalling constructor
        }

        public Trolley(String id, int bucketCount, int bucketCapacity, WarehouseLocation location)
        {
            this.ID = id;
            this.BucketCount = bucketCount;
            this.BucketCapacity = bucketCapacity;
            this.Location = location;
        }


        public override WarehouseLocation GetLocation()
        {
            return Location;
        }

        public override string ToString()
        {
            return "Trolley: " +ID + " " + BucketCount + " " + BucketCapacity + " " + Location;
        }
    }
}