using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Comment
{
	public class TotalCommentResponseDTO
	{
		public int PostId { get; set; }
		public int TotalComments { get; set; }

	}
}
