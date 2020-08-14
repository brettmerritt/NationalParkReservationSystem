using Capstone.DAL;
using Capstone.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the connection string from the appsettings.json file
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("npcampground");

            ICampgroundSqlDAO campgroundDAO = new CampgroundSqlDAO(connectionString);
            IParkSqlDAO parkDAO = new ParkSqlDAO(connectionString);
            IReservationSqlDAO reservationDAO = new ReservationSqlDAO(connectionString);
            ISiteSqlDAO siteDAO = new SiteSqlDAO(connectionString);

            NationalParkCLI cli = new NationalParkCLI(parkDAO, campgroundDAO, siteDAO, reservationDAO);
            cli.RunCLI();
        }
    }
}
