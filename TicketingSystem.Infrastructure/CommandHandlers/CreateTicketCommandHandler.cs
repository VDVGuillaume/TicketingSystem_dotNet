using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.CommandHandlers
{
    public class CreateTicketCommandHandler : BaseCommandHandler<CreateTicketCommand, Ticket>
    {

        public CreateTicketCommandHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<Ticket> ExecuteCommandAsync(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            var ticketType = await _mediator.Send(new GetTicketTypeByNameQuery { Name = request.Type});

            if (ticketType == null) 
            {
                throw new ValidationException(Constants.ERROR_TICKET_TYPE_NOT_FOUND);
            }

            var ticket = new Ticket(request.Title, request.Description, ticketType, request.Client, request.Attachments);
            await _dbContext.Tickets.AddAsync(ticket);
            _dbContext.SaveChanges();

            return ticket;
        }
    }
}
