using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Booking_clothes.Data;
using Booking_clothes.Models;
using System.Security.Claims;

namespace Booking_clothes.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MyContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;


        public ProductsController(MyContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }


        public async Task<IActionResult> ReviewsByProductId(int id)
        {
            var myContext = _context.Reviews.Include(o => o.Products).Where(p => p.ClothesId == id).Include(r=>r.User).ToListAsync();
            return View(await myContext);
        }
        // GET: Reservations

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
            var reservations = _context.ReservationDetails
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
            var Products = _context.Products.Where(p => p.Id == clothId).SingleOrDefault();
      
            ViewBag.ClothId = clothId;

            return View();
        }

/*        public IActionResult Reservation(int clothId)
        {
            DateTime today = DateTime.Today;
            DateTime endDate = today.AddMonths(2); // Show two months at a time

            // Fetch all reservation details for the specific ClothId within the date range
            var reservations = _context.ReservationDetails
                .Where(rd => rd.ClothId == 1 && rd.StartReservationDate <= endDate && rd.EndReservationDate >= today)
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
            var Products = _context.Products.Where(p => p.Id == clothId).SingleOrDefault();
            ViewBag.ProductName = Products.Name;
            ViewBag.ClothId = clothId;

            return View();
        }
*/


        // GET: Products
        public async Task<IActionResult> Index()
        {
            var myContext = _context.Products.Include(p => p.Category);
            return View(await myContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Color,CategoryId,PricePerDay,Availability,DiscountValue,Stock,CreatedAt,IsDeleted,ImageFile1,ImageFile2,ImageFile3")] Products products)
        {
            try
            {
                if (products.ImageFile1 != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(products.ImageFile1.FileName);
                    string path = Path.Combine(wwwRootPath, "Images", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await products.ImageFile1.CopyToAsync(fileStream);
                    }

                    products.Image1 = fileName;
                }
                if (products.ImageFile2 != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(products.ImageFile2.FileName);
                    string path = Path.Combine(wwwRootPath, "Images", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await products.ImageFile2.CopyToAsync(fileStream);
                    }

                    products.Image2 = fileName;
                }
                if (products.ImageFile3 != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(products.ImageFile3.FileName);
                    string path = Path.Combine(wwwRootPath, "Images", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await products.ImageFile3.CopyToAsync(fileStream);
                    }

                    products.Image3 = fileName;
                }

                _context.Add(products);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Product created successfully!" });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return Json(new { success = false, message = "An error occurred while creating the product. Please try again." });
            }
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", products.CategoryId);
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Color,CategoryId,PricePerDay,Availability,DiscountValue,Stock,Image1,Image2,Image3,CreatedAt,IsDeleted,ImageFile1,ImageFile2,ImageFile3")] Products products)
        {
            if (id != products.Id)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            try
            {
                if (products.ImageFile1 != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(products.ImageFile1.FileName);
                    string path = Path.Combine(wwwRootPath, "Images", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await products.ImageFile1.CopyToAsync(fileStream);
                    }

                    products.Image1 = fileName;
                }
                if (products.ImageFile2 != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(products.ImageFile2.FileName);
                    string path = Path.Combine(wwwRootPath, "Images", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await products.ImageFile2.CopyToAsync(fileStream);
                    }

                    products.Image2 = fileName;
                }
                if (products.ImageFile3 != null)
                {
                    string wwwRootPath = webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(products.ImageFile3.FileName);
                    string path = Path.Combine(wwwRootPath, "Images", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await products.ImageFile3.CopyToAsync(fileStream);
                    }

                    products.Image3 = fileName;
                }

                _context.Update(products);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Product updated successfully!" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(products.Id))
                {
                    return Json(new { success = false, message = "Product not found." });
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while updating the product." });
            }
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                ViewData["DeleteSuccess"] = false;
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                ViewData["DeleteSuccess"] = false;
                return NotFound();
            }

            if (Request.Method == "POST")
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                ViewData["DeleteSuccess"] = true;
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }
        public async Task<IActionResult> DeleteConfirmedReview(int id)
        {
            if (_context.Reviews == null)
            {
                return Json(new { success = false, message = "Entity set 'MyContext.Products' is null." });
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Review deleted successfully." });
            }

            return Json(new { success = false, message = "Review not found." });
        }


        // POST: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Json(new { success = false, message = "Entity set 'MyContext.Products' is null." });
            }

            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Product deleted successfully." });
            }

            return Json(new { success = false, message = "Product not found." });
        }

        [HttpPost]
        public async Task<IActionResult> SearchByProducttName(string? name)
        {
            var product = _context.Products.Include(p=>p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                product = product.Where(u => u.Name.Contains(name));
            }

            var products = await product.ToListAsync();


            return View("Index", products);
        }
        private bool ProductsExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult CreateError()
        {
            return View();
        }
        public IActionResult CreateSuccess()
        {
            return View();
        }

    }
}
