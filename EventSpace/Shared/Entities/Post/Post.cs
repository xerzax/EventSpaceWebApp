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
    public class Post  :BaseEntity
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? PhotoName { get; set; }

        [NotMapped]
        public IFormFile? PhotoUrl { get; set; }
    }
}
