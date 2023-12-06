using Microsoft.Data.SqlClient;
using MovieGallery.Models;
using System.Data;

namespace MovieGallery.DAL
{
    public class UserMethods
    {
        string connectionString = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = MovieGallery; Integrated Security = True; Connect Timeout = 30; Encrypt=False;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False";

        public List<UserModel> GetUserList(out string errormsg)
        {
            errormsg = "";
            List<UserModel> userList = new List<UserModel>();

            using(SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand dbCommand = new SqlCommand("GetUsers", dbConnection))
                {
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        dbConnection.Open();
                         
                        using (SqlDataReader reader = dbCommand.ExecuteReader())
                        {
                            if(reader.HasRows)
                            {
                                UserModel dbUser = new UserModel
                                {
                                    UserName = reader["Username"].ToString(),
                                    Password = reader["Passwrd"].ToString()
                                };
                                userList.Add(dbUser);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        errormsg = e.Message;
                    }
                }
            }

            return userList;
        }
    }
}
