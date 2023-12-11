using Microsoft.Data.SqlClient;
using MovieGallery.Models;
using Newtonsoft.Json;
using System.Data;

namespace MovieGallery.DAL
{
    public class MovieMethods
    {
        string connectionString = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = MovieGallery; Integrated Security = True; Connect Timeout = 30; Encrypt=False;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False";

        private readonly RatingMethods _ratingMethods;
        private readonly ProducerMethods _producerMethods;
        private readonly ActorMethods _actorMethods;
        public MovieMethods()
        {
            _ratingMethods = new RatingMethods();
            _producerMethods = new ProducerMethods();
            _actorMethods = new ActorMethods();
        }
        private SqlConnection CreateConnection()
        {
            SqlConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();
            return dbConnection;
        }

        public int DeleteMovieProducer(int movieId, int producerId, out string errorMessage)
        {
            string errormMessage = string.Empty;
            return _producerMethods.DeleteMovieProducer(movieId, producerId, out errorMessage);
        }
        public int InsertMovieProducer(int movieId, Name name, out string errorMessage)
        {
            string errormMessage = string.Empty;
            return _producerMethods.InsertMovieProducer(name.FirstName, name.LastName, movieId, out errorMessage);
        }

        public int DeleteCast(int movieId, int actorId, out string errorMessage)
        {
            string errormMessage = string.Empty;
            return _actorMethods.DeleteCast(movieId, actorId, out errorMessage);
        }
        public int InsertCast(int movieId, Name name, out string errorMessage)
        {
            string errormMessage = string.Empty;
            return _actorMethods.InsertCast(name.FirstName, name.LastName, movieId, out errorMessage);
        }
        private bool HeroMovieExists(out string errorMessage)
        {
            errorMessage = "";
            bool isTableEmpty = false;

            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("IsHeroMovieTableEmpty", dbConnection))
                {
                    try
                    {
                        dbConnection.Open();

                        // Set the command type to StoredProcedure
                        dbCommand.CommandType = CommandType.StoredProcedure;

                        // Execute the stored procedure and get the result
                        object result = dbCommand.ExecuteScalar();

                        // Check if the result is DBNull or not
                        if (result != DBNull.Value)
                        {
                            // Convert the result to int
                            int rowCount = Convert.ToInt32(result);

                            // Check the result to determine if the table is empty
                            isTableEmpty = rowCount == 1;
                        }
                        else
                        {
                            // Handle DBNull value (considering the table is not empty)
                            isTableEmpty = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage = $"Error checking if HeroMovie table is empty: {ex.Message}";
                        // You may log or handle the exception according to your application's needs
                    }
                }
            }

