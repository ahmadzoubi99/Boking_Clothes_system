using Booking_clothes.Models;

namespace Booking_clothes.IService
{
    public interface IProductService
    {
        Products GetProductById(int productId);
        IEnumerable<Products> GetAllProducts();
        void UpdateProduct(Products product);
        void DecreaseStock(int productId, int quantity, string size);
        void IncreaseStock(int productId, int quantity, string size);
        bool IsStockAvailable(int productId, int quantity, string size);
    }
}
