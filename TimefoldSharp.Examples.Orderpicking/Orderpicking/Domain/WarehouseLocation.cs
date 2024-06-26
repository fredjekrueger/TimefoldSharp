using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimefoldSharp.Examples.Orderpicking.Orderpicking.Domain
{
    public class WarehouseLocation
    {

        public string ShelvingId { get; set; }
        public Shelving.Side Side { get; set; }
        public int Row { get; set; }

        public WarehouseLocation()
        {
            //marshalling constructor
        }

        public WarehouseLocation(String shelvingId, Shelving.Side side, int row)
        {
            this.ShelvingId = shelvingId;
            this.Side = side;
            this.Row = row;
        }

        public override string ToString()
        {
            return "WarehouseLoc{shelvingId='" + ShelvingId + "', side=" + Side + ", row=" + Row + '}';
        }
    }
}
