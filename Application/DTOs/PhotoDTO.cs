using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PhotoDTO
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Tags { get; set; }

        public string PhotoName { get; set; }
    }

    public class PhotoDetailsDTO
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Tags { get; set; }

        public string PhotoName { get; set; }

        public string CreatedBy { get; set; }

        public bool IsFollowedByUser { get; set; }

        public Guid CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<PhotoComment> PhotoComments { get; set; }
    }

    public class PhotoComment
    {
        public string CommentedUserImageUrl { get; set; }

        public string CommentedBy { get; set; }

        public string Comment { get; set; }
    }
}