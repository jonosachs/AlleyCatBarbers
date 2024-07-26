using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlleyCatBarbers.Models
{
    public class Email
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string To {  get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

        //public IFormFile Attachment { get; set; }


        public Email()
        {
            
        }

    }

    
}
