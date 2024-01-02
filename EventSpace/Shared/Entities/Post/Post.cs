using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EventSpace.Shared.Entities.Post
{
    public class Post
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "Title is required")]

        public string? Title { get; set; }
        public string? PhotoName { get; set; }

        //public int UserId { get; set; }

        //[ForeignKey("UserId")]
        //public SuperHero AppUser { get; set; }


        [NotMapped]
        [Required(ErrorMessage = "Photo is required")]

        public IFormFile? PhotoUrl { get; set; }
    }
}
