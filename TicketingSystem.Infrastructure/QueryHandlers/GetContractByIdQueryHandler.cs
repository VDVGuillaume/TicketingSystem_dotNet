using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.QueryHandlers
{
    public class GetContractByIdQueryHandler : BaseQueryHandler<GetContractByIdQuery, Contract>
    {
        public GetContractByIdQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<Contract> ExecuteCommandAsync(GetContractByIdQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.Contracts
                .Include(x => x.Type)
                .Include(x => x.Client)
                .FirstOrDefault(x => x.ContractId == request.Id);
        }
    }
}
