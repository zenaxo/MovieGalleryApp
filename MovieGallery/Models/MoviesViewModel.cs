using MovieGallery.Models;

public class MoviesViewModel
{
    public List<Movie> Movies { get; set; }
    public Options Options { get; set; } = new Options();
    public Movie HeroMovie { get; set; }
}