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
    public class GetClientsQueryHandler : BaseQueryHandler<GetClientsQuery, IQueryable<Client>>
    {
        public GetClientsQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<IQueryable<Client>> ExecuteCommandAsync(GetClientsQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.Client;
        }
    }
}
