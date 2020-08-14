using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class SiteSqlDAO : ISiteSqlDAO
    {
        private string connectionString;
        /*private string SqlSiteQuery = @"SELECT TOP 5 * FROM site
                                            WHERE site.campground_id = @campgroundId
                                            AND site.site_id NOT IN (SELECT site_id	
					                        FROM reservation			
					                        WHERE from_date < @departureDate AND to_date > @arrivalDate)";*/

        private string SqlSiteQuery = @"Select top 5 count(reservation.site_id) as reservation_count,site.site_id, site.site_number, site.max_occupancy, site.accessible, site.max_rv_length, site.utilities, campground.campground_id, campground.daily_fee from site
            JOIN campground ON campground.campground_id = site.campground_id
            join reservation on site.site_id = reservation.site_id
            WHERE campground.campground_id = @campgroundId
            AND site.site_id NOT IN (SELECT site_id	FROM reservation			
            WHERE from_date < @departureDate AND to_date > @arrivalDate)
            group by site.site_id, site.site_number, campground.campground_id, site.max_occupancy, site.accessible, site.max_rv_length, site.utilities, campground.daily_fee
            order by reservation_count desc";
      
        public SiteSqlDAO(string dbConnectionString)
        {           
            connectionString = dbConnectionString;
        }

        public IList<Site> GetAvailableSites(int campgroundId, DateTime arrivalDate, DateTime departureDate)
        {
            List<Site> sites = new List<Site>();
            

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand(SqlSiteQuery, connection);
                    cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
                    cmd.Parameters.AddWithValue("@arrivalDate", arrivalDate.ToShortDateString());
                    cmd.Parameters.AddWithValue("@departureDate", departureDate.ToShortDateString());

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site site = ConvertReaderToSite(reader);
                        sites.Add(site);
                    }
                }
            }
            catch (SqlException se)
            {
                Console.WriteLine("There has been an error");
            }
            return sites;

        }

        private Site ConvertReaderToSite(SqlDataReader reader)
        {
            Site site = new Site();
            site.Id = Convert.ToInt32(reader["site_id"]);
            site.CampgroundId = Convert.ToInt32(reader["campground_id"]);
            site.SiteNumber = Convert.ToInt32(reader["site_number"]);
            site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
            site.Accessible = Convert.ToBoolean(reader["accessible"]);
            site.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
            site.Utilities = Convert.ToBoolean(reader["utilities"]);

            return site;
        }
    }
}
