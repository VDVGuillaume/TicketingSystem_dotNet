using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.QueryHandlers
{
    public class GetTicketTypeByNameQueryHandler : BaseQueryHandler<GetTicketTypeByNameQuery, TicketType>
    {
        public GetTicketTypeByNameQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<TicketType> ExecuteCommandAsync(GetTicketTypeByNameQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.TicketTypes.FirstOrDefault(x => x.Name == request.Name);
        }
    }
}
