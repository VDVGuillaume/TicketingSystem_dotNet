using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.QueryHandlers
{
    public class GetActiveContractByClientQueryHandler : BaseQueryHandler<GetActiveContractByClientQuery, Contract>
    {
        public GetActiveContractByClientQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<Contract> ExecuteCommandAsync(GetActiveContractByClientQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.Contracts
                .Include(x => x.Client)
                .FirstOrDefault(x => 
                        x.Client == request.Client
                    &&  x.Status == ContractStatus.Lopend); 
        }
    }
}
