using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimefoldSharp.Examples.Orderpicking.Orderpicking.Domain
{
    public class Shelving
    {
        public static int ROWS_SIZE = 10;

        public enum Side
        {
            LEFT,
            RIGHT
        }

        public string ID { get; }

        public int X { get; }
        public int Y { get; }

        public Shelving(String id, int x, int y)
        {
            this.ID = id;
            this.X = x;
            this.Y = y;
        }

        public static string NewShelvingId(char column, char row)
        {
            return "(" + column.ToString() + "," + row.ToString() + ")";
        }
    }
}
