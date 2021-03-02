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
    public class GetClientByNameQueryHandler : BaseQueryHandler<GetClientByNameQuery, Client>
    {
        public GetClientByNameQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<Client> ExecuteCommandAsync(GetClientByNameQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.Client.FirstOrDefault(x => x.Name == request.Name); 
        }
    }
}
