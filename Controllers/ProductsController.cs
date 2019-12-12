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
            ProductDatabase.AddProduct(productName);
            return RedirectToAction("Index");
        }
    }
}
