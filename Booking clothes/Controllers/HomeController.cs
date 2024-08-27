using Booking_clothes.Data;
using Booking_clothes.IService;
using Booking_clothes.Models;
using Booking_clothes.Service;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Booking_clothes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

		private readonly MyContext myContext;
/*        private readonly ProductService _productService;
*/
        public HomeController(ILogger<HomeController> logger,MyContext myContext)
        {
            _logger = logger;
            this.myContext = myContext;
        }



        public IActionResult ContactUS()
        {
            if(User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
            {
                ViewBag.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            var countOfItem = HttpContext.Session.GetInt32("countOfItem");
            ViewBag.Count = countOfItem;
            return View();
        }
        public IActionResult Index()
        {
            var products = myContext.Products.Include(c => c.Category).ToList();
            var categories = myContext.Categories.ToList();
            var testimonials = myContext.Testimonials.Include(u => u.User).Where(t => t.Status == "Approved").ToList();
            var model3 = Tuple.Create<IEnumerable<Category>, IEnumerable<Products>, IEnumerable<Testimonial>>(categories, products, testimonials);
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
            {
                ViewBag.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            var countOfItem = HttpContext.Session.GetInt32("countOfItem");
            ViewBag.Count = countOfItem;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = myContext.ApplicationUsers.Where(u => u.Id == userId).FirstOrDefault();

            return View("Index", model3);
            }


        public async Task<IActionResult> ProductDetails(int id, DateTime? startDate, DateTime? endDate)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
            {
                ViewBag.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var countOfItem = HttpContext.Session.GetInt32("countOfItem");
            ViewBag.Count = countOfItem;
            ViewBag.Id = id;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["UserId"] = userId;
            ViewBag.ProductId = id;

            var product = myContext.Products
                .Include(c => c.productSizes)
                .ThenInclude(cd => cd.Size)
                .FirstOrDefault(c => c.Id == id);

            // Check if the product exists
            if (product == null)
            {
                return NotFound();
            }

            // Calculate the number of reviews
            var countReview = myContext.Reviews.Where(r => r.ClothesId == id).Count();

            // Calculate the average rating
            if (countReview > 0)
            {
                var totalRating = myContext.Reviews.Where(r => r.ClothesId == id).Sum(r => r.Rating);
                ViewBag.AverageRating = (double)totalRating / countReview;
            }
            else
            {
                ViewBag.AverageRating = 0; // No reviews yet
            }

            ViewBag.CountReview = countReview;

            // Calculate the number of days between startDate and endDate
            if (startDate.HasValue && endDate.HasValue)
            {
                var differenceInDays = (endDate.Value - startDate.Value).TotalDays;
                ViewBag.DaysBetween = differenceInDays;
            }
            else
            {
                ViewBag.DaysBetween = "N/A"; // Handle cases where dates are not provided
            }

            // Handle nullable DateTime formatting
            ViewBag.StartDate = startDate.HasValue ? startDate.Value.ToString("yyyy/MM/dd") : "";
            ViewBag.EndDate = endDate.HasValue ? endDate.Value.ToString("yyyy/MM/dd") : "";

            return View(product);
        }


        public async Task<IActionResult> Shop()
		{
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
            {
                ViewBag.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var countOfItem = HttpContext.Session.GetInt32("countOfItem");
            ViewBag.Count = countOfItem;

            // Fetch all categories
            var categories = await myContext.Categories.ToListAsync();

            // Fetch products that have at least one size associated
            var productsWithSizes = await myContext.Products
                .Where(p => myContext.ProductSize.Any(ps => ps.ProductId == p.Id))
                .ToListAsync();

            var model = Tuple.Create<IEnumerable<Category>, IEnumerable<Products>>(categories, productsWithSizes);

            return View("Shop", model);
        }


            [HttpPost]
		public async Task<IActionResult> Reservations(DateTime startDate, DateTime endDate, int size, int quantity, int clothId)
		{
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
            {
                ViewBag.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var countOfItem = HttpContext.Session.GetInt32("countOfItem");
            ViewBag.Count = countOfItem;
            // Validate dates
            if (endDate <= startDate)
			{
				ModelState.AddModelError("", "End date must be after the start date.");
				var products = await myContext.Products
					.Include(c => c.productSizes)
					.ThenInclude(cd => cd.Size)
					.Where(c => !c.IsDeleted && c.Availability)
					.ToListAsync();

				return View(products); // Return all clothes with an error message
			}
            return View(); // Return all clothes with an error message

        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GetProductByCategory(int CategoryId)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
            {
                ViewBag.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var countOfItem = HttpContext.Session.GetInt32("countOfItem");
            ViewBag.Count = countOfItem;

            var product=myContext.Products.Include(c => c.Category).Include(c=>c.productSizes).Where(p=> p.Id==CategoryId).FirstOrDefault();
            return RedirectToAction("Shop", "Home");
        }

        public async Task<IActionResult> ProductByCategorie(int id)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
            {
                ViewBag.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var countOfItem = HttpContext.Session.GetInt32("countOfItem");
            ViewBag.Count = countOfItem;

            // Fetch categories
            var categories = await myContext.Categories.ToListAsync();

            // Fetch products by category that have at least one size associated
            var productsWithSizes = await myContext.Products
                .Where(p => p.CategoryId == id && myContext.ProductSize.Any(ps => ps.ProductId == p.Id))
                .ToListAsync();

            var model = Tuple.Create<IEnumerable<Category>, IEnumerable<Products>>(categories, productsWithSizes);

            return View("Shop", model);
        }

/*        public async Task<IActionResult> ProductByCategorie(int id)
		{
            var countOfItem = HttpContext.Session.GetInt32("countOfItem");
            ViewBag.Count = countOfItem; 

            var products = await myContext.Products.Where(p => p.CategoryId == id).ToListAsync();
			var categories = await myContext.Categories.ToListAsync();

			var model = Tuple.Create<IEnumerable<Category>, IEnumerable<Products>>(categories, products);

			return View("Shop", model);
		}*/
        public async Task<IActionResult> AllProductInAllCategorie()
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
            {
                ViewBag.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var countOfItem = HttpContext.Session.GetInt32("countOfItem");
            ViewBag.Count = countOfItem;

            // Fetch all categories
            var categories = await myContext.Categories.ToListAsync();

            // Fetch products that have at least one size associated
            var productsWithSizes = await myContext.Products
                .Where(p => myContext.ProductSize.Any(ps => ps.ProductId == p.Id))
                .ToListAsync();

            var model = Tuple.Create<IEnumerable<Category>, IEnumerable<Products>>(categories, productsWithSizes);

            return View("Shop", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContact(string Name, string Email, string Subject, string Message, DateTime CreatedAt)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
            {
                ViewBag.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var countOfItem = HttpContext.Session.GetInt32("countOfItem");
            ViewBag.Count = countOfItem;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var contactUs = new ContactUs
            {
                Name = Name,
                Email = Email,
                Subject = Subject,
                Message = Message,
                CreatedAt = CreatedAt,
                UserId = userId
            };

            try
            {
                myContext.Add(contactUs);
                await myContext.SaveChangesAsync();
                return Json(new { success = true, message = "Thank you for contacting us." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "There was an error submitting your message. Please try again later." });
            }
        }


        public IActionResult Reservation(int clothId)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
            {
                ViewBag.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var countOfItem = HttpContext.Session.GetInt32("countOfItem");
            ViewBag.Count = countOfItem; 

            DateTime today = DateTime.Today;
            DateTime endDate = today.AddMonths(2); // Show two months at a time

            // Fetch all reservation details for the specific ClothId within the date range
            var reservations = myContext.ReservationDetails
                .Where(rd => rd.ClothId == clothId && rd.StartReservationDate <= endDate && rd.EndReservationDate >= today)
                .GroupBy(rd => rd.ReservationId) // Group by ReservationId to get unique reservations
                .Select(g => new
                {
                    ReservationId = g.Key,
                    StartReservationDate = g.Min(rd => rd.StartReservationDate),
                    EndReservationDate = g.Max(rd => rd.EndReservationDate)
                })
                .ToList();

            // Calculate reserved dates
            var reservedDates = reservations
                .SelectMany(r => Enumerable.Range(0, 1 + (r.EndReservationDate - r.StartReservationDate).Days)
                .Select(offset => r.StartReservationDate.AddDays(offset)))
                .ToList();

            ViewBag.ReservedDates = reservedDates;
            var Products = myContext.Products.Where(p => p.Id == clothId).SingleOrDefault();
            ViewBag.ProductName = Products.Name;
            ViewBag.ClothId = clothId;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateReview(string UserId, int ProductId, int Rating, string ReviewText)
        {
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
            {
                ViewBag.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var review = new Review()
                {
                    UserId = userId,
                    ReviewText = ReviewText,
                    ClothesId = ProductId,
                    Rating = Rating,
                    CreatedAt = DateTime.Now
                };

                myContext.Add(review);
                await myContext.SaveChangesAsync();
                return Ok(new { success = true }); // Return a success response
            }

            return BadRequest(); // Return an error response
        }


        public async Task<IActionResult> SearchByProducttName(string? name)
		{
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
            {
                ViewBag.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var countOfItem = HttpContext.Session.GetInt32("countOfItem");
            ViewBag.Count = countOfItem;

            // Get all products
            var productQuery = myContext.Products.Include(c => c.Category).AsQueryable();

			// Filter by name if provided
			if (!string.IsNullOrEmpty(name))
			{
				productQuery = productQuery.Where(u => u.Name.Contains(name));
			}

			// Get the filtered products
			var filteredProducts = await productQuery.ToListAsync();

			// Get all categories
			var categories = await myContext.Categories.ToListAsync();

			// Create a tuple with categories and filtered products
			var model = Tuple.Create<IEnumerable<Category>, IEnumerable<Products>>(categories, filteredProducts);

			// Return the Shop view with the filtered model
			return View("Shop", model);
		}


        public IActionResult CTestimonial()
        {
            var count = HttpContext.Session.GetInt32("countOfItem");
            ViewBag.Count = HttpContext.Session.GetInt32("countOfItem");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CTestimonial([Bind("Id,UserId,TestimonialText,IsDeleted,CreatedAt,Status")] Testimonial testimonial)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = myContext.Users.Where(u => u.Id == userId).FirstOrDefault();

            testimonial.UserId = userId;
            testimonial.Status = "Pending";


            myContext.Add(testimonial);
            await myContext.SaveChangesAsync();

            // Redirect to the Index action
            return RedirectToAction(nameof(Index));


            // If the model state is not valid, set the ViewData and return the view
            ViewData["UserId"] = new SelectList(myContext.Users, "Id", "Id", testimonial.UserId);
            return View(testimonial);
        }

        private bool TestimonialExists(int id)
        {
            return (myContext.Testimonials?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
