using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
	public class TierRequestDTO
	{
		public string? Name { get; set; }
		public int TotalSeats { get; set; }
		public int Price { get; set; }

	}
}
