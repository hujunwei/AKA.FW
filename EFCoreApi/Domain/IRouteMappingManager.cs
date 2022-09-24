using EFCoreApi.DTOs;

namespace EFCoreApi.Domain;

public interface IRouteMappingManager
{
    Task<RouteMappingDto> GetRouteMappingByIdForUser(Guid id);
    Task<IEnumerable<RouteMappingDto>> ListRouteMappingsForUser();
    Task<IEnumerable<RouteMappingDto>> ListOfficialRouteMappings();
    Task<RouteMappingDto> AddRouteMapping(RouteMappingDto routeMappingDto);
    Task<RouteMappingDto> UpdateRouteMapping(RouteMappingDto routeMappingDto);
    Task DeleteRouteMapping(Guid id);
    Task<RouteMappingDto> FindRouteMappingBySourceAlias(string sourceAlias);
}