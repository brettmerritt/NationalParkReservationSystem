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
    public class ParkSqlDAOTests : TestInitializer
    {   [TestMethod]
        public void DisplayAvailableParksTest()
        {
            ParkSqlDAO parkSqlDAO = new ParkSqlDAO(connectionString);
            IList<Park> parks = parkSqlDAO.DisplayAvailableParks();
            Assert.IsTrue(parks.Count > 0);
        }

        // should work once we change the return type of selectpark() from IList to Park
        //[TestMethod]
        //public void SelectPark()
        //{
        //    ParkSqlDAO parkSqlDAO = new ParkSqlDAO(connectionString);
        //    Park parks = parkSqlDAO.SelectPark(parkId.ToString());
        //    Assert.AreEqual(parkNameToTest, park.Name);
        //}
    }
}
