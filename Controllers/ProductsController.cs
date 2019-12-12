using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reviews.Models;

namespace Reviews.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            this.logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(ProductDatabase.AllProducts());
        }
    }
}
