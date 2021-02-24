using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Application.Exceptions;
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
            var ticket = _dbContext.Tickets.FirstOrDefault(x => x.Ticketnr == request.Ticketnr);

            if (ticket == null) 
            {
                throw new ValidationException(Constants.ERROR_TICKET_NOT_FOUND);
            }

            ticket.Description = request.Description;
            _dbContext.Tickets.Update(ticket);
            await _dbContext.SaveChangesAsync();

            return ticket;
        }
    }
}
