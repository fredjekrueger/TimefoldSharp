using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using TimefoldSharp.Core.API.Solver;
using TimefoldSharp.Core.Config.Solver;
using TimefoldSharp.Core.Constraints.Streams.Bavet.Common.Index;
using TimefoldSharp.Examples.Orderpicking.Orderpicking.Domain;
using TimefoldSharp.Examples.Orderpicking.Orderpicking.Solver;

namespace TimefoldSharp.Examples.Orderpicking
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateLogger();

            var config = new SolverConfig()
            .WithSolutionClass(typeof(OrderPickingSolution))
            .WithEntityClasses(typeof(TrolleyStep))
            .WithConstraintProviderClass(typeof(OrderPickingConstraintProvider))
            .WithTerminationSpentLimit(TimeSpan.FromSeconds(1000));

            SolverFactory solverFactory = SolverFactory.Create(config);
            OrderPickingSolution problem = GenerateDemoData();

            Solver solver = solverFactory.BuildSolver();
            OrderPickingSolution solution = (OrderPickingSolution)solver.Solve(problem);

            Console.ReadLine();
        }

        private static WarehouseLocation START_LOCATION = new WarehouseLocation(Shelving.NewShelvingId('A', '1'), Shelving.Side.LEFT, 0);
        private static int TROLLEYS_COUNT = 5;
        private static int BUCKET_COUNT = 4;
        private static int BUCKET_CAPACITY = 60 * 40 * 20;
        private static int ORDERS_COUNT = 8;
        private static int ORDER_ITEMS_SIZE_MINIMUM = 1;
        public enum ProductFamily
        {
            GENERAL_FOOD,
            FRESH_FOOD,
            MEET_AND_FISH,
            FROZEN_PRODUCTS,
            FRUITS_AND_VEGETABLES,
            HOUSE_CLEANING,
            DRINKS,
            SNACKS,
            PETS
        }

        private static OrderPickingSolution GenerateDemoData()
        {
            List<Trolley> trolleys = BuildTrolleys(TROLLEYS_COUNT, BUCKET_COUNT, BUCKET_CAPACITY, START_LOCATION);
            List<Order> orders = BuildOrders(ORDERS_COUNT);
            List<TrolleyStep> trolleySteps = BuildTrolleySteps(orders);
            return new OrderPickingSolution(trolleys, trolleySteps);
        }

        public static List<TrolleyStep> BuildTrolleySteps(List<Order> orders)
        {
            List<TrolleyStep> result = new List<TrolleyStep>();
            foreach (var order in orders)
            {
                result.AddRange(BuildTrolleySteps(order));
            }
            return result;
        }

        public static List<TrolleyStep> BuildTrolleySteps(Order order)
        {
            List<TrolleyStep> steps = new List<TrolleyStep>();
            foreach (var item in order.Items)
            {
                TrolleyStep trolleyStep = new TrolleyStep(item);
                steps.Add(trolleyStep);
            }
            return steps;
        }

        public static List<Trolley> BuildTrolleys(int size, int bucketCount, int bucketCapacity, WarehouseLocation startLocation)
        {
            List<Trolley> result = new List<Trolley>(size);
            for (int i = 1; i <= size; i++)
            {
                result.Add(new Trolley(i.ToString(), bucketCount, bucketCapacity, startLocation));
            }
            return result;
        }

        public static List<Order> BuildOrders(int size)
        {
            List<Product> products = BuildProducts();
            return BuildOrders(size, products);
        }

        static long shiftID = 0;
        static double counterD = 0.0;
        static double GetFixedNumberD()
        {
            if (counterD >= 1.0)
                counterD = 0;
            counterD += 0.1f;
            return counterD;
        }

        static int counter = 0;
        static int GetFixedNumber(int max)
        {
            counter += 1;
            if (counter >= max)
                counter = 0;
            return counter;
        }

        private static List<Order> BuildOrders(int size, List<Product> products)
        {
            List<Order> orderList = new List<Order>();
            Order order;
            for (int orderNumber = 1; orderNumber <= size; orderNumber++)
            {
                int orderItemsSize = ORDER_ITEMS_SIZE_MINIMUM + GetFixedNumber(products.Count - ORDER_ITEMS_SIZE_MINIMUM);
                List<OrderItem> orderItems = new List<OrderItem>();
                HashSet<string> orderProducts = new HashSet<string>();
                order = new Order(orderNumber.ToString(), orderItems);
                int itemNumber = 1;
                for (int i = 0; i < orderItemsSize; i++)
                {
                    int productItemIndex = GetFixedNumber(products.Count);
                    Product product = products[productItemIndex];
                    if (!orderProducts.Contains(product.ID))
                    {
                        orderItems.Add(new OrderItem((itemNumber++).ToString(), order, product));
                        orderProducts.Add(product.ID);
                    }

                }
                orderList.Add(order);
            }
            return orderList;
        }

        private static Dictionary<ProductFamily, List<string>> SHELVINGS_PER_FAMILY = new Dictionary<ProductFamily, List<string>>
            {
                { ProductFamily.FRUITS_AND_VEGETABLES, new List<string>() { Shelving.NewShelvingId('A', '1'), Shelving.NewShelvingId('A', '2') } },
                { ProductFamily.FRESH_FOOD, new List<string>() { Shelving.NewShelvingId('A', '3') } },
                { ProductFamily.MEET_AND_FISH, new List<string>() { Shelving.NewShelvingId('B', '2'), Shelving.NewShelvingId('B', '3') } },
                { ProductFamily.FROZEN_PRODUCTS, new List<string>() { Shelving.NewShelvingId('B', '2'), Shelving.NewShelvingId('B', '1') } },
                { ProductFamily.DRINKS, new List<string>() { Shelving.NewShelvingId('D', '1') } },
                { ProductFamily.SNACKS, new List<string>() { Shelving.NewShelvingId('D', '2') } },
                { ProductFamily.GENERAL_FOOD, new List<string>() { Shelving.NewShelvingId('B', '2'), Shelving.NewShelvingId('C', '3'), Shelving.NewShelvingId('D', '2'), Shelving.NewShelvingId('D', '3') } },
                { ProductFamily.HOUSE_CLEANING, new List<string>() { Shelving.NewShelvingId('E', '2'), Shelving.NewShelvingId('E', '1') } },
                { ProductFamily.PETS, new List<string>() { Shelving.NewShelvingId('E', '3') } }
            };

        private static Random random = new Random(37);

        private static List<Product> BuildProducts()
        {
            return PRODUCTS.Select(productFamilyPair => {
                List<string> shelvingIds = SHELVINGS_PER_FAMILY[productFamilyPair.GetFamily()];
                int shelvingIndex = GetFixedNumber(shelvingIds.Count);
                Shelving.Side shelvingSide = (Shelving.Side)GetFixedNumber(Enum.GetValues(typeof(Shelving.Side)).Length);
                int shelvingRow = GetFixedNumber(Shelving.ROWS_SIZE) + 1;
                WarehouseLocation warehouseLocation = new WarehouseLocation(shelvingIds[shelvingIndex], shelvingSide, shelvingRow);
                return new Product(productFamilyPair.GetProduct().ID,
                    productFamilyPair.GetProduct().Name,
                    productFamilyPair.GetProduct().Volume,
                    warehouseLocation);
            }).ToList();
        }

        public class ProductFamilyPair
        {
            Product product;
            ProductFamily family;

            public ProductFamilyPair(Product product, ProductFamily family)
            {
                this.product = product;
                this.family = family;
            }

            public Product GetProduct()
            {
                return product;
            }

            public ProductFamily GetFamily()
            {
                return family;
            }
        }

        private static List<ProductFamilyPair> PRODUCTS = new List<ProductFamilyPair>() {
           new ProductFamilyPair(new Product(NextId(), "Kelloggs Cornflakes", 30 * 12 * 35, null), ProductFamily.GENERAL_FOOD),
           new ProductFamilyPair(new Product(NextId(), "Cream Crackers", 23 * 7 * 2, null), ProductFamily.GENERAL_FOOD),

           new ProductFamilyPair(new Product(NextId(), "Tea Bags 240 packet", 2 * 6 * 15, null), ProductFamily.GENERAL_FOOD),
           new ProductFamilyPair(new Product(NextId(), "Tomato Soup Can", 10 * 10 * 10, null), ProductFamily.GENERAL_FOOD),
           new ProductFamilyPair(new Product(NextId(), "Baked Beans in Tomato Sauce", 10 * 10 * 10, null), ProductFamily.GENERAL_FOOD),

           new ProductFamilyPair(new Product(NextId(), "Classic Mint Sauce", 8 * 10 * 8, null), ProductFamily.GENERAL_FOOD),
           new ProductFamilyPair(new Product(NextId(), "Raspberry Conserve", 8 * 10 * 8, null), ProductFamily.GENERAL_FOOD),
           new ProductFamilyPair(new Product(NextId(), "Orange Fine Shred Marmalade", 7 * 8 * 7, null), ProductFamily.GENERAL_FOOD),

           new ProductFamilyPair(new Product(NextId(), "Free Range Eggs 6 Pack", 15 * 10 * 8, null), ProductFamily.FRESH_FOOD),
           new ProductFamilyPair(new Product(NextId(), "Mature Cheddar 400G", 10 * 9 * 5, null), ProductFamily.FRESH_FOOD),
           new ProductFamilyPair(new Product(NextId(), "Butter Packet", 12 * 5 * 5, null), ProductFamily.FRESH_FOOD),

           new ProductFamilyPair(new Product(NextId(), "Iceberg Lettuce Each", 2500, null), ProductFamily.FRUITS_AND_VEGETABLES),
           new ProductFamilyPair(new Product(NextId(), "Carrots 1Kg", 1000, null), ProductFamily.FRUITS_AND_VEGETABLES),
           new ProductFamilyPair(new Product(NextId(), "Organic Fair Trade Bananas 5 Pack", 1800, null), ProductFamily.FRUITS_AND_VEGETABLES),
           new ProductFamilyPair(new Product(NextId(), "Gala Apple Minimum 5 Pack", 25 * 20 * 10, null), ProductFamily.FRUITS_AND_VEGETABLES),
           new ProductFamilyPair(new Product(NextId(), "Orange Bag 3kg", 29 * 20 * 15, null), ProductFamily.FRUITS_AND_VEGETABLES),

           new ProductFamilyPair(new Product(NextId(), "Fairy Non Biological Laundry Liquid 4.55L", 5000, null), ProductFamily.HOUSE_CLEANING),
           new ProductFamilyPair(new Product(NextId(), "Toilet Tissue 8 Roll White", 50 * 20 * 20, null), ProductFamily.HOUSE_CLEANING),
           new ProductFamilyPair(new Product(NextId(), "Kitchen Roll 200 Sheets x 2", 30 * 30 * 15, null), ProductFamily.HOUSE_CLEANING),
           new ProductFamilyPair(new Product(NextId(), "Stainless Steel Cleaner 500Ml", 500, null), ProductFamily.HOUSE_CLEANING),
           new ProductFamilyPair(new Product(NextId(), "Antibacterial Surface Spray", 12 * 4 * 25, null), ProductFamily.HOUSE_CLEANING),

           new ProductFamilyPair(new Product(NextId(), "Beef Lean Steak Mince 500g", 500, null), ProductFamily.MEET_AND_FISH),
           new ProductFamilyPair(new Product(NextId(), "Smoked Salmon 120G", 150, null), ProductFamily.MEET_AND_FISH),
           new ProductFamilyPair(new Product(NextId(), "Steak Burgers 454G", 450, null), ProductFamily.MEET_AND_FISH),
           new ProductFamilyPair(new Product(NextId(), "Pork Cooked Ham 125G", 125, null), ProductFamily.MEET_AND_FISH),
           new ProductFamilyPair(new Product(NextId(), "Chicken Breast Fillets 300G", 300, null), ProductFamily.MEET_AND_FISH),

           new ProductFamilyPair(new Product(NextId(), "6 Milk Bricks Pack", 22 * 16 * 21, null), ProductFamily.DRINKS),
           new ProductFamilyPair(new Product(NextId(), "Milk Brick", 1232, null), ProductFamily.DRINKS),
           new ProductFamilyPair(new Product(NextId(), "Skimmed Milk 2.5L", 2500, null), ProductFamily.DRINKS),
           new ProductFamilyPair(new Product(NextId(), "3L Orange Juice", 3 * 1000, null), ProductFamily.DRINKS),

           new ProductFamilyPair(new Product(NextId(), "Alcohol Free Beer 4 Pack", 30 * 15 * 30, null), ProductFamily.DRINKS),
           new ProductFamilyPair(new Product(NextId(), "Pepsi Regular Bottle", 1000, null), ProductFamily.DRINKS),
           new ProductFamilyPair(new Product(NextId(), "Pepsi Diet 6 x 330ml", 35 * 12 * 12, null), ProductFamily.DRINKS),

           new ProductFamilyPair(new Product(NextId(), "Schweppes Lemonade 2L", 2000, null), ProductFamily.DRINKS),
           new ProductFamilyPair(new Product(NextId(), "Coke Zero 8 x 330ml", 40 * 12 * 12, null), ProductFamily.DRINKS),
           new ProductFamilyPair(new Product(NextId(), "Natural Mineral Water Still 6 X 1.5Ltr", 6 * 1500, null), ProductFamily.DRINKS),

           new ProductFamilyPair(new Product(NextId(), "Cocktail Crisps 6 Pack", 20 * 10 * 10, null), ProductFamily.SNACKS)
   };

        private static long currentId = 0;

        static string NextId()
        {
            return (currentId++).ToString();
        }

        private static void CreateLogger()
        {
            Log.Logger = new LoggerConfiguration()
                                // add console as logging target
                                .WriteTo.Console()
                                // set default minimum level
                                .MinimumLevel.Debug()
                                .CreateLogger();
        }
    }
}