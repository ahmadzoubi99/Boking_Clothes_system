namespace Booking_clothes.Models
{
    public class CartViewModel
    {
        public List<CartItem> Items { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
