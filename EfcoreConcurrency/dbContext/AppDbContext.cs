using EfcoreConcurrency.Model;
using Microsoft.EntityFrameworkCore;


namespace EfcoreConcurrency.dbContext;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public DbSet<Product1> Products1 { get; set; }

    public DbSet<Account> Accounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Project1Db;Trusted_Connection=True;MultipleActiveResultSets=true;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Product (basic - no concurrency)
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(p => p.Stock)
                     .IsRequired()
                     .IsConcurrencyToken(); // Equivalent to [ConcurrencyCheck]
        });

        // Product1 (with concurrency token)
        modelBuilder.Entity<Product1>(entity =>
        {
            entity.ToTable("Products1");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(p => p.Stock)
                  .IsRequired();

            entity.Property(p => p.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken();
        });

        // Account
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Accounts");

            entity.HasKey(a => a.Id);

            entity.Property(a => a.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(a => a.Balance)
                  .IsRequired()
                  .HasColumnType("decimal(18,2)");
        });
    }

}
