using MovieGallery.Models;

public class MoviesViewModel
{
    public List<Movie> Movies { get; set; }
    public RatingMethods RatingMethods { get; set; }
    public string FilterOption { get; set; } = "All";
    public bool IsSortedByAverageRating { get; set; }
}