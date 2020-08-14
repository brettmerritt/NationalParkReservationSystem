using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class ParkSqlDAO : IParkSqlDAO
    {
        private string connectionString;
        public ParkSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Park> DisplayAvailableParks()
        {
            List<Park> parks = new List<Park>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("select * from park order by park.name;", connection);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Park park = ConvertReaderToPark(reader);
                        parks.Add(park);
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("An error occurred communicating with the database.");
                Console.WriteLine(e.Message);
                throw;
            }
            return parks;
        }


        public Park SelectPark(int parkId)
        {
            List<Park> parks = new List<Park>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("select * from park where park_id = @parkId", connection);
                    cmd.Parameters.AddWithValue("@parkId", parkId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Park park = ConvertReaderToPark(reader);
                        return park;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred communicating with the database. ");
                Console.WriteLine(e.Message);
                throw;
            }
            return null;
        }

        /*                using (SqlConnection connection = new SqlConnection(connectionString))
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
                }*/


        private Park ConvertReaderToPark(SqlDataReader reader)
        {
            Park park = new Park();
            park.Id = Convert.ToInt32(reader["park_id"]);
            park.Name = Convert.ToString(reader["name"]);
            park.Location = Convert.ToString(reader["location"]);
            park.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
            park.Area = Convert.ToInt32(reader["area"]);
            park.Visitors = Convert.ToInt32(reader["visitors"]);
            park.Description = Convert.ToString(reader["description"]);

            return park;
        }
    }
}
