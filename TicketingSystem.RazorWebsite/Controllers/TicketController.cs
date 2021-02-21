using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicketingSystem.RazorWebsite.Models;

namespace TicketingSystem.RazorWebsite.Controllers
{
    [Authorize(Roles = "Customer,SupportManager")]
    public class TicketController : Controller
    {

        private readonly ILogger<TicketViewModel> _logger;
        private readonly IMediator _mediator;

        public TicketController(
            ILogger<TicketViewModel> logger,            
            IMediator mediator)
        {           
            _logger = logger;
            _mediator = mediator;
        }

        [Authorize(Roles = "Customer,SupportManager")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Customer,SupportManager")]
        [HttpGet]
        public IActionResult CreateTicket()
        {
            return View();
        }

        [Authorize(Roles = "Customer,SupportManager")]
        [HttpPost]
        public IActionResult CreateTicket(TicketViewModel model)
        {
            return View(model);
        }

    }
}
