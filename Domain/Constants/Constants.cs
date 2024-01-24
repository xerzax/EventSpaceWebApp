using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants
{
	public abstract class Constants
	{
		public abstract class Roles
		{
			public const string Admin = "Admin";
			public const string Organizer = "Organizer";
			public const string User = "User";
		}

        public abstract class Passwords
        {
			public const string Password = "SuperAdmin@123";
		}
    }
}
