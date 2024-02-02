using EventSpaceApi.Domain.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
	public class PostDTO
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string PhotoName { get; set; }
		public Guid UserId { get; set; }
		public string UserName { get; set; }
		public string PhotoUrl { get; set; }
	}
}
