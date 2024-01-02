using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSpace.Shared.Entities.Post
{
    public class Blog
    
        : Post
    {
        [Required(ErrorMessage = "Content is required")]

        public string? Content { get; set; }
    }
}
