using System;
using System.ComponentModel.DataAnnotations;

namespace MovieGallery.Models
{
    public class MovieDetail
    {
        public int MovieID { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string MovieImage { get; set; }

        public List<MovieProducer> MovieProducers { get; set; }
        public List<Cast> Casts { get; set; }
    }
}
