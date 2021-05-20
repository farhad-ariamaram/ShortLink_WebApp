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

        public IndexModel(ShortLinkDBContext db)
        {
            _db = db;

        }

        public IActionResult OnGet(string id)
        {
            string userAgent = Request.Headers["User-Agent"].ToString().ToLower();
            string ip = HttpContext.Connection.RemoteIpAddress.ToString();
            string browser = GetBrowserNameWithVersion(userAgent);

            if (!_getBrowser(userAgent))
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

            LogUser(q.FirstOrDefault().Id, browser, ip, userAgent);

            string destination = q.FirstOrDefault().Link;

            if (destination.StartsWith("http") || destination.StartsWith("https"))
            {
                return Redirect(destination);
            }

            return Redirect("http://" + destination);
        }

        private bool _getBrowser(string userAgent)
        {
            if (userAgent.IndexOf("chrome") > -1)
            {
                if (userAgent.IndexOf("samsungbrowser") > -1)
                {
                    return false;
                }
                return true;
            }

            if (userAgent.IndexOf("firefox") > -1)
            {
                return true;
            }

            if (userAgent.IndexOf("safari") > -1)
            {
                return true;
            }

            return false;
        }

        private void LogUser(long pageid, string browser, string ip, string ua)
        {
            Log t = new Log()
            {
                PageId = pageid,
                Date = DateTime.Now,
                Browser = browser,
                Ip = ip,
                UserAgent = ua
            };

            _db.Logs.Add(t);
            _db.SaveChanges();
        }

        private string GetBrowserNameWithVersion(string ua)
        {
            return "";
        }
    }
}
