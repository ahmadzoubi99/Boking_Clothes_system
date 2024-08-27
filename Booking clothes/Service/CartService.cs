using Booking_clothes.IService;
using Booking_clothes.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Drawing;
using Microsoft.CodeAnalysis;

namespace Booking_clothes.Service
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }
        public void AddToCart(Products product, string size, DateTime StartDate, DateTime endDate, int NumberOfDaysRent)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(item => item.ProductId == product.Id && item.Size == size );
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Size = size,
                    Color = product.Color,
                    StartDate= StartDate,
                    endDate = endDate,
                    NumberOfDaysRent = NumberOfDaysRent,
                    Price = product.PricePerDay -( product.DiscountValue/100 *product.PricePerDay),
                    ImageUrl = product.Image1, // Assuming you're using the first image
/*                    Quantity = quantity
*/                };
                cart.Add(cartItem);
            }
            else
            {
                cartItem.NumberOfDaysRent += NumberOfDaysRent;
            }
            SaveCart(cart);
        }

        public List<CartItem> GetCart()
        {
            var cartJson = _session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }
            return JsonConvert.DeserializeObject<List<CartItem>>(cartJson);
        }

        public void RemoveFromCart(int productId, string size, string color)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(item => item.ProductId == productId && item.Size == size && item.Color == color);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
                SaveCart(cart);
            }
        }

        public void ClearCart()
        {
            _session.Remove("Cart");
        }

        public void SaveCart(List<CartItem> cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);
            _session.SetString("Cart", cartJson);
        }



        /*  private readonly IHttpContextAccessor _httpContextAccessor;
          private readonly ISession _session;
          private const string CartSessionKey = "CartSessionKey"; // Define the session key here

          public CartService(IHttpContextAccessor httpContextAccessor)
          {
              _httpContextAccessor = httpContextAccessor;
              _session = _httpContextAccessor.HttpContext.Session;
          }

          public void AddToCart(CartItem cartItem, DateTime? startDate, DateTime? endDate)
          {
              var cart = GetCartItems();
              var existingItem = cart.FirstOrDefault(c =>
                  c.ProductId == cartItem.ProductId &&
                  c.Size == cartItem.Size &&
                  c.Color == cartItem.Color);

              if (existingItem != null)
              {
                  existingItem.Quantity += cartItem.Quantity;
                  existingItem.Price = cartItem.Price * existingItem.Quantity; // Adjust price calculation
                  existingItem.StartDate = startDate;
                  existingItem.EndDate = endDate;
              }
              else
              {
                  cartItem.StartDate = startDate;
                  cartItem.EndDate = endDate;
                  cart.Add(cartItem);
              }

              SaveCartItems(cart);
          }

          public void RemoveFromCart(int productId, string size, string color)
          {
              var cart = GetCartItems();
              var itemToRemove = cart.FirstOrDefault(c =>
                  c.ProductId == productId &&
                  c.Size == size &&
                  c.Color == color);

              if (itemToRemove != null)
              {
                  cart.Remove(itemToRemove);
                  SaveCartItems(cart);
              }
          }

          public void ClearCart()
          {
              SaveCartItems(new List<CartItem>());
          }

          public List<CartItem> GetCartItems()
          {
              var cartJson = _session.GetString(CartSessionKey);
              if (string.IsNullOrEmpty(cartJson))
              {
                  return new List<CartItem>();
              }

              return JsonConvert.DeserializeObject<List<CartItem>>(cartJson) ?? new List<CartItem>();
          }

          public decimal GetCartTotal()
          {
              var cart = GetCartItems();
              return cart.Sum(c => c.Price * c.Quantity);
          }

          private void SaveCartItems(List<CartItem> cart)
          {
              var cartJson = JsonConvert.SerializeObject(cart);
              _session.SetString(CartSessionKey, cartJson);
          }*/
    }
}
