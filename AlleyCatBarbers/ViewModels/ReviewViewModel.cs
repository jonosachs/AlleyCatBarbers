using System.ComponentModel.DataAnnotations;

namespace AlleyCatBarbers.ViewModels
{
    public class ReviewViewModel
    {
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public string Comments { get; set; }

    }
}
