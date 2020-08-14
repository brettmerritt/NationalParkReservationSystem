﻿using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public interface IParkSqlDAO
    {
        IList<Park> DisplayAvailableParks();

        Park SelectPark(int parkId);
    }
}
