using AutoMapper;
using RoyalVilla.Api.Annotations;
using RoyalVilla.Api.Dto;
using RoyalVilla.Api.Repositories;

namespace RoyalVilla.Api.Profiles;

/// <summary>
/// Villa Data Conversion Profile
/// </summary>
[AutoRegisterService]
public class VillaProfile : Profile
{
    /// <summary>
    /// Default constructor for VillaProfile
    /// </summary>
    public VillaProfile()
    {
        CreateMap<VillaRow, VillaData>();
    }
}
