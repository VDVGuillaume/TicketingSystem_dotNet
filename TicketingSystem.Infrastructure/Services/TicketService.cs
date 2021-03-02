using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;
using TicketingSystem.Infrastructure.Services.Interfaces;

namespace TicketingSystem.Infrastructure.Services
{
    public class TicketService : ITicketService
    {
        private readonly IMediator _mediator;
        private readonly TicketingSystemDbContext _dbContext;

        public TicketService(IMediator mediator, TicketingSystemDbContext dbContext)
        {
            _mediator = mediator ?? throw new ArgumentNullException();
            _dbContext = dbContext ?? throw new ArgumentNullException();
        }

        private string DetermineAttachmentLocation(int ticketNr, string fileName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), $@"wwwroot\attachments\{ticketNr}", fileName);
        }

        private void ValidateTicketStatus(Ticket ticket)
        {
            if (ticket.Status == TicketStatus.Geannuleerd)
            {
                throw new ValidationException(Constants.ERROR_TICKET_STATUS_CANCELLED);
            }

            if (ticket.Status == TicketStatus.Afgehandeld)
            {
                throw new ValidationException(Constants.ERROR_TICKET_STATUS_CLOSED);
            }
        }

        public async Task<Ticket> CreateTicket(CreateTicketCommand request)
        {
            // validate if ticketType is valid input
            var ticketType = await _mediator.Send(new GetTicketTypeByNameQuery { Name = request.Type });
            if (ticketType == null)
            {
                throw new ValidationException(Constants.ERROR_TICKET_TYPE_NOT_FOUND);
            }

            // create new ticket
            var ticket = new Ticket(request.Title, request.Description, ticketType, request.Client);
            await _dbContext.Tickets.AddAsync(ticket);

            // save ticket
            // this step comes before adding the attachments because we want to link the attachment location to the ticket ID
            _dbContext.SaveChanges();

            //create attachments
            if (request.Attachments != null)
            {
                foreach (var file in request.Attachments)
                {
                    if (file != null)
                    {
                        var attachment = new Attachment(file.FileName);
                        var filePath = DetermineAttachmentLocation(ticket.Ticketnr, file.FileName);
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            ticket.Attachments.Add(attachment);
                        }
                    }
                }
            }

            // save attachments
            _dbContext.SaveChanges();

            return ticket;
        }

        public async Task<Ticket> UpdateTicket(UpdateTicketCommand request)
        {
            var ticket = await _mediator.Send(new GetTicketByIdQuery { Id = request.Ticketnr });
            var ticketType = await _mediator.Send(new GetTicketTypeByNameQuery { Name = request.Type });

            //validate
            if (ticket == null)
                throw new ValidationException(Constants.ERROR_TICKET_NOT_FOUND);
            if (ticketType == null && !string.IsNullOrEmpty(request.Type))
                throw new ValidationException(Constants.ERROR_TICKET_TYPE_NOT_FOUND);
            ValidateTicketStatus(ticket);

            if (!string.IsNullOrEmpty(request.Title))
                ticket.Title = request.Title;
            if (!string.IsNullOrEmpty(request.Type))
                ticket.Type = ticketType;
            if (!string.IsNullOrEmpty(request.Description))
                ticket.Description = request.Description;

            await _dbContext.SaveChangesAsync();

            return ticket;
        }

        public async Task<Ticket> CancelTicket(CancelTicketCommand request)
        {
            var ticket = await _mediator.Send(new GetTicketByIdQuery { Id = request.Ticketnr });

            //validate
            if (ticket == null)
                throw new ValidationException(Constants.ERROR_TICKET_NOT_FOUND);
            ValidateTicketStatus(ticket);

            ticket.Status = TicketStatus.Geannuleerd;

            await _dbContext.SaveChangesAsync();

            return ticket;
        }
    }
}
