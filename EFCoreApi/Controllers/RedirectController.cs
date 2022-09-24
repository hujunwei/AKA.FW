using EFCoreApi.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace EFCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RedirectController : ControllerBase
    {
        private readonly IRouteMappingManager _routeMappingsManager;

        public RedirectController(IRouteMappingManager routeMappingManager)
        {
            _routeMappingsManager = routeMappingManager;
        }

        // GET: api/Redirect/{sourceAlias}
        [HttpGet("{sourceAlias}")]
        public async Task<RedirectResult> RedirectTo(string sourceAlias)
        {
            Exception<ArgumentNullException>.ThrowOn(() => string.IsNullOrWhiteSpace(sourceAlias), $"Parameter {nameof(sourceAlias)} cannot be empty.");

            var routeMapping = await _routeMappingsManager.FindRouteMappingBySourceAlias(sourceAlias);

            return RedirectPermanent(routeMapping.TargetUrl);
        }
    }
}
