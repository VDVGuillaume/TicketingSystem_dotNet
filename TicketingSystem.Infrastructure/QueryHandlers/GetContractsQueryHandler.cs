using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.QueryHandlers
{
    public class GetContractsQueryHandler : BaseQueryHandler<GetContractsQuery, IQueryable<Contract>>
    {
        public GetContractsQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<IQueryable<Contract>> ExecuteCommandAsync(GetContractsQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.Contracts
                .Include(x => x.Type)
                .Include(x => x.Client);
        }
    }
}
