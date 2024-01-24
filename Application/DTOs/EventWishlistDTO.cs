using EventSpaceApi.Domain.Entity.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
	public class EventWishlistDTO
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public int EventId { get; set; }
	}

}
