using Microsoft.AspNetCore.Mvc;
using MovieGallery.Data;
using MovieGallery.Models;

namespace MovieGallery.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MoviesController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Movie> objMovieList = _db.Movies;
            return View(objMovieList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }
        //GET
        public IActionResult Details(int id)
        {
            Movie movie = _db.Movies.FirstOrDefault(m => m.MovieID == id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Movie obj)
        {
            if (ModelState.IsValid)
            {
                _db.Movies.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult InsertMovie()
        {
            MovieDetail movieDetail = new MovieDetail();
            MovieMethods movieMethods = new MovieMethods();

            //('The Shawshank Redemption', 'Drama', 'the_shawshank_redemption_.jpg', '1994-09-10')
            int i = 0;
            string error = "";

            movieDetail.Title = "The Shawshank Redemption";
            movieDetail.Genre = "Drama";
            movieDetail.MovieImage = "the_shawshank_redemption_.jpg";
            movieDetail.ReleaseDate = new DateTime(1994, 09, 10);

            i = movieMethods.InsertMovie(movieDetail, out error);
            ViewBag.error = error;
            ViewBag.antal = i;

            return View();
        }
    }
}
