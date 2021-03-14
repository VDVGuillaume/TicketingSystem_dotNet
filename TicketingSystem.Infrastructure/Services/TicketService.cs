using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Application.Exceptions;
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
            return Path.Combine($@"wwwroot\attachments\{ticketNr}", fileName);
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
            // validate client
            if (request.Client == null)
                throw new ValidationException(Constants.ERROR_CLIENT_NOT_FOUND);

            // validate if ticketType is valid input
            var ticketType = await _mediator.Send(new GetTicketTypeByNameQuery { Name = request.Type });
            if (ticketType == null)
            {
                throw new ValidationException(Constants.ERROR_TICKET_TYPE_NOT_FOUND);
            }

            // get active contract
            var contract = await _mediator.Send(new GetActiveContractByClientQuery { Client = request.Client});
            if (contract == null) 
            {
                throw new ValidationException(Constants.ERROR_ACTIVE_CONTRACT_NOT_FOUND);
            }

            //if (contract.Type.TicketCreationTypes.Select(x => x.Name == "Applicatie").Any()) {  }

            // create new ticket
            var ticket = new Ticket(request.Title, request.Description, ticketType, request.Client, contract);
            await _dbContext.Tickets.AddAsync(ticket);

            // save ticket
            // this step comes before adding the attachments because we want to link the attachment location to the ticket ID
            _dbContext.SaveChanges();
                       

            await AddAttachment(new AddAttachmentCommand { Attachments = request.Attachments}, ticket);

            return ticket;
        }

        

        public async Task AddAttachment(AddAttachmentCommand request, Ticket ticket)
        {
            //create attachments
            if (request.Attachments != null)
            {
                foreach (var file in request.Attachments)
                {
                    if (file != null)
                    {
                        var filePath = DetermineAttachmentLocation(ticket.Ticketnr, file.FileName);
                        var attachment = new Attachment(file.FileName, $"attachments/{ticket.Ticketnr}/{file.FileName}");
                        Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(Directory.GetCurrentDirectory(), filePath)));

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
        }



        public async Task<Ticket> UpdateTicket(UpdateTicketCommand request)
        {
            var ticket = await _mediator.Send(new GetTicketByIdQuery { Id = request.Ticketnr });
            var ticketType = await _mediator.Send(new GetTicketTypeByNameQuery { Name = request.Type });
            var assignedEngineer = await _mediator.Send(new GetUserByUsernameQuery { Username = request.AssignedEngineer });

            //validate
            if (ticket == null)
                throw new ValidationException(Constants.ERROR_TICKET_NOT_FOUND);
            if (ticketType == null && !string.IsNullOrEmpty(request.Type))
                throw new ValidationException(Constants.ERROR_TICKET_TYPE_NOT_FOUND);
            ValidateTicketStatus(ticket);
            //if (assignedEngineer.IsInRole("Technician"))

            if (!string.IsNullOrEmpty(request.Title))
                ticket.Title = request.Title;
            if (!string.IsNullOrEmpty(request.Type))
                ticket.Type = ticketType;
            if (!string.IsNullOrEmpty(request.Description))
                ticket.Description = request.Description;
            if (!string.IsNullOrEmpty(request.AssignedEngineer))
                ticket.AssignedEngineer = assignedEngineer;

            await _dbContext.SaveChangesAsync();
           
            await AddAttachment(new AddAttachmentCommand { Attachments = request.Attachments }, ticket);

            return ticket;
        }

        public async Task<Comment> PostComment (PostCommentCommand request)
        {
            var ticket = await _mediator.Send(new GetTicketByIdQuery { Id = request.Ticketnr });
            
            //validate
            if (ticket == null)
                throw new ValidationException(Constants.ERROR_TICKET_NOT_FOUND);
            if (string.IsNullOrEmpty(request.Text))
                throw new ValidationException(Constants.ERROR_EMPTY_COMMENT);

            ValidateTicketStatus(ticket);

            var comment = new Comment() { Text = request.Text, CreatedBy = request.CreatedBy, DateAdded = request.DateAdded };
            ticket.Comments.Add(comment);

            await _dbContext.SaveChangesAsync();

            return comment;
        }

        public async Task<Ticket> CancelTicket(CancelTicketCommand request)
        {
            var ticket = await _mediator.Send(new GetTicketByIdQuery { Id = request.Ticketnr });

            //validate
            if (ticket == null)
                throw new ValidationException(Constants.ERROR_TICKET_NOT_FOUND);
            ValidateTicketStatus(ticket);

            ticket.Status = TicketStatus.Geannuleerd;
            ticket.DateClosed = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            return ticket;
        }
    }
}
