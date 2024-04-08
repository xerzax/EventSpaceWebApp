using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Implementation.Repository;
using Infrastructure.Implementation.Services;
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
			services.AddScoped<IEventService, EventService>();
			services.AddScoped<IBlogService, BlogService>();
			services.AddScoped<IPhotoService, PhotoService>();
			services.AddScoped<IFileService, FileService>();
			services.AddScoped<FileSettingConfig>();
			services.AddScoped<IPlaylistService, PlaylistService>();
			services.AddScoped<IEmailService, EmailService>();
			services.AddScoped<ITodoService, TodoService>();
			services.AddScoped<ITierService, TierService>();
			services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IYoutubeService, YouTubeService>();


            return services;
		}

	}

}
