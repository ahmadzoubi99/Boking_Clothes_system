using Booking_clothes.Data;
using Booking_clothes.IService;
using Booking_clothes.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking_clothes.Service
{
    public class ProductService: IProductService
    {
        private readonly MyContext _context;

        public ProductService(MyContext context)
        {
            _context = context;
        }

        public Products GetProductById(int productId)
        {
            return _context.Products
                .Include(p => p.productSizes)
                .FirstOrDefault(p => p.Id == productId);
        }

        public IEnumerable<Products> GetAllProducts()
        {
            return _context.Products
                .Include(p => p.productSizes)
                .ToList();
        }

        public void UpdateProduct(Products product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void DecreaseStock(int productId, int quantity, string size)
        {
            var productSize = _context.ProductSize
                .FirstOrDefault(ps => ps.ProductId == productId && ps.Size.SizeName == size);

            if (productSize != null && productSize.Quantity >= quantity)
            {
                productSize.Quantity -= quantity;
                _context.SaveChanges();
            }
            else
            {
                // Optionally throw an exception or handle the case where stock is insufficient
            }
        }

        public void IncreaseStock(int productId, int quantity, string size)
        {
            var productSize = _context.ProductSize
                .FirstOrDefault(ps => ps.ProductId == productId && ps.Size.SizeName == size);

            if (productSize != null)
            {
                productSize.Quantity += quantity;
                _context.SaveChanges();
            }
        }

        public bool IsStockAvailable(int productId, int quantity, string size)
        {
            var productSize = _context.ProductSize
                .FirstOrDefault(ps => ps.ProductId == productId && ps.Size.SizeName == size);

            return productSize != null && productSize.Quantity >= quantity;
        }


        public IQueryable<Products> FilterProducts(decimal? minPrice, decimal? maxPrice, string color)
        {
            var query = _context.Products.AsQueryable();

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.PricePerDay >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.PricePerDay <= maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(color))
            {
                query = query.Where(p => p.Color.ToLower() == color.ToLower());
            }

            return query;
        }
    }
}
