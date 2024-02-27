using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Dependency
{
	public interface IGetUserByID
	{
		Task<string> UserId { get;}
	}
}
