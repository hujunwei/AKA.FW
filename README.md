

# AKA.FREEHEEL: A handy web app tool gives you straight-to-the-point experience for every freewheeler's everyday's work and life.

#### **AKA.Freewheel as a central company resource and information gathering and sharing place!**

- Freewheelers could browse any company resouces and information like wikis, internal tools, portals, events, notifications. Especially for new-comers who needs a lot of information and links.

#### **AKA.Freewheel as a work & life efficiency booster!**

- Create and manage your own alias, give it a short and memorable name, and pointing to and deep linking to any website, even google.com as long as you can imagine.
- Share links across email, chat without Edit Link button anymore.
- Make your browser bookmarks cleaner. No need to bookmark any official alias, especially let everyone bookmark something like 'wiki.freewheel.com'
- Attending meetings without hassle to prepare a lot links you want to show to your audience before the meeting, especially needed to browse chat history, email history, your bookmarks to gather all random links. All those are imprinted in your brain as you alias-ed them!
- For internal small tools, no need to do Domain Registration with DNS providers. Just use akafw.net!
- Needs to change your production URL but afraid to miss any customers in your notification? Just alias it like 'akafw.net/my_production_website'.

#### **AKA.Freewheel as the most-efficient advertising supportive tool ! **

- Have a new tool or website that you needs to annouce and advertise to your audience? Instead give them QR code that requires people unlock the phone, open some app... Just give them an Alias link. Like "akafw.net/mynewtool". Everyone can memorize your tool at the time they see it, and people more willing to open such easy to acessible tool!

  

## Implementation Summary

|          | **UsersModule**               | **MappingModule**         | **RedirectModule**             |
| :------- | :---------------------------- | ------------------------- | ------------------------------ |
| Backend  | UserController/RoleController | MappingController         | RedirectController             |
| Frontend | Login/Register  Page          | CRUD Table in My/Official | Canvas backdrop when redirects |



## Users Module

Use's login, Register, Identity Module powered by: **EF framework for Identity** (Ideally integrate with LDAP)

Core functionality:

- CreateUser
- CreateRole
- AssignRoleToUser
- Login (Issue JWT token and return UserInfo)



## Mapping Module

### MappingModel 

| Name        | Type       | Desc                                                      |
| ----------- | ---------- | --------------------------------------------------------- |
| Id          | GUID       | Entity Id                                                 |
| Name        | String     | [UI] Friendly Name for mapping                            |
| ShortUrl    | String     | [UI] The shortened url                                    |
| TargetUrl   | String     | [UI] The target url                                       |
| IsActive    | Boolean    | Control redirect is activated                             |
| IsOfficial  | Boolean    | Default false, only admin have update authority           |
| CreatedBy   | String     | uid                                                       |
| UpdatedBy   | String     | uid                                                       |
| CreatedAt   | DateTime   | Timestamp                                                 |
| UpdateAt    | DateTime   | Timestamp                                                 |
| ClientData  | String     | JSON data in case for some custom fields. Do not use now. |
| _rowVersion | RowVersion | Etag, optimistic concurrency control                      |



### MappingAccessor

Powered by abstration of EntityAccessor.cs, contains only CRUD with to SQL data table.



### MappingManager (Domain)

Core business logic, contains validation, logicial check for Mapping CRUD.

**Create:**

```c#
Exception<ArgumentException>.ThrowOn(
  () => !validationResult.IsValid, 
  $"Validation error occurred. Error: {validationResult.Errors.FirstOrDefault()}");

Exception<InvalidOperationException>.ThrowOn(
  () => existingMappingsWithSameSourceAlias.Any(), 
  "Cannot create the short alias link because the alias you provided had already been taken.");

Exception<InvalidOperationException>.ThrowOn(
  () => existOfficialMappingsWithSameTargetUrl.Any(), 
  "Cannot create the short alias link because there is an existing official alias link to the TargetUrl you provided.");

Exception<InvalidOperationException>.ThrowOn(
  () => routeMappingDto.IsOfficial && !currentUserRoles.Any(role => role.Equals("admin", StringComparison.OrdinalIgnoreCase)), 
  "Cannot create official alias link as current sign-in user is not admin.");
```

