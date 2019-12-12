using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Reviews.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Reviews.Utils;

namespace Reviews
{
    public static class Authorization
    {
        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options => {
                options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("https://gameofreviews.com/role", "Employee"));
                options.AddPolicy("ReviewRemovalPolicy", policy => policy.Requirements.Add(new RemoveReviewPermission()));
            });

            services.AddSingleton<IAuthorizationHandler, RemoveReviewPolicy>();
        }
    }

    public class RemoveReviewPermission : IAuthorizationRequirement
    {
    }

    public class RemoveReviewPolicy : AuthorizationHandler<RemoveReviewPermission>
    {
        private readonly ILogger<RemoveReviewPolicy> logger;
        private readonly IHttpContextAccessor httpAccessor;

        public RemoveReviewPolicy(ILogger<RemoveReviewPolicy> logger, IHttpContextAccessor httpAccessor)
        {
            this.logger = logger;
            this.httpAccessor = httpAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RemoveReviewPermission requirement)
        {
            if (context.User.IsInRole("Employee"))
            {
                context.Succeed(requirement);
            }

            if(UserIsReviewOwner(context))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private bool UserIsReviewOwner(AuthorizationHandlerContext context)
        {
            var httpContext = httpAccessor.HttpContext;

            if (httpContext.RetrieveRouteParameter("id", out var productId)  
                && httpContext.RetrieveFormField("reviewId", out var reviewId))
            {
                var product = ProductDatabase.FindProduct(productId);
                if (product == null)
                {
                    return false;
                }

                var review = product.FindReview(reviewId);
                if (review == null)
                {
                    return false;
                }

                return review.Author == context.User.Identity.Name;
            }

            return false;
        }
    }
}