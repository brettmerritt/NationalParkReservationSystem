using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Capstone.Models;
using System.Data.SqlClient;
using Capstone.DAL;

namespace Capstone.Tests
{   [TestClass]
    public class ReservationSqlDAOTests : TestInitializer
    { 
      [TestMethod]
      public void SearchReservationsTest()
      {
            DateTime fromDate = DateTime.Parse("2020-08-10");
            DateTime toDate = DateTime.Parse("2020-08-11");
            
            ReservationSqlDAO reservationSql = new ReservationSqlDAO(connectionString);
            IList<Reservation> reservations = new List<Reservation>();
            reservations = reservationSql.SearchReservations("1", fromDate, toDate);
            Assert.IsTrue(reservations.Count > 0);

      }


        [TestMethod]
        public void AddReservationsTest()
        {
            //string arrival = "2020-07-11";
            //string departure = "2020-07-15";
            DateTime arrival = DateTime.Parse("2020-07-11");
            DateTime departure = DateTime.Parse("2020-07-15");
            ReservationSqlDAO reservationSqlDAO = new ReservationSqlDAO(connectionString);
            Reservation reservation = new Reservation
            {
                SiteId = siteId,
                Name = "Gentry",
                FromDate = arrival,
                ToDate = departure
            };
            int resIdResult = reservationSqlDAO.AddReservations(reservation);
            Assert.IsTrue(resIdResult > 0);
        }
    }
}
