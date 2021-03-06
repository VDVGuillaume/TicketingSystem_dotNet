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
    public class CancelContractCommandHandler : BaseCommandHandler<CancelContractCommand, Contract>
    {
        private IContractService _contractService;
        public CancelContractCommandHandler(IMediator mediator, TicketingSystemDbContext dbContext, IContractService contractService) : base(mediator, dbContext)
        {
            _contractService = contractService ?? throw new ArgumentNullException();
        }

        public async override Task<Contract> ExecuteCommandAsync(CancelContractCommand request, CancellationToken cancellationToken)
        {
            return await _contractService.CancelContract(request);
        }
    }
}


