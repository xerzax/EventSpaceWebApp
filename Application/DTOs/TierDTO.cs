using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
	public class TierDTO
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public int TotalSeats { get; set; }
		public int AmountRaised { get; set; }
		public int SoldSeats { get; set; }
		public int AvailableSeats { get; set; }
		public int Price { get; set; }
		public int EventId { get; set; }

	}
}
