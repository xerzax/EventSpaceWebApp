using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Event
{
    public class DashboardEventResponseDTO
    {
        public string Name { get; set; }

        public int TotalTicketsSold { get; set; }

        public decimal TotalAmountRaised { get; set; }

        public string Venue { get; set; }

        public string Artist { get; set; }

        public decimal DonatedAmount { get; set; }

        public DateTime EventDate { get; set; }

        public int RemainingSeats { get; set; }
    }
}
