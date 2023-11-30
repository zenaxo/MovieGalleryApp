namespace MovieGallery.Models
{
    public class Rating
    {
        public double AverageRating { get; set; }
        public int NumberOfRatings { get; set; }
        public List<Star>? Stars { get; set; }
    }
}
