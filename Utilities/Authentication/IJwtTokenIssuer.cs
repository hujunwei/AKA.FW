using System.Security.Claims;

namespace Utilities.Authentication;

public interface IJwtTokenIssuer
{
    string IssueJwtToken(IEnumerable<Claim> claims);
}