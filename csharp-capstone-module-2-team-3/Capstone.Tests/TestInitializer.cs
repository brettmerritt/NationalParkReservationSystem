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
        public class TestInitializer
        {
            protected TransactionScope transactionScope;
            protected string connectionString = @"Data Source=.\SQLExpress;Initial Catalog=npcampground;Integrated Security=true";
            protected string parkNameToTest = "Yellowstone";
            protected int parkCount = 0;
            protected int parkId = 0;
            protected string campgroundNameToTest = "stick city";
            protected int campCount = 0;
            protected decimal campFee = 400;
            protected int campId = 0;
            protected int siteId = 0;
            protected int resId = 0;    
            protected  DateTime arrivalDate = DateTime.Parse("2020-06-01");
            protected DateTime departureDate = DateTime.Parse("2020-06-10");

        [TestInitialize]
            public void Initialize()
            {
                //intitialize transaction scope
                transactionScope = new TransactionScope();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //open connection
                    conn.Open();
                    //set command text
                    string sqlSelect = $"select count(name) from park where name = '{parkNameToTest}'";
                    //instantiate command object, set the connection
                    SqlCommand sqlCommand = new SqlCommand(sqlSelect, conn);
                    //get/set data
                    parkCount = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    //if exists, we're good. If not, add it
                    if (parkCount == 0)
                    {
                        string sqlInsert = $"insert into park values ('{parkNameToTest}', 'Earth', '2020-09-15', 51, 1, 'Number one campsite, everything is great here, lots of sticks')";
                        sqlCommand = new SqlCommand(sqlInsert, conn);
                        parkId = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    }

                 //grab parkId for parkNameToTest
                 string selectId = $"select park_id from park where name = '{parkNameToTest}'";
                 sqlCommand = new SqlCommand(selectId, conn);
                 parkId = Convert.ToInt32(sqlCommand.ExecuteScalar());

                 //make sure test campground is associated with test park
                 string sqlSelectCampground = $"select count(name) from campground where park_id = '{parkId}' and name = '{campgroundNameToTest}'";
                 sqlCommand = new SqlCommand(sqlSelectCampground, conn);
                 campCount = Convert.ToInt32(sqlCommand.ExecuteScalar());
                 // it probably doesn't exist as far as I know there aren't any campgrounds called stick city but i've been wrong before and I'll be wrong again so let's add it
                     if (campCount == 0)
                     {
                         string sqlInsert = $"insert into campground values ({parkId}, '{campgroundNameToTest}', 1, 2, {campFee:C2})";
                         sqlCommand = new SqlCommand(sqlInsert, conn);
                         campId = Convert.ToInt32(sqlCommand.ExecuteScalar());
                     }

                 //grab campgroundId for campNameToTest
                 string grabId = $"select campground_id from campground where name = '{campgroundNameToTest}'";
                 SqlCommand command = new SqlCommand(grabId, conn);
                 campId = Convert.ToInt32(command.ExecuteScalar());


                 //now let's add a site to our campground
                 string insertSite = $"insert into site values ({campId}, 42, 2, 0, 0, 0)";
                 sqlCommand = new SqlCommand(insertSite, conn);
                 //grab site ID
                 siteId = Convert.ToInt32(sqlCommand.ExecuteScalar());

                //now let's add a reservation 
                
                    try
                    {
                        

                         string insertRes = $"insert into reservation values('{siteId}', Gentry, {arrivalDate.ToShortDateString()}, {departureDate.ToShortDateString()})";
                         sqlCommand = new SqlCommand(insertRes, conn);
                         resId = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    } catch (Exception e)
                    {
                    }
                }
            }
            [TestCleanup]
            public void Cleanup()
            {
                transactionScope.Dispose();
            }

        }
    
}
