using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShortLink.Models
{
    public class Log
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Page")]
        public long PageId { get; set; }

        public DateTime Date { get; set; }

        [MaxLength(50)]
        public string Ip { get; set; }

        [MaxLength(50)]
        public string Browser { get; set; }

        [MaxLength(1000)]
        public string UserAgent { get; set; }



        public Page Page { get; set; }
    }
}
