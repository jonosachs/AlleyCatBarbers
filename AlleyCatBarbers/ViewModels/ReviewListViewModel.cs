namespace AlleyCatBarbers.ViewModels
{
    public class ReviewListViewModel
    {
        public IEnumerable<ReviewViewModel> Reviews { get; set; }
        public int AverageRating { get; set; }
    }
}
