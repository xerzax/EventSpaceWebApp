using Identity.Dependency;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Implementation
{
	public class GetUserByIDService : IGetUserByID
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public GetUserByIDService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public Task<string> UserId
		{
			get
			{
				var userIdClaimValue = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

				return Task.FromResult(userIdClaimValue);
			}
		}
	}
}
