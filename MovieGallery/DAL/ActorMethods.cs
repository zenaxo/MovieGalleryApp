using Microsoft.Data.SqlClient;
using MovieGallery.Models;
using System.Data;
using System.Data.Common;

namespace MovieGallery.DAL
{
    public class ActorMethods
    {
        string connectionString = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = MovieGallery; Integrated Security = True; Connect Timeout = 30; Encrypt=False;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False";

        public List<Actor> GetActorsForMovie(int movieId, out string errorMessage)
        {
            errorMessage = "";

            List<Actor> actors = new List<Actor>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("GetActorsForMovie", connection))
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
                                Actor actor = new Actor
                                {
                                    ActorId = Convert.ToInt32(reader["ActorId"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                };

                                actors.Add(actor);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }
                }
            }

            return actors;
        }
        public int InsertCast(string firstName, string lastName, int movieId, out string errorMessage)
        {
            errorMessage = "";

            // Try to get the CastId
            int tryId = GetActorId(firstName, lastName, out errorMessage);

            // If the Cast already exists keep tryId as actorId, else insert a new actor and retrieve the Id
            int actorId = (tryId != -1) ? tryId : InsertActor(firstName, lastName, out errorMessage);

            if(CastExists(actorId, movieId, out errorMessage) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(firstName))
            {
                return -1;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("InsertCast", connection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    dbCommand.Parameters.Add("@actorId", SqlDbType.Int).Value = actorId;
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
                            errorMessage = "Failed to insert Cast";
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

        private bool CastExists(int actorId, int movieId, out string errorMessage)
        {
            errorMessage = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("CastExists", connection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    dbCommand.Parameters.Add("@actorId", SqlDbType.Int).Value = actorId;
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

        public int DeleteCast(int movieId, int actorId, out string errorMessage)
        {
            errorMessage = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("DeleteCast", connection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    dbCommand.Parameters.Add("@movieId", SqlDbType.Int).Value = movieId;
                    dbCommand.Parameters.Add("@actorId", SqlDbType.Int).Value = actorId;

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
                            errorMessage = "Failed to delete Cast.";
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
        private int GetActorId(string firstName, string lastName, out string errorMessage)
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
                using (SqlCommand dbCommand = new SqlCommand("GetActorId", connection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    dbCommand.Parameters.Add("@firstName", SqlDbType.VarChar).Value = firstName;
                    dbCommand.Parameters.Add("@lastName", SqlDbType.VarChar).Value = lastName;

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = dbCommand.ExecuteReader())
                        {
                            if (reader.Read() && reader["ActorID"] != DBNull.Value)
                            {
                                // Actor exists, return the ID
                                return Convert.ToInt32(reader["ActorID"]);
                            }
                            else
                            {
                                // Actor does not exist
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
        private int InsertActor(string firstName, string lastName, out string errorMessage)
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
                using (SqlCommand dbCommand = new SqlCommand("InsertActor", connection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    dbCommand.Parameters.Add("@firstName", SqlDbType.VarChar).Value = firstName;
                    dbCommand.Parameters.Add("@lastName", SqlDbType.VarChar).Value = lastName;

                    // Add output parameter for ActorId
                    SqlParameter actorIdParameter = new SqlParameter("@actorId", SqlDbType.Int);
                    actorIdParameter.Direction = ParameterDirection.Output;
                    dbCommand.Parameters.Add(actorIdParameter);

                    try
                    {
                        connection.Open();

                        // Execute the query
                        int rowsAffected = dbCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Successful insertion, return the ActorID
                            return (int)actorIdParameter.Value; ;
                        }
                        else
                        {
                            // Insertion failed
                            errorMessage = "Failed to insert Actor.";
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
