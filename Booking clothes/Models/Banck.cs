using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Booking_clothes.Models
{
    public class Banck
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(3)]
        public string CVV { get; set; }

        [Required, MaxLength(100)]
        public string CardHolder { get; set; }

        [Required, MaxLength(16)]
        public string CardNumber { get; set; }

        public DateTime ExpiryDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; }
    }
}
