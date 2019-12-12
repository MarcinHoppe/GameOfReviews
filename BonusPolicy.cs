using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Reviews.Models;

namespace Reviews
{
    public class RemoveProductPermission : IAuthorizationRequirement
    {
    }

    public class RemoveProductPolicy : AuthorizationHandler<RemoveProductPermission, Product>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                        RemoveProductPermission requirement,
                                                        Product resource)
        {
            if (resource.Owner == context.User.Identity.Name)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}