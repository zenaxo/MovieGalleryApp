using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MovieGallery.Data;
using MovieGallery.Models;

namespace MovieGallery.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MoviesController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
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
                // Check if an image file is uploaded
                if (obj.ImageFile != null && obj.ImageFile.Length > 0)
                {
                    // Generate a unique filename for the image
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + obj.ImageFile.FileName;

                    // Set the path for saving the image in the wwwroot/images folder
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", uniqueFileName);

                    // Save the image to the specified path
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        obj.ImageFile.CopyTo(fileStream);
                    }

                    // Set the MovieImage property to the unique filename
                    obj.MovieImage = uniqueFileName;
                }
                MovieMethods movieMethods = new MovieMethods();

                int i = 0;
                string error = "";

                i = movieMethods.InsertMovie(obj, out error);

                if (i > 0)
                {
                    // Movie successfully inserted, redirect to Index
                    return RedirectToAction("Index");
                }
                else
                {
                    // Handle the case where movie insertion failed
                    ModelState.AddModelError("", $"Failed to insert movie: {error}");
                }
            }

            // If ModelState is not valid, return to the same view with validation errors
            return View(obj);
        }
    }
}
