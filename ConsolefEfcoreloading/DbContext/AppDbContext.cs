using ConsolefEfcoreloading.Model;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Order> Orders { get; set; }

    private readonly bool _useLazyLoading;

    public AppDbContext(bool useLazyLoading = false)
    {
        _useLazyLoading = useLazyLoading;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var builder = options.UseSqlite("Data Source=mydb.db");

        // Conditionally enable lazy loading
        if (_useLazyLoading)
        {
            builder.UseLazyLoadingProxies();
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Customer>()
            .HasOne(c => c.Address)
            .WithOne(a => a.Customer)
            .HasForeignKey<Address>(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId);
    }
}
