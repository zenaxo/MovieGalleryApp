using Microsoft.Data.SqlClient;
using MovieGallery.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace MovieGallery.DAL
{
    public class RatingMethods
    {
        string connectionString = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = MovieGallery; Integrated Security = True; Connect Timeout = 30; Encrypt=False;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False";

        private static int BiasedRandom(Random rnd, int minValue, int maxValue, double probabilityMultiplier)
        {
            double value = rnd.NextDouble();
            double biasedValue = Math.Pow(value, probabilityMultiplier);
            int range = maxValue - minValue;

            int biasedRandom = minValue + (int)(biasedValue * range);

            return Math.Clamp(biasedRandom, minValue, maxValue - 1);
        }
        public void GenerateRatings(int movieId, int numRatings, string ratingValue, out string errormsg)
        {
            errormsg = "";

            if (ratingValue == "Random")
            {
                Random rnd = new Random();

                for (int i = 0; i < numRatings; i++)
                {
                    int maxRating = 5;
                    // Lower value yields higher bias for higher ratings
                    double probabilityMultiplier = 0.4;

                    int rating = BiasedRandom(rnd, 1, maxRating + 1, probabilityMultiplier);

                    string result = AddRating(movieId, rating);

                    if (!string.IsNullOrEmpty(result))
                    {
                        errormsg += result + Environment.NewLine;

                        break;
                    }
                }
            }
            else
            {
                if (int.TryParse(ratingValue, out int rating))
                {
                    for (int i = 0; i < numRatings; i++)
                    {
                        string result = AddRating(movieId, rating);

                        if (!string.IsNullOrEmpty(result))
                        {
                            // Handle the error (log, return, etc.)
                            errormsg += result + Environment.NewLine;

                            break;
                        }
                    }
                }
                else
                {
                    errormsg = "Invalid ratingValue. Please provide a valid integer.";
                }
            }
        }
        private string AddRating(int movieId, int rating)
        {
            string errorMessage = "";

            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("AddRating", dbConnection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    dbCommand.Parameters.Add("@movieId", SqlDbType.Int).Value = movieId;
                    dbCommand.Parameters.Add("@rating", SqlDbType.Int).Value = rating;

                    try
                    {
                        dbConnection.Open();

                        // Execute the stored procedure to insert the rating
                        dbCommand.ExecuteNonQuery();

                        errorMessage = "";
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }
                }
            }

            return errorMessage;
        }
        public Star GetStarForMovie(int movieId, int rating, out string errorMessage)
        {
            Star star = new Star();
            errorMessage = "";

            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("GetStarForMovie", dbConnection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    dbCommand.Parameters.Add("@movieId", SqlDbType.Int).Value = movieId;
                    dbCommand.Parameters.Add("@rating", SqlDbType.Int).Value = rating;
                    dbCommand.Parameters.Add("@total", SqlDbType.Int).Direction = ParameterDirection.Output;

                    try
                    {
                        dbConnection.Open();
                        dbCommand.ExecuteNonQuery();

                        // Retrieve the output parameter values
                        if (dbCommand.Parameters["@total"].Value != DBNull.Value)
                        {
                            star.rating = rating; // Set the provided rating value
                            star.total = Convert.ToInt32(dbCommand.Parameters["@total"].Value);
                        }
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }
                }
            }

            return star;
        }
        public List<Star> GetStars(int movieId, out string errorMessage)
        {
            List<Star> list = new List<Star>();
            errorMessage = "";
            for (int i = 5; i >= 1; i--)
            {
                list.Add(GetStarForMovie(movieId, i, out errorMessage));
            }
            return list;
        }
        public double GetAverageRating(int movieId, out string errorMessage)
        {
            double averageRating = 0;
            errorMessage = "";

            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("GetAverageRating", dbConnection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    dbCommand.Parameters.Add("@movieId", SqlDbType.Int).Value = movieId;
                    dbCommand.Parameters.Add("@averageRating", SqlDbType.Float).Direction = ParameterDirection.Output;

                    try
                    {
                        dbConnection.Open();
                        dbCommand.ExecuteNonQuery();

                        // Retrieve the output parameter value
                        if (dbCommand.Parameters["@averageRating"].Value != DBNull.Value)
                        {
                            averageRating = Convert.ToDouble(dbCommand.Parameters["@averageRating"].Value);
                        }
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }
                }
            }

            return averageRating;
        }

        public int GetNumberOfRatings(int movieId, out string errorMessage)
        {
            int numberOfRatings = 0;
            errorMessage = "";

            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("GetNumberOfRatings", dbConnection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    dbCommand.Parameters.Add("@movieId", SqlDbType.Int).Value = movieId;
                    dbCommand.Parameters.Add("@numberOfRatings", SqlDbType.Int).Direction = ParameterDirection.Output;

                    try
                    {
                        dbConnection.Open();
                        dbCommand.ExecuteNonQuery();

                        // Retrieve the output parameter value
                        if (dbCommand.Parameters["@numberOfRatings"].Value != DBNull.Value)
                        {
                            numberOfRatings = Convert.ToInt32(dbCommand.Parameters["@numberOfRatings"].Value);
                        }
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }
                }
            }

            return numberOfRatings;
        }
        public List<Movie> GetMovieListSortedByAverageRating(List<Movie> movieList, out string errormsg)
        {
            List<Movie> sortedMovies = new List<Movie>();

            foreach (var movie in movieList)
            {
                // Calculate average rating for each movie
                double averageRating = GetAverageRating(movie.MovieID, out errormsg);
                int numberOfRatings = GetNumberOfRatings(movie.MovieID, out errormsg);

                if (!string.IsNullOrEmpty(errormsg))
                {
                    // Handle error, log, or return an appropriate response
                    return null;
                }

                // Assign the average rating to the movie
                Rating rating = new Rating();
                rating.AverageRating = averageRating;
                rating.NumberOfRatings = numberOfRatings;
                movie.Rating = rating;

                sortedMovies.Add(movie);
            }

            // Sort the movies by average rating in descending order
            sortedMovies = sortedMovies.OrderByDescending(m => m.Rating.AverageRating).ToList();

            errormsg = "";
            return sortedMovies;
        }
    }
}
