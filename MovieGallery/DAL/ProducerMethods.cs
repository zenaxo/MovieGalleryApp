using Microsoft.Data.SqlClient;
using MovieGallery.Models;
using System.Data;
using System.Data.Common;

namespace MovieGallery.DAL
{
    public class ProducerMethods
    {
        string connectionString = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = MovieGallery; Integrated Security = True; Connect Timeout = 30; Encrypt=False;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False";

        public List<Producer> GetProducersForMovie(int movieId, out string errorMessage)
        {
            errorMessage = "";

            List<Producer> producers = new List<Producer>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("GetProducersForMovie", connection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    dbCommand.Parameters.Add("@movieId", SqlDbType.Int).Value = movieId;

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = dbCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Producer producer = new Producer
                                {
                                    ProducerId = Convert.ToInt32(reader["ProducerId"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                };

                                producers.Add(producer);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }
                }
            }

            return producers;
        }

        public int DeleteMovieProducer(int movieId, int producerId, out string errorMessage)
        {
            errorMessage = "";
            int affectedRows = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("DeleteMovieProducer", connection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    dbCommand.Parameters.Add("@movieId", SqlDbType.Int).Value = movieId;
                    dbCommand.Parameters.Add("@producerId", SqlDbType.Int).Value = producerId;

                    try
                    {
                        connection.Open();

                        affectedRows = dbCommand.ExecuteNonQuery();

                        if (affectedRows == 1)
                        {
                            return affectedRows;
                        }
                        else
                        {
                            errorMessage = "Error: Movie producer does not exist";
                            return -1;
                        }
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                        return -1;
                    }
                }
            }
        }
        public List<Name> GetMovieProducerNames(int movieId, out string errorMessage)
        {
            errorMessage = "";

            List<Name> movieProducerNames = new List<Name>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("GetMovieProducerNames", connection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    dbCommand.Parameters.Add("@movieId", SqlDbType.Int).Value = movieId;

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = dbCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Name name = new Name
                                {
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                };

                                movieProducerNames.Add(name);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }
                }
            }

            return movieProducerNames;
        }
        public int InsertMovieProducer(string firstName, string lastName, int movieId, out string errorMessage)
        {
            errorMessage = "";

            // Try to get the ProducerId
            int tryId = GetProducerId(firstName, lastName, out errorMessage);

            // If the producer already exists keep tryId as producerId, else insert a new producer and retrieve the Id
            int producerId = (tryId != -1) ? tryId : InsertProducer(firstName, lastName, out errorMessage);

            if(MovieProducerExists(producerId, movieId, out errorMessage) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(firstName))
            {
                return -1;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("InsertMovieProducer", connection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    dbCommand.Parameters.Add("@producerId", SqlDbType.Int).Value = producerId;
                    dbCommand.Parameters.Add("@movieId", SqlDbType.Int).Value = movieId;

                    try
                    {
                        connection.Open();

                        // Execute the query
                        int rowsAffected = dbCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return rowsAffected;
                        }
                        else
                        {
                            errorMessage = "Failed to insert Movie Producer";
                            return -1;
                        }

                    }
                    catch (Exception e) {
                        errorMessage = e.Message;
                        return -1;
                    }
                }
            }      
        }

        private bool MovieProducerExists(int producerId, int movieId, out string errorMessage)
        {
            errorMessage = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("MovieProducerExists", connection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    dbCommand.Parameters.Add("@producerId", SqlDbType.Int).Value = producerId;
                    dbCommand.Parameters.Add("@movieId", SqlDbType.Int).Value = movieId;
                    dbCommand.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;

                    try
                    {
                        connection.Open();

                        // Execute the stored procedure
                        dbCommand.ExecuteNonQuery();

                        // Check the output parameter value
                        int result = Convert.ToInt32(dbCommand.Parameters["@result"].Value);

                        if(result == 1)
                        {
                            return true;
                        } 
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                        return false;
                    }
                }
            }
        }
        private int GetProducerId(string firstName, string lastName, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Parameter validation
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                errorMessage = "First name and last name cannot be null or empty.";
                return -1;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("GetProducerId", connection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    dbCommand.Parameters.Add("@firstName", SqlDbType.VarChar).Value = firstName;
                    dbCommand.Parameters.Add("@lastName", SqlDbType.VarChar).Value = lastName;

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = dbCommand.ExecuteReader())
                        {
                            if (reader.Read() && reader["ProducerID"] != DBNull.Value)
                            {
                                // Producer exists, return the ID
                                return Convert.ToInt32(reader["ProducerID"]);
                            }
                            else
                            {
                                // Producer does not exist
                                return -1;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                        return -1;
                    }
                }
            }
        }

        private int InsertProducer(string firstName, string lastName, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Parameter validation
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                errorMessage = "First name and last name cannot be null or empty.";
                return 0; // Return 0 on failure
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("InsertProducer", connection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    dbCommand.Parameters.Add("@firstName", SqlDbType.VarChar).Value = firstName;
                    dbCommand.Parameters.Add("@lastName", SqlDbType.VarChar).Value = lastName;

                    // Add output parameter for ProducerId
                    SqlParameter producerIdParameter = new SqlParameter("@producerId", SqlDbType.Int);
                    producerIdParameter.Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add(producerIdParameter);

                    try
                    {
                        connection.Open();

                        // Execute the query
                        int rowsAffected = dbCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Successful insertion, return the ProducerID
                            return (int)producerIdParameter.Value; ;
                        }
                        else
                        {
                            // Insertion failed
                            errorMessage = "Failed to insert producer.";
                            return 0;
                        }
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                        return 0;
                    }
                }
            }
        }
    }
}
