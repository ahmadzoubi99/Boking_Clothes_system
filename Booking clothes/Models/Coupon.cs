using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Booking_clothes.Models
{
	public class Coupon
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }  // Changed to Id


		[Required]
		[StringLength(50)]
		public string Code { get; set; }


		[Required]
		/*        [Range(0, 100)] // Percentage must be between 0 and 100
        */
		[Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100.")]
		public decimal DiscountPercentage { get; set; }

		[Required]
		public DateTime ExpiryDate { get; set; }

		public bool IsActive { get; set; } = true;

		public bool IsDeleted { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;

	}
}
