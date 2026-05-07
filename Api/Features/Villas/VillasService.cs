using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.MakerInterfaces;
using RoyalVilla.Api.Annotations;
using RoyalVilla.Api.Features.Villas.RequestData;
using RoyalVilla.Api.Features.Villas.ResponseData;

namespace RoyalVilla.Api.Features.Villas;

/// <summary>
/// Villas CRUD Operations service
/// </summary>
[AutoRegisterService]
public sealed class VillasService(VillasRepository repo) : IService
{
    /// <summary>
    /// Get all villas with paging
    /// </summary>
    /// <param name="pageQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IEnumerable<VillaData>> GetVillasWithPagingAsync(
        PageQuery pageQuery,
        CancellationToken cancellationToken = default
    )
    {
        var rows = await repo.GetVillasWithPagingAsync(pageQuery, cancellationToken);

        return rows.Select(r => r.ToVillaData());
    }

    /// <summary>
    /// Get Villa Data by Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<VillaData?> GetVillaByIdAsync(int id, CancellationToken cancellationToken)
    {
        var row = await repo.GetVillaByIdAsync(id, cancellationToken);
        return row?.ToVillaData();
    }

    /// <summary>
    /// Create new villa data
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<VillaData> CreateVillaAsync(
        CreateOrUpdateVillaDto dto,
        CancellationToken cancellationToken
    )
    {
        var row = await repo.CreateVillaAsync(dto, cancellationToken);
        return row.ToVillaData();
    }

    /// <summary>
    /// Update existing villa data
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<VillaData?> UpdateVillaByIdAsync(
        int id,
        CreateOrUpdateVillaDto dto,
        CancellationToken cancellationToken
    )
    {
        var row = await repo.UpdateVillaAsync(id, dto, cancellationToken);
        return row?.ToVillaData();
    }

    /// <summary>
    /// Delete existing villa data
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<VillaData?> DeleteVillaByIdAsync(int id, CancellationToken cancellationToken)
    {
        var row = await repo.DeleteVillaAsync(id, cancellationToken);
        return row?.ToVillaData();
    }
}
