namespace InstaHub.Web.Controllers
{
    using System.Diagnostics;

    using InstaHub.Web.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        public IActionResult Index() => this.View();

        public IActionResult Privacy() => this.View();

        public IActionResult AboutUs() => this.View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
    }
}
