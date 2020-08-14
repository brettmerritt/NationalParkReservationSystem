using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Site
    {
        // site Id
        public int Id { get; set; }

        // campground Id 
        public int CampgroundId { get; set; }

        // site Number 
        public int SiteNumber { get; set; }

        // max occupancy of site
        public int MaxOccupancy { get; set; }

        // accessible 0 for no 1 for yes
        public bool Accessible { get; set; }

        // max RV length for those who call themselves "campers" but really aren't
        public int MaxRVLength { get; set; }

        // utilities 0 for no 1 for yes
        public bool Utilities { get; set; }
    }
}
