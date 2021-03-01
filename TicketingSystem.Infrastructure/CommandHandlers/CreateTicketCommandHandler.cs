using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;
using TicketingSystem.Infrastructure.Services;

namespace TicketingSystem.Infrastructure.CommandHandlers
{
    public class CreateTicketCommandHandler : BaseCommandHandler<CreateTicketCommand, Ticket>
    {

        public CreateTicketCommandHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<Ticket> ExecuteCommandAsync(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            var ticketService = new TicketService(_mediator, _dbContext);
            return await ticketService.CreateTicket(request);
        }
    }
}
