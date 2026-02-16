using System.Security.Claims;

namespace AgroSolutions.Property.API.Extensions;

public static class ClaimsExtensions
{
    extension(ClaimsPrincipal principal)
    {
        public int UserId => int.Parse(principal.Claims.First(c => c.Type == "UserId").Value);
    }
}
