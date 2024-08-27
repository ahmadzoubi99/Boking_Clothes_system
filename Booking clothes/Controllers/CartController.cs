using Booking_clothes.Data;
using Booking_clothes.IService;
using Booking_clothes.Models;
using Booking_clothes.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Booking_clothes.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly MyContext context;
        public CartController(MyContext context, ICartService _cartService)
        {
            this.context = context;
            this._cartService = _cartService;
        }

        // GET: Cart
        public IActionResult Index()
        {

            var cartItems = _cartService.GetCart();
            var totalAmount = _cartService.GetCart().Sum(item => item.Price * item.NumberOfDaysRent + item.Price * 0.5m);
            ViewBag.totalAmount = totalAmount;
            return View(cartItems);
        }

        // POST: Cart/AddToCart
        [HttpPost]
        public IActionResult AddToCart(int productId, string size, string color, int quantity, DateTime startDate, DateTime endDate, int NumberOfDaysRent)
        {

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            // Ideally, you would retrieve the product from the database using the productId
            var product = context.Products.Where(p => p.Id == productId).FirstOrDefault();
            var differenceInDays = (endDate - startDate).TotalDays;
     
            if (product != null)
            {
                
                _cartService.AddToCart(product, size, startDate, endDate, (int)differenceInDays);
                var cart = _cartService.GetCart();
                var countOfItem = cart.Count();
                HttpContext.Session.SetInt32("countOfItem", countOfItem);
                ViewBag.Count = countOfItem;
                return RedirectToAction("shop","Home");
            }

            return NotFound();
        }

        // POST: Cart/RemoveFromCart
        [HttpPost]
        public IActionResult RemoveFromCart(int productId, string size, string color)
        {
            _cartService.RemoveFromCart(productId, size, color);
            return RedirectToAction("Index");
        }

        // POST: Cart/ClearCart
        [HttpPost]
        public IActionResult ClearCart()
        {
            _cartService.ClearCart();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult IncreaseQuantity(int productId, string color, string size, DateTime startDate, DateTime endDate)
        {
            var product = context.Products.Where(p => p.Id == productId).FirstOrDefault();
            if (product != null)
            {
                _cartService.AddToCart(product, size, startDate, endDate, 1);
                var cart = _cartService.GetCart();
            }

            return RedirectToAction("Index");
        }
        public IActionResult Checkout()
        {
            ViewBag.totalAmount = HttpContext.Session.GetString("totalAmount");

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var cartItems = _cartService.GetCart();

            return View(cartItems);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(string CVV, string CardNumber, string cardHolder, DateTime expiryDate)
        {
            var accountInBank = await context.Banck
                .Where(b => b.CVV == CVV
                       /*     b.CardHolder == cardHolder&&
                            b.CardNumber==CardNumber*/
                )
                .FirstOrDefaultAsync();

            if (accountInBank != null)
            {
                var totalAmount = _cartService.GetCart().Sum(item => item.Price * item.NumberOfDaysRent + item.Price * 0.5m);
                ViewBag.totalAmount= totalAmount;
                if (accountInBank.Balance >= totalAmount)
                {
                    try
                    {
                        accountInBank.Balance -= totalAmount;
                        context.Update(accountInBank);

                        var addres = context.Address.Where(a => a.Id == 1).FirstOrDefault();
                        var reservation = new Reservation
                        {
                            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                            TotalAmount = totalAmount,
                            Status = "Pending",
                            Address = addres,
                            ReservationDetails = _cartService.GetCart().Select(cartItem => new ReservationDetail
                            {
                                ClothId = cartItem.ProductId,
                                Quantity = cartItem.Quantity,
                                StartReservationDate = cartItem.StartDate,
                                EndReservationDate = cartItem.endDate,
                            }).ToList()
                        };

                        context.Reservations.Add(reservation);
                        await context.SaveChangesAsync();

                        _cartService.ClearCart();

                        return Json(new { success = true, message = "Reservation successful!" });
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BankExists(accountInBank.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Insufficient balance." });
                }
            }
            else
            {
                return Json(new { success = false, message = "Visa card information is incorrect." });
            }
        }


        private bool BankExists(int id)
        {
            return context.Banck.Any(e => e.Id == id);
        }

        /*
                [HttpPost]
                public IActionResult DecreaseQuantity(int productId)
                {
                    var cart = _cartService.GetCart();
                    var cartItem = cart.FirstOrDefault(item => item.ProductId == productId);
                    if (cartItem != null && cartItem.Quantity > 1)
                    {
                        cartItem.Quantity--;
                        _cartService.SaveCart(cart);

                        var countOfItem = cart.Count();
                        HttpContext.Session.SetInt32("countOfItem", countOfItem);
                        ViewBag.Count = countOfItem;
                    }
                    else
                    {
                        _cartService.RemoveFromCart(productId);
                    }
                    return RedirectToAction("Index");
                }*/


        /*  private readonly ICartService _cartService;
          private readonly MyContext _context; // Assuming MyContext is your DbContext

          public CartController(ICartService cartService, MyContext context)
          {
              _cartService = cartService;
              _context = context;
          }

          // Displays the cart
          public IActionResult Index()
          {
              var cartItems = _cartService.GetCartItems();
              var cartTotal = _cartService.GetCartTotal();

              var viewModel = new CartViewModel
              {
                  Items = cartItems,
                  TotalAmount = cartTotal
              };

              return View(viewModel);
          }

          // Adds an item to the cart with optional reservation dates, size, and color
          public IActionResult AddToCart(int productId, int quantity, string size, string color, DateTime? startDate, DateTime? endDate)
          {
              var product = GetProductById(productId);

              if (product == null)
              {
                  return NotFound();
              }

              var cartItem = new CartItem
              {
                  ProductId = product.Id,
                  ProductName = product.Name,
                  Price = product.PricePerDay,
                  Quantity = quantity,
                  Image = product.Image1, // Assuming Image1 is the main image
                  Size = size,
                  Color = color,
                  StartDate = startDate,
                  EndDate = endDate
              };

              _cartService.AddToCart(cartItem, startDate, endDate);

              return RedirectToAction("Index");
          }

          // Removes an item from the cart
          public IActionResult RemoveFromCart(int productId, string size, string color)
          {
              _cartService.RemoveFromCart(productId, size, color);
              return RedirectToAction("Index");
          }

          // Clears the entire cart
          public IActionResult ClearCart()
          {
              _cartService.ClearCart();
              return RedirectToAction("Index");
          }

          // Dummy method to get product details (replace with your actual data access method)
          private Products GetProductById(int productId)
          {
              return _context.Products
                  .FirstOrDefault(p => p.Id == productId);
          }*/
    }
}
