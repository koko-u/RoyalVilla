using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RoyalVilla.Api.Annotations;
using RoyalVilla.Api.Dto;

namespace RoyalVilla.Api.Repositories;

/// <summary>
/// Villas Table Repository
/// </summary>
/// <param name="conn"></param>
/// <param name="logger"></param>
[AutoRegisterService]
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

    /// <summary>
    /// Get villa information by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Create new Villa data
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    public async Task<VillaRow> CreateVilla(CreateVillaDto dto, CancellationToken cancellationToken = default)
    {
        var sql = await File.ReadAllTextAsync("Sql/Villas/insert_one.sql", cancellationToken);
        await conn.OpenAsync(cancellationToken);
        var cmd = new CommandDefinition(
            commandText: sql,
            parameters: dto,
            cancellationToken: cancellationToken
        );
        var created = await conn.QuerySingleAsync<VillaRow>(cmd);
        
        return created;
    }
}