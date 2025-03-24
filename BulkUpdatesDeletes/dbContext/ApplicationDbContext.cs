
using BulkUpdatesDeletes.Model;

using Microsoft.EntityFrameworkCore;

namespace BulkUpdatesDeletes.dbContext;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Project1Db;Trusted_Connection=True;MultipleActiveResultSets=true;");
    }
}
