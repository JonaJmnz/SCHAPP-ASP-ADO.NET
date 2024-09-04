using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;

namespace SCHAPP.Authorization
{
    public class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var roleClaims = context.User.FindAll(ClaimTypes.Role);

                if (roleClaims.Any(claim => claim.Value == requirement.Role))
                {
                    context.Succeed(requirement);
                }
            }

            context.Fail();
        }
    }
}
