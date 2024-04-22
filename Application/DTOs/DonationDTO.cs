using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
	public class DonationDTO
	{
		public int Id { get; set; }
		public int AmountDonated { get; set; }
		public string UserId { get; set; }
		public int EventId { get; set; }

	}

    public class DonationResponseDTO
    {
        public int AmountDonated { get; set; }
        public string EventName { get; set; }

    }
}
