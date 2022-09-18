using AutoMapper;
using EFDataAccess.Model;
using EFIdentityFramework.Model;

namespace EFCoreApi.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Person, PersonDto>();
        CreateMap<Email, string>().ConvertUsing(a => a.Address);
        CreateMap<Address, string>().ConvertUsing(a => $"{a.StreetAddress}, {a.City}, {a.State}, {a.ZipCode}");
        
        // Identity
        CreateMap<User, UserDto>();
    }
}