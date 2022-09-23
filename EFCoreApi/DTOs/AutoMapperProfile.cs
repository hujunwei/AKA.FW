using AutoMapper;
using EFDataAccess.Model;
using EFIdentityFramework.Model;

namespace EFCoreApi.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Identity
        CreateMap<User, UserDto>();
    }
}