using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Models;
using TicketingSystem.RazorWebsite.Models;

namespace TicketingSystem.RazorWebsite.Controllers
{
    [Authorize(Roles = "Customer,SupportManager")]
    public class TicketController : Controller
    {
        private readonly ILogger<TicketViewModel> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<IdentityUser> _userManager;

        public TicketController(
            ILogger<TicketViewModel> logger,            
            IMediator mediator,
            UserManager<IdentityUser> userManager)
        {           
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
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
        public async Task<IActionResult> CreateTicket(TicketViewModel model)
        {
            // get current user
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                try
                {                 
                    await _mediator.Send(new CreateTicketCommand { Title = model.Input.Title, Description = model.Input.Description,Type = model.Input.Type});
                }
                catch
                {
                    TempData["error"] = "Sorry, something went wrong, the ticket was not created";
                }
            }
            return View(model);
        }

    }
}
