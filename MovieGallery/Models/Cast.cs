namespace MovieGallery.Models
{
    public class Cast
    {
        public int CastID { get; set; }

        public int ActorID { get; set; }
        public Actor Actor { get; set; }

        public int MovieID { get; set; }
        public Movie Movie { get; set; }
    }
}
