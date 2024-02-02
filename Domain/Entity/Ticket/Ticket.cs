using EventSpaceApi.Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using EventVerse.Core.Enums;
//using EventSpaceApi.Domain.Entity;
using EventSpaceApi.Domain.Entity.Identity;

namespace Domain.Entity.Ticket
{
	public class Ticket
	{
		public int Id { get; set; }
		public int Qty { get; set; }
		public int TotalPrice { get; set; }
		public Boolean isConfirmed { get; set; } = false;
		public string? TierName { get; set; } = "general";

		public EventType TicketType { get; set; }
		public string? Venue { get; set; }

		public string? Eventdate { get; set; }

		public Guid UserId { get; set; }


		[ForeignKey("UserId")]
		public User User { get; set; }
		public int? EventId { get; set; }

		[ForeignKey("EventId")]
		public virtual Domain.Entity.Event.Event Event { get; private set; }

	}
}
