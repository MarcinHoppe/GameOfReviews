using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reviews.Models;

namespace Reviews.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(ProductDatabase.AllProducts());
        }

        [AllowAnonymous]
        public IActionResult Reviews(string id)
        {
            logger.LogInformation("Searching for product {0}", id);
            
            var product = ProductDatabase.FindProduct(id);
            if (product == null)
            {
                logger.LogWarning("Product {0} not found", id);
                return NotFound();
            }

            logger.LogInformation("Displaying {0}", product.Name);
            return View(product);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Remove(string id)
        {
            return RedirectToAction("Reviews", new { id = id });
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public IActionResult Remove(string id, [FromForm] string reviewId)
        {
            logger.LogInformation("Removing review {0} from product {1}", reviewId, id);
            
            var product = ProductDatabase.FindProduct(id);
            if (product == null)
            {
                logger.LogWarning("Product {0} not found", id);
                return NotFound();
            }

            var removed = product.RemoveReview(reviewId);
            logger.LogInformation("Successfully removed review {0} from product {1}: {2}", id, reviewId, removed);

            return RedirectToAction("Reviews", new { id = id });
        }

        [Authorize(Roles = "Employee, User")]
        [HttpPost]
        public IActionResult Add(string id, [FromForm] string review)
        {
            logger.LogInformation("Adding review from user {0} from product {1}", User.Identity.Name, id);
            
            var product = ProductDatabase.FindProduct(id);
            if (product == null)
            {
                logger.LogWarning("Product {0} not found", id);
                return NotFound();
            }

            product.AddReview(review, User.Identity.Name);
            logger.LogInformation("Successfully added review `{0}` from user {1} to product {2}", review, User.Identity.Name, id);

            return RedirectToAction("Reviews", new { id = id });
        }
    }
}
