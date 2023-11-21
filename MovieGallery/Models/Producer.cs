namespace MovieGallery.Models
{
    public class Producer
    {
        public int ProducerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<MovieProducer> MovieProducers { get; set; }
    }
}
