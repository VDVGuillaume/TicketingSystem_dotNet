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
    public class CreateContractCommandHandler : BaseCommandHandler<CreateContractCommand, Contract>
    {
        private IContractService _contractService;
        public CreateContractCommandHandler(IMediator mediator, TicketingSystemDbContext dbContext, IContractService contractService) : base(mediator, dbContext)
        {
            _contractService = contractService ?? throw new ArgumentNullException();
        }

        public async override Task<Contract> ExecuteCommandAsync(CreateContractCommand request, CancellationToken cancellationToken)
        {
            return await _contractService.CreateContract(request);
        }
    }
}