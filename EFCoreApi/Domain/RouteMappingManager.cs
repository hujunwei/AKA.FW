using System.Security.Authentication;
using AutoMapper;
using EFCoreApi.DTOs;
using EFDataAccess.DataAccess.Accessors;
using EFDataAccess.Model;
using EFIdentityFramework.Model;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Utilities;

namespace EFCoreApi.Domain;

public class RouteMappingManager : IRouteMappingManager
{
    private readonly UserManager<User> _userManager;
    private readonly IRouteMappingAccessor _routeMappingAccessor;
    private readonly IMapper _mapper;
    private readonly IValidator<RouteMappingDto> _routeMappingDtoValidator;

    public RouteMappingManager(
        UserManager<User> userManager,
        IRouteMappingAccessor routeMappingAccessor,
        IMapper mapper,
        IValidator<RouteMappingDto> routeMappingDtoValidator)
    {
        _userManager = userManager;
        _routeMappingAccessor = routeMappingAccessor;
        _mapper = mapper;
        _routeMappingDtoValidator = routeMappingDtoValidator;
    }

    public async Task<RouteMappingDto> GetRouteMappingByIdForUser(Guid id)
    {
        var routeMapping = await _routeMappingAccessor.GetById(id);
        Exception<EntityNotFoundException>.ThrowOn(() => routeMapping == null, $"RouteMapping with id:{id} not found.");
        
        var currentUser = await getCurrentUser();
        Exception<InvalidOperationException>.ThrowOn(() => currentUser.Id.ToString() != routeMapping?.CreatedBy, $"RouteMapping with id:{id} does not belongs to current sign-in user.");    
        
        return _mapper.Map<RouteMappingDto>(routeMapping);
    }

    // NOTE: Pagination is taken care of by front-end.
    public async Task<IEnumerable<RouteMappingDto>> ListRouteMappingsForUser()
    {
        var currentUser = await getCurrentUser();
        var mappings = await _routeMappingAccessor.List(mapping => currentUser.Id.ToString() == mapping.CreatedBy);

        return _mapper.Map<IEnumerable<RouteMappingDto>>(mappings);
    }
    
    // NOTE: Pagination is taken care of by front-end.
    public async Task<IEnumerable<RouteMappingDto>> ListOfficialRouteMappings()
    {
        var mappings = await _routeMappingAccessor.List(mapping => mapping.IsOfficial);

        return _mapper.Map<IEnumerable<RouteMappingDto>>(mappings);
    }

