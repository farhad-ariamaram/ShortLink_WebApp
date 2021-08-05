using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShortLink.Models;

namespace ShortLink.Pages.linkg
{
    public class IndexModel : PageModel
    {
        private readonly LinkDBContext _context;

        public IndexModel(LinkDBContext context)
        {
            _context = context;
        }

        public string currentlink { get; set; }
        public string currentphone { get; set; }

        public async Task<IActionResult> OnGetAsync(string password, string phone, string sms, string link)
        {
            if (string.IsNullOrEmpty(password))
            {
                ViewData["login"] = false;
                return Page();
            }

            if (password != "123123")
            {
                ViewData["login"] = false;
                ViewData["wrongp"] = false;
                return Page();
            }

            if (string.IsNullOrEmpty(phone))
            {
                ViewData["login"] = true;
                currentlink = null;
                return Page();
            }

            currentphone = phone;

            if (!phone.Contains('+') || phone.Length < 13)
            {
                ViewData["login"] = true;
                ViewData["wrongf"] = false;
                currentlink = null;
                return Page();
            }

            if (!string.IsNullOrEmpty(sms))
            {
                if (sms == "1")
                {
                    ViewData["login"] = true;
                    ViewData["sms"] = "true";
                    currentlink = link;
                    return Page();
                }
            }

            ViewData["login"] = true;

            var currentlink0 = await GenerateShortLink(phone);
            if (currentlink0 != null)
            {
                currentlink = currentlink0;
            }
            else
            {
                ViewData["wrongl"] = false;
                currentlink = null;
                return Page();
            }

            return Page();
        }

        public async Task<string> GenerateShortLink(string phone)
        {
            try
            {
                if (!phone.Contains('+') || phone.Length < 13)
                {
                    return null;
                }
                string newphone = phone.Replace(" ", string.Empty);
                newphone = newphone.Substring(newphone.IndexOf('+'), 13);
                string HashId = CreateMD5(newphone + DateTime.Now.Ticks);
                while (_context.Links.Any(s => s.Id == HashId))
                {
                    HashId = CreateMD5(newphone + DateTime.Now.Ticks);
                }

                Link link = new Link()
                {
                    Id = HashId,
                    ExpireDate = DateTime.Now.AddDays(1),
                    Phone = newphone
                };

                await _context.Links.AddAsync(link);
                await _context.SaveChangesAsync();

                string realLink = "http://185.118.152.61/" + HashId;
                string[] shortedLink = Shortlink("http://185.118.152.61:8081/api/Page/" + realLink).Split(':');
                string shortlinke = "http://microsoftrazorpage.blogfa.com/page/hello?url=http://" + (shortedLink[1] + ":" + shortedLink[2]).Replace("\"", string.Empty).Replace("}", string.Empty);

                return shortlinke;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public async Task<IActionResult> OnGetSendSmsAsync()
        {
            return Page();
        }

        //Generating hash from string => used in generating ID for user registeration
        private static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        //Convert link to Shortlink
        private static string Shortlink(string url)
        {
            var request = WebRequest.Create(url);
            string text;
            request.ContentType = "application/json; charset=utf-8";
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            return text;
        }


    }
}