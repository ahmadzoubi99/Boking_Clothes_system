using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Booking_clothes.Service;

namespace Booking_clothes.Models
{
    public class ReservationDetail
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Changed to Id
        public int ReservationId { get; set; }
        [ForeignKey("ReservationId")]
        public Reservation Reservation { get; set; }
        public int ClothId { get; set; }
        [ForeignKey("ClothId")]
        public Products products { get; set; }
 
        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; } = 1;

        [Required(ErrorMessage = "Delivery date is required.")]
        public DateTime StartReservationDate { get; set; }


        [Required(ErrorMessage = "End date is required.")]
        [DateGreaterThan("StartReservationDate", ErrorMessage = "End date must be greater than or equal to start date.")]
        public DateTime EndReservationDate { get; set; }

    }
}
