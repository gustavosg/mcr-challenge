using System.Security.Claims;

namespace Application.Core.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid Id(this ClaimsPrincipal user)
        {

            if (user.Claims.ToList().Find(x => x.Type.Equals(ClaimTypes.NameIdentifier)) is Claim claim && Guid.TryParse(claim.Value, out Guid userId))
                return userId;

            return Guid.Empty;
        }

        
    }
}