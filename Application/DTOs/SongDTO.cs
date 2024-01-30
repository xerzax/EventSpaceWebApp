using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
	public class SongDTO
	{
		public int Id { get; set; }
		public string VideoCode { get; set; }
		public string Title { get; set; }
		public int PlaylistId { get; set; }
		public string PlaylistTitle { get; set; }
	}
}
