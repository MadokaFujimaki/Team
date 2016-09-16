using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPGruppArbete3
{
    class Booking
    {
        public string Class { get; set; }
        public string Room { get; set; }

        public DateTime BookingDate { get; set; }

        public string Display
        {
            get { return BookingDate.ToString() + ", " + Class + ", " + Room; }
        }
    }
}
