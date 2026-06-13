using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Npgsql;
using RoyalVilla.Application.Shared.Tx;

namespace RoyalVilla.Infrastructure.Tx;

public sealed class TxRunner(NpgsqlDataSource dataSource, ILogger<TxRunner> logger) : ITxRunner
{
    public async Task<T> ExecuteAsync<T>(
        Func<DbSession, CancellationToken, Task<T>> executor,
        CancellationToken ctn
    )
    {
        await using var conn = await dataSource.OpenConnectionAsync(ctn);
        await using var tx = await conn.BeginTransactionAsync(ctn);
        try
        {
            var result = await executor(new DbSession(conn, tx), ctn);

            await tx.CommitAsync(ctn);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during transaction");

            await tx.RollbackAsync(ctn);
            throw;
        }
    }

    public async Task ExecuteAsync(
        Func<DbSession, CancellationToken, Task> action,
        CancellationToken ctn
    )
    {
        await using var conn = await dataSource.OpenConnectionAsync(ctn);
        await using var tx = await conn.BeginTransactionAsync(ctn);
        try
        {
            await action(new DbSession(conn, tx), ctn);

            await tx.CommitAsync(ctn);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during transaction");

            await tx.RollbackAsync(ctn);
            throw;
        }
    }
}
