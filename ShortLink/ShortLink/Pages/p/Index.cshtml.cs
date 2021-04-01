using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShortLink.Models;

namespace ShortLink.Pages.p
{
    public class IndexModel : PageModel
    {
        private readonly ShortLinkDBContext _db;

        public IndexModel(ShortLinkDBContext db)
        {
            _db = db;
        }

        public IActionResult OnGet(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToPage("../Error");
            }

            var q = from a in _db.Pages
                    where a.ShortKey.Equals(id)
                    select a;

            if (!q.Any())
            {
                return RedirectToPage("../Error");
            }

            string destination = q.FirstOrDefault().Link;

            if(destination.StartsWith("http") || destination.StartsWith("https"))
            {
                return Redirect(destination);
            }

            return Redirect("http://" + destination);
        }
    }
}
