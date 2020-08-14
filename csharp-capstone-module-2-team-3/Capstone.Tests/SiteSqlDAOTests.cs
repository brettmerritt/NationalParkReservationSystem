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
    public class SiteSqlDAOTests : TestInitializer
    {
        [TestMethod]
        public void GetAvailableSites()
        {
            DateTime arrivalDate = DateTime.Parse("2020-06-16");
            DateTime departureDate = DateTime.Parse("2020-06-20");
            SiteSqlDAO siteSqlDAO = new SiteSqlDAO(connectionString);
            IList<Site> sites = siteSqlDAO.GetAvailableSites(campId.ToString(), arrivalDate, departureDate);
            Assert.IsTrue(sites.Count > 0);
        }

        //Can't quite figure this one out
        [TestMethod]
        //[ExpectedException(typeof(Exception))]
        public void GetAvailableSitesWhenDatesAlreadyBooked()
        {
            DateTime arrivalDate = DateTime.Parse("2020-06-01");
            DateTime departureDate = DateTime.Parse("2020-06-10");
            SiteSqlDAO siteSqlDAO = new SiteSqlDAO(connectionString);
            IList<Site> sites = siteSqlDAO.GetAvailableSites(campId.ToString(), arrivalDate, departureDate);
            Assert.IsTrue(sites.Count == 0);
        }
    }
}
