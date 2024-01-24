﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace EventSpaceApi.Domain.Entity.Identity;

public sealed class Role : IdentityRole<Guid>
{
    public Role()
    {
        
    }
    
    public Role(string roleName) : base(roleName)
    {
        Name = roleName;
    }
}