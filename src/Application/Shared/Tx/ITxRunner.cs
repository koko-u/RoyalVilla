using System;
using System.Threading;
using System.Threading.Tasks;

namespace RoyalVilla.Application.Shared.Tx;

public interface ITxRunner
{
    Task<T> ExecuteAsync<T>(
        Func<DbSession, CancellationToken, Task<T>> executor,
        CancellationToken ct
    );

    Task ExecuteAsync(Func<DbSession, CancellationToken, Task> action, CancellationToken ct);
}
