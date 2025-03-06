namespace ConsolefEfcoreloading.Model;

public class Customer
{
    public int Id { get; set; }
    public string? Name { get; set; }

    // One-to-One relationship with Address
    public virtual Address Address { get; set; } = default!;

    // One-to-Many relationship with Orders
    public virtual List<Order> Orders { get; set; } = new();
}

public class Address
{
    public int Id { get; set; }
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Country { get; set; } = default!;

    // Foreign Key to Customer
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = default!;
}

public class Order
{
    public int Id { get; set; }
    public string? OrderNumber { get; set; }

    // Foreign Key to Customer
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = default!;
}
