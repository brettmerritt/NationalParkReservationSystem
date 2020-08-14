using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class CampgroundSqlDAO : ICampgroundSqlDAO
    {
        private string connectionString;

        public CampgroundSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Campground> DisplayParkCampgrounds(int parkId)
        {
            List<Campground> campground = new List<Campground>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                   // ConvertReaderToCampground(reader);
                    SqlCommand cmd = new SqlCommand();
                    string sqlStatement = "select * from campground where park_id = @parkId";
                    cmd.Parameters.AddWithValue("@parkId", parkId);
                    cmd.CommandText = sqlStatement;
                    cmd.Connection = connection;
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        Campground campgrounds = ConvertReaderToCampground(reader);
                        campground.Add(campgrounds);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred while searching campgrounds.");
                Console.WriteLine(e.Message);
                throw;
            }
            return campground;
        }
        public /*IList<Campground>*/ Campground SelectCampground(int campgroundId)
        {
            List<Campground> campground = new List<Campground>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("select * from campground where campground_Id = @campgroundId;", connection);
                    cmd.Parameters.AddWithValue("@campgroundId", campgroundId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground campgrounds = ConvertReaderToCampground(reader);
                        return campgrounds;
                        //campground.Add(campgrounds);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred while selecting campground.");
                Console.WriteLine(ex.Message);
                throw;
            }
            return null;
        }

        private Campground ConvertReaderToCampground(SqlDataReader reader)
        {
            Campground campground = new Campground();
            campground.Id = Convert.ToInt32(reader["campground_Id"]);
            campground.ParkId = Convert.ToInt32(reader["park_id"]);
            campground.Name = Convert.ToString(reader["name"]);
            campground.OpenFromMonth = Convert.ToInt32(reader["open_from_mm"]);
            campground.OpenToMonth = Convert.ToInt32(reader["open_to_mm"]);
            campground.DailyFee = Convert.ToDecimal(reader["daily_fee"]);

            return campground;
        }
    }
}
