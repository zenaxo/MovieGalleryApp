namespace MovieGallery.Models
{
    public class Actor
    {
        public int ActorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<Cast> Casts { get; set; }
    }
}
