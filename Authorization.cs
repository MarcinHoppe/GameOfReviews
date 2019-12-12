using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;

namespace Reviews
{
    public static class Authorization
    {
        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options => {
                options.AddPolicy("EmployeeOnly", EmployeeOnlyPolicy);
            });
        }

        public static void EmployeeOnlyPolicy(AuthorizationPolicyBuilder policy)
        {
            policy.RequireClaim("https://gameofreviews.com/role", "Employee");
        }
    }
}