using AutoMapper;
using EFDataAccess.Model;
using EFIdentityFramework.Model;

namespace EFCoreApi.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // TODO: Convert User UID to UPN
        CreateMap<RouteMapping, RouteMappingDto>();
        // TODO: Convert User UPN to UID reverse
        
        // Identity
        CreateMap<User, UserDto>();
    }
}