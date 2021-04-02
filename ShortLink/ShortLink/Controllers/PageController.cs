using Microsoft.AspNetCore.Mvc;
using ShortLink.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShortLink.Controllers
{
    [Route("api/Page")]
    [ApiController]
    public class PageController : Controller
    {
        private readonly ShortLinkDBContext _db;

        public PageController(ShortLinkDBContext db)
        {
            _db = db;
        }

        [HttpGet("{*Link}")]
        public IActionResult Create(string Link)
        {
            if (string.IsNullOrEmpty(Link))
            {
                return Json(new { data = "Link is invalid" });
            }

            Models.Page p = new Models.Page()
            {
                CreatedDate = DateTime.Now,
                ShortKey = CreateNewShortLink(),
                Link = Link
            };

            _db.Pages.Add(p);
            _db.SaveChanges();

            return Json(new { data = HttpContext.Request.Host + "/p/" + p.ShortKey });
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
