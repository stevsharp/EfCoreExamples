

using Castle.Core.Resource;

using ConsolefEfcoreloading.Model;

using Microsoft.EntityFrameworkCore;

try
{
    using (var context = new AppDbContext())
    {

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var customer = new Customer
        {
            Name = "John Doe",
            Address = new Address
            {
                Street = "123 Main St",
                City = "New York",
                Country = "USA"
            },
            Orders = new List<Order>
        {
            new Order { OrderNumber = "ORD123" },
            new Order { OrderNumber = "ORD124" }
        }
        };

        context.Customers.Add(customer);
        context.SaveChanges();
    }


    // Lazy-loaded automatically

    Console.WriteLine($"Lazy-loaded automatically");

    using (var context = new AppDbContext(true))
    {

        var order = context.Orders.First();

        var customer = order.Customer;

        Console.WriteLine($"Customer Name: {customer.Name}");

    }

    Console.WriteLine($"Use Eager Loading");

    //  Use Eager Loading
    using (var context = new AppDbContext(false))
    {
        var order = context.Orders
            .Include(o => o.Customer) 
            .First();

        Console.WriteLine($"Customer Name: {order.Customer.Name}");
    }

    //  Use Explicit Loading
    Console.WriteLine($"Use Explicit Loading");
    using (var context = new AppDbContext(false))
    {
        var order = context.Orders.First();

        context.Entry(order).Reference(o => o.Customer).Load();

        var customer = order.Customer;

        Console.WriteLine($"Customer Name: {customer.Name}");
    }

}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

Console.WriteLine("\nPress any key to exit...");