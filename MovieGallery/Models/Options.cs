namespace MovieGallery.Models
{
    public class Options
    {
        public bool IsSortedByAverageRating { get; set; } = true;
        public string FilterOption { get; set; } = "Genres";
        public bool IsSortedByDate { get; set; } = false;

    }
}
