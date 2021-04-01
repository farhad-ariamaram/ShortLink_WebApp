using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShortLink.Models;

namespace ShortLink.Pages
{
    public class ViewModel : PageModel
    {
        private readonly ShortLinkDBContext _db;

        public ViewModel(ShortLinkDBContext db)
        {
            _db = db;
        }

        public Models.Page page { get; set; }

        public IActionResult OnGet(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToPage("Error");
            }

            page = _db.Pages.FirstOrDefault(a => a.ShortKey.Equals(id));

            if (page == null)
            {
                return RedirectToPage("Error");
            }

            return Page();
        }



    }
}
