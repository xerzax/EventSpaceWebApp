using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Comment
{
	public class CommentRequestDTO
	{
		public int PostId { get; set; }
		public string PostType { get; set; }

		public string Content { get; set; }
	}
}
