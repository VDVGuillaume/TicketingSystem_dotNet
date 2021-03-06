using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.QueryHandlers
{
    public class GetActiveContractByClientIdQueryHandler : BaseQueryHandler<GetActiveContractByClientIdQuery, IQueryable<Contract>>
    {

        public GetActiveContractByClientIdQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<IQueryable<Contract>> ExecuteCommandAsync(GetActiveContractByClientIdQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.Contracts
                .Include(x => x.Type)
                .Include(x => x.Client)
                .Where(x => x.Client.Id == request.ClientId && x.Status == ContractStatus.Lopend);

        }

    }
}
