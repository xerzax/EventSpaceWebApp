using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Event_DTO
{
    public class EventResponseDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Venue { get; set; }
        public DateTime Date { get; set; }
        public string? Artist { get; set; }
        public decimal StartingPrice { get; set; }
        public int TotalSeats { get; set; }
        public int SoldSeats { get; set; }
        public int AmountRaised { get; set; }
        public int AvailableSeats { get; set; }

    }
    public class EventCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TicketsSoldByCategory
    {
        public string CategoryName { get; set; }
        public int TicketsSold { get; set; }
    }

    public class TicketsSoldByEvent
    {
        public string Name { get; set; }
        public int TicketsSold { get; set; }
        public int TotalTickets { get; set; }
        public int AvailableTickets { get; set; }


    }
}
