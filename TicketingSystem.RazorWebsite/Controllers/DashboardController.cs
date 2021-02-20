using Microsoft.AspNetCore.Mvc;

namespace TicketingSystem.RazorWebsite.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
