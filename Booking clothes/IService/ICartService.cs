using Booking_clothes.Models;

namespace Booking_clothes.IService
{
    public interface ICartService
    {
        public void AddToCart(Products product, string size, DateTime StartDate, DateTime endDate, int NumberOfDaysRent);
         public List<CartItem> GetCart();
        public void RemoveFromCart(int productId, string size, string color);
        public void ClearCart();
        public void SaveCart(List<CartItem> cart);


    /*    public decimal GetCartTotal();
        public void AddToCart(CartItem cartItem, DateTime? startDate, DateTime? endDate);
        public void RemoveFromCart(int productId, string size, string color);
        public void ClearCart();
        public List<CartItem> GetCartItems();
*/



    }
}
