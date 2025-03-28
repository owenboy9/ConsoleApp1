using System;
using System.Collections.Generic;
using System.Linq;


namespace AssetTracker
{
    // Base class for all assets
    abstract class Asset
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Price { get; set; }

        // Constructor
        public Asset(string brand, string model, DateTime purchaseDate, decimal price)
        {
            Brand = brand;
            Model = model;
            PurchaseDate = purchaseDate;
            Price = price;
        }

        // Abstract method to return asset type (e.g., Computer, Phone)
        public abstract string GetAssetType();
    }

    // Computer class (inherits from Asset)
    class Computer : Asset
    {
        public string Type { get; set; } // Laptop or Desktop

        public Computer(string brand, string model, DateTime purchaseDate, decimal price, string type)
            : base(brand, model, purchaseDate, price)
        {
            Type = type;
        }

        public override string GetAssetType()
        {
            return "Computer";
        }
    }

    // Phone class (inherits from Asset)
    class Phone : Asset
    {
        public string OS { get; set; } // iOS, Android, etc.

        public Phone(string brand, string model, DateTime purchaseDate, decimal price, string os)
            : base(brand, model, purchaseDate, price)
        {
            OS = os;
        }

        public override string GetAssetType()
        {
            return "Phone";
        }
    }

    class Program
    {
        static void Main()
        {
            List<Asset> assets = new List<Asset>();

            // User input loop
            while (true)
            {
                Console.WriteLine("\nEnter asset type (Computer/Phone) or 'exit' to finish:");
                string type = Console.ReadLine()?.Trim().ToLower();

                if (type == "exit") break;

                Console.Write("Enter brand: ");
                string brand = Console.ReadLine();

                Console.Write("Enter model: ");
                string model = Console.ReadLine();

                Console.Write("Enter purchase date (yyyy-mm-dd): ");
                DateTime purchaseDate;
                while (!DateTime.TryParse(Console.ReadLine(), out purchaseDate))
                {
                    Console.Write("Invalid date format! Enter again (yyyy-mm-dd): ");
                }

                Console.Write("Enter price: ");
                decimal price;
                while (!decimal.TryParse(Console.ReadLine(), out price))
                {
                    Console.Write("Invalid price! Enter again: ");
                }

                if (type == "computer")
                {
                    Console.Write("Enter type (Laptop/Desktop): ");
                    string compType = Console.ReadLine();
                    assets.Add(new Computer(brand, model, purchaseDate, price, compType));
                }
                else if (type == "phone")
                {
                    Console.Write("Enter operating system (iOS/Android/etc.): ");
                    string os = Console.ReadLine();
                    assets.Add(new Phone(brand, model, purchaseDate, price, os));
                }
                else
                {
                    Console.WriteLine("Invalid asset type! Please enter 'Computer' or 'Phone'.");
                }
            }

            Console.WriteLine("\n--- Sorted Assets (By Type & Purchase Date) ---");
            DisplaySortedAssets(assets);
        }

        static void DisplaySortedAssets(List<Asset> assets)
        {
            // Sorting: First by type (Computers first), then by purchase date (ascending)
            var sortedAssets = assets.OrderBy(a => a.GetAssetType()).ThenBy(a => a.PurchaseDate).ToList();

            DateTime today = DateTime.Today;
            TimeSpan threeYears = TimeSpan.FromDays(3 * 365);
            TimeSpan threeMonthsBeforeThreeYears = TimeSpan.FromDays((3 * 365) - 90);

            foreach (var asset in sortedAssets)
            {
                bool isNearEndOfLife = (today - asset.PurchaseDate) >= threeMonthsBeforeThreeYears &&
                                       (today - asset.PurchaseDate) < threeYears;

                if (isNearEndOfLife)
                {
                    Console.ForegroundColor = ConsoleColor.Red; // Highlight in red
                }

                Console.WriteLine($"{asset.GetAssetType()} | {asset.Brand} {asset.Model} | Purchased: {asset.PurchaseDate.ToShortDateString()} | Price: ${asset.Price}");

                Console.ResetColor(); // Reset console color
            }
        }
    }
}



/*
class Program
{
    static void Main()
    {
        ProductManager productManager = new ProductManager();

        while (true)
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1 - Add Product");
            Console.WriteLine("2 - Display Products");
            Console.WriteLine("3 - Search for a Product");
            Console.WriteLine("4 - Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    productManager.AddProduct();
                    break;
                case "2":
                    productManager.DisplayProducts();
                    break;
                case "3":
                    Console.Write("Enter product name to search: ");
                    string searchQuery = Console.ReadLine();
                    productManager.SearchProduct(searchQuery);
                    break;
                case "4":
                    Console.WriteLine("Exiting program...");
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }
}

class ProductManager
{
    private List<Product> products = new List<Product>();

    public void AddProduct()
    {
        while (true)
        {
            Console.Write("\nEnter Category (or 'q' to return to menu): ");
            string category = Console.ReadLine();
            if (category.ToLower() == "q") break;

            Console.Write("Enter Product Name: ");
            string name = Console.ReadLine();

            decimal price;
            while (true)
            {
                Console.Write("Enter Price: ");
                if (decimal.TryParse(Console.ReadLine(), out price) && price > 0)
                    break;
                Console.WriteLine("Invalid price. Please enter a valid decimal (e.g., 12,33).");
            }

            products.Add(new Product(category, name, price));
            Console.WriteLine("Product added successfully!");
        }
    }

    public void DisplayProducts()
    {
        if (products.Count == 0)
        {
            Console.WriteLine("\nNo products in the list.");
            return;
        }

        var sortedProducts = products.OrderBy(p => p.Price).ToList();
        Console.WriteLine("\nProduct List (Sorted by Price):");

        foreach (var product in sortedProducts)
        {
            Console.WriteLine($"{product.Name} | {product.Category} | {product.Price:F2} SEK");
        }

        decimal totalPrice = sortedProducts.Sum(p => p.Price);
        Console.WriteLine($"Total Price: {totalPrice:F2} SEK");
    }

    public void SearchProduct(string query)
    {
        var foundProducts = products.Where(p => p.Name.Equals(query, StringComparison.OrdinalIgnoreCase)).ToList();

        if (foundProducts.Any())
        {
            Console.WriteLine("\nSearch Results:");
            foreach (var product in foundProducts)
            {
                Console.WriteLine($"{product.Name} | {product.Category} | {product.Price:F2} SEK");
            }
        }
        else
        {
            Console.WriteLine("\nProduct not found.");
        }
    }
}

class Product
{
    public string Category { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Product(string category, string name, decimal price)
    {
        Category = category;
        Name = name;
        Price = price;
    }
}

*/