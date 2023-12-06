using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MovieGallery.DAL;
using MovieGallery.Models;

namespace MovieGallery.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly MovieMethods _movieMethods;

        public MoviesController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _movieMethods = new MovieMethods();
        }

        public IActionResult Index(string filterOption = "Genres", bool isSortedByAverageRating = false, bool isSortedByDate = false)
        {
            ViewBag.Error = TempData["Error"];

            Options options = new Options
            {
                FilterOption = filterOption,
                IsSortedByAverageRating = isSortedByAverageRating,
                IsSortedByDate = isSortedByDate
            };

            string errorMessage;
            var movies = _movieMethods.GetMovieList(options, out errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                // Handle the error, e.g., log it or display an error message
                ViewBag.ErrorMessage = errorMessage;
            }

            var viewModel = new MoviesViewModel
            {
                Movies = movies,
                Options = options
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // If it's an AJAX request, return a partial view
                return PartialView("_DynamicContent", viewModel);
            }

            return View(viewModel);
        }
        public IActionResult Search(string title)
        {
            List<Movie> searchResults = _movieMethods.SearchMoviesByTitle(title, out string errorMessage);

            var searchResultsDTO = searchResults.Select(movie => new MovieSearchResult
            {
                MovieID = movie.MovieID,
                Title = movie.Title,
            }).ToList();
            return Json(searchResultsDTO);
        }

        // GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Movie obj, List<Producer> producers, int numRatings, string ratingValue)
        {
            try
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

                    string error = "";
                    MovieMethods movieMethods = new MovieMethods();
                    int i = movieMethods.InsertMovie(obj, producers, numRatings, ratingValue, out error);
                    TempData["Error"] = error;

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

                // If ModelState is not valid, print out validation errors
                foreach (var key in ModelState.Keys)
                {
                    foreach (var error in ModelState[key].Errors)
                    {
                        Console.WriteLine($"Field: {key}, Error: {error.ErrorMessage}");
                    }
                }

                // Return to the same view with validation errors
                return View(obj);
            }
            catch (Exception ex)
            {
                // Log the exception for further analysis
                Console.WriteLine($"Exception: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while processing your request. Please try again.");
                return View(obj);
            }
        }
        //GET
        public IActionResult Details(int id)
        {
          
            string errormsg;
            Movie movie = _movieMethods.GetMovieById(id, out errormsg);

            if (movie == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(errormsg))
            {
                TempData["Error"] = errormsg;
            }

            return View(movie);
        }

        public IActionResult Delete(int id)
        {
            string errormsg;
            Movie movie = _movieMethods.GetMovieById(id, out errormsg);

            if (movie == null)
            {
                return NotFound();
            }
            var viewModel = new MoviesViewModel();
            if (!string.IsNullOrEmpty(errormsg))
            {
                TempData["Error"] = errormsg;
            }

            return View(movie);
        }
        public IActionResult ConfirmDelete(int id)
        {
            string errormsg;

            // Retrieve the movie using the ID passed in the form
            Movie movie = _movieMethods.GetMovieById(id, out errormsg);

            if (movie == null)
            {
                return NotFound();
            }

            _movieMethods.DeleteMovie(id, out errormsg);

            if (!string.IsNullOrEmpty(errormsg))
            {
                TempData["Error"] = errormsg;
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            string errormsg;
            Movie movie = _movieMethods.GetMovieById(id, out errormsg);

            if (movie == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(errormsg))
            {
                TempData["Error"] = errormsg;
            }

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Movie obj, int numRatings, string ratingValue, List<Producer> producers)
        {
            if (ModelState.IsValid)
            {
                string error = "";
                // Check if an image file is uploaded
                if (obj.ImageFile != null && obj.ImageFile.Length > 0)
                {
                    // Generate a unique filename for the image
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + obj.ImageFile.FileName;

                    // Set the path for saving the image in the wwwroot/images folder
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", uniqueFileName);

                    // Save the new image to the specified path
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        obj.ImageFile.CopyTo(fileStream);
                    }

                    // Set the MovieImage property to the unique filename
                    obj.MovieImage = uniqueFileName;
                }
                else
                {
                    // No new image uploaded, retain the existing image path
                    Movie existingMovie = _movieMethods.GetMovieById(obj.MovieID, out error); // Replace with your own method to get the existing movie details
                    obj.MovieImage = existingMovie.MovieImage;
                }

                int i = 0;

                i = _movieMethods.UpdateMovie(obj, producers, numRatings, ratingValue, out error);
                if (!string.IsNullOrEmpty(error))
                {
                    TempData["Error"] = error;
                }

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
    }
}
