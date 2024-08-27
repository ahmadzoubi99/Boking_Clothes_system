using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking_clothes.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(100)]
        public string LastName { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
        [StringLength(255, ErrorMessage = "Image URL cannot be longer than 255 characters.")]
        public string Image { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Testimonial> Testimonials { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Review> Reviews { get; set; }  // Change from Review to Reviews





    }
}
