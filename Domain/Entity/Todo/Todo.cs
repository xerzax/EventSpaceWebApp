using Domain.Entity.Base;
using EventSpaceApi.Domain.Entity;
using EventSpaceApi.Domain.Entity.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Todo
{
	public class Todo: BaseEntity
	{
		public int Id { get; set; }
		public string TaskName { get; set; }
		public DateTime DueDate { get; set; }
		public bool IsCompleted { get; set; } = false;
		public Guid UserId { get; set; }

		[ForeignKey("UserId")]
		public User User { get; set; }
	}
}
