using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Booking_clothes.Models
{
    public class Size
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Changed to Id

        [Required(ErrorMessage = "Size is required.")]
        [StringLength(10, ErrorMessage = "Size cannot be longer than 10 characters.")]
        public string SizeName { get; set; }
        public ICollection<ProductSize> ClothesDetails { get; set; }
    }
}
