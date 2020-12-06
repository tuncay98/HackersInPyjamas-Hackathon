using System;
using System.Collections.Generic;

#nullable disable

namespace HackerInPyjamas.Models
{
    public partial class ReportedPost
    {
        public ReportedPost()
        {
            UserReports = new HashSet<UserReport>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UserReport> UserReports { get; set; }
    }
}
