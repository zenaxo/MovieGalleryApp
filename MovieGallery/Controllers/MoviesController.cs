using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MovieGallery.Models;

namespace MovieGallery.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MoviesController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            MovieMethods movieMethods = new MovieMethods();
            string errormsg;

            // Check if the flag is present in TempData
            if(TempData.ContainsKey("FilterGenres"))
            {
                // Filter logic here
                string genre = ViewBag.genre;
                List<Movie> filteredMovieList = movieMethods.GetMoviesFilteredByGenre(genre, out errormsg);

                if(!string.IsNullOrEmpty(errormsg))
                {
                    ViewBag.ErrorMessage = errormsg;
                }

                // Clear the flag from TempData
                TempData.Remove("FilterGenres");

                return View(new MoviesViewModel { Movies = filteredMovieList, RatingMethods = new RatingMethods() });
            }

            // Check if the flag is present in TempData
            if (TempData.ContainsKey("SortByRating"))
            {
                // Sorting logic here
                RatingMethods ratingMethods = new RatingMethods();
                List<Movie> sortedMovieList = ratingMethods.GetMoviesSortedByAverageRating(out errormsg);

                if (!string.IsNullOrEmpty(errormsg))
                {
                    ViewBag.ErrorMessage = errormsg;
                }

                // Clear the flag from TempData
                TempData.Remove("SortByRating");

                return View(new MoviesViewModel { Movies = sortedMovieList, RatingMethods = new RatingMethods() });
            }

            // Default logic without sorting
            List<Movie> objMovieList = movieMethods.GetAllMovies(out errormsg);

            if (!string.IsNullOrEmpty(errormsg))
            {
                ViewBag.ErrorMessage = errormsg;
            }

            return View(new MoviesViewModel { Movies = objMovieList, RatingMethods = new RatingMethods() });
        }

        public IActionResult FilterGenres(string genre)
        {
            ViewBag.genre = genre;
            MovieMethods movieMethods = new MovieMethods();
            string errormsg;
            List<Movie> filteredMovieList = movieMethods.GetMoviesFilteredByGenre(genre, out errormsg);

            if (!string.IsNullOrEmpty(errormsg))
            {
                ViewBag.Error = errormsg;
            }

            TempData["FilterGenres"] = true;

            return RedirectToAction("Index");

        }

        public IActionResult SortByRating()
        {
            RatingMethods ratingMethods = new RatingMethods();
            string errormsg;

            List<Movie> sortedMovieList = ratingMethods.GetMoviesSortedByAverageRating(out errormsg);

            if (!string.IsNullOrEmpty(errormsg))
            {
                ViewBag.Error = errormsg;
            }

            // Set a flag in TempData to indicate sorting by rating
            TempData["SortByRating"] = true;

            // Return the Index view with the sorted movie list
            return RedirectToAction("Index", new MoviesViewModel { Movies = sortedMovieList, RatingMethods = ratingMethods });
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }
        //GET
        public IActionResult Details(int id)
        {
            MovieMethods movieMethods = new MovieMethods();
            string errormsg;
            Movie movie = movieMethods.GetMovieById(id, out errormsg);

            if (movie == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(errormsg))
            {
                ViewBag.ErrorMessage = errormsg;
            }

            return View(movie);
        }
        public IActionResult Edit(int id)
        {
            MovieMethods movieMethods = new MovieMethods();
            string errormsg;
            Movie movie = movieMethods.GetMovieById(id, out errormsg);

            if (movie == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(errormsg))
            {
                ViewBag.ErrorMessage = errormsg;
            }

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Movie obj)
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

                i = movieMethods.UpdateMovie(obj, out error);

                if (i > 0)
                {
                    // Movie successfully updated, redirect to Index
                    return RedirectToAction("Index");
                }
                else
                {
                    // Handle the case where movie update failed
                    ModelState.AddModelError("", $"Failed to update movie: {error}");
                }
            }

            // If ModelState is not valid, return to the same view with validation errors
            return View(obj);
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
                    FileInfo fileInfo = new FileInfo(obj.ImageFile.FileName);
                    string uniqueFileName = Guid.NewGuid().ToString() + fileInfo.Extension;

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
