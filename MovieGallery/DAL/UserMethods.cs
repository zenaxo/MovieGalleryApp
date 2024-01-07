using Microsoft.Data.SqlClient;
using MovieGallery.Models;
using System.Data;

namespace MovieGallery.DAL
{
    public class UserMethods
    {
        string connectionString = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = MovieGallery; Integrated Security = True; Connect Timeout = 30; Encrypt=False;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False";

        // Fixa så att den returnerar den user som man loggar in med om giltig, annars returnera null. Detta iom isAdmin skiten.
        public bool CheckUser(UserModel user, out string errormsg)
        {
            errormsg = "";

            using(SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("GetUsers", dbConnection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.Add("@userName", SqlDbType.VarChar).Value = user.UserName;
                    dbCommand.Parameters.Add("@password", SqlDbType.VarChar).Value = user.Password;

                    try
                    {
                        dbConnection.Open();

                        using (SqlDataReader reader = dbCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                return true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        errormsg = e.Message;
                    }
                }
            }

            return false;
        }
    }
}
