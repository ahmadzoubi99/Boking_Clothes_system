using Booking_clothes.Service;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Booking_clothes.Models
{
	public class ProductSize
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }  // Changed to Id
		public int Quantity { get; set; }
		public int SizeID { get; set; }
		[ForeignKey("SizeID")]
		public Size Size { get; set; }
		public int ProductId { get; set; }
		[ForeignKey("ProductId")]
        public Products Products { get; set; }

	}
}
