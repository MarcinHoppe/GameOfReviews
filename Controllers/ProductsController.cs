using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reviews.Models;

namespace Reviews.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> logger;
        private readonly IAuthorizationService authorizationService;

        public ProductsController(ILogger<ProductsController> logger, IAuthorizationService authorizationService)
        {
            this.logger = logger;
            this.authorizationService = authorizationService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(ProductDatabase.AllProducts());
        }

        [HttpGet]
        public IActionResult Add()
        {
            return RedirectToAction("Index");
        }

        [Authorize(Policy = "EmployeeOnly")]
        [HttpPost]
        public IActionResult Add([FromForm] string productName)
        {
            logger.LogInformation("Adding product {0}", productName);
            ProductDatabase.AddProduct(productName, User.Identity.Name);
            return RedirectToAction("Index");
        }

        [Authorize(Policy = "EmployeeOnly")]
        [HttpPost]
        public async Task<IActionResult> Remove(string id)
        {
            logger.LogInformation("Removing product {0}", id);

            var product = ProductDatabase.FindProduct(id);
            if (product == null)
            {
                logger.LogWarning("Product {0} not found", id);
                return NotFound();
            }

            var authorization = await authorizationService.AuthorizeAsync(User, product, "ProductRemovalPolicy");
            if (!authorization.Succeeded)
            {
                logger.LogError("User {0} cannot remove product {1}", User.Identity.Name, id);
                return Forbid();
            }

            ProductDatabase.RemoveProduct(id);

            return RedirectToAction("Index");
        }
    }
}
