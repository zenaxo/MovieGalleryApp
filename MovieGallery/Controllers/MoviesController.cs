using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index(string filterOption, bool isSortedByAverageRating)
        {
            string errorMessage;
            var movies = _movieMethods.GetMovieList(out errorMessage, filterOption, isSortedByAverageRating);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                // Handle the error, e.g., log it or display an error message
                ViewBag.ErrorMessage = errorMessage;
            }

            var viewModel = new MoviesViewModel
            {
                Movies = movies,
                FilterOption = filterOption,
                IsSortedByAverageRating = isSortedByAverageRating
            };

            return View(viewModel);
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

        public IActionResult Delete(int id)
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
        public IActionResult ConfirmDelete(int id)
        {
            MovieMethods movieMethods = new MovieMethods();
            string errormsg;

            // Retrieve the movie using the ID passed in the form
            Movie movie = movieMethods.GetMovieById(id, out errormsg);

            if (movie == null)
            {
                return NotFound();
            }

            movieMethods.DeleteMovie(id, out errormsg);

            if (!string.IsNullOrEmpty(errormsg))
            {
                ViewBag.ErrorMessage = errormsg;
            }

            return RedirectToAction("Index");
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
        public IActionResult Create(Movie obj, List<Producer> producers)
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
                    int i = movieMethods.InsertMovie(obj, producers, out error);
                    ViewBag.Error = error;

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

    }
}
