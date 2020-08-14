using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public interface IReservationSqlDAO
    {
        IList<Reservation> SearchReservations(int campgroundId, DateTime arrivalDate, DateTime departureDate);

        int AddReservations(Reservation newReservation);
    }
}
