using Riok.Mapperly.Abstractions;
using RoyalVilla.Domain.Models;
using RoyalVilla.Infrastructure.Rows;

namespace RoyalVilla.Infrastructure.Mappers;

[Mapper(ThrowOnPropertyMappingNullMismatch = true)]
public static partial class VillaRowMapper
{
    public static partial Villa ToModel(this VillaRow row);
}
