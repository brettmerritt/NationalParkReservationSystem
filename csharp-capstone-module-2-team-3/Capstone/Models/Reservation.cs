using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models 
{
    public class Reservation
    {
        // reservation Id
        public int Id { get; set; }

        // side Id
        public int SiteId { get; set; }

        // reservation name (specifying last name of family)
        public string Name { get; set; }

        // date reservation starts --yyyy-mm-dd
        public DateTime FromDate { get; set; }
        
        // date reservation ends --yyyy-mm-dd
        public DateTime ToDate { get; set; }

        // date reservation was created (NULLABLE)
        public DateTime CreateDate { get; set; }
    }
}
