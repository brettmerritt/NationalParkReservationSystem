using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class ReservationSqlDAO : IReservationSqlDAO
    {
        private string connectionString;
        private string SqlCommand = @"Select top 5 count(reservation.site_id) as reservation_count,site.site_number as sn, site.max_occupancy as mo, site.accessible as sa, site.max_rv_length as mrvl, site.utilities as su, campground.daily_fee as df from site
                    JOIN campground ON site.campground_id = campground.campground_id
                    JOIN reservation on site.site_id = reservation.site_id
                    WHERE site.site_id = @siteID
                    AND site.site_id NOT IN (SELECT site_id	FROM reservation			
                    WHERE from_date < @arrivalDate AND to_date > @departureDate)
                    GROUP BY site.site_number, site.max_occupancy, site.accessible, site.max_rv_length, site.utilities, campground.daily_fee
                    ORDER BY reservation_count desc";
        public ReservationSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        
        public IList<Reservation> SearchReservations(int siteID, DateTime arrivalDate, DateTime departureDate)
        {
            List<Reservation> reservations = new List<Reservation>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(SqlCommand);


//SELECT top 5 site.site_number, site.max_occupancy, site.accessible, site.max_rv_length, site.utilities, campground.daily_fee FROM reservation JOIN site ON site.site_id = reservation.site_id JOIN campground ON campground.campground_id = site.campground_id WHERE site.campground_id = @campgroundId and reservation.from_date NOT BETWEEN @arrivalDate and @departureDate AND reservation.to_date NOT BETWEEN @arrivalDate and @departureDate", connection);
                    cmd.Parameters.AddWithValue("@siteID", siteID);
                    cmd.Parameters.AddWithValue("@arrivalDate", arrivalDate.ToShortDateString());
                    cmd.Parameters.AddWithValue("@departureDate", departureDate.ToShortDateString());                   

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation reservation = ConvertReaderReservation(reader);
                        reservations.Add(reservation);
                    }
                }                               
            } catch (SqlException se)
            {
                Console.WriteLine("There has been an error");
            }
            return reservations;

        }
        
        
        public int AddReservations(Reservation newReservation)
        {
            int newReservationID = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
                
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("insert into reservation values (@site_id, @name, @from_date, @to_date, @create_date);select scope_identity();", connection);
                    //string sqlStatement = "insert into reservation values (@site_id, @name, @from_date, @to_date, @create_date);select scope_identity();";
                    //cmd.CommandText = sqlStatement;
                    cmd.Parameters.AddWithValue("@site_id", newReservation.SiteId);
                    cmd.Parameters.AddWithValue("@name", newReservation.Name);
                    cmd.Parameters.AddWithValue("@from_date", newReservation.FromDate.ToShortDateString());
                    cmd.Parameters.AddWithValue("@to_date", newReservation.ToDate.ToShortDateString());
                    cmd.Parameters.AddWithValue("@create_date", newReservation.CreateDate);

                    newReservationID = Convert.ToInt32(cmd.ExecuteScalar());
                                       
                } catch (SqlException se)
                {
                    Console.WriteLine("There was an error");
                }
            }

            return newReservationID;
        }

        private Reservation ConvertReaderReservation(SqlDataReader reader)
        {
            Reservation reservation = new Reservation();
            reservation.Id = Convert.ToInt32(reader["reservation_id"]);
            reservation.SiteId = Convert.ToInt32(reader["site_id"]);
            reservation.Name = Convert.ToString(reader["name"]);
            reservation.FromDate = Convert.ToDateTime(reader["from_date"]);
            reservation.ToDate = Convert.ToDateTime(reader["to_date"]);
            reservation.CreateDate = Convert.ToDateTime(reader["create_date"]);            

            return reservation;
        }

    }
}
