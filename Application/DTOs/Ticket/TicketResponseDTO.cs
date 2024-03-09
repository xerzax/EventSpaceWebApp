using EventVerse.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Ticket
{
	public class TicketResponseDTO
	{
        public int Qty { get; set; }
		public string TierName { get; set; }
		public int TotalPrice {  get; set; }
		public Boolean isConfirmed { get; set; }
		public EventType TicketType { get; set; }
		public string Venue {  get; set; }
		public string Eventdate { get; set; }

    }
}
