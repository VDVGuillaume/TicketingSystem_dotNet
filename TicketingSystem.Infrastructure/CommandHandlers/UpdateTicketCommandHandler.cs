using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Application.Exceptions;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.CommandHandlers
{
    public class UpdateTicketCommandHandler : BaseCommandHandler<UpdateTicketCommand, Ticket>
    {
        public UpdateTicketCommandHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<Ticket> ExecuteCommandAsync(UpdateTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _mediator.Send(new GetTicketByIdQuery { Id = request.Ticketnr });
            var ticketType = await _mediator.Send(new GetTicketTypeByNameQuery { Name = request.Type });

            if (ticket == null) 
                throw new ValidationException(Constants.ERROR_TICKET_NOT_FOUND);
            if(ticketType == null && !string.IsNullOrEmpty(request.Type))
                throw new ValidationException(Constants.ERROR_TICKET_TYPE_NOT_FOUND);

            if(!string.IsNullOrEmpty(request.Title))
                ticket.Title = request.Title;
            if(!string.IsNullOrEmpty(request.Type))
                ticket.Type = ticketType;
            if (!string.IsNullOrEmpty(request.Description))
                ticket.Description = request.Description;

            _dbContext.Tickets.Update(ticket);
            await _dbContext.SaveChangesAsync();

            return ticket;
        }
    }
}
