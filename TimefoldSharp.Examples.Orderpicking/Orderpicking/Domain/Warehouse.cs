using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TimefoldSharp.Examples.Orderpicking.Orderpicking.Domain.Warehouse;
using TimefoldSharp.Examples.Orderpicking.Orderpicking.Domain;
using System.Data.Common;

namespace TimefoldSharp.Examples.Orderpicking.Orderpicking.Domain
{
    public class Warehouse
    {

        public static List<char> Columns = new List<char>() { 'A', 'B', 'C', 'D', 'E' };
        public static List<char> Rows = new List<char>() { '1', '2', '3' };

        private static int SHELVING_WIDTH = 2;
        private static int SHELVING_HEIGHT = 10;
        private static int SHELVING_PADDING = 3;
        private static Dictionary<string, Shelving> SHELVING_MAP = new Dictionary<string, Shelving>();
        private static string SHELVING_NOT_FOUND_ERROR = "Shelving: %s was not found in current Warehouse structure.";

        static Warehouse()
        {
            int shelvingX = 0;
            int shelvingY;
            Shelving shelving;

            foreach (var col in Columns)
            {
                shelvingY = 0;
                foreach (var row in Rows)
                {
                    shelving = new Shelving(Shelving.NewShelvingId(col, row), shelvingX, shelvingY);
                    SHELVING_MAP.Add(shelving.ID, shelving);
                    shelvingY = shelvingY + SHELVING_HEIGHT + SHELVING_PADDING;
                }
                shelvingX = shelvingX + SHELVING_WIDTH + SHELVING_PADDING;
            }
        }

        public static int CalculateDistance(WarehouseLocation start, WarehouseLocation end)
        {
            Shelving startShelving = SHELVING_MAP[start.ShelvingId];
            Shelving endShelving = SHELVING_MAP[end.ShelvingId];
            
            int deltaX = 0;
            int deltaY;

            int startX = GetAbsoluteX(startShelving, start);
            int startY = GetAbsoluteY(startShelving, start);
            int endX = GetAbsoluteX(endShelving, end);
            int endY = GetAbsoluteY(endShelving, end);

            if (startShelving == endShelving)
            {
                //same shelving
                if (start.Side == end.Side)
                {
                    //same side
                    deltaY = Math.Abs(startY - endY);
                }
                else
                {
                    //different side, calculate shortest walk.
                    deltaX = SHELVING_WIDTH;
                    deltaY = CalculateBestYDistanceInShelvingRow(start.Row, end.Row);
                }
            }
            else if (startShelving.Y == endShelving.Y)
            {
                //distinct shelvings but on the same warehouse row
                if (Math.Abs(startX - endX) == SHELVING_PADDING)
                {
                    //neighbor shelvings, but also contiguous side
                    deltaX = SHELVING_PADDING;
                    deltaY = Math.Abs(startY - endY);
                }
                else
                {
                    //any other combination of shelvings but in the same warehouse row
                    deltaX = Math.Abs(startX - endX);
                    deltaY = CalculateBestYDistanceInShelvingRow(start.Row, end.Row);
                }
            }
            else
            {
                //shelvings on different warehouse rows
                deltaX = Math.Abs(startX - endX);
                deltaY = Math.Abs(startY - endY);
            }
            return deltaX + deltaY;
        }

        public static int CalculateDistanceToTravel(Trolley trolley)
        {
            int distance = 0;
            WarehouseLocation previousLocation = trolley.Location;
            TrolleyStep nextElement = trolley.NextElement;
            while (nextElement != null)
            {
                distance += CalculateDistance(previousLocation, nextElement.GetLocation());
                previousLocation = nextElement.GetLocation();
                nextElement = nextElement.NextElement;
            }
            distance += CalculateDistance(previousLocation, trolley.Location);
            return distance;
        }

        private static int CalculateBestYDistanceInShelvingRow(int startY, int endY)
        {
            int northDirectionDistance = startY + endY;
            int southDirectionDistance = (SHELVING_HEIGHT - startY) + (SHELVING_HEIGHT - endY);
            return Math.Min(northDirectionDistance, southDirectionDistance);
        }

        /**
         * Calculates the absolute X position of a location considering the warehouse structure and the shelving where it's
         * contained.
         */
        private static int GetAbsoluteX(Shelving shelving, WarehouseLocation location)
        {
            if (location.Side == Shelving.Side.LEFT)
            {
                return shelving.X;
            }
            else
            {
                return shelving.X + SHELVING_WIDTH;
            }
        }

        /**
         * Calculates the absolute Y position of a location considering the warehouse structure and the shelving where it's
         * contained.
         */
        private static int GetAbsoluteY(Shelving shelving, WarehouseLocation location)
        {
            return shelving.Y + location.Row;
        }
    }
}