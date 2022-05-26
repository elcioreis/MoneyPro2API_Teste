using MoneyPro2.Models;
using System.Security.Claims;

namespace MoneyPro2.Extensions;
public static class RoleClaimsExtension
{
    public static IEnumerable<Claim> GetClaims(this User user)
    {
        var result = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
         };

        result.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Slug)));

        return result;
    }
}