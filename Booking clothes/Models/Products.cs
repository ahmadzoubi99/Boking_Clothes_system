using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Booking_clothes.Models
{
    public class Products
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Changed to Id

        [Required(ErrorMessage = "Cloth name is required.")]
        [StringLength(150, ErrorMessage = "Cloth name cannot be longer than 150 characters.")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required(ErrorMessage = "Price per day is required.")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PricePerDay { get; set; }
        public bool Availability { get; set; } = true;
        public decimal DiscountValue { get; set; }
        public int Stock { get; set; }

        [StringLength(500, ErrorMessage = "Image URL cannot be longer than 255 characters.")]
        public string Image1 { get; set; }

        [StringLength(500, ErrorMessage = "Image URL cannot be longer than 255 characters.")]
        public string Image2 { get; set; }

        [StringLength(500, ErrorMessage = "Image URL cannot be longer than 255 characters.")]
        public string Image3 { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; }
        [NotMapped]
		public IFormFile? ImageFile1 { get; set; }
        [NotMapped]
        public IFormFile? ImageFile2 { get; set; }
        [NotMapped]
        public IFormFile? ImageFile3 { get; set; }


		public ICollection<Review> Review { get; set; }
		public ICollection<ProductSize> productSizes { get; set; }



	}
}
