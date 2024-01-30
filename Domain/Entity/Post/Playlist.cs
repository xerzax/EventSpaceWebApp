using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Post
{
	[Table("Playlist")]
	public class Playlist: Post
	{
		/*[Required(ErrorMessage = "This field is required")]*/
		public virtual ICollection<Song> Songs { get; set; }
	}
}
