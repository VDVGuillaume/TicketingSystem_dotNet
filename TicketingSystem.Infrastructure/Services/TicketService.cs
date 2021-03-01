using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.Services
{
    public class TicketService
    {
        private readonly IMediator _mediator;
        private readonly TicketingSystemDbContext _dbContext;

        public TicketService(IMediator mediator, TicketingSystemDbContext dbContext)
        {
            _mediator = mediator ?? throw new ArgumentNullException();
            _dbContext = dbContext ?? throw new ArgumentNullException();
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

            //create attachments
            if (request.Attachments != null) 
            {
                foreach (var file in request.Attachments) 
                {
                    if (file != null) 
                    {
                        var attachment = new Attachment(file.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), $@"wwwroot\attachments\{ticket.Ticketnr}", file.FileName);
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                            ticket.Attachments.Add(attachment);
                        }
                    }
                }
            }

            // save changes
            _dbContext.SaveChanges();

            return ticket;
        }

    }
}
