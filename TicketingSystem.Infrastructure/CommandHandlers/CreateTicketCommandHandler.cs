using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Commands;
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
            var ticket = new Ticket(request.Title, request.Description, request.Type, request.Client, request.Attachments);
            await _dbContext.Tickets.AddAsync(ticket);
            _dbContext.SaveChanges();

            return ticket;
        }

    }
}
