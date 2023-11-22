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
        public DateTime ReleaseDate { get; set; }
        public string MovieImage { get; set; }

        public List<MovieProducer> MovieProducers { get; set; }
        public List<Cast> Casts { get; set; }
    }

}
