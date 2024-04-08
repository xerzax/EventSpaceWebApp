using EventSpaceApi.Domain.Entity.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Follow
{
    public class UserFollowings
    {
        public Guid FollowerId { get; set; }
        public Guid FollowingId { get; set; }

        [ForeignKey("FollowerId")]
        public User Follower { get; set; }

        [ForeignKey("FollowingId")]
        public User Following { get; set; }
    }
}
