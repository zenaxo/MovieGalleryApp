namespace MovieGallery.Models
{
    public class MovieProducer
    {
        public int MProducerID { get; set; }

        public int ProducerID { get; set; }
        public Producer Producer { get; set; }

        public int MovieID { get; set; }
        public Movie? Movie { get; set; }
    }
}