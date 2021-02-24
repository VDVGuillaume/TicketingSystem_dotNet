using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.CommandHandlers
{
    class CancelTicketCommandHandler : BaseCommandHandler<CancelTicketCommand, Ticket>
    {
        public CancelTicketCommandHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<Ticket> ExecuteCommandAsync(CancelTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = _dbContext.Tickets.First(x => x.Ticketnr == request.Ticketnr);
            ticket.Status = request.Status;
            _dbContext.Tickets.Update(ticket);
            await _dbContext.SaveChangesAsync();

            return ticket;
        }
    }
}

