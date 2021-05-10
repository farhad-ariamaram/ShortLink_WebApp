using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShortLink.Models
{
    public class Link
    {
        [Key]
        public string Id { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string Phone { get; set; }
    }
}
