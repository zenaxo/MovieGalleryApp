using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace MovieGallery.Models
{
    public class RatingMethods
    {
        string connectionString = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = MovieGallery; Integrated Security = True; Connect Timeout = 30; Encrypt=False;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False";

        public double GetAverageRating(int movieId, out string errormsg)
        {
            double averageRating = 0;

            // Create SQL Connection
            SqlConnection dbConnection = new SqlConnection();

            // Connection to SQL Server
            dbConnection.ConnectionString = connectionString;

            // SQL query to calculate the average rating for a movie
            string sqlQuery = "SELECT AVG(Rating) AS AverageRating FROM Ratings WHERE MovieID = @movieId";
            SqlCommand dbCommand = new SqlCommand(sqlQuery, dbConnection);
            dbCommand.Parameters.Add("movieId", System.Data.SqlDbType.Int).Value = movieId;

            try
            {
                dbConnection.Open();

                // Execute the SQL query
                var result = dbCommand.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    averageRating = Convert.ToDouble(result);
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

            return averageRating;
        }

        public int GetNumberOfRatings(int movieId, out string errormsg)
        {
            int numberOfRatings = 0;

            // Create SQL Connection
            SqlConnection dbConnection = new SqlConnection();

            // Connection to SQL Server
            dbConnection.ConnectionString = connectionString;

            // SQL query to count the number of ratings for a movie
            string sqlQuery = "SELECT COUNT(*) FROM Ratings WHERE MovieID = @movieId";
            SqlCommand dbCommand = new SqlCommand(sqlQuery, dbConnection);
            dbCommand.Parameters.Add("movieId", System.Data.SqlDbType.Int).Value = movieId;

            try
            {
                dbConnection.Open();

                // Execute the SQL query
                numberOfRatings = (int)dbCommand.ExecuteScalar();

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

            return numberOfRatings;
        }
        public List<Movie> GetMoviesSortedByAverageRating(out string errormsg)
        {
            List<Movie> movies = new List<Movie>();

            // Create SQL Connection
            SqlConnection dbConnection = new SqlConnection();

            // Connection to SQL Server
            dbConnection.ConnectionString = connectionString;

            // Use a stored procedure to retrieve movies sorted by average rating
            string storedProcedureName = "GetMoviesSortedByAverageRating";
            SqlCommand dbCommand = new SqlCommand(storedProcedureName, dbConnection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            try
            {
                dbConnection.Open();

                // Execute the stored procedure
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
    }
}
