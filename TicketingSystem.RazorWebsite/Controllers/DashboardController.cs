using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicketingSystem.Domain.ViewModels;
using TicketingSystem.RazorWebsite.Models.Tickets;

namespace TicketingSystem.RazorWebsite.Controllers
{
    [Authorize(Roles = "Customer,SupportManager")]
    public class DashboardController : Controller
    {
        private readonly ILogger<TicketViewModel> _logger;
        private readonly IMediator _mediator;

        public DashboardController(
            ILogger<TicketViewModel> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
