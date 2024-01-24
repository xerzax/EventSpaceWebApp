using Application.Interfaces.Repository;
using Infrastructure.Data;
using Infrastructure.Implementation.Repository;
using Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dependency
{
	public static class InfrastructureService
	{
		public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("DefaultConnection");

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString,
					b => b.MigrationsAssembly("Infrastructure")));

			services.AddScoped<IDbInitializer, DbInitializer>();

			services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			return services;
		}

	}

}
