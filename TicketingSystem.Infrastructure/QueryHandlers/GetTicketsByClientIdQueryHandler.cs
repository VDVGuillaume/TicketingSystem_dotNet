using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.QueryHandlers
{
    public class GetTicketsByClientIdQueryHandler : BaseQueryHandler<GetTicketsByClientIdQuery, IQueryable<Ticket>>
    {
        public GetTicketsByClientIdQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<IQueryable<Ticket>> ExecuteCommandAsync(GetTicketsByClientIdQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.Tickets
                .Include(x => x.Type)
                .Include(x => x.Client)
                .Where(x => x.Client.Id == request.ClientId);
        }
    }
}
