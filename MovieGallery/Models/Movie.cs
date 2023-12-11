using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieGallery.Models
{
    public class Movie
    {
        [Key]
        public int MovieID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Genre { get; set; }
        [DisplayName("Release Date")]
        public DateTime ReleaseDate { get; set; }
        [DisplayName("Description")]
        public string? MovieDescription { get; set; }
        [DisplayName("Movie Cover")]
        public string? MovieImage { get; set; }
        [NotMapped]
        [DisplayName("Image")]
        public IFormFile? ImageFile { get; set; }
        [DisplayName("Background image")]
        public string? MovieBackgroundImage { get; set; }
        [NotMapped]
        [DisplayName("Background Image")]
        public IFormFile? BackgroundFile { get; set; }
        public List<Producer> Producers { get; set; } = new List<Producer>();
        public List<Actor> Actors { get; set; } = new List<Actor>();
        public Rating? Rating { get; set; }

        // For editing and adding a movie
        public int NumRatings { get; set; } = 0;
        public string RatingValue { get; set; } = string.Empty;
        public List<Name> ProducerNames { get; set; } = new List<Name>();
        public List<Name> ActorNames { get; set; } = new List<Name>();
    }

}
