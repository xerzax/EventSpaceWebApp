using EventSpaceApi.Domain.Entity.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Identity
{
	public class UserContext
	{
        public Guid UserId { get; set; }
		public string UserName { get; set; }

        public Role Role { get; set; }
	}
}
