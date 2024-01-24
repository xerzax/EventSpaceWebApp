using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Base
{
	public class BaseEntity
	{
		public DateTime CreatedAt { get; set; } = DateTime.Now;

		public DateTime LastUpdatedAt { get; set; }

		public DateTime DeletedAt { get; set; }
		public bool IsDeleted { get; set; } = false;
	}
}
