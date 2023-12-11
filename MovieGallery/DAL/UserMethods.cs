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
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    UserModel readUser = new UserModel
                                    {
                                        UserName = reader["Username"].ToString(),
                                        Password = reader["Passwrd"].ToString()
                                    };
                                    userList.Add(readUser);
                                }
                            }
                        }

                        foreach(UserModel dbUser in userList)
                        {
                            if(dbUser.UserName == user.UserName && dbUser.Password == user.Password)
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
