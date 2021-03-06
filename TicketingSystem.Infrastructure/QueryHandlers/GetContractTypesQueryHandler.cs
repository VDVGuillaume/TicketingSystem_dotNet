using MediatR;
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
    public class GetContractTypesQueryHandler : BaseQueryHandler<GetContractTypesQuery, IQueryable<ContractType>>
    {
        public GetContractTypesQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<IQueryable<ContractType>> ExecuteCommandAsync(GetContractTypesQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.ContractTypes;
        }

    }
}
