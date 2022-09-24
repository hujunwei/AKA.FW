using EFCoreApi.Domain;
using EFCoreApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace EFCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RouteMappingsController : ControllerBase
    {
        private readonly IRouteMappingManager _routeMappingsManager;

        public RouteMappingsController(IRouteMappingManager routeMappingManager)
        {
            _routeMappingsManager = routeMappingManager;
        }

        // GET: api/routeMappings/official
        [HttpGet("official")]
        public async Task<IEnumerable<RouteMappingDto>> ListOfficialRouteMappings()
        {
            return await _routeMappingsManager.ListOfficialRouteMappings();
        }
        
        // GET: api/routeMappings/my
        [HttpGet("my")]
        public async Task<IEnumerable<RouteMappingDto>> ListRouteMappingsForUser()
        {
            return await _routeMappingsManager.ListRouteMappingsForUser();
        }

        // GET: api/routeMappings/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<RouteMappingDto> GetRouteMappingByIdForUser(string id)
        {
            return await _routeMappingsManager.GetRouteMappingByIdForUser(new Guid(id));
        }

        // POST: api/routeMappings
        [HttpPost]
        public async Task<RouteMappingDto> Post([FromBody] RouteMappingDto routeMappingDto)
        {
            Exception<ArgumentNullException>.ThrowOn(() => routeMappingDto == null, $"RouteMapping to be created cannot be null.");

            return await _routeMappingsManager.AddRouteMapping(routeMappingDto);
        }

        // PATCH: api/routeMappings/5
        [HttpPatch("{id}")]
        public async Task<RouteMappingDto> Update(string id, [FromBody] RouteMappingDto routeMappingDto)
        {
            Exception<ArgumentNullException>.ThrowOn(() => routeMappingDto == null, $"RouteMapping to be updated cannot be null.");
            
            routeMappingDto.Id = id;

            return await _routeMappingsManager.UpdateRouteMapping(routeMappingDto); 
        }

        // DELETE: api/routeMappings/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _routeMappingsManager.DeleteRouteMapping(new Guid(id));
        }
    }
}
