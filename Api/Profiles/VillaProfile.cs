using AutoMapper;
using RoyalVilla.Api.Dto;
using RoyalVilla.Api.Repositories;

namespace RoyalVilla.Api.Profiles;

public class VillaProfile : Profile
{
    public VillaProfile()
    {
        CreateMap<VillaRow, VillaData>();
    }
}