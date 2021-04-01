using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ShortLink.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShortLink.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ShortLinkDBContext _db;

        public IndexModel(ShortLinkDBContext db)
        {
            _db = db;
        }

        public void OnGet()
        {

        }

        [BindProperty]
        public Models.Page page { get; set; }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            Models.Page p = new Models.Page()
            {
                CreatedDate = DateTime.Now,
                ShortKey = CreateNewShortLink(),
                Link = page.Link
            };

            _db.Pages.Add(p);
            _db.SaveChanges();

            return RedirectToPage("View",new { id=p.ShortKey });
        }

        private string CreateNewShortLink()
        {
            string key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);

            while (_db.Pages.Any(s => s.ShortKey == key))
            {
                key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            }

            return key;
        }
    }
}
