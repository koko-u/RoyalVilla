using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RoyalVilla.Application.Features.Villas.Repositories;
using RoyalVilla.Application.Shared.Tx;
using RoyalVilla.Domain.Models;

namespace RoyalVilla.Application.Features.Villas.Services;

public sealed class VillasService(ITxRunner txRunner, IVillasRepository villasRepository)
{
    public async Task<IEnumerable<Villa>> GetAllAsync(CancellationToken ct)
    {
        return await txRunner.ExecuteAsync(villasRepository.GetAllAsync, ct);
    }
}
