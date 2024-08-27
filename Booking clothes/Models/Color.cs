using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Booking_clothes.Models
{
    public class Color
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Changed to Id

        [Required(ErrorMessage = "Color is required.")]
        [StringLength(20, ErrorMessage = "Color cannot be longer than 20 characters.")]
        public string ColorName { get; set; }
		public bool IsDeleted { get; set; }


    }
}