**Update:**

```c#
Exception<ArgumentException>.ThrowOn(
  () => !validationResult.IsValid, 
  $"Validation error occurred. Error: {validationResult.Errors.FirstOrDefault()}");
        
Exception<InvalidOperationException>.ThrowOn(
  () => routeMappingDto.IsOfficial && !currentUserRoles.Any(role => role.Equals("admin", StringComparison.OrdinalIgnoreCase)), 
  "Cannot update official alias link as current sign-in user is not admin.");
        
Exception<InvalidOperationException>.ThrowOn(
  () => existingMappingsWithSameSourceAlias.Any(), 
  "Cannot update the short alias link because the alias you provided had already been taken.");

Exception<InvalidOperationException>.ThrowOn(
  () => existOtherOfficialMappingsWithSameTargetUrl.Any(), 
  "Cannot update the short alias link because there is another existing official alias link to the TargetUrl you provided.");
        
Exception<InvalidOperationException>.ThrowOn(
  () => existingRouteMapping == null,
  "Cannot update RouteMapping because unable find existing RouteMapping entry in database.");
```

**Delete:**

```c#
Exception<InvalidOperationException>.ThrowOn(
  () => existing == null,
  $"Cannot delete RouteMapping with Id:{id} because the entity not found");
        
Exception<InvalidOperationException>.ThrowOn(
  () => existing?.IsOfficial == true && 
  !currentUserRoles.Any(role => role.Equals("admin", StringComparison.OrdinalIgnoreCase)), 
  "Cannot delete official alias link as current sign-in user is not admin.");
```



### MappingController 

```
[Authorize] POST: api/routemapping
[Authorize] GET: api/routemapping/official
[Authorize] GET: api/routemapping/my
[Authorize] GET: api/mapping/{id} (mapping details view, optional, like CRUD on person R/W access list) 
[Authorize] PATCH: api/routemapping 
[Authorize] DELETE: api/routemapping/{id} 
```



## RedirectModule

```c#
public async Task<RouteMappingDto> FindRouteMappingBySourceAlias(string sourceAlias)
{
  Expression<Func<RouteMapping, bool>> predicate = mapping => mapping.SourceAlias.ToLower().Equals(sourceAlias.ToLower());

  var matchedMappings = await _routeMappingAccessor.List(predicate);

  return _mapper.Map<RouteMappingDto>(matchedMappings.First());
}
```



## UI

- Powered by Material Design React: https://www.creative-tim.com/learning-lab/react/quick-start/material-dashboard/.



## CI/CD

- **API** powered by Azure Web App. 
- **UI** powered by Azure Static Web.
- **Domain Registration** powered by Namecheap.com.



## MISCs

- Live Project: https://akafw.net.

  - Start your first redirect: Type **akafw.net/bjoadmin** in browser address bar, this will directly to freewheel's beijing admin wiki page. 
  - Internal to freewheel, only email with freewheel.com/apac.freewheel.com can register and create his/her customized alias link.
  - Redirects requires **no** login/register. For example, if one create his own https://akafw,net/my_alias -> https://somewebsite.com/.../...?xxx=yyy, everyone uses this alias redirection rule.

- This repo does not have azure deploy files, but locally runable. If needed, please contact project owner.

  - For backend

    - Install .NET Core 6, Microsoft Sql Server
    - Clone repo
    - Restore nuget package
    - Build project
    - Update Local Database (Using Entity Framework Core with Code-First approach, update-database command will create a brand new database from scratch automatically) for EFDataAccess & EFIdentityFramework project.
    - Hit debug/run  VisualStudio or Rider to launch EFCoreAPI project.

  - For frontend

    **reactclient** is the base folder for frontend. Run `npm ci`, and then `npm start` to start the project

- For contributors: Nuget package directly addition by IDE may fail because of Central Package Management [Introducing Central Package Management - The NuGet Blog (microsoft.com).](https://devblogs.microsoft.com/nuget/introducing-central-package-management/)

​	
