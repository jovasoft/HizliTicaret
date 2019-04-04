using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ContactViewModel contactViewModel)
        {
            if (ModelState.IsValid)
            {
                string role = string.Empty;

                if (User.IsInRole("Admin")) role = "Admin";
                else if (User.IsInRole("Merchant")) role = "Satıcı";
                else if (User.IsInRole("User")) role = "Kullanıcı";
                else role = "Ziyaretçi";

                string subject = "Bir " + role + " Size Mesaj Gönderdi.";

                string message = contactViewModel.Message + Environment.NewLine + Environment.NewLine + "İsim: " + contactViewModel.Name + Environment.NewLine + "Mail: " + contactViewModel.Mail + Environment.NewLine + "Tarih: " + DateTime.Now;

                SmtpClient client = new SmtpClient("smtp.live.com", 587);
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("hizliticaret00@outlook.com", "lbrLwy2fnu");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("hizliticaret00@outlook.com");
                mailMessage.To.Add("admin@etoraman.com");
                mailMessage.Body = message;
                mailMessage.Subject = subject;
                client.Send(mailMessage);

                ViewData["success"] = true;
                return View();
            }

            ViewData["success"] = false;
            return View();
        }
    }
}