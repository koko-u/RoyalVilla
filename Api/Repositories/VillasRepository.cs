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
/// <param name="dataSource"></param>
/// <param name="logger"></param>
[AutoRegisterService]
public sealed class VillasRepository(NpgsqlDataSource dataSource, ILogger<VillasRepository> logger) : IRepository
{
    /// <summary>
    /// Get all villas Information
    /// </summary>
    /// <returns>Villa information</returns>
    public async Task<IEnumerable<VillaRow>> GetAllVillasAsync(CancellationToken cancellationToken = default)
    {
        var sql = await File.ReadAllTextAsync("Sql/Villas/select_all.sql", cancellationToken);

        var conn = await dataSource.OpenConnectionAsync(cancellationToken);
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
        var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        var cmd = new CommandDefinition(
            commandText: sql,
            parameters: new { Id = id },
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
    public async Task<VillaRow> CreateVilla(CreateOrUpdateVillaDto dto, CancellationToken cancellationToken = default)
    {
        var sql = await File.ReadAllTextAsync("Sql/Villas/insert_one.sql", cancellationToken);
        var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        var cmd = new CommandDefinition(
            commandText: sql,
            parameters: dto,
            cancellationToken: cancellationToken
        );
        var created = await conn.QuerySingleAsync<VillaRow>(cmd);

        return created;
    }

    /// <summary>
    /// Update (override) Villa data
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<VillaRow?> UpdateVilla(int id, CreateOrUpdateVillaDto dto,
        CancellationToken cancellationToken = default)
    {
        var sql = await File.ReadAllTextAsync("Sql/Villas/update_by_id.sql", cancellationToken);
        var conn = await dataSource.OpenConnectionAsync(cancellationToken);
        var tx = await conn.BeginTransactionAsync(cancellationToken);
        try
        {
            var cmd = new CommandDefinition(
                commandText: sql,
                parameters: new
                {
                    Id = id,
                    dto.Name,
                    dto.Details,
                    dto.Rate,
                    dto.SquareFeet,
                    dto.Occupancy,
                    dto.ImageUrl
                },
                transaction: tx,
                cancellationToken: cancellationToken
            );
            var updated = await conn.QuerySingleOrDefaultAsync<VillaRow>(cmd);
            await tx.CommitAsync(cancellationToken);

            return updated;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error updating villa with ID {Id}", id);
            await tx.RollbackAsync(cancellationToken);
            return null;
        }
    }
}