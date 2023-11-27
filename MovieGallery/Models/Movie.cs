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
        [DisplayName("Movie Cover")]
        public string? MovieImage { get; set; }
        [DisplayName("Description")]
        public string MovieDescription { get; set; }
        [NotMapped]
        [DisplayName("Image")]
        public IFormFile ImageFile { get; set; }
        public List<MovieProducer> ?Producers { get; set; }
        public double AverageRating { get; set; }
        public int NumberOfRatings { get; set; }
    }

}
