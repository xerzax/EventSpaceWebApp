using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Post
{
	[Table("Photo")]
	public class Photo: Post
	{
		public string? Tags { get; set; }
	}
}
