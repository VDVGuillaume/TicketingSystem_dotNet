using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;
using TicketingSystem.Domain.ViewModels;
using TicketingSystem.RazorWebsite.Models;
using TicketingSystem.RazorWebsite.Models.Tickets;

namespace TicketingSystem.RazorWebsite.Controllers
{
    [Authorize(Roles = "Customer,SupportManager")]
    public class DashboardController : Controller
    {
        private readonly ILogger<TicketViewModel> _logger;
        private readonly IMediator _mediator; 
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(
            ILogger<TicketViewModel> logger,
            IMediator mediator, 
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _mediator = mediator; 
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Customer,SupportManager")]
        public async Task<IActionResult> Index()
        {
            IQueryable<Ticket> tickets;
            if (User.IsInRole("SupportManager"))
            {
                tickets = await _mediator.Send(new GetTicketsQuery());
            }
            else
            {
                var client = await _mediator.Send(new GetClientByUserQuery {Username = User.Identity.Name});
                tickets = await _mediator.Send(new GetTicketsByClientIdQuery { ClientId = client.Id});
            }

            var openTickets = tickets.Where(x => x.Status == TicketStatus.Aangemaakt || x.Status == TicketStatus.InBehandeling).Take(10);
            var closedTickets = tickets.Where(x => x.Status == TicketStatus.Afgehandeld || x.Status == TicketStatus.Geannuleerd).OrderByDescending(x => x.Ticketnr).Take(10);

            var model = new DashboardViewModel 
            {
                OpenTickets = _mapper.Map<List<Ticket>, List<TicketBaseInfoViewModel>>(openTickets.ToList()),
                ClosedTickets = _mapper.Map<List<Ticket>, List<TicketBaseInfoViewModel>>(closedTickets.ToList()),
            };

            return View(model);
        }
    }
}
