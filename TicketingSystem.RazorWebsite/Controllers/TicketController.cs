using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;
using TicketingSystem.RazorWebsite.Models.Tickets;

namespace TicketingSystem.RazorWebsite.Controllers
{
    [Authorize(Roles = "Customer,SupportManager")]
    public class TicketController : Controller
    {
        private readonly ILogger<TicketViewModel> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public TicketController(
            ILogger<TicketViewModel> logger,            
            IMediator mediator,
            IMapper mapper,
            UserManager<IdentityUser> userManager)
        {           
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize(Roles = "Customer,SupportManager")]
        public async Task<IActionResult> Index()
        {
            //TODO add get tickets for supportManager view
            var tickets = await _mediator.Send(new GetTicketsByClientIdQuery { ClientId = _userManager.GetUserId(User)});
            var ticketsIndexDto = _mapper.Map<List<Ticket>, List<TicketIndexDTO>>(tickets.ToList());
            var model = new TicketsIndexViewModel { Tickets = ticketsIndexDto };

            return View(model);
        }

        [Authorize(Roles = "Customer,SupportManager")]
        public async Task<IActionResult> Details(string id)
        {
            //TODO add get tickets for supportManager view
            var ticket = await _mediator.Send(new GetTicketByIdQuery { Id = id });
            var ticketsDetailsDto = _mapper.Map<Ticket, TicketDetailsDTO>((Ticket)ticket);
            var model = new TicketDetailsViewModel { Ticket = ticketsDetailsDto };

            return View(model);
        }

        [Authorize(Roles = "Customer,SupportManager")]
        [HttpGet]
        public async Task<IActionResult> CreateTicket()
        {
            var ticketTypes = await _mediator.Send(new GetTicketTypesQuery());
            var model = new TicketViewModel();

            model.TicketTypes = new List<SelectListItem>();
            foreach (var ticketType in ticketTypes) 
            {
                model.TicketTypes.Add(new SelectListItem { Value=ticketType.Id.ToString(), Text=ticketType.Name});
            }

            return View(model);
        }

        [Authorize(Roles = "Customer,SupportManager")]
        [HttpPost]
        public async Task<IActionResult> CreateTicket(TicketViewModel model)
        {
            IdentityUser client = null;
            if (User.IsInRole("SupportManager") && !string.IsNullOrEmpty(model.Input.ClientUsername))
            {
                client = await _mediator.Send(new GetUserByUsernameQuery { Username = model.Input.ClientUsername });

                if (client == null)
                {
                    ModelState.AddModelError(string.Empty, "Client not found.");
                    return View(model);
                }
            } else 
            {
                client = await _userManager.GetUserAsync(User);
            }
            // get current user
            
            if (ModelState.IsValid)
            {
                if (client == null)
                {
                    ModelState.AddModelError(string.Empty, "Client is required.");
                    return View(model);
                }

                try
                {                 
                    await _mediator.Send(new CreateTicketCommand { 
                        Title = model.Input.Title, 
                        Description = model.Input.Description,
                        Type = model.Input.Type,
                        Client = client});
                }
                catch
                {
                    TempData["error"] = "Sorry, something went wrong, the ticket was not created";
                }
            }

            return LocalRedirect(model.ReturnUrl ?? Url.Content("~/Ticket/Index"));
        }

    }
}
