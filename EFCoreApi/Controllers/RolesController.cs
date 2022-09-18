using EFIdentityFramework.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using Utilities;

namespace EFCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RolesController> _logger;

        public RolesController(RoleManager<Role> roleManager, UserManager<User> userManager, ILogger<RolesController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }
        //
        // // GET: api/Roles
        // [HttpGet]
        // public IEnumerable<string> Get()
        // {
        //     return new string[] { "value1", "value2" };
        // }
        //
        // // GET: api/Roles/5
        // [HttpGet("{id}", Name = "Get")]
        // public string Get(int id)
        // {
        //     return "value";
        // }

        // POST: api/Roles
        [HttpPost]
        public async Task<Role> CreateRole([FromBody] Role role)
        {
            // TODO: Should put biz logic to a specific wrapper manager 
            Exception<ArgumentNullException>.ThrowOn(() => role == null || string.IsNullOrWhiteSpace(role.Name), $"Role name cannot be null.");
            
            var roleExists = await _roleManager.RoleExistsAsync(role.Name);
            Exception<EntityAlreadyExistsException>.ThrowOn(() => roleExists, $"Role with {role.Name} already exists.");

            var res = await _roleManager.CreateAsync(role);
            Exception<EntityUpdateException>.ThrowOn(() => res is not { Succeeded: true }, $"Cannot create role with name {role.Name}. Error: {res.Errors.ToJson()}");
            
            var createdRole = await _roleManager.FindByNameAsync(role.Name);

            return createdRole;
        }
        
        // POST: api/Roles/AddUserToRole/1/5
        [HttpPost]
        [Route("addusertorole/{roleId}/{userId}")]
        public async Task<IdentityResult> AddUserToRole(string roleId, string userId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            Exception<KeyNotFoundException>.ThrowOn(() => role == null, $"Role with Id {roleId} does not exists.");

            var user = await _userManager.FindByIdAsync(userId);
            Exception<KeyNotFoundException>.ThrowOn(() => user == null, $"User with Id {userId} does not exists.");

            var res = await _userManager.AddToRoleAsync(user, role.Name);

            return res;
        }

        // // PUT: api/Roles/5
        // [HttpPut("{id}")]
        // public void Put(int id, [FromBody] string value)
        // {
        // }
        //
        // // DELETE: api/Roles/5
        // [HttpDelete("{id}")]
        // public void Delete(int id)
        // {
        // }
    }
}
