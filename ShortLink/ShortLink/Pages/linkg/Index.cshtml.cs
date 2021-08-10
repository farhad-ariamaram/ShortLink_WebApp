using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShortLink.Models;

namespace ShortLink.Pages.linkg
{
    public class IndexModel : PageModel
    {
        private readonly LinkDbContext _context;

        public IndexModel(LinkDbContext context)
        {
            _context = context;
        }

        public string currentlink { get; set; }
        public string currentphone { get; set; }

        public async Task<IActionResult> OnGetAsync(string password, string phone, string sms, string link)
        {

            if (string.IsNullOrEmpty(password))
            {
                ViewData["login"] = null;
                return Page();
            }

            if (password != "123123")
            {
                ViewData["login"] = null;
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
                    var ph = phone.Replace(" ", string.Empty);
                    var t = _context.TblSmsSents.Where(a => a.Phone == ph);
                    if (t.Any())
                    {
                        ViewData["repeated"] = true;
                        ViewData["login"] = true;
                        ViewData["sms"] = "False";
                        currentlink = link;
                        return Page();
                    }

                    var msg = "لینک ثبت نام:\n" + "http://" + link;

                    var smsRes = await send(phone.Replace(" ", string.Empty), msg);
                    if (smsRes)
                    {
                        await _context.AddAsync(new TblSmsSent
                        {
                            Date = DateTime.Now,
                            Phone = ph,
                            Message = link
                        });

                        await _context.SaveChangesAsync();

                        ViewData["login"] = true;
                        ViewData["sms"] = "true";
                        currentlink = link;
                    }
                    else
                    {
                        ViewData["login"] = true;
                        ViewData["sms"] = "False";
                        currentlink = link;
                    }

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
                while (_context.TblLinks.Any(s => s.Id == HashId))
                {
                    HashId = CreateMD5(newphone + DateTime.Now.Ticks);
                }

                TblLink link = new TblLink()
                {
                    Id = HashId,
                    ExpireDate = DateTime.Now.AddDays(1),
                    Phone = newphone
                };

                await _context.TblLinks.AddAsync(link);
                await _context.SaveChangesAsync();

                string realLink = "http://185.118.152.61/" + HashId;
                string[] shortedLink = Shortlink("http://185.118.152.61:8081/api/Page/" + realLink).Split(':');
                string shortlinke = (shortedLink[1] + ":" + shortedLink[2]).Replace("\"", string.Empty).Replace("}", string.Empty);

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

        //send sms
        string baseUrl = "http://185.118.152.61:8081/api/Sms/";
        //string baseUrl = "http://192.168.10.247:5000/api/Sms/";
        public async Task<bool> send(string phone, string body)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri($"{baseUrl}send?phone={phone}&body={body}");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync("");
                    if (response.IsSuccessStatusCode)
                    {
                        string res = await response.Content.ReadAsStringAsync();
                        if (res == "true")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }

            }
        }
    }
}