using Duende.IdentityServer.EntityFramework.Options;
using EventSpace.Server.Models;
using EventSpace.Shared;
using EventSpace.Shared.Entities.Post;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EventSpace.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {

        }
        
        public DbSet<Blog> Blog => Set<Blog>();
        public DbSet<Photo> Photo => Set<Photo>();



    }
}