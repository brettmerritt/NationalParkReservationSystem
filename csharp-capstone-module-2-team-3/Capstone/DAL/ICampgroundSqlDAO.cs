using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public interface ICampgroundSqlDAO
    {
        IList<Campground> DisplayParkCampgrounds(int parkId);

        Campground SelectCampground(int CampgroundId);

    }
}
