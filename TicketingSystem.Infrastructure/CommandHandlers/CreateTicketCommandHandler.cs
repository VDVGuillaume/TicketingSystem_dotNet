using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application;
using TicketingSystem.Domain.Application.Commands;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;
using TicketingSystem.Infrastructure.Services;
using TicketingSystem.Infrastructure.Services.Interfaces;

namespace TicketingSystem.Infrastructure.CommandHandlers
{
    public class CreateTicketCommandHandler : BaseCommandHandler<CreateTicketCommand, Ticket>
    {
        private ITicketService _ticketService;

        public CreateTicketCommandHandler(IMediator mediator, TicketingSystemDbContext dbContext, ITicketService ticketService) : base(mediator, dbContext)
        {
            _ticketService = ticketService ?? throw new ArgumentNullException();
        }

        public async override Task<Ticket> ExecuteCommandAsync(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            return await _ticketService.CreateTicket(request);
        }
    }
}
