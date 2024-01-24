using EventSpaceApi.Domain.Entity.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Event
{
	public class EventWishlist
	{
		public int Id { get; set; }
		public Guid UserId { get; set; }

		[ForeignKey("UserId")]
		public User User { get; set; }
		[ForeignKey("Event")]
		public int EventId { get; set; }
		public virtual Event Event { get; private set; }
	}
}