    public async Task<RouteMappingDto> AddRouteMapping(RouteMappingDto routeMappingDto)
    {
        await _routeMappingDtoValidator.ValidateAsync(routeMappingDto);

        // TODO: Add index because we are querying by targetUrl & alias
        var existMappingsWithSameTargetUrlOrAliasUrl = await _routeMappingAccessor.List(mapping =>
            mapping.TargetUrl.ToLower().Equals(routeMappingDto.TargetUrl.ToLower()) ||
            mapping.SourceAlias.ToLower().Equals(routeMappingDto.SourceAlias.ToLower()));

        var existOfficialMappingsWithSameTargetUrl = existMappingsWithSameTargetUrlOrAliasUrl.Where(mapping => 
            mapping.IsOfficial &&
            mapping.TargetUrl.Equals(routeMappingDto.TargetUrl, StringComparison.OrdinalIgnoreCase));
        Exception<InvalidOperationException>.ThrowOn(() => existOfficialMappingsWithSameTargetUrl.Any(), 
            "Cannot create the short alias link because there is an existing official alias link to the TargetUrl you provided.");
        
        var existOfficialMappingsWithSameAliasUrl = existMappingsWithSameTargetUrlOrAliasUrl.Where(mapping => 
            mapping.IsOfficial &&
            mapping.SourceAlias.Equals(routeMappingDto.SourceAlias, StringComparison.OrdinalIgnoreCase));
        Exception<InvalidOperationException>.ThrowOn(() => existOfficialMappingsWithSameAliasUrl.Any(), 
            "Cannot create the short alias link because there is an existing official alias link to the SourceAlias you provided.");
        
        var currentUser = await getCurrentUser();
        var currentUserRoles = await _userManager.GetRolesAsync(currentUser);
        Exception<InvalidOperationException>.ThrowOn(() => 
            routeMappingDto.IsOfficial && 
            !currentUserRoles.Any(role => role.Equals("admin", StringComparison.OrdinalIgnoreCase)), 
            "Cannot create official alias link as current sign-in user is not admin.");

        var existMappingsForUser = existMappingsWithSameTargetUrlOrAliasUrl.Where(mapping => 
            (mapping.TargetUrl.Equals(routeMappingDto.TargetUrl, StringComparison.OrdinalIgnoreCase) || mapping.SourceAlias.Equals(routeMappingDto.SourceAlias, StringComparison.OrdinalIgnoreCase)) &&
            mapping.CreatedBy.Equals(currentUser.Id.ToString(), StringComparison.OrdinalIgnoreCase));
        Exception<InvalidOperationException>.ThrowOn(() => existMappingsForUser.Any(), "Alias link already exists on your list.");

        var RouteMapping = new RouteMapping
        {
            Name = routeMappingDto.Name,
            SourceAlias = routeMappingDto.SourceAlias,
            TargetUrl = routeMappingDto.TargetUrl,
            IsActive = routeMappingDto.IsActive,
            IsOfficial = routeMappingDto.IsOfficial,
            CreatedBy = currentUser.Id.ToString(),
            UpdatedBy = currentUser.Id.ToString(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdRouteMapping = await _routeMappingAccessor.Add(RouteMapping);

        return _mapper.Map<RouteMappingDto>(createdRouteMapping);
    }

    public async Task<RouteMappingDto> UpdateRouteMapping(RouteMappingDto routeMappingDto)
    {
        await _routeMappingDtoValidator.ValidateAsync(routeMappingDto);
        
        var existMappingsWithSameTargetUrlOrAliasUrl = await _routeMappingAccessor.List(mapping =>
            mapping.TargetUrl.ToLower().Equals(routeMappingDto.TargetUrl.ToLower()) ||
            mapping.SourceAlias.ToLower().Equals(routeMappingDto.SourceAlias.ToLower()));

        var existOfficialMappingsWithSameTargetUrl = existMappingsWithSameTargetUrlOrAliasUrl.Where(mapping => 
            mapping.IsOfficial &&
            mapping.TargetUrl.Equals(routeMappingDto.TargetUrl, StringComparison.OrdinalIgnoreCase));
        Exception<InvalidOperationException>.ThrowOn(() => existOfficialMappingsWithSameTargetUrl.Any(), 
            "Cannot update the short alias link because there is an existing official alias link to the TargetUrl you provided.");
        
        var existOfficialMappingsWithSameAliasUrl = existMappingsWithSameTargetUrlOrAliasUrl.Where(mapping => 
            mapping.IsOfficial &&
            mapping.SourceAlias.Equals(routeMappingDto.SourceAlias, StringComparison.OrdinalIgnoreCase));
        Exception<InvalidOperationException>.ThrowOn(() => existOfficialMappingsWithSameAliasUrl.Any(), 
            "Cannot update the short alias link because there is an existing official alias link to the SourceAlias you provided.");

        var currentUser = await getCurrentUser();
        var currentUserRoles = await _userManager.GetRolesAsync(currentUser);
        Exception<InvalidOperationException>.ThrowOn(() => 
                routeMappingDto.IsOfficial && 
                !currentUserRoles.Any(role => role.Equals("admin", StringComparison.OrdinalIgnoreCase)), 
            "Cannot update official alias link as current sign-in user is not admin.");

        var existingRouteMapping = await _routeMappingAccessor.GetById(routeMappingDto.Id);
        Exception<InvalidOperationException>.ThrowOn(() => existingRouteMapping == null,
            "Cannot update RouteMapping because unable find existing RouteMapping entry in database.");
        
        existingRouteMapping!.Id = existingRouteMapping.Id;
        existingRouteMapping.Name = string.IsNullOrWhiteSpace(routeMappingDto.Name)
            ? existingRouteMapping.Name
            : routeMappingDto.Name;
        existingRouteMapping.SourceAlias =
            string.IsNullOrWhiteSpace(routeMappingDto.SourceAlias)
                ? existingRouteMapping.SourceAlias
                : routeMappingDto.SourceAlias;
        existingRouteMapping.TargetUrl =
            string.IsNullOrWhiteSpace(routeMappingDto.TargetUrl)
                ? existingRouteMapping.TargetUrl
                : routeMappingDto.TargetUrl;
        existingRouteMapping.IsActive = routeMappingDto.IsActive;
        existingRouteMapping.UpdatedAt = DateTime.UtcNow;
        existingRouteMapping.UpdatedBy = currentUser.Id.ToString();

        var updatedRouteMapping = await _routeMappingAccessor.Update(existingRouteMapping);

        return _mapper.Map<RouteMappingDto>(updatedRouteMapping);
    }

    public async Task DeleteRouteMapping(Guid id)
    {
        var existing = await _routeMappingAccessor.GetById(id);
        Exception<InvalidOperationException>.ThrowOn(() => existing == null,
            $"Cannot delete RouteMapping with Id:{id} because the entity not found");
        
        var currentUser = await getCurrentUser();
        var currentUserRoles = await _userManager.GetRolesAsync(currentUser);
        Exception<InvalidOperationException>.ThrowOn(() => 
                existing?.IsOfficial == true && 
                !currentUserRoles.Any(role => role.Equals("admin", StringComparison.OrdinalIgnoreCase)), 
            "Cannot delete official alias link as current sign-in user is not admin.");

        await _routeMappingAccessor.Delete(existing!);
    }

    // TODO: Maybe move to a custom authorize attribute or utility class?
    private async Task<User> getCurrentUser()
    {
        // We make sure that upn should be the email, which implies user.email and user.name are duplicates.
        // Since we took shortcut of using EF Identity's table and model, live with duplicates now is most convenient.
        var upn = ServiceRuntimeContext.CurrentUserClaims?.Identity?.Name;
        Exception<AuthenticationException>.ThrowOn(() => string.IsNullOrEmpty(upn), $"User is not authenticated.");
        
        var currentUser = await _userManager.FindByNameAsync(upn);
        Exception<AuthenticationException>.ThrowOn(() => currentUser == null, $"User does not exist.");
        
        return currentUser;
    }
}