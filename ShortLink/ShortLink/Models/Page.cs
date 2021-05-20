using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ShortLink.Models
{
    public partial class Page
    {
        [Key]
        public long Id { get; set; }
        public string ShortKey { get; set; }
        [Required(ErrorMessage ="لینک را وارد کنید")]
        [DataAnnotationsExtensions.Url(UrlOptions.OptionalProtocol, ErrorMessage = "فرمت لینک وارد شده صحیح نمی‌باشد")]
        public string Link { get; set; }
        public DateTime CreatedDate { get; set; }


        public List<Log> Logs { get; set; }
    }
}
