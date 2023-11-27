using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MovieGallery.Models
{
    public class MovieMethods
    {
        string connectionString = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = MovieGallery; Integrated Security = True; Connect Timeout = 30; Encrypt=False;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False";
        private readonly MovieProducerMethods _movieProducerMethods;

        public MovieMethods()
        {
            _movieProducerMethods = new MovieProducerMethods();
        }
        private SqlConnection CreateConnection()
        {
            SqlConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();
            return dbConnection;
        }
        public List<Movie> GetAllMovies(out string errormsg)
        {
            List<Movie> movies = new List<Movie>();

            // Create SQL Connection
            SqlConnection dbConnection = new SqlConnection();

            // Connection to SQL Server
            dbConnection.ConnectionString = connectionString;

            // SQL query to retrieve all movies
            string sqlQuery = "SELECT MovieID, Title, Genre, MovieImage, ReleaseDate, MovieDescription FROM Movies";
            SqlCommand dbCommand = new SqlCommand(sqlQuery, dbConnection);

            try
            {
                dbConnection.Open();

                // Execute the SQL query
                SqlDataReader reader = dbCommand.ExecuteReader();

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

                        movies.Add(movie);
                    }
                }

                errormsg = "";
            }
            catch (Exception e)
            {
                errormsg = e.Message;
            }
            finally
            {
                dbConnection.Close();
            }
            foreach (var movie in movies)
            {
                movie.Producers = _movieProducerMethods.GetProducersByMovieId(movie.MovieID, out errormsg);
            }

            return movies;
        }
        public List<Movie> GetMoviesByGenre(string genre, out string errormsg)
        {
            // Create SQL Connection
            SqlConnection conn = new SqlConnection();

            // Connection to the SQL Server
            conn.ConnectionString = connectionString;

            // Use a store procedure to retrieve movies filtered by genre
            SqlCommand sqlCommand = new SqlCommand("GetMoviesByGenre", conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            sqlCommand.Parameters.Add("@Genre", System.Data.SqlDbType.VarChar).Value = genre;

            List<Movie> results = new List<Movie>();

            try
            {
                conn.Open();

                // Execute the stored procedure
                SqlDataReader reader = sqlCommand.ExecuteReader();

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

                errormsg = "";
            }
            catch (Exception ex)
            {
                errormsg = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return results;
        }
        public Movie GetMovieById(int movieId, out string errormsg)
        {
            Movie movie = null;

            // Create SQL Connection
            SqlConnection dbConnection = new SqlConnection();

            // Connection to SQL Server
            dbConnection.ConnectionString = connectionString;

            // SQL query to retrieve a movie by ID
            string sqlQuery = "SELECT * FROM Movies WHERE MovieID = @movieId";
            SqlCommand dbCommand = new SqlCommand(sqlQuery, dbConnection);
            dbCommand.Parameters.Add("movieId", System.Data.SqlDbType.Int).Value = movieId;

            try
            {
                dbConnection.Open();

                // Execute the SQL query
                SqlDataReader reader = dbCommand.ExecuteReader();

                // Check if a row is returned
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
                        MovieDescription = reader["MovieDescription"].ToString()
                    };
                }

                errormsg = "";
            }
            catch (Exception e)
            {
                errormsg = e.Message;
            }
            finally
            {
                dbConnection.Close();
            }

            return movie;
        }

        public List<Movie> GetMovieList(out string errormsg, string option = "all", bool isSorted = false)
        {
            List<string> genres = GlobalVariables.Genres;
            RatingMethods ratingMethods = new RatingMethods();

            List<Movie> results = new List<Movie>();

            // If option is present in the genre list...
            if(genres.Contains(option))
            {
               // Create the list of movies for that genre
               List<Movie> movieListUnsorted = GetMoviesByGenre(option, out errormsg);
               // Should the list of movies be sorted by average rating or not?
               results = isSorted ? ratingMethods.GetMovieListSortedByAverageRating(movieListUnsorted, out errormsg) : movieListUnsorted;
            } else
            {
                List<Movie> movieListUnsorted = GetAllMovies(out errormsg);
                results = isSorted ? ratingMethods.GetMovieListSortedByAverageRating(movieListUnsorted, out errormsg) : movieListUnsorted;
            }
            // Get the average rating and number of ratings for each movie in the movie list
            foreach (Movie movie in results)
            {
                movie.AverageRating = ratingMethods.GetAverageRating(movie.MovieID, out errormsg);
                movie.NumberOfRatings = ratingMethods.GetNumberOfRatings(movie.MovieID, out errormsg);
            }
            return results;
        }
        public int InsertMovie(Movie movie, List<Producer> producers, out string errormsg)
        {
            using (SqlConnection dbConnection = CreateConnection())
            using (SqlTransaction transaction = dbConnection.BeginTransaction())
            {
                try
                {
                    string sqlstring = "INSERT INTO Movies (Title, Genre, MovieImage, ReleaseDate, MovieDescription) VALUES (@title, @genre, @image_url, @release_date, @movie_description); SELECT SCOPE_IDENTITY()";
                    SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection, transaction);
                    dbCommand.Parameters.Add(new SqlParameter("@title", SqlDbType.VarChar, 50) { Value = movie.Title });
                    dbCommand.Parameters.Add(new SqlParameter("@genre", SqlDbType.VarChar, 50) { Value = movie.Genre });
                    dbCommand.Parameters.Add(new SqlParameter("@image_url", SqlDbType.VarChar, 50) { Value = movie.MovieImage });
                    dbCommand.Parameters.Add(new SqlParameter("@release_date", SqlDbType.Date) { Value = movie.ReleaseDate });
                    dbCommand.Parameters.Add(new SqlParameter("@movie_description", SqlDbType.VarChar) { Value = movie.MovieDescription });

                    int movieId = Convert.ToInt32(dbCommand.ExecuteScalar());

                    if (movieId > 0)
                    {
                        foreach (var producer in producers)
                        {
                            string producerErrorMsg;
                            _movieProducerMethods.AddProducerToMovie(movieId, producer, out producerErrorMsg);

                            if (!string.IsNullOrEmpty(producerErrorMsg))
                            {
                                // Log or handle the producer insertion error
                            }
                        }

                        transaction.Commit();
                        errormsg = "";
                        return 1; // Success
                    }
                    else
                    {
                        errormsg = "Failed to insert movie into the Database.";
                        return 0; // Failure
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
            // Create SQL Connection
            SqlConnection dbConnection = new SqlConnection();

            // Connection to SQL Server
            dbConnection.ConnectionString = connectionString;

            // sqlstring and update movie in db
            string sqlstring = "UPDATE Movies SET Title = @title, Genre = @genre, MovieImage = @image_url, ReleaseDate = @release_date, MovieDescription = @movie_description WHERE MovieID = @movieId";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("movieId", System.Data.SqlDbType.Int).Value = movie.MovieID;
            dbCommand.Parameters.Add("title", System.Data.SqlDbType.VarChar, 50).Value = movie.Title;
            dbCommand.Parameters.Add("genre", System.Data.SqlDbType.VarChar, 50).Value = movie.Genre;
            dbCommand.Parameters.Add("image_url", System.Data.SqlDbType.VarChar, 50).Value = movie.MovieImage;
            dbCommand.Parameters.Add("release_date", System.Data.SqlDbType.Date).Value = movie.ReleaseDate;
            dbCommand.Parameters.Add("movie_description", System.Data.SqlDbType.VarChar).Value = movie.MovieDescription;

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Failed to update movie in the Database."; }
                return i;
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public int DeleteMovie(int movieId, out string errormsg)
        {
            // Create SQL Connection
            SqlConnection dbConnection = new SqlConnection();

            // Connection to SQL Server
            dbConnection.ConnectionString = connectionString;

            // SQL query to delete a movie by ID
            string sqlQuery = "DELETE FROM Movies WHERE MovieID = @movieId";
            SqlCommand dbCommand = new SqlCommand(sqlQuery, dbConnection);
            dbCommand.Parameters.Add("movieId", System.Data.SqlDbType.Int).Value = movieId;

            try
            {
                dbConnection.Open();

                // Execute the SQL query
                int rowsAffected = dbCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    errormsg = "";
                }
                else
                {
                    errormsg = "No rows were affected. Movie with ID " + movieId + " not found.";
                }

                return rowsAffected;
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

    }
}
