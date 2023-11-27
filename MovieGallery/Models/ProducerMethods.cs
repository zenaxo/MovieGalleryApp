using Microsoft.Data.SqlClient;

namespace MovieGallery.Models
{
    public class ProducerMethods
    {
        string connectionString = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = MovieGallery; Integrated Security = True; Connect Timeout = 30; Encrypt=False;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False";
        private SqlConnection CreateConnection()
        {
            SqlConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();
            return dbConnection;
        }
        private bool ProducerExists(string FirstName, string LastName)
        {
            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT COUNT(*) FROM Producers WHERE FirstName = @first_name AND LastName = @last_name";
                using (SqlCommand dbCommand = new SqlCommand(sqlQuery, dbConnection))
                {
                    dbCommand.Parameters.Add("@first_name", System.Data.SqlDbType.VarChar).Value = FirstName;
                    dbCommand.Parameters.Add("@last_name", System.Data.SqlDbType.VarChar).Value = LastName;

                    try
                    {
                        dbConnection.Open();
                        int count = (int)dbCommand.ExecuteScalar();
                        return count > 0;
                    }
                    catch (Exception ex)
                    {
                        // Handle exception (e.g., log it, throw it, etc.)
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                }
            }
        }

    }
}
