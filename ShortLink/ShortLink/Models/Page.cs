using System;
using System.Collections.Generic;

#nullable disable

namespace ShortLink.Models
{
    public partial class Page
    {
        public long Id { get; set; }
        public string ShortKey { get; set; }
        public string Link { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
