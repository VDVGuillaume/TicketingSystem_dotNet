﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private List<TicketStatus> ConvertFilterToTicketStatuses(string filter)
        {
            List<TicketStatus> ticketStatuses = new List<TicketStatus>();
            if (!string.IsNullOrEmpty(filter))
            {
                var filters = filter.Split(",");
                foreach (var f in filters)
                {
                    if (Enum.TryParse<TicketStatus>(f, out var ticketStatus))
                    {
                        ticketStatuses.Add(ticketStatus);
                    }
                }
            }
            else
            {
                ticketStatuses.Add(TicketStatus.Aangemaakt);
                ticketStatuses.Add(TicketStatus.InBehandeling);
            }

            return ticketStatuses;
        }

        [Authorize(Roles = "Customer,SupportManager")]
        public async Task<IActionResult> Index([FromQuery]string statusFilter)
        {
            List<TicketStatus> ticketStatusFilter = ConvertFilterToTicketStatuses(statusFilter);
            IQueryable<Ticket> tickets;
            if (User.IsInRole("SupportManager"))
            {
                tickets = await _mediator.Send(new GetTicketsQuery());
            }
            else {
                tickets = await _mediator.Send(new GetTicketsByClientIdQuery { ClientId = _userManager.GetUserId(User) });
            }

            //TODO add get tickets for supportManager view
            var filteredTickets = tickets.Where(x => ticketStatusFilter.Contains(x.Status));

            var ticketsIndexDto = _mapper.Map<List<Ticket>, List<TicketBaseInfoViewModel>>(filteredTickets.ToList());
            var model = new TicketIndexViewModel 
            { 
                Tickets = ticketsIndexDto, 
                FilterInput = new FilterInputModel 
                { 
                    FilterStatusCreated = ticketStatusFilter.Contains(TicketStatus.Aangemaakt),
                    FilterStatusInProgress = ticketStatusFilter.Contains(TicketStatus.InBehandeling),
                    FilterStatusClosed = ticketStatusFilter.Contains(TicketStatus.Afgehandeld),
                    FilterStatusCancelled = ticketStatusFilter.Contains(TicketStatus.Geannuleerd)
                }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(TicketIndexViewModel model) 
        {
            if (ModelState.IsValid) 
            {
                var sb = new StringBuilder();
                string delimiter = ",";

                void AppendToStringBuilder(string text)
                {
                    if (sb.Length > 0)
                        sb.Append(delimiter);
                    sb.Append(text);
                }

                if (model.FilterInput.FilterStatusCreated)
                    AppendToStringBuilder(TicketStatus.Aangemaakt.ToString());

                if (model.FilterInput.FilterStatusInProgress)
                    AppendToStringBuilder(TicketStatus.InBehandeling.ToString());

                if (model.FilterInput.FilterStatusClosed)
                    AppendToStringBuilder(TicketStatus.Afgehandeld.ToString());

                if (model.FilterInput.FilterStatusCancelled)
                    AppendToStringBuilder(TicketStatus.Geannuleerd.ToString());

                return await Index(sb.ToString());
            }

            return View(model);
        }

        [Authorize(Roles = "Customer,SupportManager")]
        public async Task<IActionResult> Details(string id)
        {
            var ticket = await _mediator.Send(new GetTicketByIdQuery { Id = id });
            var ticketsDetailsDto = _mapper.Map<Ticket, TicketDetailsDTO>((Ticket)ticket);
            var model = new TicketDetailsViewModel { Ticket = ticketsDetailsDto };

            return View(model);
        }

        [Authorize(Roles = "Customer,SupportManager")]
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var ticket = await _mediator.Send(new GetTicketByIdQuery { Id = id });
            var ticketsDetailsDto = _mapper.Map<Ticket, TicketDetailsDTO>((Ticket)ticket);
            var model = new TicketUpdateViewModel { Ticket = ticketsDetailsDto };
            var ticketTypes = await _mediator.Send(new GetTicketTypesQuery());
            model.TicketTypes = new List<SelectListItem>();
            foreach (var ticketType in ticketTypes)
            {
                model.TicketTypes.Add(new SelectListItem { Value = ticketType.Id.ToString(), Text = ticketType.Name });
            }

            return View(model);
        }

        [Authorize(Roles = "Customer,SupportManager")]
        [HttpPost]
        public async Task<IActionResult> Update(TicketUpdateViewModel model)
        {
            if (User.IsInRole("SupportManager"))
            {
                //TODO: update stuff for Title, Description and Type
            }
            else
            {
                //TODO: update stuff for Description
            }
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
