using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RoyalVilla.Application.Shared.Tx;
using RoyalVilla.Domain.Models;

namespace RoyalVilla.Application.Features.Villas.Repositories;

public interface IVillasRepository
{
    Task<IEnumerable<Villa>> GetAllAsync(DbSession session, CancellationToken ct);
}
