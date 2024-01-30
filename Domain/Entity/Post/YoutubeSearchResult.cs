using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Post
{
	[Table("YoutubeSearchResult")]
	public class YoutubeSearchResult
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public string ThumbnailUrl { get; set; }
		public string VideoId { get; set; }
		public string ChannelTitle { get; set; }
	}
}
