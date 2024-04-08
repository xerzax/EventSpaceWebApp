using Domain.Entity.Post;
using EventSpaceApi.Domain.Entity.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entity.Comment
{
    public class Comment
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int? BlogId { get; set; }
        public int? PhotoId { get; set; }
        public string PostType { get; set; }

        public string Content { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public User User { get; set; }  

        [NotMapped]
        public Domain.Entity.Post.Post Post { get; set; }

        [JsonIgnore]
        public Blog? Blog { get; set; }

        [JsonIgnore]
        public Photo? Photo { get; set; }
    }
}
