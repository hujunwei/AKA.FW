using System.Security.Authentication;
using System.Security.Claims;
using AutoMapper;
using EFCoreApi.DTOs;
using EFCoreApi.Infra.Authentication;
using EFIdentityFramework.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Utilities;
using Utilities.Authentication;

namespace EFCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly IJwtTokenIssuer _jwtTokenIssuer;

        public LoginController(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper, IJwtTokenIssuer jwtTokenIssuer)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtTokenIssuer = jwtTokenIssuer;
        }

        [HttpPost]
        public async Task<LoginResponse> Login(LoginRequestPayload loginRequestPayload)
        {
            var user = await _userManager.FindByNameAsync(loginRequestPayload.UserName);
            Exception<KeyNotFoundException>.ThrowOn(() => user == null, $"User: {loginRequestPayload.UserName} does not exist.");

            var loginResult = await _userManager.CheckPasswordAsync(user, loginRequestPayload.Password);
            Exception<AuthenticationException>.ThrowOn(() => !loginResult, "Incorrect password.");

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            Exception<AuthenticationException>.ThrowOn(() => roles == null, $"User: {loginRequestPayload.UserName} has no roles assigned, please assign roles first.");

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            
            var jwtTokenForUser =  _jwtTokenIssuer.IssueJwtToken(claims);

            return new LoginResponse
            {
                UserInfo = _mapper.Map<UserDto>(user),
                Token = jwtTokenForUser
            };
        }
    }
}
