using Microsoft.AspNetCore.Mvc;

namespace Booking_clothes.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}
