using BulkUpdatesDeletes.dbContext;
using BulkUpdatesDeletes.Model;

using Microsoft.EntityFrameworkCore;

try
{
    using var context = new ApplicationDbContext();

    // Seed sample data
    if (!context.Users.Any())
    {
        context.Users.AddRange(
            new User { Name = "Spyros", Email = "Spyros@google.gr", IsInactive = false, LastLogin = DateTime.UtcNow.AddYears(-2) },
            new User { Name = "George", Email = "George@google.gr", IsInactive = false, LastLogin = DateTime.UtcNow.AddYears(-1).AddDays(-1) }
        );
        await context.SaveChangesAsync();
    }

    // Track changes manually before update
    var affectedUsers = await context.Users
        .Where(u => u.LastLogin < DateTime.UtcNow.AddYears(-1))
        .Select(u => new { u.Id, u.Name, OldValue = u.IsInactive })
        .ToListAsync();

    await context.Users
        .Where(u => u.LastLogin < DateTime.UtcNow.AddYears(-1))
        .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.IsInactive, true));

    await context.AuditLogs.AddRangeAsync(affectedUsers.Select(user => new AuditLog
    {
        EntityName = "User",
        EntityId = user.Id,
        ChangeType = "Updated",
        ChangedAt = DateTime.UtcNow,
        Description = $"User {user.Name} was updated: IsInactive {user.OldValue} → true"
    }));

    await context.SaveChangesAsync();

    Console.WriteLine("Audit completed.");
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

Console.WriteLine("\nPress any key to exit...");