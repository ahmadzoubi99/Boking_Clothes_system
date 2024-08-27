using Booking_clothes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Booking_clothes.Controllers
{
    public class AdminController : Controller
    {
        private readonly MyContext _context;
        public AdminController(MyContext context)
        {
            this._context = context;
                
        }
/*        public IActionResult UserAccount()
        {
            var user = _context.ApplicationUsers.FirstOrDefault();
            return View(user);
        }*/

        public IActionResult category()
        {
            return View();
        }
        public IActionResult Shop()
        {
            return View();
        }
        public IActionResult Index()
        {
            var totalSale = _context.Reservations.Sum(p => p.TotalAmount);
            ViewBag.totalSale = totalSale;

            ViewBag.productsCount = _context.Products.Count();
            ViewBag.userCount = _context.ApplicationUsers.Count();
            // Calculate the total sales for the last day
            var yesterday = DateTime.Now.Date.AddDays(-1);
            var today = DateTime.Now.Date.AddDays(1);
/*            var today = DateTime.Now.Date;
*/            var lastDaySales = _context.Reservations
                                       .Where(p => p.ReservationDate >= yesterday && p.ReservationDate < today)
                                       .Sum(p => (decimal?)p.TotalAmount) ?? 0;
            ViewBag.lastDaySales = lastDaySales;
            return View();
        }

        public async Task<IActionResult> UserAccount()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);

        }




    }
}
