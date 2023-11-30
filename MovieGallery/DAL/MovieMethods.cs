using Microsoft.Data.SqlClient;
using MovieGallery.Models;
using System.Data;

namespace MovieGallery.DAL
{
    public class MovieMethods
    {
        string connectionString = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = MovieGallery; Integrated Security = True; Connect Timeout = 30; Encrypt=False;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False";

        private readonly RatingMethods _ratingMethods;
        private readonly ProducerMethods _producerMethods;
        public MovieMethods()
        {
            _ratingMethods = new RatingMethods();
            _producerMethods = new ProducerMethods();
        }
        private SqlConnection CreateConnection()
        {
            SqlConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();
            return dbConnection;
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
                                    ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"]),
                                    MovieDescription = reader["MovieDescription"].ToString(),
                                    Rating = GetRatingForMovie(movieId, out errorMessage),
                                    Producers = _producerMethods.GetProducersForMovie(movieId, out errorMessage),

                                };
                            }
                        }

                        // Get Movie Producers
                        //List<Producer> producers = new List<Producer>();
                        //using (SqlCommand producerCommand = new SqlCommand("GetProducersForMovie", dbConnection))
                        //{
                        //    producerCommand.CommandType = CommandType.StoredProcedure;
                        //    producerCommand.Parameters.Add("@MovieID", SqlDbType.Int).Value = movieId;

                        //    // Execute the command
                        //    using (SqlDataReader producerReader = producerCommand.ExecuteReader())
                        //    {
                        //        if (producerReader.HasRows)
                        //        {
                        //            while (producerReader.Read())
                        //            {
                        //                string firstName = producerReader["FirstName"].ToString();
                        //                string lastName = producerReader["LastName"].ToString();

                        //                // Create a Producer and add it to the list
                        //                producers.Add(new Producer
                        //                {
                        //                    FirstName = firstName,
                        //                    LastName = lastName
                        //                });
                        //            }
                        //        }
                        //    }
                        //}

                        //// Assign the list of producers to the movie object
                        //movie.Producers = producers;

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
        public List<Movie> GetMovieList(out string errormsg, string option = "all", bool isSorted = false)
        {
            List<string> genres = GlobalVariables.Genres;

            List<Movie> results = new List<Movie>();

            // If option is present in the genre list...
            if (genres.Contains(option))
            {
                // Create the list of movies for that genre
                List<Movie> movieListUnsorted = GetMoviesByGenre(option, out errormsg);
                // Should the list of movies be sorted by average rating or not?
                results = isSorted ? _ratingMethods.GetMovieListSortedByAverageRating(movieListUnsorted, out errormsg) : movieListUnsorted;
            }
            else
            {
                List<Movie> movieListUnsorted = GetAllMovies(out errormsg);
                results = isSorted ? _ratingMethods.GetMovieListSortedByAverageRating(movieListUnsorted, out errormsg) : movieListUnsorted;
            }
            // Get the average rating and number of ratings for each movie in the movie list
            foreach (Movie movie in results)
            {
                movie.Rating = GetRatingForMovie(movie.MovieID, out errormsg);
                movie.Producers = _producerMethods.GetProducersForMovie(movie.MovieID, out errormsg);
            }
            return results;
        }
        public int InsertMovie(Movie movie, List<Producer> producers, int numRatings, string ratingValue, out string errormsg)
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

                            foreach(var producer in producers)
                            {
                                _producerMethods.InsertMovieProducer(producer.FirstName, producer.LastName, movieId, out errormsg);
                            }

                            _ratingMethods.GenerateRatings(movieId, numRatings, ratingValue, out errormsg);

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
        public int UpdateMovie(Movie movie, List<Producer> producers, int numRatings, string ratingValue, out string errormsg)
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
                                _ratingMethods.GenerateRatings(movie.MovieID, numRatings, ratingValue, out errormsg);

                                foreach (var producer in producers)
                                {
                                    _producerMethods.InsertMovieProducer(producer.FirstName, producer.LastName, movie.MovieID, out errormsg);
                                }

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
