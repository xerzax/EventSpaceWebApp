using Application.Interfaces.Identity;
using EventSpaceApi.Domain.Entity.Identity;
using Identity.Implementation;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Dependency
{
	public static class IdentityService
	{
		public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddIdentity<User, Role>(options =>
			{
				options.SignIn.RequireConfirmedAccount = true;
				options.Password.RequireDigit = true;
				options.Password.RequiredLength = 8;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = true;
			}).AddEntityFrameworkStores<ApplicationDbContext>()
			  .AddDefaultTokenProviders();

			services.Configure<IdentityOptions>(options =>
				options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier);

			services.AddHttpContextAccessor();

			services.AddAuthentication();

			services.AddTransient<IUserIdentityService, UserIdentityService>();

			return services;
		}
	}
}
