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

        public IActionResult Index()
        {
            return View(ProductDatabase.AllProducts());
        }

        public IActionResult Comments(string id)
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

        [HttpPost]
        public IActionResult Remove(string id, [FromForm] string reviewId)
        {
            logger.LogInformation("Removing comment {0} from product {1}", reviewId, id);
            
            var product = ProductDatabase.FindProduct(id);
            if (product == null)
            {
                logger.LogWarning("Product {0} not found", id);
                return NotFound();
            }

            var removed = product.RemoveComment(reviewId);
            logger.LogInformation("Successfully removed comment {0} from product {1}: {2}", id, reviewId, removed);

            return RedirectToAction("Comments", new { id = id });
        }

        [HttpPost]
        public IActionResult Add(string id, [FromForm] string comment)
        {
            logger.LogInformation("Adding review from user {0} from product {1}", User.Identity.Name, id);
            
            var product = ProductDatabase.FindProduct(id);
            if (product == null)
            {
                logger.LogWarning("Product {0} not found", id);
                return NotFound();
            }

            product.AddReview(comment, User.Identity.Name);
            logger.LogInformation("Successfully added comment `{0}` from user {1} to product {2}", comment, User.Identity.Name, id);

            return RedirectToAction("Comments", new { id = id });
        }
    }
}
