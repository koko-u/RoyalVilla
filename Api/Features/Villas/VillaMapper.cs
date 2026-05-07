using Riok.Mapperly.Abstractions;
using RoyalVilla.Api.Features.Villas.ResponseData;
using RoyalVilla.Api.Features.Villas.RowData;

namespace RoyalVilla.Api.Features.Villas;

/// <summary>
/// Mapper for converting between VillaRow and VillaData entities.
/// </summary>
[Mapper]
public static partial class VillaMapper
{
    /// <summary>
    /// Convert to VillaData
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    public static partial VillaData ToVillaData(this VillaRow row);
}
