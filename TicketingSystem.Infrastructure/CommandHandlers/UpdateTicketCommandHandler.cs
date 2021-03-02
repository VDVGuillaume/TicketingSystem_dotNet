using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Models;
using TicketingSystem.Infrastructure.Services.Interfaces;

namespace TicketingSystem.Infrastructure.CommandHandlers
{
    public class UpdateTicketCommandHandler : BaseCommandHandler<UpdateTicketCommand, Ticket>
    {
        private ITicketService _ticketService;

        public UpdateTicketCommandHandler(IMediator mediator, TicketingSystemDbContext dbContext, ITicketService ticketService) : base(mediator, dbContext)
        {
            _ticketService = ticketService ?? throw new ArgumentNullException();
        }

        public async override Task<Ticket> ExecuteCommandAsync(UpdateTicketCommand request, CancellationToken cancellationToken)
        {
            return await _ticketService.UpdateTicket(request);
        }
    }
}
