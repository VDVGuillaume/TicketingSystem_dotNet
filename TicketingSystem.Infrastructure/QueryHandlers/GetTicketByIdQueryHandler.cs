using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketingSystem.Domain.Application.Queries;
using TicketingSystem.Domain.Models;

namespace TicketingSystem.Infrastructure.QueryHandlers
{
    public class GetTicketByIdQueryHandler : BaseQueryHandler<GetTicketByIdQuery, Ticket>
    {
        public GetTicketByIdQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<Ticket> ExecuteCommandAsync(GetTicketByIdQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.Tickets.FirstOrDefault(x => x.Ticketnr.ToString() == request.Id);
        }
    }
}
