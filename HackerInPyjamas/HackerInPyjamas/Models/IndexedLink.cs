using System;
using System.Collections.Generic;

#nullable disable

namespace HackerInPyjamas.Models
{
    public partial class IndexedLink
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        public int? PageIndex { get; set; }
    }
}
