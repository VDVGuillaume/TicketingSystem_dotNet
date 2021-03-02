using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public class GetClientByUserQueryHandler : BaseQueryHandler<GetClientByUserQuery, Client>
    {
        public GetClientByUserQueryHandler(IMediator mediator, TicketingSystemDbContext dbContext) : base(mediator, dbContext)
        {
        }

        public async override Task<Client> ExecuteCommandAsync(GetClientByUserQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.Users
                .Include(x => x.Client)
                .FirstOrDefault(x => x.UserName == request.Username)
                ?.Client;
        }
    }
}
