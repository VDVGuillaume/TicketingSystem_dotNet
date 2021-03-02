using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Models;
using TicketingSystem.Infrastructure.Services;
using TicketingSystem.Infrastructure.Services.Interfaces;

namespace TicketingSystem.Infrastructure.CommandHandlers
{
    class CancelTicketCommandHandler : BaseCommandHandler<CancelTicketCommand, Ticket>
    {
        private ITicketService _ticketService;
        public CancelTicketCommandHandler(IMediator mediator, TicketingSystemDbContext dbContext, ITicketService ticketService) : base(mediator, dbContext)
        {
            _ticketService = ticketService ?? throw new ArgumentNullException();
        }

        public async override Task<Ticket> ExecuteCommandAsync(CancelTicketCommand request, CancellationToken cancellationToken)
        {
            return await _ticketService.CancelTicket(request);
        }
    }
}

