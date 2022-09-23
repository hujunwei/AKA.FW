## Summary

|          | **UsersModule**               | **MappingModule**                                  | **RedirectModule**                      | **Admin**Module                      |
| :------- | :---------------------------- | -------------------------------------------------- | --------------------------------------- | ------------------------------------ |
| Backend  | UserController/RoleController | MappingController                                  | RedirectController                      | [optional]                           |
| Frontend | Login/Register  Page          | List  View Table     My/Official/~~IHaveAccessTo~~ | [optional] Error page if not authorized | [optional] Manage Official urls page |



## Users Module

Use's login, Register, Identity Module powered by: **EF framework for Identity** (Ideally integrate with LDAP)



## Mapping Module

### MappingModel 

| Name            | Type       | Desc                                                      |
| --------------- | ---------- | --------------------------------------------------------- |
| Id              | GUID       | Entity Id                                                 |
| Name            | String     | [UI] Friendly Name for mapping                            |
| ~~Description~~ | ~~String~~ | ~~[Optional][UI] [UI] Desc for mapping~~                  |
| ShortUrl        | String     | [UI] The shortened url                                    |
| TargetUrl       | String     | [UI] The target url                                       |
| IsActive        | Boolean    | Control redirect is activated                             |
| IsOfficial      | Boolean    | Default false, only admin have update authority           |
| CreatedBy       | String     | uid                                                       |
| UpdatedBy       | String     | uid                                                       |
| CreatedAt       | DateTime   | Timestamp                                                 |
| UpdateAt        | DateTime   | Timestamp                                                 |
| ClientData      | String     | JSON data in case for some custom fields. Do not use now. |
| _rowVersion     | RowVersion | Etag, optimistic concurrency control                      |

### MappingAccessor

```c#
Mapping CreateMapping(string uid, string shortUrl, string targetUrl) {
	var createdMap = new RouteMapping { uid, shortUrl, targetUrl };

	Ctx.CoreMapping.Add(map);
	Ctx.SaveChanges();

 	return createdMap;
}
```

and R, U, D... on mapping table.

### MappingManager (Domain)

```c#
Mapping CreateMappingForUser(string uid, string shortUrl, string targetUrl) { 
  Exception<InvalidOperationException>.ThrowOn(() => PREDEFINED_URLS.Any(url => url == shortUrl), "Predefined URL cannot be used, please use another alias");
  
  var existOfficialMappings = mappingAccessor.Query<Mapping>(m => m.shortUrl == shortUrl && m.targetUrl == targetUrl && m.isOffical == true);
  Exception<InvalidOperationException>.ThrowOn(() => existOfficialMappings.Any(), nameof(existOfficialMappings), "Cannot create the short alias link because there is an existing official mapping.");
  
  // Real code only query once.
  var existMappingsForUser = mappingAccessor.Query<Mapping>(m => m.shortUrl == shortUrl && m.targetUrl == targetUrl && m.uid == currentuid);
  Exception<InvalidOperationException>.ThrowOn(() => existMappingsForUser.Any(), nameof(existMappingsForUser), "Alias link already exists on your list.");
  
  // NOTE: Adding this logic is disputable, but apparently this makes thing more complicate
  // We do not provide suggestion if <short, target> exists for other users, instead, duplicates controlled brutely by 100 maxium for a user
  //
  // Maybe a bad design for experience, instead error out, should allow user continue create?
  // But what if target is google.com? and we have 1000000 mappings for google?
  //
  // var existMappingsByTargetUrl = mappingAccessor.Query<Mapping>(m => m.targetUrl == targetUrl);
  // Exception<ConflictException>.ThrowOn(() => existMappingsByTargetUrl.Any(), nameof(existMappingsByTargetUrl), $"Target url already exist, you wanna try {existMappingsByTargetUrl}?");
  
 	return mappingAccessor.Create(uid, shortUrl, targetUrl);
}

Mapping CreateMappingForAdmin() {
  	// Check isUser has Admin Role
}

// used for CRUD portal
List<Mapping> ListMappingsForUser(string uid, ...) {
	// [Optional] with SKIP().TAKE() pagination logic, better in accessors.
  // ... with uids translated to upn from user managers.
}

List<Mapping> ListOfficialMappings(string uid, ...) {
	// [Optional] with SKIP().TAKE() pagination logic, better in accessors.
  // ... with uids translated to upn from user managers.
}

// used for Redirect module
Mapping FindMappingByShortUrlAndUser(string uid, ...) {
	// ... with SKIP().TAKE() pagination logic, better in accessors.
  // ... with uids translated to upn from user managers.
}

Mapping UpdateMappingForUser(string uid, ...) {
	// ... with SKIP().TAKE() pagination logic, better in accessors.
  // ... with uids translated to upn from user managers.
}

Mapping UpdateMappingForAdmin(string uid, ...) {
	// ... with SKIP().TAKE() pagination logic, better in accessors.
  // ... with uids translated to upn from user managers.
}

Void DeleteMappingForUser(string uid, ...) {
	// ... with SKIP().TAKE() pagination logic, better in accessors.
  // ... with uids translated to upn from user managers.
}

Void DeleteMappingForAdmin(string uid, ...) {
	// ... with SKIP().TAKE() pagination logic, better in accessors.
  // ... with uids translated to upn from user managers.
}

```

and U, D....

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
// GET: aka.freewheel/{short_url} 
// Q: how to do correct logic here? e.g. No login needed but protected by corp network?

// Do we need this? if yes, and this is biz logic so put in Domain managers
if (HttpContext.User.ClaimsPrincipal == null || User.IsNotRegistered()) {
	return BadRequest("You are not authorized to use this service");
}

var mapping = mappingManager.FindMappingByShortUrl(short_url);

if (mapping == null) {
	return BadRequest("No mapping");	
}

httpContext.Redirect(mapping.targetUrl);
```



## AdminModule

Used for CRUD official modules. Optional! In manager. Temporarily use an super admin account.



## UI

- Powered by Material Design React: https://www.creative-tim.com/learning-lab/react/quick-start/material-dashboard/.



## CI/CD

- **API** powered by Azure Web App. 
- **UI** powered by Azure Static Web.
- **Domain** powered by Godaddy.



## MISCs

- Central Package Management [Introducing Central Package Management - The NuGet Blog (microsoft.com)](https://devblogs.microsoft.com/nuget/introducing-central-package-management/)

