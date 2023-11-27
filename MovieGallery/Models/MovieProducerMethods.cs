using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MovieGallery.Models
{
    public class MovieProducerMethods
    {
        string connectionString = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = MovieGallery; Integrated Security = True; Connect Timeout = 30; Encrypt=False;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False";

        private SqlConnection CreateConnection()
        {
            SqlConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();
            return dbConnection;
        }
        public List<MovieProducer> GetProducersByMovieId(int movieId, out string errormsg)
        {
            List<MovieProducer> movieProducers = new List<MovieProducer>();

            // Create SQL Connection
            SqlConnection dbConnection = new SqlConnection();

            // Connection to SQL Server
            dbConnection.ConnectionString = connectionString;

            // SQL query to retrieve producers for a specific movie
            string sqlQuery = "SELECT * FROM MovieProducers WHERE MovieID = @movieId";
            SqlCommand dbCommand = new SqlCommand(sqlQuery, dbConnection);
            dbCommand.Parameters.Add("movieId", System.Data.SqlDbType.Int).Value = movieId;

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
                        // Create a MovieProducer object for each row and add it to the list
                        MovieProducer movieProducer = new MovieProducer
                        {
                            MProducerID = Convert.ToInt32(reader["MProducerID"]),
                            ProducerID = Convert.ToInt32(reader["ProducerID"]),
                            MovieID = Convert.ToInt32(reader["MovieID"])
                        };

                        movieProducers.Add(movieProducer);
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

            return movieProducers;
        }

        public int AddProducerToMovie(int movieId, Producer producer, out string errormsg)
        {
            try
            {
                using (SqlConnection dbConnection = CreateConnection())
                {
                    string checkProducerSql = "SELECT ProducerID FROM Producers WHERE FirstName = @firstName AND LastName = @lastName";
                    SqlCommand checkProducerCommand = new SqlCommand(checkProducerSql, dbConnection);
                    checkProducerCommand.Parameters.Add(new SqlParameter("@firstName", SqlDbType.VarChar) { Value = producer.FirstName });
                    checkProducerCommand.Parameters.Add(new SqlParameter("@lastName", SqlDbType.VarChar) { Value = producer.LastName });

                    object existingProducerId = checkProducerCommand.ExecuteScalar();

                    int producerId;

                    if (existingProducerId != null)
                    {
                        producerId = (int)existingProducerId;
                    }
                    else
                    {
                        string insertProducerSql = "INSERT INTO Producers (FirstName, LastName) VALUES (@firstName, @lastName); SELECT SCOPE_IDENTITY();";
                        SqlCommand insertProducerCommand = new SqlCommand(insertProducerSql, dbConnection);
                        insertProducerCommand.Parameters.Add(new SqlParameter("@firstName", SqlDbType.VarChar) { Value = producer.FirstName });
                        insertProducerCommand.Parameters.Add(new SqlParameter("@lastName", SqlDbType.VarChar) { Value = producer.LastName });

                        producerId = Convert.ToInt32(insertProducerCommand.ExecuteScalar());
                    }

                    string insertMovieProducerSql = "INSERT INTO MovieProducers (MovieID, ProducerID) VALUES (@movieId, @producerId)";
                    SqlCommand insertMovieProducerCommand = new SqlCommand(insertMovieProducerSql, dbConnection);
                    insertMovieProducerCommand.Parameters.Add(new SqlParameter("@movieId", SqlDbType.Int) { Value = movieId });
                    insertMovieProducerCommand.Parameters.Add(new SqlParameter("@producerId", SqlDbType.Int) { Value = producerId });

                    int i = insertMovieProducerCommand.ExecuteNonQuery();

                    if (i == 1)
                    {
                        errormsg = "";
                    }
                    else
                    {
                        errormsg = "Failed to add producer to the movie in the Database.";
                    }

                    return i;
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
