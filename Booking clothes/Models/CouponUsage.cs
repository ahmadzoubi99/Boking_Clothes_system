using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Booking_clothes.Models
{
	public class CouponUsage
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public int CouponID { get; set; }

		[ForeignKey("CouponID")]
		public Coupon Coupon { get; set; }

		public string UserID { get; set; } // Nullable if not applicable

		[ForeignKey("UserID")]
		public ApplicationUser User { get; set; }


		public int ReservationID { get; set; }
		[ForeignKey("ReservationID")]
		public Reservation Reservation { get; set; }

		public DateTime UsedAt { get; set; } = DateTime.Now;


	}
}
