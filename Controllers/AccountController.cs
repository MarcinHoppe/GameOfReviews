using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Reviews.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> logger;

        public AccountController(ILogger<AccountController> logger)
        {
            this.logger = logger;
        }

        public async Task Login(string returnUrl = "/")
        {
            logger.LogInformation("Login");
            await HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties { RedirectUri = returnUrl });
        }

        [Authorize]
        public async Task Logout()
        {
            logger.LogInformation("Logout");
            await HttpContext.SignOutAsync("Auth0", new AuthenticationProperties { RedirectUri = Url.Action("Index", "Home") });
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}