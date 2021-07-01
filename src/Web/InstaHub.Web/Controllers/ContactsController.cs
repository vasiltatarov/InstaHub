namespace InstaHub.Web.Controllers
{
    using System.Threading.Tasks;

    using InstaHub.Common;
    using InstaHub.Services.Data;
    using InstaHub.Services.Messaging;
    using InstaHub.Web.ViewModels.Contacts;
    using Microsoft.AspNetCore.Mvc;

    using static InstaHub.Common.GlobalConstants;

    public class ContactsController : Controller
    {
        private const string RedirectedFromContactForm = "RedirectedFromContactForm";

        private readonly IContactService contactService;
        private readonly IEmailSender emailSender;

        public ContactsController(IContactService contactService, IEmailSender emailSender)
        {
            this.contactService = contactService;
            this.emailSender = emailSender;
        }

        public IActionResult Index() => this.View();

        [HttpPost]
        public async Task<IActionResult> Index(ContactFormViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
            await this.contactService.Add(model.Name, model.Email, model.Message, ip);

            await this.emailSender.SendEmailAsync(model.Email, model.Name, MyEmail, "Missing", model.Message);

            this.TempData[RedirectedFromContactForm] = true;

            return this.RedirectToAction("ThankYou");
        }

        public IActionResult ThankYou()
            => this.TempData[RedirectedFromContactForm] == null
                ? this.NotFound()
                : this.View();
    }
}
