using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieGallery.Models
{
    public class Movie
    {
        public int MovieID { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string ReleaseDate { get; set; }

        public List<MovieProducer> MovieProducers { get; set; }
        public List<Cast> Casts { get; set; }
    }

}
