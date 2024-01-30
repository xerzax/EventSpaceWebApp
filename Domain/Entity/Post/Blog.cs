using Domain.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Post
{
	[Table("Blog")]
	public class Blog : Post
	{
		/*[Required(ErrorMessage = "Content is required")]*/

		public string? Content { get; set; }
	}
}
