using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.QueryHandlers
{
    public class GetTicketsQueryHandler : BaseQueryHandler<GetTicketsQuery, IQueryable<Ticket>>
    {
        public GetTicketsQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<IQueryable<Ticket>> ExecuteCommandAsync(GetTicketsQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.Tickets.AsNoTracking();
        }
    }
}
