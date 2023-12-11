using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieGallery.DAL;
using MovieGallery.Models;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MovieGallery.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly MovieMethods _movieMethods;
        private readonly UserMethods _userMethods;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(IWebHostEnvironment webHostEnvironment, ILogger<MoviesController> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _movieMethods = new MovieMethods();
            _logger = logger;
        }

        public IActionResult Index(string filterOption = "Genres", bool isSortedByAverageRating = false, bool isSortedByDate = false)
        {
            ViewBag.Error = TempData["Error"];

            string errorMessage;

            Options options = new Options
            {
                FilterOption = filterOption,
                IsSortedByAverageRating = isSortedByAverageRating,
                IsSortedByDate = isSortedByDate
            };

            var movies = _movieMethods.GetMovieList(options, out errorMessage);
            var heroMovie = _movieMethods.GetHeroMovie(out errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                // Handle the error, e.g., log it or display an error message
                ViewBag.ErrorMessage = errorMessage;
            }

            var viewModel = new MoviesViewModel
            {
                Movies = movies,
                Options = options,
                HeroMovie = heroMovie,
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
        public IActionResult Create(Movie obj)
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
                    if (obj.BackgroundFile != null && obj.BackgroundFile.Length > 0)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + obj.BackgroundFile.FileName;
                        string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", uniqueFileName);

                        using (var filestream = new FileStream(imagePath, FileMode.Create))
                        {
                            obj.BackgroundFile.CopyTo(filestream);
                        }

                        obj.MovieBackgroundImage = uniqueFileName;
                    }

                    string error = "";
                    MovieMethods movieMethods = new MovieMethods();
                    int i = movieMethods.InsertMovie(obj, out error);
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
        public IActionResult Edit(Movie obj)
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
                    Movie existingMovie = _movieMethods.GetMovieById(obj.MovieID, out error);
                    obj.MovieImage = existingMovie.MovieImage;
                }

                if (obj.BackgroundFile != null && obj.BackgroundFile.Length > 0)
                {
                    // Generate a unique filename for the image
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + obj.BackgroundFile.FileName;

                    // Set the path for saving the image in the wwwroot/images folder
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", uniqueFileName);

                    // Save the new image to the specified path
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        obj.BackgroundFile.CopyTo(fileStream);
                    }

                    // Set the MovieImage property to the unique filename
                    obj.MovieBackgroundImage = uniqueFileName;
                }
                else
                {
                    // No new image uploaded, retain the existing image path
                    Movie existingMovie = _movieMethods.GetMovieById(obj.MovieID, out error);
                    obj.MovieBackgroundImage = existingMovie.MovieBackgroundImage;
                }
                int i = 0;

                i = _movieMethods.UpdateMovie(obj, out error);
                if (!string.IsNullOrEmpty(error))
                {
                    TempData["Error"] = error;
                }

                if (i > 0)
                {
                    // Movie successfully updated, redirect to Index
                    return RedirectToAction("Index");
                }
            }
            // If ModelState is not valid, return to the same view with validation errors
            return View(obj);
        }

        [HttpPost]
        public JsonResult SetHeroMovie(int movieId)
        {
            string errorMessage = string.Empty;
            _movieMethods.SetHeroMovie(movieId, out errorMessage);

            return Json(new { errorMessage });
        }

        [HttpPost]
        public IActionResult AddProducer(Name producerName, int movieId)
        {
            string errorMsg = string.Empty;

            _movieMethods.InsertMovieProducer(movieId, producerName, out errorMsg);

            Movie movie = _movieMethods.GetMovieById(movieId, out errorMsg);

            return PartialView("_EditProducersPartial", movie);
        }

        [HttpPost]
        public IActionResult DeleteProducer(int producerId, int movieId)
        {
            string errorMsg = string.Empty;

            _movieMethods.DeleteMovieProducer(movieId, producerId, out errorMsg);

            Movie movie = _movieMethods.GetMovieById(movieId, out errorMsg);

            return PartialView("_EditProducersPartial", movie);

        }

        [HttpPost]
        public IActionResult AddActor(Name actorName, int movieId)
        {
            string errorMsg = string.Empty;

            _movieMethods.InsertCast(movieId, actorName, out errorMsg);

            Movie movie = _movieMethods.GetMovieById(movieId, out errorMsg);

            return PartialView("_EditActorsPartial", movie);
        }

        [HttpPost]
        public IActionResult DeleteActor(int actorId, int movieId)
        {
            string errorMsg = string.Empty;

            _movieMethods.DeleteCast(movieId, actorId, out errorMsg);

            Movie movie = _movieMethods.GetMovieById(movieId, out errorMsg);

            return PartialView("_EditActorsPartial", movie);

        }

        // LOGIN STUFF BELOW
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel user)
        {
            string errormsg = "";

            if(user != null)
            {
                bool userCorrect = _userMethods.CheckUser(user, out errormsg);
                if(userCorrect)
                {
                    HttpContext.Session.SetString("UserName", user.UserName);
                    return RedirectToAction("Index");
                }
            }

            return View("Login", user);
        }
    }
}
