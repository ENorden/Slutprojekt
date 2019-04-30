using System;
using System.Collections.Generic;

namespace Slutprojekt.Models.Entities
{
    public partial class Follower
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FollowerId { get; set; }
    }
}
