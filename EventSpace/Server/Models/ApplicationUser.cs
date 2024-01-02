using EventSpace.Shared;
using Microsoft.AspNetCore.Identity;

namespace EventSpace.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<SuperHero> SuperHeroes { get; set; } = new List<SuperHero>();

    }
}