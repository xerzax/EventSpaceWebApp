using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Event
{
	public class Tier
	{
		public int Id { get; set; }

		//[Required(ErrorMessage ="Tier name is required")]
		//[StringLength(20, ErrorMessage = "Name length can't be more than 20.")]
		public string? Name { get; set; }
		//[Required(ErrorMessage = "This field is required")]
		public int TotalSeats { get; set; }

		public int AmountRaised { get; set; }
		public int SoldSeats { get; set; }
		public int AvailableSeats { get; set; }

		//[Required(ErrorMessage = "This field is required")]
		public int Price { get; set; }

		[ForeignKey("Event")]
		public int EventId { get; set; }
		public virtual Event Event { get; private set; }

	}
}
