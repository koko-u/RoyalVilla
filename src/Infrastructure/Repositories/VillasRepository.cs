using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using KozLibraries.DapperSqlHelper;
using RoyalVilla.Application.Features.Villas.Repositories;
using RoyalVilla.Application.Shared.Tx;
using RoyalVilla.Domain.Models;
using RoyalVilla.Infrastructure.Mappers;
using RoyalVilla.Infrastructure.Rows;

namespace RoyalVilla.Infrastructure.Repositories;

public sealed class VillasRepository(SqlResource sql) : IVillasRepository
{
    public async Task<IEnumerable<Villa>> GetAllAsync(DbSession session, CancellationToken ct)
    {
        var (conn, tx) = session;
        var cmd = new CommandDefinition(
            commandText: await sql.GetAsync("villas/select_all.sql", ct),
            transaction: tx,
            cancellationToken: ct
        );
        var rows = await conn.QueryAsync<VillaRow>(cmd);

        return rows.Select(r => r.ToModel());
    }
}
