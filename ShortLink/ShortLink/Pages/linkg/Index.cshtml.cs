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

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnGetPhoneAsync(string phone)
        {
            string newphone = phone.Replace(" ", string.Empty);
            newphone = newphone.Substring(newphone.IndexOf('+'),13);
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

            _context.Links.Add(link);
            _context.SaveChanges();

            string realLink = "http://185.118.152.61/" + HashId;
            string[] shortedLink = Shortlink("http://185.118.152.61:8081/api/Page/" + realLink).Split(':');
            string shortlinke = "http://" + (shortedLink[1] + ":" + shortedLink[2]).Replace("\"", string.Empty).Replace("}", string.Empty);

            return new JsonResult(shortlinke);
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