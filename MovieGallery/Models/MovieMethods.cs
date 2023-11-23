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
    }
}
