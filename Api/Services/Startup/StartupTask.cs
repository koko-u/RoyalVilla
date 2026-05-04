using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace RoyalVilla.Api.Services.Startup;

/// <summary>
/// Application startup task
/// </summary>
public sealed class StartupTask(NpgsqlConnection conn, ILogger<StartupTask> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await conn.OpenAsync(cancellationToken);
        var version = await conn.QuerySingleAsync<string>("SELECT version()", cancellationToken);
        logger.LogInformation($"Application started with PostgreSQL version: {version}");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}