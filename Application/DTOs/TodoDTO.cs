using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
	public class TodoDTO
	{
		public string TaskName { get; set; }
		public DateTime DueDate { get; set; }
		public bool IsCompleted { get; set; } = false;
	}
}
