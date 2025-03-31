using EfcoreConcurrency.dbContext;

using Microsoft.EntityFrameworkCore;

using System;

class Program
{
    static async Task Main()
    {
        var success = await TryPurchaseProductAsync(1);
        Console.WriteLine(success ? "Purchase complete!" : "Out of stock or concurrency conflict.");

        var successForProduct = await TryPurchaseProductAsyncForProduct(1);
        Console.WriteLine(successForProduct ? "Purchase complete!" : "Out of stock or concurrency conflict.");

        var transferSuccess = await TryWithdrawAsync(1, 100);
        Console.WriteLine(transferSuccess ? "Withdrawal successful!" : "Insufficient funds or locked.");

        Console.WriteLine("\nPress any key to exit...");

        Console.ReadKey();
    }

    static async Task<bool> TryWithdrawAsync(int accountId, decimal amount)
    {
        using var context = new AppDbContext();
        using var transaction = await context.Database.BeginTransactionAsync();

        var account = await context.Accounts
            .FromSqlRaw("SELECT * FROM Accounts WITH (UPDLOCK, ROWLOCK) WHERE Id = {0}", accountId)
            .AsTracking()
            .FirstOrDefaultAsync();

        if (account == null || account.Balance < amount)
            return false;

        account.Balance -= amount;
        await context.SaveChangesAsync();
        await transaction.CommitAsync();

        return true;
    }


    static async Task<bool> TryPurchaseProductAsyncForProduct(int productId)
    {
        using var context = new AppDbContext();

        var product = await context.Products1.FindAsync(productId);

        if (product == null || product.Stock <= 0)
            return false;

        product.Stock--;

        try
        {
            await context.SaveChangesAsync(); // Uses RowVersion
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            // Conflict occurred — someone else updated it
            return false;
        }
    }


    static async Task<bool> TryPurchaseProductAsync(int productId)
    {
        using var context = new AppDbContext();

        var product = await context.Products.FindAsync(productId);

        if (product == null || product.Stock <= 0)
            return false;

        product.Stock--;

        try
        {
            await context.SaveChangesAsync(); // EF checks original Stock value
            Console.WriteLine("Purchase succeeded.");
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            Console.WriteLine("Purchase failed due to concurrency conflict.");
            return false;
        }
    }


}