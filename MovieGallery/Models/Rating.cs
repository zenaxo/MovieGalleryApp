using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieGallery.Models
{
    public class Rating
    {
        [Key]
        public int RatingID { get; set; }
        public int MovieID { get; set; }
        [Required]
        public int RatingValue { get; set; }
        [ForeignKey("MovieID")]
        public Movie Movie { get; set;}
    }
}
