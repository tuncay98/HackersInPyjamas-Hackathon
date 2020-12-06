using System;
using System.Collections.Generic;

#nullable disable

namespace HackerInPyjamas.Models
{
    public partial class UserReport
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ReportedPostId { get; set; }

        public virtual ReportedPost ReportedPost { get; set; }
    }
}
