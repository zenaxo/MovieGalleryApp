using MovieGallery.Models;
using Microsoft.EntityFrameworkCore;

namespace MovieGallery.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<MovieProducer> MovieProducers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure primary key for MovieProducer
            modelBuilder.Entity<MovieProducer>()
                .HasKey(mp => mp.MProducerID);

            // Add other configurations for your entities...

            base.OnModelCreating(modelBuilder);
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
    }
}
