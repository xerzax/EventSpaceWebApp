using Domain.Constants;
using EventSpaceApi.Domain.Entity.Identity;
using Domain.Constants;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Seed
{
	public class DbInitializer : IDbInitializer
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly RoleManager<Role> _roleManager;
		private readonly UserManager<User> _userManager;

		public DbInitializer(ApplicationDbContext dbContext, RoleManager<Role> roleManager, UserManager<User> userManager)
		{
			_dbContext = dbContext;
			_roleManager = roleManager;
			_userManager = userManager;
		}

		public async Task Initialize()
		{
			try
			{
				if (!_roleManager.RoleExistsAsync(Constants.Roles.Admin).GetAwaiter().GetResult())
				{
					await _roleManager.CreateAsync(new Role(Constants.Roles.Admin));
					await _roleManager.CreateAsync(new Role(Constants.Roles.User));
				}
				var superAdminUser = new User
				{
					Name = "Super Admin",
					Email = "superadmin@superadmin.com",
					NormalizedEmail = "SUPERADMIN@SUPERADMIN.COM",
					UserName = "superadmin@superadmin.com",
					NormalizedUserName = "SUPERADMIN@SUPERADMIN.COM",
					Address = "Evelyn Street",
					State = "State Somewhere",
					PhoneNumber = "9800000000",
					EmailConfirmed = true,
					PhoneNumberConfirmed = true,
					SecurityStamp = Guid.NewGuid().ToString("D")
				};

				if (!await _userManager.Users.AnyAsync(u => u.UserName == superAdminUser.UserName))
				{
					await _userManager.CreateAsync(superAdminUser, Constants.Passwords.Password);
					await _userManager.AddToRoleAsync(superAdminUser, Constants.Roles.Admin);
				}

				await _dbContext.SaveChangesAsync();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
	}
}

