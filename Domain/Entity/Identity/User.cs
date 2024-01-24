using Microsoft.AspNetCore.Identity;


namespace EventSpaceApi.Domain.Entity.Identity;


public class User : IdentityUser<Guid>
{
	public string? Name { get; set; }
	public string? ProfilePicture { get; set; }

    public string? Address { get; set; }

    public string? State { get; set; }

    public string? ImageURL { get; set; }
}