            return !isTableEmpty;
        }
        public Movie GetHeroMovie(out string errorMessage)
        {
            errorMessage = "";
            Movie movie = new Movie();

            // If the hero movie does not exist...
            if(!HeroMovieExists(out errorMessage))
            {
                // Retrieve the latest added movie
                movie = GetLatestAddedMovie(out errorMessage);
                // Set the hero movie
                SetHeroMovie(movie.MovieID, out errorMessage);
                // Return that movie
                return movie;
            }

            // The hero movie exists...

           using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("GetHeroMovie", dbConnection)) {

                    dbCommand.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        dbConnection.Open();

                        SqlParameter outputParameter = new SqlParameter("@MovieID", SqlDbType.Int);
                        outputParameter.Direction = ParameterDirection.Output;
                        dbCommand.Parameters.Add(outputParameter);

                        dbCommand.ExecuteNonQuery();

                        int movieId = Convert.ToInt32(outputParameter.Value);

                        movie = GetMovieById(movieId, out errorMessage);

                    }
                    catch (Exception e)
                    {
                        errorMessage = "Error retrieving hero movie:" + e.Message;
                    }
                }
            }

            return movie;

        }

        public int SetHeroMovie(int movieId, out string errorMessage)
        {
            errorMessage = "";
            int affectedRows = -1;

            try
            {
                using (SqlConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();

                    using (SqlCommand dbCommand = new SqlCommand("SetHeroMovie", dbConnection))
                    {
                        dbCommand.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        dbCommand.Parameters.AddWithValue("@InputMovieID", movieId);

                        // Execute the stored procedure
                        affectedRows = dbCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return affectedRows;
        }
        public Movie GetLatestAddedMovie(out string errorMessage)
        {
            Movie latestAddedMovie = null;
            errorMessage = "";

            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("GetLatestAddedMovie", dbConnection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        dbConnection.Open();

                        using (SqlDataReader reader = dbCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var movieId = Convert.ToInt32(reader["MovieID"]);
                                    latestAddedMovie = new Movie
                                    {
                                        MovieID = movieId,
                                        Title = reader["Title"].ToString(),
                                        Genre = reader["Genre"].ToString(),
                                        MovieImage = reader["MovieImage"].ToString(),
                                        MovieBackgroundImage = reader["MovieBackgroundImage"].ToString(),
                                        ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"]),
                                        MovieDescription = reader["MovieDescription"].ToString(),
                                        Rating = GetRatingForMovie(movieId, out errorMessage),
                                        Producers = _producerMethods.GetProducersForMovie(movieId, out errorMessage),
                                        Actors = _actorMethods.GetActorsForMovie(movieId, out errorMessage),
                                    };
                                }
                            }
                            else
                            {
                                latestAddedMovie = new Movie
                                {
                                    MovieID = -1,
                                    Title = "Movie Not Found",
                                    Genre = "Error",
                                    MovieImage = "image-not-found.jpg",
                                    MovieBackgroundImage = "image-not-found.jpg",
                                    ReleaseDate = DateTime.MinValue,
                                    MovieDescription = "No movie found with the specified criteria.",
                                    Rating = new Rating
                                    {
                                        AverageRating = 0,
                                        NumberOfRatings = 0,
                                    }
                                };
                            }
                        }

                        errorMessage = "";
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }
                }
            }

            return latestAddedMovie;
        }
        public List<Movie> GetAllMovies(out string errorMessage)
        {
            List<Movie> movies = new List<Movie>();
            errorMessage = "";

            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("GetAllMovies", dbConnection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        dbConnection.Open();

                        using (SqlDataReader reader = dbCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Movie movie = new Movie
                                    {
                                        MovieID = Convert.ToInt32(reader["MovieID"]),
                                        Title = reader["Title"].ToString(),
                                        Genre = reader["Genre"].ToString(),
                                        MovieImage = reader["MovieImage"].ToString(),
                                        MovieBackgroundImage= reader["MovieBackgroundImage"].ToString(),
                                        ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"]),
                                        MovieDescription = reader["MovieDescription"].ToString()
                                    };

                                    movies.Add(movie);
                                }
                            }
                        }

                        errorMessage = "";
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }
                }
            }

            return movies;
        }
        public List<Movie> SearchMoviesByTitle(string title, out string errorMessage)
        {
            List<Movie> results = new List<Movie>();
            errorMessage = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("SearchMoviesByTitle", conn))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    sqlCommand.Parameters.Add("@title", SqlDbType.NVarChar).Value = title;

                    try
                    {
                        conn.Open();

                        // Execute the stored procedure
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            // Check if there are any rows returned
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    // Create a Movie object for each row and add it to the list
                                    Movie movie = new Movie
                                    {
                                        MovieID = Convert.ToInt32(reader["MovieID"]),
                                        Title = reader["Title"].ToString(),
                                        Genre = reader["Genre"].ToString(),
                                        MovieImage = reader["MovieImage"].ToString(),
                                        MovieBackgroundImage = reader["MovieBackgroundImage"].ToString(),
                                        ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"]),
                                        MovieDescription = reader["MovieDescription"].ToString()
                                    };

                                    results.Add(movie);
                                }
                            }
                        }

                        errorMessage = "";
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                    }
                }
            }

            return results;
        }
        public List<Movie> GetMoviesByGenre(string genre, out string errorMessage)
        {
            List<Movie> results = new List<Movie>();
            errorMessage = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("GetMoviesByGenre", conn))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    sqlCommand.Parameters.Add("@Genre", SqlDbType.VarChar).Value = genre;

                    try
                    {
                        conn.Open();

                        // Execute the stored procedure
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            // Check if there are any rows returned
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    // Create a Movie object for each row and add it to the list
                                    Movie movie = new Movie
                                    {
                                        MovieID = Convert.ToInt32(reader["MovieID"]),
                                        Title = reader["Title"].ToString(),
                                        Genre = reader["Genre"].ToString(),
                                        MovieImage = reader["MovieImage"].ToString(),
                                        MovieBackgroundImage = reader["MovieBackgroundImage"].ToString(),
                                        ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"])
                                    };

                                    results.Add(movie);
                                }
                            }
                        }

                        errorMessage = "";
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                    }
                }
            }

            return results;
        }
        public Movie GetMovieById(int movieId, out string errorMessage)
        {
            Movie movie = null;
            errorMessage = "";

            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("GetMovieById", dbConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    sqlCommand.Parameters.Add("@movieId", SqlDbType.Int).Value = movieId;

                    try
                    {
                        dbConnection.Open();

                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                // Create a Movie object for the row
                                movie = new Movie
                                {
                                    MovieID = Convert.ToInt32(reader["MovieID"]),
                                    Title = reader["Title"].ToString(),
                                    Genre = reader["Genre"].ToString(),
                                    MovieImage = reader["MovieImage"].ToString(),
                                    MovieBackgroundImage = reader["MovieBackgroundImage"].ToString(),
                                    ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"]),
                                    MovieDescription = reader["MovieDescription"].ToString(),
                                    Rating = GetRatingForMovie(movieId, out errorMessage),
                                    Producers = _producerMethods.GetProducersForMovie(movieId, out errorMessage),
                                    Actors = _actorMethods.GetActorsForMovie(movieId, out errorMessage),

                                };
                            }
                        }

                        errorMessage = "";
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }
                }
            }

            return movie;
        }
        private Rating GetRatingForMovie(int movieId, out string errorMessage)
        {
            errorMessage = "";

            Rating rating = new Rating
            {
                AverageRating = _ratingMethods.GetAverageRating(movieId, out errorMessage),
                NumberOfRatings = _ratingMethods.GetNumberOfRatings(movieId, out errorMessage),
                Stars = _ratingMethods.GetStars(movieId, out errorMessage),
            };
            return rating;
        }
        public List<Movie> GetMovieList(Options options, out string errormsg)
        {
            List<string> genres = GlobalVariables.Genres;

            List<Movie> results = new List<Movie>();

            // If option is present in the genre list...
            if (genres.Contains(options.FilterOption))
            {
                // Create the list of movies for that genre
                List<Movie> movieListUnsorted = GetMoviesByGenre(options.FilterOption, out errormsg);
                // Should the list of movies be sorted by average rating or not?
                results = options.IsSortedByAverageRating ? _ratingMethods.GetMovieListSortedByAverageRating(movieListUnsorted, out errormsg) : movieListUnsorted;
            }
            else
            {
                List<Movie> movieListUnsorted = GetAllMovies(out errormsg);
                results = options.IsSortedByAverageRating ? _ratingMethods.GetMovieListSortedByAverageRating(movieListUnsorted, out errormsg) : movieListUnsorted;
            }
            // Should the movies be ordered by date?
            if(options.IsSortedByDate)
            {
                results = OrderMovieListByDate(results, out errormsg);
            }
            // Get the average rating and number of ratings for each movie in the movie list
            foreach (Movie movie in results)
            {
                movie.Rating = GetRatingForMovie(movie.MovieID, out errormsg);
                movie.Producers = _producerMethods.GetProducersForMovie(movie.MovieID, out errormsg);
                movie.Actors = _actorMethods.GetActorsForMovie(movie.MovieID, out errormsg);
            }
            return results;
        }

        private List<Movie> OrderMovieListByDate(List<Movie> movielist, out string errormsg)
        {
            errormsg = string.Empty;

            try
            {
                List<Movie> orderedMovies = movielist.OrderByDescending(movie => movie.ReleaseDate).ToList();
                return orderedMovies;
            }
            catch (Exception e)
            {
                errormsg = $"Error ordering movies: {e.Message}";
                return null;
            }
          
        }
        public int InsertMovie(Movie movie, out string errormsg)
        {
            using (SqlConnection dbConnection = CreateConnection())
            using (SqlTransaction transaction = dbConnection.BeginTransaction())
            {
                try
                {
                    using (SqlCommand dbCommand = new SqlCommand("InsertMovie", dbConnection, transaction))
                    {
                        dbCommand.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        dbCommand.Parameters.Add("@title", SqlDbType.VarChar, 50).Value = movie.Title;
                        dbCommand.Parameters.Add("@genre", SqlDbType.VarChar, 50).Value = movie.Genre;
                        dbCommand.Parameters.Add("@image_url", SqlDbType.NVarChar, 1000).Value = movie.MovieImage;
                        dbCommand.Parameters.Add("@image_large", SqlDbType.NVarChar, 1000).Value = movie.MovieBackgroundImage;
                        dbCommand.Parameters.Add("@release_date", SqlDbType.Date).Value = movie.ReleaseDate;
                        dbCommand.Parameters.Add("@movie_description", SqlDbType.VarChar).Value = movie.MovieDescription;

                        // Output parameter for movieId
                        SqlParameter movieIdParam = new SqlParameter("@movieId", SqlDbType.Int);
                        movieIdParam.Direction = ParameterDirection.Output;
                        dbCommand.Parameters.Add(movieIdParam);

                        dbCommand.ExecuteNonQuery();

                        int movieId = Convert.ToInt32(movieIdParam.Value);

                        if (movieId > 0)
                        {
                            transaction.Commit();
                            errormsg = "";

                            _ratingMethods.GenerateRatings(movieId, movie.NumRatings, movie.RatingValue, out errormsg);

                            return 1; // Success
                        }
                        else
                        {
                            errormsg = "Failed to insert movie into the Database.";
                            return 0; // Failure
                        }
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    errormsg = e.Message;
                    return 0;
                }
            }
        }
        public int UpdateMovie(Movie movie, out string errormsg)
        {
            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("UpdateMovie", dbConnection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    dbCommand.Parameters.Add("@movieId", SqlDbType.Int).Value = movie.MovieID;
                    dbCommand.Parameters.Add("@title", SqlDbType.VarChar, 50).Value = movie.Title;
                    dbCommand.Parameters.Add("@genre", SqlDbType.VarChar, 50).Value = movie.Genre;
                    dbCommand.Parameters.Add("@image_url", SqlDbType.NVarChar, 1000).Value = movie.MovieImage;
                    dbCommand.Parameters.Add("@image_large", SqlDbType.NVarChar, 1000).Value = movie.MovieBackgroundImage;
                    dbCommand.Parameters.Add("@release_date", SqlDbType.Date).Value = movie.ReleaseDate;
                    dbCommand.Parameters.Add("@movie_description", SqlDbType.VarChar).Value = movie.MovieDescription;

                    try
                    {
                        dbConnection.Open();
                        using (SqlTransaction transaction = dbConnection.BeginTransaction())
                        {
                            dbCommand.Transaction = transaction;

                            try
                            {
                                dbCommand.ExecuteNonQuery();
                                errormsg = "";
                                // Commit the transaction if everything is successful
                                transaction.Commit();

                                // Generate ratings after updating the movie
                                _ratingMethods.GenerateRatings(movie.MovieID, movie.NumRatings, movie.RatingValue, out errormsg);

                                return 1;
                            }
                            catch (Exception e)
                            {
                                // Rollback the transaction if an error occurs
                                transaction.Rollback();
                                errormsg = $"Error updating movie. Error: {e.Message}";
                                return 0;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        errormsg = e.Message;
                        return 0;
                    }
                }
            }
        }
        public int DeleteMovie(int movieId, out string errormsg)
        {
            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("DeleteMovie", dbConnection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    dbCommand.Parameters.Add("@movieId", SqlDbType.Int).Value = movieId;

                    try
                    {
                        dbConnection.Open();

                        // Execute the stored procedure
                        int rowsAffected = dbCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            errormsg = "";
                        }
                        else
                        {
                            errormsg = $"No rows were affected. Movie with ID {movieId} not found.";
                        }

                        return rowsAffected;
                    }
                    catch (Exception e)
                    {
                        errormsg = $"Error deleting movie. Error: {e.Message}";
                        return 0;
                    }
                }
            }
        }
    }
}
