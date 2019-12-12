using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Reviews
{
    public static class Authentication
    {
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    options.DefaultChallengeScheme =
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect("Auth0", options =>
            {
                options.Authority = $"https://{configuration["Auth0:DOMAIN"]}";
                options.ClientId = configuration["Auth0:CLIENT_ID"];
                options.ClientSecret = configuration["Auth0:CLIENT_SECRET"];
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.CallbackPath = new PathString("/callback");
                options.ClaimsIssuer = "Auth0";

                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "https://gameofreviews.com/role"
                };

                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProviderForSignOut = context => 
                    {
                        var logoutUri = $"https://{configuration["Auth0:DOMAIN"]}/v2/logout?client_id={configuration["Auth0:CLIENT_ID"]}";
                        var postLogoutUri = context.Properties.RedirectUri;
                        if (!string.IsNullOrEmpty(postLogoutUri))
                        {
                            if (postLogoutUri.StartsWith("/"))
                            {
                                // transform to absolute
                                var request = context.Request;
                                postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                            }
                            logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                        }

                        context.Response.Redirect(logoutUri);
                        context.HandleResponse();
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}