using System;
using System.Collections.Generic;

#nullable disable

namespace ShortLink.Models
{
    public partial class TblSmsSent
    {
        public long Id { get; set; }
        public string Phone { get; set; }
        public DateTime? Date { get; set; }
        public string Message { get; set; }
    }
}
