namespace Booking_clothes.Models
{
    public class CartItem
    {

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime endDate { get; set; }

        public int NumberOfDaysRent { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }

        public decimal Total => Price * Quantity;

    }
}
