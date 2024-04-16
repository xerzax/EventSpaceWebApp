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

		
		public int Qty { get; set; }
		
		public string? TierName { get; set; }
		public int EventId { get; set; }

	}


	public class CreateTicketDTO
	{


		public Guid UserId  { get; set; }

		public int Qty { get; set; }
		public string? TierName { get; set; }
		public int EventId { get; set; }
		public string? TicketCode { get; set; }
		public int TotalPrice { get; set; }
		public string? Venue { get; set; }
		public string? Date { get; set; }




	}

	public class PaymentRequestDTO
		{

        public int? TierId { get; set; }

        public int EventId { get; set; }
		public int Qty { get; set; } = 1;

	}



}
