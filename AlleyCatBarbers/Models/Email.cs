using System.ComponentModel.DataAnnotations;

namespace AlleyCatBarbers.Models
{
    public class Email
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string From { get; set; }
        
        [Required]
        [EmailAddress]
        public string To {  get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

    }
}
