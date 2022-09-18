using AutoMapper;
using EFCoreApi.DTOs;
using EFIdentityFramework.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using Utilities;

namespace EFCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserManager<User> userManager, IMapper mapper, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        // // GET: api/Users
        // [HttpGet]
        // public IEnumerable<string> Get()
        // {
        //     return new string[] { "value1", "value2" };
        // }
        //
        // // GET: api/Users/5
        // [HttpGet("{id}", Name = "Get")]
        // public string Get(int id)
        // {
        //     return "value";
        // }
        
        // POST: api/Users
        [HttpPost]
        public async Task<UserDto> CreateUser([FromBody] UserDto userDto)
        {
            // TODO: User validator, e.g UserName should be email.
            Exception<ArgumentNullException>.ThrowOn(() => userDto == null, "User cannot be null.");
            
            // TODO: Biz logic to manager
            var existingUser = await _userManager.FindByNameAsync(userDto.UserName);
            Exception<EntityAlreadyExistsException>.ThrowOn(() => existingUser != null, $"User with name: {userDto.UserName} already exists.");

            var user = new User
            {
                UserName = userDto.UserName,
                NickName = string.IsNullOrWhiteSpace(userDto.NickName) ? userDto.UserName : userDto.NickName,
                Email = userDto.Email,
                CreationTime = DateTime.UtcNow
            };
            
            var res = await _userManager.CreateAsync(user, userDto.Password);
            Exception<EntityUpdateException>.ThrowOn(() => res is not { Succeeded: true }, $"Cannot create userDto. Errors {res.Errors.ToJson()}");

            var createdUser = await _userManager.FindByNameAsync(userDto.UserName);

            return _mapper.Map<UserDto>(createdUser);
        }
        
        // // PUT: api/Users/5
        // [HttpPut("{id}")]
        // public void Put(int id, [FromBody] string value)
        // {
        // }
        //
        // // DELETE: api/Users/5
        // [HttpDelete("{id}")]
        // public void Delete(int id)
        // {
        // }
    }
}
