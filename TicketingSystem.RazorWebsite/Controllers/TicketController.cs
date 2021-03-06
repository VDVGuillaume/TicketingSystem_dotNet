using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Application.Exceptions;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketController(
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

        private async Task<List<SelectListItem>> GetTicketTypes(string selectedValue = null)
        {
            var ticketTypes = await _mediator.Send(new GetTicketTypesQuery());
            var result = new List<SelectListItem>();
            foreach (var ticketType in ticketTypes)
            {
                result.Add(new SelectListItem { Value = ticketType.Name, Text = ticketType.Name, Selected = ticketType.Name == selectedValue });
            }

            return result;
        }

        private async Task<List<SelectListItem>> GetClients(string selectedValue = null)
        {
            var clients = await _mediator.Send(new GetClientsQuery());
            var result = new List<SelectListItem>();
            foreach (var client in clients)
            {
                result.Add(new SelectListItem { Value = client.Name, Text = client.Name, Selected = client.Name == selectedValue });
            }
            return result;
        }

        private async Task<List<SelectListItem>> GetEngineerUsers(string selectedValue = null)
        {
            var engineerUsers = await _mediator.Send(new GetEngineerUsersQuery());
            var result = new List<SelectListItem>();
            foreach (var engineerUser in engineerUsers)
            {
                result.Add(new SelectListItem { Value = engineerUser.UserName, Text = engineerUser.UserName, Selected = engineerUser.UserName == selectedValue });
            }

            return result;
        }

        [HttpGet]
        [Authorize(Roles = "Customer,SupportManager")]
        public async Task<IActionResult> Index([FromQuery] string statusFilter)
        {
            List<TicketStatus> ticketStatusFilter = ConvertFilterToTicketStatuses(statusFilter);
            IQueryable<Ticket> tickets;
            if (User.IsInRole("SupportManager"))
            {
                tickets = await _mediator.Send(new GetTicketsQuery());
            }
            else
            {
                var client = await _mediator.Send(new GetClientByUserQuery { Username = User.Identity.Name });
                tickets = await _mediator.Send(new GetTicketsByClientIdQuery { ClientId = client.Id});
            }

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
        [Authorize(Roles = "Customer,SupportManager")]
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

        [HttpGet]
        [Authorize(Roles = "Customer,SupportManager")]
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _mediator.Send(new GetTicketByIdQuery { Id = id });

            if (ticket == null)
            {
                return RedirectToAction("Index");
            }

            var ticketsDetailsDto = _mapper.Map<Ticket, TicketDetailInfoViewModel>(ticket);
            var model = new TicketDetailsViewModel { Ticket = ticketsDetailsDto };

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Customer,SupportManager")]
        public async Task<IActionResult> Update(int id)
        {
            var ticket = await _mediator.Send(new GetTicketByIdQuery { Id = id });

            if (ticket == null)
            {
                return RedirectToAction("Index");
            }

            var ticketsDetailsDto = _mapper.Map<Ticket, TicketDetailInfoViewModel>((Ticket)ticket);
            var model = new TicketUpdateViewModel { Ticket = ticketsDetailsDto };
            model.TicketTypes = await GetTicketTypes(ticket.Type.Name);
            model.EngineerUsers = await GetEngineerUsers(ticket.AssignedEngineer?.UserName);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Customer,SupportManager")]
        public async Task<IActionResult> Update([FromQuery] int id, TicketUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (User.IsInRole("SupportManager"))
                    {
                        await _mediator.Send(new UpdateTicketCommand
                        {
                            Ticketnr = id,
                            Title = model.Input.Title,
                            Description = model.Input.Description,
                            Type = model.Input.Type,
                            AssignedEngineer = model.Input.AssignedEngineer,
                            Attachments = model.Input.Attachments
                        });
                    }
                    else
                    {
                        await _mediator.Send(new UpdateTicketCommand
                        {
                            Ticketnr = id,
                            Description = model.Input.Description,
                            Attachments = model.Input.Attachments
                        });
                    }
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("ValidationError", ex.Message);
                    return View("Update", model);
                }
            }

            return RedirectToAction("Details", new { id = id });
        }

        [Authorize(Roles = "Customer,SupportManager")]
        [HttpGet]
        public async Task<IActionResult> CreateTicket()
        {
            var model = new TicketViewModel();

            model.TicketTypes = await GetTicketTypes();
            model.Clients = await GetClients();
            if (!User.IsInRole("SupportManager")) 
            {
                Client client = await _mediator.Send(new GetClientByUserQuery { Username = User.Identity.Name });
                var result = await _mediator.Send(new GetActiveContractByClientIdQuery { ClientId = client.Id });
                model.HasActiveContract = result.Any(); 
            }

            return View(model);
        }

        [Authorize(Roles = "Customer,SupportManager")]
        [HttpPost]
        public async Task<IActionResult> CreateTicket(TicketViewModel model)
        {
            Client client;
            if (User.IsInRole("SupportManager") && !string.IsNullOrEmpty(model.Input.ClientName))
            {
                client = await _mediator.Send(new GetClientByNameQuery { Name = model.Input.ClientName });

                if (client == null)
                {
                    ModelState.AddModelError(string.Empty, "Klant is niet gevonden.");
                    return View(model);
                }
            }
            else
            {
                client = await _mediator.Send(new GetClientByUserQuery { Username = User.Identity.Name });
            }

            if (ModelState.IsValid)
            {
                if (client == null)
                {
                    ModelState.AddModelError(string.Empty, "Klant is verplicht.");
                    return View(model);
                }

                try
                {
                    await _mediator.Send(new CreateTicketCommand
                    {
                        Title = model.Input.Title,
                        Description = model.Input.Description,
                        Type = model.Input.Type,
                        Client = client,
                        Attachments = model.Input.Attachments,
                        DateRequested = DateTime.Now
                    });
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(model);
                }
            }

            return LocalRedirect(model.ReturnUrl ?? Url.Content("~/Ticket/Index"));
        }

        [Authorize(Roles = "Customer,SupportManager")]
        [HttpPost]
        public async Task<IActionResult> Cancel([FromQuery] int id, TicketDetailsViewModel model)
        {
            try
            {
                var ticket = await _mediator.Send(new CancelTicketCommand
                {
                    Ticketnr = id
                });

                model.Ticket = _mapper.Map<Ticket, TicketDetailInfoViewModel>(ticket);
                return View("Details", model);
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("ValidationError", ex.Message);
                return View("Details", model);
            }
        }


        [Authorize(Roles = "Customer,SupportManager")]
        [HttpPost]
        public async Task<IActionResult> PostComment([FromQuery] int id, TicketDetailsViewModel model)
        {

          

            try
            {
                await _mediator.Send(new PostCommentCommand
                {
                    Ticketnr = id,
                    Text = model.Input.Comment,
                    DateAdded = DateTime.Now,
                    CreatedBy = User.Identity.Name
                    
                }); ;            

            }
            catch (ValidationException ex)
            {
               
                ModelState.AddModelError("ValidationError", ex.Message);
            }
            finally
            {
                var ticket = await _mediator.Send(new GetTicketByIdQuery { Id = id });
                var ticketsDetailsDto = _mapper.Map<Ticket, TicketDetailInfoViewModel>((Ticket)ticket);
                model = new TicketDetailsViewModel { Ticket = ticketsDetailsDto };                
            }
            return View("Details", model);
        }

        [Authorize(Roles = "Customer,SupportManager")]
        public async Task<IActionResult> DownloadAttachment([FromQuery]int attachmentId) 
        {
            var attachment = await _mediator.Send(new GetAttachmentByIdQuery { AttachmentId = attachmentId });
            if (attachment == null) 
            {
                ModelState.AddModelError("ValidationError", "bijlage bestaat niet.");
                return View();
            }

            return File(attachment.VirtualPath, "APPLICATION/octet-stream", attachment.Name);
        }
    }
}
