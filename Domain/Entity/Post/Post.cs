using Domain.Entity.Base;
using EventSpaceApi.Domain.Entity;
using EventSpaceApi.Domain.Entity.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entity.Post
{
	[Table("Post")]
	public class Post : BaseEntity
	{
		public int Id { get; set; }

		//[Required(ErrorMessage = "Title is required")]
		public string? Title { get; set; }
		public string? PhotoName { get; set; }

		[ForeignKey("UserId")]
		public Guid UserId { get; set; }

		[JsonIgnore]
		public User User { get; set; }

		[NotMapped]
		/*[Required(ErrorMessage = "Photo is required")]*/

		[JsonIgnore]
		public IFormFile? PhotoUrl { get; set; }
	}
}
