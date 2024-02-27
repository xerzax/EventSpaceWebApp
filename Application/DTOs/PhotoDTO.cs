using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
	public class PhotoDTO
	{
		public string? Title { get; set; }
		public string? Tags { get; set; }
		public IFormFile Photo { get; set; }
		public string PhotoName { get; set; }

	}
}
