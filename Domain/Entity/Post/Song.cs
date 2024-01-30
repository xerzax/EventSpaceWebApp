using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Post
{
	[Table("Song")]
	public class Song
	{
		public int Id { get; set; }
		public string? VideoCode { get; set; }
		public string? Title { get; set; }

		[ForeignKey("Playlist")]
		public int PlaylistId { get; set; }
		public virtual Playlist Playlist { get; set; }
	}
}
