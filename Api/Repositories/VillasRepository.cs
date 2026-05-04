using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace RoyalVilla.Api.Repositories;

public sealed class VillasRepository(NpgsqlConnection conn, ILogger<VillasRepository> logger) : IRepository
{
    /// <summary>
    /// Get all villas Information
    /// </summary>
    /// <returns>Villa information</returns>
    public async Task<IEnumerable<VillaRow>> GetAllVillasAsync(CancellationToken cancellationToken = default)
    {
        var sql = await File.ReadAllTextAsync("Sql/Villas/select_all.sql", cancellationToken);
        await conn.OpenAsync(cancellationToken);
        var rows = await conn.QueryAsync<VillaRow>(sql, cancellationToken);

        return rows;
    }

    public async Task<VillaRow?> GetVillaByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var sql = await File.ReadAllTextAsync("Sql/Villas/select_one.sql", cancellationToken);
        await conn.OpenAsync(cancellationToken);
        var cmd = new CommandDefinition(
            commandText: sql,
            parameters: new { id = id },
            cancellationToken: cancellationToken
        );
        var row = await conn.QuerySingleOrDefaultAsync<VillaRow>(cmd);

        return row;
    }
}