using Microsoft.EntityFrameworkCore.Diagnostics;

using System.Data.Common;

namespace BulkUpdatesDeletes.Interceptor;

public class AuditInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        
        Console.WriteLine($"SQL Executed: {command.CommandText}");

        return base.ReaderExecuting(command, eventData, result);
    }
}
