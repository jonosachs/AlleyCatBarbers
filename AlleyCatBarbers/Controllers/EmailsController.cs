using Microsoft.AspNetCore.Mvc;
using AlleyCatBarbers.Models;
using AlleyCatBarbers.Services;


namespace AlleyCatBarbers.Controllers
{
    public class EmailsController : Controller
    {
        private readonly IEmailSender _emailSender;

        public EmailsController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult SendEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(Email model)
        {
            if (ModelState.IsValid)
            {
                
                var (EmailSent, Message) = await _emailSender.SendEmailAsync(model.To, model.Subject, model.Message);

                if (EmailSent)
                {
                    ViewBag.EmailSent = true;
                    ViewBag.Message = Message;
                }
                else
                {
                    ViewBag.EmailSent = false;
                    ViewBag.Error = Message;
                }
            }
            else
            {
                ViewBag.Error = "Invalid form data";
                ViewBag.EmailSent = false;
            }

            return View(model);
        }

    }
}
