using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieGallery.Models
{
    public class MovieMethods
    {
        string connectionString = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = MovieGallery; Integrated Security = True; Connect Timeout = 30; Encrypt=False;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False";

        public List<Movie> GetAllMovies(out string errormsg)
        {
            List<Movie> movies = new List<Movie>();

            // Create SQL Connection
            SqlConnection dbConnection = new SqlConnection();

            // Connection to SQL Server
            dbConnection.ConnectionString = connectionString;

            // SQL query to retrieve all movies
            string sqlQuery = "SELECT MovieID, Title, Genre, MovieImage, ReleaseDate FROM Movies";
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
                            ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"])
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

            return movies;
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
                        ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"])
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
        public int UpdateMovie(Movie movie, out string errormsg)
        {
            // Create SQL Connection
            SqlConnection dbConnection = new SqlConnection();

            // Connection to SQL Server
            dbConnection.ConnectionString = connectionString;

            // sqlstring and update movie in db
            string sqlstring = "UPDATE Movies SET Title = @title, Genre = @genre, MovieImage = @image_url, ReleaseDate = @release_date WHERE MovieID = @movieId";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("movieId", System.Data.SqlDbType.Int).Value = movie.MovieID;
            dbCommand.Parameters.Add("title", System.Data.SqlDbType.VarChar, 50).Value = movie.Title;
            dbCommand.Parameters.Add("genre", System.Data.SqlDbType.VarChar, 50).Value = movie.Genre;
            dbCommand.Parameters.Add("image_url", System.Data.SqlDbType.VarChar, 50).Value = movie.MovieImage;
            dbCommand.Parameters.Add("release_date", System.Data.SqlDbType.Date).Value = movie.ReleaseDate;

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
        public int InsertMovie(Movie movie, out string errormsg)
        {
            // Create SQL Connection
            SqlConnection dbConnection = new SqlConnection();

            // Connection to SQL Server
            dbConnection.ConnectionString = connectionString;

            // sqlstring and add movie to db
            String sqlstring = "INSERT INTO Movies (Title, Genre, MovieImage, ReleaseDate) VALUES (@title, @genre, @image_url, @release_date)";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("title", System.Data.SqlDbType.VarChar, 50).Value = movie.Title;
            dbCommand.Parameters.Add("genre", System.Data.SqlDbType.VarChar, 50).Value = movie.Genre;
            dbCommand.Parameters.Add("image_url", System.Data.SqlDbType.VarChar, 50).Value = movie.MovieImage;
            dbCommand.Parameters.Add("release_date", System.Data.SqlDbType.Date).Value = movie.ReleaseDate;

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Failed to insert image into Database."; }
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

        public List<Movie> GetMoviesFilteredByGenre(string genre, out string errormsg)
        {
            // Create SQL Connection
            SqlConnection conn = new SqlConnection();

            // Connection to the SQL Server
            conn.ConnectionString = connectionString;

            // Use a store procedure to retrieve movies filtered by genre
            SqlCommand sqlCommand = new SqlCommand("GetMoviesFilteredByGenre", conn)
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
    }
}
