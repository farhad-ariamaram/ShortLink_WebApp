using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShortLink.Models;
using Wangkanai.Detection.Services;

namespace ShortLink.Pages.p
{
    public class IndexModel : PageModel
    {
        private readonly ShortLinkDBContext _db;

        private readonly IDetectionService _detectionService;

        public IndexModel(ShortLinkDBContext db, IDetectionService detectionService)
        {
            _db = db;
            _detectionService = detectionService;

        }

        public IActionResult OnGet(string id)
        {
            if (!(_detectionService.Browser.Name.ToString().ToLower().Equals("chrome") ||
                _detectionService.Browser.Name.ToString().ToLower().Equals("firefox") ||
                _detectionService.Browser.Name.ToString().ToLower().Equals("safari")))
            {
                return Page();
            }

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

            if (destination.StartsWith("http") || destination.StartsWith("https"))
            {
                return Redirect(destination);
            }

            return Redirect("http://" + destination);
        }
    }
}
