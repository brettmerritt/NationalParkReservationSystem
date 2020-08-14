using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Capstone.Models;
using System.Data.SqlClient;
using Capstone.DAL;

namespace Capstone.Tests
{   
    [TestClass]
    public class CampgroundSqlDAOTests : TestInitializer
    {
        [TestMethod]
        public void DisplayParkCampgrounds()
        {
            CampgroundSqlDAO campgroundSqlDAO = new CampgroundSqlDAO(connectionString);
            IList<Campground> campgrounds = campgroundSqlDAO.DisplayParkCampgrounds(parkId.ToString());
            Assert.IsTrue(campgrounds.Count > 0);
        }

        [TestMethod]
        public void SelectCampgroundTest()
        {
            CampgroundSqlDAO campgroundSqlDAO = new CampgroundSqlDAO(connectionString);
            Campground campground = campgroundSqlDAO.SelectCampground(campId.ToString());
            Assert.AreEqual(campgroundNameToTest, campground.Name);
        }
    }
}
