using EventVerse.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
	public class EventDTO
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
		public string? EventPhoto {  get; set; }
        public List<TierDTO> TierList { get; set; }

    }
}
