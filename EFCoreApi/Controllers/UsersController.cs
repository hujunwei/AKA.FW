using AutoMapper;
using EFCoreApi.DTOs;
using EFIdentityFramework.Model;
using FluentValidation;
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
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly IValidator<UserDto> _userDtoValidator;

        public UsersController(UserManager<User> userManager, RoleManager<Role> roleManager, IValidator<UserDto> userDtoValidator, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userDtoValidator = userDtoValidator;
            _mapper = mapper;
        }

        // POST: api/Users
        // This will create a User and assign Role.User to him/her.
        // Called as Register by front-end.
        [HttpPost]
        public async Task<UserDto> CreateUser([FromBody] UserDto userDto)
        {
            // TODO: User validator, e.g UserName should be email.
            Exception<ArgumentNullException>.ThrowOn(() => userDto == null, "User cannot be null.");

            var result = await _userDtoValidator.ValidateAsync(userDto);
            Exception<ArgumentException>.ThrowOn(() => !result.IsValid, $"Validation error occurred. Error: {result.Errors.FirstOrDefault()}");
            
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
            var role = await _roleManager.FindByNameAsync("user");
            await _userManager.AddToRoleAsync(createdUser, role.Name);

            return _mapper.Map<UserDto>(createdUser);
        }
    }
}
