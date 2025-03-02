using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main()
    {
        using var context = new AppDbContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Seed some data
        Console.WriteLine("Adding products...");
        context.Products.AddRange(
            new Product { Name = "Laptop", Price = 1200 },
            new Product { Name = "Mouse", Price = 25 }
        );
        context.SaveChanges();

        Console.WriteLine("\nAll Products (Before Soft Delete):");
        DisplayProducts();

        // Soft delete a product
        SoftDeleteProduct(1);

        Console.WriteLine("\nAll Products (After Soft Delete):");
        DisplayProducts();

        Console.WriteLine("\nAll Products (Including Soft Deleted):");
        DisplayAllProducts();

        Console.WriteLine("\nPress any key to exit...");
    }

    static void DisplayProducts()
    {
        using var context = new AppDbContext();
        var products = context.Products.ToList();
        foreach (var product in products)
        {
            Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.Price}");
        }
    }

    static void DisplayAllProducts()
    {
        using var context = new AppDbContext();
        var products = context.Products.IgnoreQueryFilters().ToList();
        foreach (var product in products)
        {
            Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: {product.Price}, IsDeleted: {product.IsDeleted}");
        }
    }

    static void SoftDeleteProduct(int productId)
    {
        using var context = new AppDbContext();
        var product = context.Products.Find(productId);
        if (product != null)
        {
            product.IsDeleted = true;
            context.SaveChanges();
            Console.WriteLine($"Product ID {productId} soft deleted.");
        }
    }
}
