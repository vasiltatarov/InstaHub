namespace InstaHub.Web.Controllers
{
    using InstaHub.Web.ViewModels.Contacts;
    using Microsoft.AspNetCore.Mvc;

    public class ContactsController : Controller
    {
        private const string RedirectedFromContactForm = "RedirectedFromContactForm";

        public IActionResult Index() => this.View();

        [HttpPost]
        public IActionResult Index(ContactFormViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            //var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();

            this.TempData[RedirectedFromContactForm] = true;

            return this.RedirectToAction("ThankYou");
        }

        public IActionResult ThankYou()
            => this.TempData[RedirectedFromContactForm] == null
                ? this.NotFound()
                : this.View();
    }
}
