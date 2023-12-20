using Microsoft.AspNetCore.Mvc;
using MovieGallery.DAL;
using MovieGallery.Models;

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
            _userMethods = new UserMethods();
            _logger = logger;
        }

        public IActionResult Index()
        {
            Options options = new Options();
            ViewBag.Error = TempData["Error"];

            string errorMessage;


            var movies = _movieMethods.GetMovieList(options, out errorMessage);
            var heroMovie = _movieMethods.GetHeroMovie(out errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.ErrorMessage = errorMessage;
            }

            var viewModel = new MoviesViewModel
            {
                Movies = movies,
                Options = options,
                HeroMovie = heroMovie,
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateMovieList([FromBody] Options options)
        {
            string errorMessage;

            var movies = _movieMethods.GetMovieList(options, out errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.ErrorMessage = errorMessage;
            }

            var viewModel = new MoviesViewModel
            {
                Movies = movies,
                Options = options,
                // HeroMovie is not part of the partial view...
                HeroMovie = movies.Count > 0 ? movies[0] : null,
            };

            return PartialView("_DynamicContent", viewModel);
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
        public IActionResult Create([FromForm] Movie obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (TrySaveImage(obj.ImageFile!, out string uniqueFileName))
                    {
                        obj.MovieImage = uniqueFileName;
                    } 
                    else
                    {
                        ViewBag["Error"] = "Only images are allowed";
                    }

                    if (TrySaveImage(obj.BackgroundFile!, out uniqueFileName))
                    {
                        obj.MovieBackgroundImage = uniqueFileName;
                    }
                    else
                    {
                        ViewBag["Error"] = "Only images are allowed";
                    }

                    string error = "";
                    int i = _movieMethods.InsertMovie(obj, out error);

                    return Ok();
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex;
                return BadRequest(ex.Message);
            }
        }
        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPEG", ".JPE", ".BMP", ".GIF", ".PNG" };
        private bool TrySaveImage(IFormFile imageFile, out string uniqueFileName)
        {
            uniqueFileName = string.Empty;

            if (imageFile != null && imageFile.Length > 0 && ImageExtensions.Contains(Path.GetExtension(imageFile.FileName)))
            {
                uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", uniqueFileName);

                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }

                return true;
            }

            return false;
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
        public IActionResult Edit([FromForm] Movie obj)
        {
            if (ModelState.IsValid)
            {
                string error = "";
                if (TrySaveImage(obj.ImageFile!, out string uniqueFileName))
                {
                    obj.MovieImage = uniqueFileName;
                }
                else
                {
                    Movie existingMovie = _movieMethods.GetMovieById(obj.MovieID, out error);
                    obj.MovieImage = existingMovie.MovieImage;
                }

                if (TrySaveImage(obj.BackgroundFile!, out uniqueFileName))
                {
                    obj.MovieBackgroundImage = uniqueFileName;
                }
                else
                {
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
                    return RedirectToAction("Index");
                }
            }
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
