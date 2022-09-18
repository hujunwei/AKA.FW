using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Utilities.Authentication;

public class JwtTokenIssuer : IJwtTokenIssuer
{
    private readonly IConfiguration _configuration;

    public JwtTokenIssuer(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string IssueJwtToken(IEnumerable<Claim> claims)
    {
        var key = _configuration["JWT:SigningKey"];
        var expirationInDays = int.Parse(_configuration["JWT:ExpireDays"]);

        var signingKeyInBytes = Encoding.UTF8.GetBytes(key);
        var expires = DateTime.UtcNow.AddDays(expirationInDays);

        var signingKey = new SymmetricSecurityKey(signingKeyInBytes);
        var credential = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new JwtSecurityToken(claims: claims, expires: expires, signingCredentials: credential);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}