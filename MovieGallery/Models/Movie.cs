using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MovieGallery.Models
{
    public class Movie
    {
        [Key]
        public int MovieID { get; set; }

        // Title
        [Required(ErrorMessage = "Title is required")]
        [StringLength(30, ErrorMessage = "{0} Title can not be longer than {1}")]
        public string Title { get; set; }

        // Genre
        [Required(ErrorMessage = "The genre field is required")]
        public string Genre { get; set; }

        // Release date
        [DisplayName("Release date")]
        [Required(ErrorMessage = "Release date is required")]
        public DateTime ReleaseDate { get; set; }


        // Movie description
        [DisplayName("Description")]
        [Required(ErrorMessage = "The movie description field is required")]
        public string MovieDescription { get; set; }


        // Movie cover image string
        [DisplayName("Movie Cover")]
        public string? MovieImage { get; set; }

        // Movie cover image file
        [NotMapped]
        [DisplayName("Image")]
        public IFormFile? ImageFile { get; set; }

        // Background image string
        [DisplayName("Background image")]
        public string? MovieBackgroundImage { get; set; }

        // Background image file
        [NotMapped]
        [DisplayName("Background Image")]
        public IFormFile? BackgroundFile { get; set; }


        public List<Producer> Producers { get; set; } = new List<Producer>();


        public List<Actor> Actors { get; set; } = new List<Actor>();


        public Rating? Rating { get; set; }

        [Required(ErrorMessage = "Number of ratings field is required")]
        public int NumRatings { get; set; } = 0;

        [Required(ErrorMessage ="Rating value field is required")]
        public string RatingValue { get; set; } = string.Empty;

    }

}
