using EventSpaceApi.Domain.Entity;
using EventVerse.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Ticket
{
	public class TicketInfo
	{
		public int Id { get; set; }
		public DateTime EventDate { get; set; }
		public string Venue { get; set; }

		public int qty { get; set; }
		public string? Seat { get; set; }
	}
}
