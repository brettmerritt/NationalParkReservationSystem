using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Capstone.Models
{
    public class Campground
    {
        // campground Id
        public int Id { get; set; }

        // park Id
        public int ParkId { get; set; }

        // campground name 
        public string Name { get; set; }

        // park is open from this date
        public int OpenFromMonth { get; set; }

        // park is open until this date
        public int OpenToMonth { get; set; }

        //daily fee for campground
        public decimal DailyFee { get; set; }
    }
}
