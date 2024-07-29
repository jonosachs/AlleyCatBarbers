using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AlleyCatBarbers.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        // TODO: Add start-end time for bookings
        //public DateTime StartTime { get; set; }
        //public DateTime EndTime { get; set; }
        
        [BindNever]
        public string UserId { get; set; }
        
        [BindNever]
        public IdentityUser User { get; set; }
        
        public int ServiceId { get; set; }

        [BindNever]
        public Service Service { get; set; }

        public Booking()
        {
            Date = DateTime.UtcNow;
        }
    }
}
