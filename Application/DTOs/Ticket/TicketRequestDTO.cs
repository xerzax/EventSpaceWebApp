using EventSpaceApi.Domain.Entity.Identity;
using EventVerse.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Ticket
{
	public class TicketRequestDTO
	{

		public int Id { get; set; }
		public int Qty { get; set; }
		public int TotalPrice { get; set; }
		public Boolean isConfirmed { get; set; } = false;
		public string? TierName { get; set; } = "general";
		public EventType TicketType { get; set; }
		public string? Venue { get; set; }
		public string? Eventdate { get; set; }

	}
}
