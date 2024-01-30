using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
	public class PlaylistDTO
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string PhotoName { get; set; }
		public Guid UserId { get; set; }
		public string UserName { get; set; }
		public List<SongDTO> Songs { get; set; }
	}
}
