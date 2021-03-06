using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.QueryHandlers
{
    public class GetContractTypeByNameQueryHandler : BaseQueryHandler<GetContractTypeByNameQuery, ContractType>
    {
        public GetContractTypeByNameQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<ContractType> ExecuteCommandAsync(GetContractTypeByNameQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.ContractTypes.FirstOrDefault(x => x.Name == request.Name);
        }
    }
}
