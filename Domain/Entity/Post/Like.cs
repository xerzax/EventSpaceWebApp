using EventSpaceApi.Domain.Entity.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entity.Post
{
	public class Like
	{
		public int Id { get; set; }
		public Guid UserId { get; set; }
		public int PostId { get; set; }
		public string PostType { get; set; }

		[JsonIgnore]
		[ForeignKey("UserId")]
		public User User { get; set; }

		[NotMapped]
		public Post Post { get; set; }

		[JsonIgnore]
		public Blog? Blog { get; set; }

		[JsonIgnore]
		public Photo? Photo { get; set; }
	}
}
