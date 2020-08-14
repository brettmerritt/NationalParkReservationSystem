using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Park
    {
        // park Id
        public int Id { get; set; }

        // name of park
        public string Name { get; set; }

        // location of park
        public string Location { get; set; }

        // date park was established --DateTime? yyyy-mm-dd format in db
        public DateTime EstablishDate { get; set; }

        // area of park
        public int Area { get; set; }

        // visitors (yearly?)
        public int Visitors { get; set; }

        // park description
        public string Description { get; set; }
    }
}
