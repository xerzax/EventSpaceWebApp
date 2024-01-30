using Domain;
using Domain.Entity.Event;
using Domain.Entity.Post;
using EventSpaceApi.Domain.Entity.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{

	public sealed class ApplicationDbContext : IdentityDbContext<User, Role, Guid, UserClaims, UserRoles, UserLogin, RoleClaims, UserToken>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
			Database.EnsureCreated();
		}

		#region Identity Tables
		public DbSet<User> Users { get; set; }

		public DbSet<Role> Roles { get; set; }

		public DbSet<UserRoles> UserRoles { get; set; }

		public DbSet<UserToken> UserToken { get; set; }

		public DbSet<UserLogin> UserLogin { get; set; }

		public DbSet<UserClaims> UserClaims { get; set; }

		public DbSet<RoleClaims> RoleClaims { get; set; }

		public DbSet<Event> Events { get; set; }

		public DbSet<EventWishlist> Eventwishlists { get; set; }

		public DbSet<Tier> Tiers { get; set; }

		public DbSet<Donation> Donations { get; set; }

		/*public DbSet<Post> Posts { get; set; }*/
		public DbSet<Blog> Blogs { get; set; }
		public DbSet<Photo> Photos { get; set; }
		public DbSet<Playlist> Playlists { get; set; }
		public DbSet<Song> Songs { get; set; }
		#endregion

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			base.OnModelCreating(builder);

			#region Identity Entities Configuration
			builder.Entity<User>().ToTable("Users");
			builder.Entity<Role>().ToTable("Roles");
			builder.Entity<UserToken>().ToTable("Tokens");
			builder.Entity<UserRoles>().ToTable("UserRoles");
			builder.Entity<RoleClaims>().ToTable("RoleClaims");
			builder.Entity<UserClaims>().ToTable("UserClaims");
			builder.Entity<UserLogin>().ToTable("LoginAttempts");
			builder.Entity<Event>()
				.HasOne(e => e.User)
				.WithMany()
				.HasForeignKey(e => e.UserId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.Entity<Donation>()
				.HasOne(d => d.User)
				.WithMany()
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.Restrict);


			builder.Entity<EventWishlist>()
				.HasOne(ew => ew.User)
				.WithMany()
				.HasForeignKey(ew => ew.UserId)
				.OnDelete(DeleteBehavior.Restrict);
			#endregion
		}
	}
}
